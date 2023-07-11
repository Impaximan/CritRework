using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
                    }
                    break;
                }
            }
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            AddCritType(item);
        }

        public void AddCritType(Item item)
        {
            if (item.damage == -1 || item.ammo != AmmoID.None)
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


            if (CritRework.randomCritPool.Count <= 0)
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
            return item.DamageType != DamageClass.Summon && item.DamageType != DamageClass.SummonMeleeSpeed && item.ammo == AmmoID.None && !item.consumable;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
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


            if (tooltips.Exists(x => x.Name == "CritChance"))
            {
                int index = tooltips.FindIndex(x => x.Name == "CritChance");
                tooltips.RemoveAt(index);

                if (CanHaveCrits(item))
                {
                    if (critType == null)
                    {
                        TooltipLine line = new TooltipLine(Mod, "NoCrit", "Currently cannot critically strike" +
                            "\nCan be reforged to gain the ability to");
                        line.OverrideColor = Color.Gray;

                        tooltips.Add(line);
                    }
                    else
                    {
                        TooltipLine line = new TooltipLine(Mod, "CritDamage", Math.Round(Main.LocalPlayer.GetWeaponDamage(item, true) * CritType.CalculateActualCritMult(critType, item, Main.LocalPlayer)) + " critical strike damage");

                        tooltips.Insert(index, line);
                    }
                }
            }

            if (!CanHaveCrits(item) && item.ammo == AmmoID.None)
            {
                TooltipLine line = new TooltipLine(Mod, "CritDamage", "Unable to critically strike");
                line.OverrideColor = Color.Gray;

                tooltips.Add(line);
            }

            if (critType != null)
            {
                TooltipLine line = new TooltipLine(Mod, "CritDescription", critType.GetDescription());
                line.OverrideColor = critType.Color;

                int indexA = tooltips.FindLastIndex(x => x.Name.Contains("Tooltip"));
                int indexB = tooltips.FindIndex(x => x.Name == "UseMana");
                int indexC = tooltips.FindIndex(x => x.Name == "Knockback");

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
                else
                {
                    tooltips.Add(line);
                }
            }
        }
    }
}
