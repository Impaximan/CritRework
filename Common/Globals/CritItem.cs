using CritRework.Common.ModPlayers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace CritRework.Common.Globals
{
    class CritItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public CritType critType = null;
        public static LocalizedText necromanticTooltip;
        public static LocalizedText unknownTooltip;
        public static LocalizedText unableTooltip;

        public override void SetStaticDefaults()
        {
            necromanticTooltip = Mod.GetLocalization($"NecromanticTooltip");
            unknownTooltip = Mod.GetLocalization($"UnknownTooltip");
            unableTooltip = Mod.GetLocalization($"UnableTooltip");
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(critType != null);
            if (critType != null)
            {
                writer.Write(critType.InternalName);
            }
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            if (reader.ReadBoolean())
            {
                critType = CritRework.GetCrit(reader.ReadString());
            }
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            if (critType != null) tag.Add("critType", critType.InternalName);
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.ContainsKey("critType")) critType = CritRework.GetCrit(tag.GetString("critType"));
        }

        public override void PostReforge(Item item)
        {
            AddCritType(item);
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

        public override bool OnPickup(Item item, Player player)
        {
            if (item.IsCurrency)
            {
                player.GetModPlayer<CritPlayer>().timeSinceGoldPickup = 0;
            }
            return base.OnPickup(item, player);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && item.prefix == ModContent.PrefixType<Content.Prefixes.Weapon.Necromantic>() && !player.moonLeech)
            {
                player.Heal(Content.Prefixes.Weapon.Necromantic.healAmount);
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
            if (CritRework.loadedCritTypes.Count > 0)
            {
                foreach (CritType crit in CritRework.loadedCritTypes)
                {
                    if (crit.ForceOnItem(entity))
                    {
                        critType = crit;
                        break;
                    }
                }
            }

            List<int> affectedShortswords = new()
            {
                ItemID.CopperShortsword,
                ItemID.TinShortsword,
                ItemID.IronShortsword,
                ItemID.LeadShortsword,
                ItemID.SilverShortsword,
                ItemID.TungstenShortsword,
                ItemID.GoldShortsword,
                ItemID.PlatinumShortsword
            };

            if (affectedShortswords.Contains(entity.type))
            {
                entity.crit += 35;
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
            if (item.damage == -1 || item.ammo != AmmoID.None || item.accessory || !CanHaveCrits(item))
            {
                return;
            }

            if (CritRework.loadedCritTypes.Count <= 0)
            {
                return;
            }


            foreach (CritType crit in CritRework.loadedCritTypes)
            {
                if (crit.ForceOnItem(item))
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

        //public float GetCritDamageMult(Player player, Item item)
        //{
        //    float mult = critType.GetDamageMult(player, item);
        //    return mult;
        //}

        public static bool CanHaveCrits(Item item)
        {
            return item.DamageType.UseStandardCritCalcs && item.useStyle != ItemUseStyleID.None && !item.consumable && item.damage > 0;
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
                            TooltipLine line2 = new TooltipLine(Mod, "CritDescription", critPlayer.slotMachineCrit.description.Value);
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
                            TooltipLine line = new TooltipLine(Mod, "NoCrit", unknownTooltip.Value);
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

                            string colorHex = "fff88d";

                            if (CritRework.overrideCritColor)
                            {
                                colorHex = CritRework.critColor.Hex3();
                            }

                            if (firstAmmo != null)
                            {
                                string ammoExtra = "+" + Math.Round(Main.LocalPlayer.GetWeaponDamage(firstAmmo, true) * CritType.CalculateActualCritMult(critType, item, Main.LocalPlayer));
                                words.Insert(1, "([c/" + colorHex + ":" + damageExtra + ammoExtra + "])");
                            }
                            else
                            {
                                words.Insert(1, "([c/" + colorHex + ":" + damageExtra + "])");
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
                TooltipLine line = new TooltipLine(Mod, "NoCrit", unableTooltip.Value);
                line.OverrideColor = Color.Gray;

                tooltips.Add(line);
            }

            if (critType != null)
            {
                TooltipLine line = new TooltipLine(Mod, "CritDescription", critType.description.Value);
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

            if (item.prefix == ModContent.PrefixType<Content.Prefixes.Weapon.Necromantic>())
            {
                int i = tooltips.FindIndex(x => x.Name == "PrefixCritChance");
                tooltips.Insert(i + 1, new TooltipLine(Mod, "PrefixNecromantic", necromanticTooltip.Value)
                {
                    IsModifier = true,
                });
            }
        }
    }
}
