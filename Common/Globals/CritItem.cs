using CritRework.Common.ModPlayers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace CritRework.Common.Globals
{
    class CritItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public CritType critType = null;

        public override void SaveData(Item item, TagCompound tag)
        {
            if (critType != null) tag.Add("critType", critType.GetType().ToString());
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.ContainsKey("critType")) critType = CritRework.GetCrit(tag.GetString("critType"));
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if (critType == null) AddCritType(item);
            return base.ChoosePrefix(item, rand);
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (critType == null)
            {
                AddCritType(item);
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (item.damage > 0 && !item.accessory)
            {
                if (player.GetModPlayer<CritPlayer>().lastWeaponUsed != item)
                {
                    player.GetModPlayer<CritPlayer>().freshItemTime = 0;
                }
                player.GetModPlayer<CritPlayer>().lastWeaponUsed = item;
            }
            return base.UseItem(item, player);
        }

        public override void HoldItem(Item item, Player player)
        {
            if (critType == null)
            {
                AddCritType(item);
            }
        }

        //public override GlobalItem Clone(Item from, Item to)
        //{
        //    CritItem critItem = new CritItem();
        //    critItem.critType = critType;
        //    return critItem;
        //}

        public override void SetDefaults(Item entity)
        {
            if (CritRework.forcedCritTypes.Count > 0)
            {
                foreach (CritType crit in CritRework.forcedCritTypes)
                {
                    crit.ForceOnItem(out int itemType);

                    if (entity.type == itemType)
                    {
                        critType = crit;
                        break;
                    }
                }
            }
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            CritPlayer critPlayer = Main.LocalPlayer.GetModPlayer<CritPlayer>();
            if (critPlayer.timeSinceLastTooltipShown <= 2)
            {
                if (critPlayer.slotMachineCritCrafting && critPlayer.slotMachineCrit.CanApplyTo(item))
                {
                    critType = critPlayer.slotMachineCrit;
                    critPlayer.currentSlotTime = CritPlayer.maxCurrentSlotTime;
                    return;
                }
            }
            AddCritType(item);
        }

        public void AddCritType(Item item)
        {
            if (item.damage == -1 || item.ammo != AmmoID.None || item.accessory)
            {
                return;
            }

            if (CritRework.loadedCritTypes.Count <= 0)
            {
                return;
            }


            foreach (CritType crit in CritRework.forcedCritTypes)
            {
                crit.ForceOnItem(out int itemType);

                if (item.type == itemType)
                {
                    critType = crit;
                    return;
                }
            }


            if (CritRework.randomCritPool.Count <= 0 || item.consumable)
            {
                return;
            }


            if (!item.DamageType.CountsAsClass(DamageClass.Summon) && CritRework.randomCritPool.Exists(x => x.CanApplyTo(item)))
            {
                CritType appliedType = Main.rand.Next(CritRework.randomCritPool);
                while (!appliedType.CanApplyTo(item))
                {
                    appliedType = Main.rand.Next(CritRework.randomCritPool);
                }
                critType = appliedType;
            }
        }

        public float GetCritDamageMult(Player player, Item item)
        {
            float mult = critType.GetDamageMult(player, item);
            return mult;
        }

        public static bool CanHaveCrits(Item item)
        {
            return item.DamageType != DamageClass.Summon && item.DamageType != DamageClass.SummonMeleeSpeed && item.ammo == AmmoID.None && !item.consumable && !item.accessory && item.damage > 0;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            CritPlayer critPlayer = Main.LocalPlayer.GetModPlayer<CritPlayer>();
            int index = tooltips.FindIndex(x => x.Name == "CritChance");

            if (critPlayer.slotMachineCritCrafting)
            {
                critPlayer.slotMachineItem = item;
            }

            foreach (Tuple<string, string> conversion in CritRework.tooltipConversions)
            {
                foreach (TooltipLine tooltip in tooltips)
                {
                    if (tooltip.Text.Contains(conversion.Item1, StringComparison.OrdinalIgnoreCase))
                    {
                        tooltip.Text = tooltip.Text.Replace(conversion.Item1, conversion.Item2, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }

            Item firstAmmo = (item.useAmmo != AmmoID.None && Main.LocalPlayer.HasAmmo(item)) ? Main.LocalPlayer.ChooseAmmo(item) : null;

            if (tooltips.Exists(x => x.Name == "Damage") && firstAmmo != null)
            {
                int i = tooltips.FindIndex(x => x.Name == "Damage");
                int stringIndex = tooltips[i].Text.IndexOf(" ");

                string ammoExtra1 = "+" + Main.LocalPlayer.GetWeaponDamage(firstAmmo, true);
                tooltips[i].Text = tooltips[i].Text.Insert(stringIndex, ammoExtra1);
            }

            if (tooltips.Exists(x => x.Name == "CritChance"))
            {
                tooltips.RemoveAt(index);

                if (CanHaveCrits(item) || critType != null)
                {
                    if (critType == null)
                    {
                        if (critPlayer.slotMachineCritCrafting && critPlayer.slotMachineCrit != null)
                        {
                            critPlayer.timeSinceLastTooltipShown = 0;
                            TooltipLine line2 = new TooltipLine(Mod, "CritDescription", critPlayer.slotMachineCrit.GetDescription());
                            line2.OverrideColor = critPlayer.slotMachineCrit.Color;

                            int indexA = tooltips.FindLastIndex(x => x.Name.Contains("Tooltip"));
                            int indexB = tooltips.FindIndex(x => x.Name == "Consumable");
                            int indexC = tooltips.FindIndex(x => x.Name == "UseMana");
                            int indexD = tooltips.FindIndex(x => x.Name == "Knockback");

                            if (indexA != -1)
                            {
                                tooltips.Insert(indexA + 1, line2);
                            }
                            else if (indexB != -1)
                            {
                                tooltips.Insert(indexB + 1, line2);
                            }
                            else if (indexC != -1)
                            {
                                tooltips.Insert(indexC + 1, line2);
                            }
                            else if (indexD != -1)
                            {
                                tooltips.Insert(indexD + 1, line2);
                            }
                            else
                            {
                                tooltips.Add(line2);
                            }

                            string ammoExtra = firstAmmo != null ? (" + " + Math.Round(Main.LocalPlayer.GetWeaponDamage(firstAmmo, true) * CritType.CalculateActualCritMult(critPlayer.slotMachineCrit, item, Main.LocalPlayer))) : "";
                            TooltipLine line3 = new TooltipLine(Mod, "CritDamage", Math.Round(Main.LocalPlayer.GetWeaponDamage(item, true) * CritType.CalculateActualCritMult(critPlayer.slotMachineCrit, item, Main.LocalPlayer)) + ammoExtra + " critical strike damage");

                            tooltips.Insert(index, line3);
                        }
                        else
                        {
                            TooltipLine line = new TooltipLine(Mod, "NoCrit", "Unknown critical strike condition");
                            line.OverrideColor = Color.Gray;

                            tooltips.Add(line);

                            int i = tooltips.FindIndex(x => x.Name == "Damage");
                            tooltips[i].Text = tooltips[i].Text.Insert(tooltips[i].Text.IndexOf(' '), " ([c/808080:???])");

                            //TooltipLine line3 = new TooltipLine(Mod, "CritDamage", "??? critical strike damage");
                            //line3.OverrideColor = Color.Gray;

                            //tooltips.Insert(index, line3);
                        }
                    }
                    else
                    {

                        if (tooltips.Exists(x => x.Name == "Damage"))
                        {
                            int i = tooltips.FindIndex(x => x.Name == "Damage");
                            List<string> words = tooltips[i].Text.Split(' ').ToList();

                            string damageExtra = Math.Round(Main.LocalPlayer.GetWeaponDamage(item, true) * CritType.CalculateActualCritMult(critType, item, Main.LocalPlayer)).ToString();
                            if (firstAmmo != null)
                            {
                                string ammoExtra = "+" + Math.Round(Main.LocalPlayer.GetWeaponDamage(firstAmmo, true) * CritType.CalculateActualCritMult(critType, item, Main.LocalPlayer));
                                words.Insert(1, "([c/fff88d:" + damageExtra + ammoExtra + "])");
                            }
                            else
                            {
                                words.Insert(1, "([c/fff88d:" + damageExtra + "])");
                            }

                            string final = "";

                            foreach (string word in words)
                            {
                                final += word + " ";
                            }
                            tooltips[i].Text = final;
                        }

                        //string ammoExtra = firstAmmo != null ? (" + " + Math.Round(Main.LocalPlayer.GetWeaponDamage(firstAmmo, true) * CritType.CalculateActualCritMult(critType, item, Main.LocalPlayer))) : "";
                        //TooltipLine line = new TooltipLine(Mod, "CritDamage", Math.Round(Main.LocalPlayer.GetWeaponDamage(item, true) * CritType.CalculateActualCritMult(critType, item, Main.LocalPlayer)) + ammoExtra + " critical strike damage");

                        //tooltips.Insert(index, line);
                    }
                }
            }

            if (!CanHaveCrits(item) && item.ammo == AmmoID.None && critType == null && item.damage > 0)
            {
                TooltipLine line = new TooltipLine(Mod, "NoCrit", "Unable to critically strike");
                line.OverrideColor = Color.Gray;

                tooltips.Add(line);
            }

            if (critType != null)
            {
                TooltipLine line = new TooltipLine(Mod, "CritDescription", critType.GetDescription());
                line.OverrideColor = critType.Color;

                int indexA = tooltips.FindLastIndex(x => x.Name.Contains("Tooltip"));
                int indexB = tooltips.FindIndex(x => x.Name == "Consumable");
                int indexC = tooltips.FindIndex(x => x.Name == "UseMana");
                int indexD = tooltips.FindIndex(x => x.Name == "Knockback");

                if (indexA != -1)
                {
                    tooltips.Insert(indexA + 1, line);
                }
                else if (indexB != -1)
                {
                    tooltips.Insert(indexB + 1, line);
                }
                else if (indexC != -1)
                {
                    tooltips.Insert(indexC + 1, line);
                }
                else if (indexD != -1)
                {
                    tooltips.Insert(indexD + 1, line);
                }
                else
                {
                    tooltips.Add(line);
                }
            }
        }
    }
}
