using CritRework.Common.ModPlayers;
using CritRework.Content.CritTypes.WhetstoneSpecific;
using CritRework.Content.Items.Equipable.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
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

        public static LocalizedText pirateHatTooltip;
        public static LocalizedText pirateShirtTooltip;
        public static LocalizedText piratePantsTooltip;
        public static LocalizedText pirateBonus;

        public bool forceCanCrit = false;
        public bool recoverableArrow = false;

        public override void SetStaticDefaults()
        {
            necromanticTooltip = Mod.GetLocalization($"NecromanticTooltip");
            unknownTooltip = Mod.GetLocalization($"UnknownTooltip");
            unableTooltip = Mod.GetLocalization($"UnableTooltip");
            pirateHatTooltip = Mod.GetLocalization($"PirateHat");
            pirateShirtTooltip = Mod.GetLocalization($"PirateShirt");
            piratePantsTooltip = Mod.GetLocalization($"PiratePants");
            pirateBonus = Mod.GetLocalization($"PirateSetBonus");
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

        int counter = 0;
        float alphaMult = 0f;
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!CritRework.showActiveCrits) return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);

            if (item.TryGetGlobalItem(out CritItem c) && c.critType != null && c.critType.ShowWhenActive && Main.LocalPlayer.inventory.Contains(item))
            {
                Texture2D texture = ModContent.Request<Texture2D>("CritRework/Shine1").Value;

                try
                {
                    counter++;

                    float baseScale = 30f;
                    if (Main.LocalPlayer.HeldItem == item && !Main.playerInventory) baseScale = 80f;

                    if (c.critType.ShouldCrit(Main.LocalPlayer, item, null, null, new NPC.HitModifiers()))
                    {
                        alphaMult += (1f - alphaMult) * 0.2f;
                    }
                    else
                    {
                        alphaMult *= 0.9f;
                    }

                    spriteBatch.Draw(texture, position, null, Color.Lerp(CritRework.overrideCritColor ? CritRework.critColor : Color.Yellow, Color.White, 0.5f) * alphaMult, counter * MathHelper.Pi / 100f, texture.Size() / 2, MathHelper.Lerp(alphaMult, 1f, 0.75f) * baseScale / texture.Size().Length() * (1f + 0.1f * (float)Math.Sin(counter / 10f)) * Main.UIScale, SpriteEffects.None, 0f);

                }
                catch (Exception ex)
                {
                    Main.NewText("Crit condition incompatible with crit display", Color.Red);
                }
            }

            return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override void PostReforge(Item item)
        {
            if (Main.rand.NextFloat() < CritRework.randomHijackChance)
            {
                AddCritType(item);

                if (CritRework.randomHijackSound)
                {
                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Hijack")
                    {
                        Volume = 0.5f
                    });
                }
            }
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

            if (recoverableArrow)
            {
                player.GetModPlayer<CritPlayer>().timeSinceArrowPickup = 0;
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

        public override void SetDefaults(Item item)
        {
            if (CritRework.loadedCritTypes.Count > 0)
            {
                foreach (CritType crit in CritRework.loadedCritTypes)
                {
                    if (crit.ForceOnItem(item))
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

            if (affectedShortswords.Contains(item.type))
            {
                item.crit += 35;
            }

            //CHANGE PIRATE ARMOR

            if (CritRework.pirateArmorRework)
            {
                if (item.type == ItemID.PirateHat)
                {
                    item.vanity = false;
                    item.defense = 8;
                    item.rare = ItemRarityID.Pink;
                    item.value = Item.buyPrice(0, 15, 0, 0);
                }

                if (item.type == ItemID.PirateShirt)
                {
                    item.vanity = false;
                    item.defense = 12;
                    item.rare = ItemRarityID.Pink;
                    item.value = Item.buyPrice(0, 15, 0, 0);
                }

                if (item.type == ItemID.PiratePants)
                {
                    item.vanity = false;
                    item.defense = 10;
                    item.rare = ItemRarityID.Pink;
                    item.value = Item.buyPrice(0, 15, 0, 0);
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

        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (source is EntitySource_DropAsItem dropSource)
            {
                recoverableArrow = true;
            }
        }

        public void AddCritType(Item item)
        {
            if (!forceCanCrit && (item.damage == -1 || item.ammo != AmmoID.None || item.accessory || !CanHaveCrits(item)))
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
            if (item.TryGetGlobalItem(out CritItem cItem) && cItem.forceCanCrit)
            {
                return true;
            }

            return item.DamageType.UseStandardCritCalcs && item.useStyle != ItemUseStyleID.None && !item.consumable && item.damage > 0;
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (player.HasEquip<MugShot>() && critType is Greedy)
            {
                damage *= 0.75f;
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            if (player.GetModPlayer<CritPlayer>().accessoryEffects.Contains("PirateHat") && Main.rand.NextBool(4))
            {
                return false;
            }
            return base.CanConsumeAmmo(weapon, ammo, player);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            //Add pirate armor effects
            if (CritRework.pirateArmorRework)
            {
                if (item.type == ItemID.PirateHat)
                {
                    player.GetCritChance(DamageClass.Ranged) += 10;
                    player.GetCritChance(DamageClass.Summon) += 10;
                    player.GetModPlayer<CritPlayer>().accessoryEffects.Add("PirateHat");
                    player.GetDamage(DamageClass.Ranged) += 0.05f;
                }

                if (item.type == ItemID.PirateShirt)
                {
                    player.GetCritChance(DamageClass.Ranged) += 10;
                    player.GetCritChance(DamageClass.Summon) += 10;
                    player.maxMinions++;
                    player.GetDamage(DamageClass.Summon) += 0.05f;
                }

                if (item.type == ItemID.PiratePants)
                {
                    player.GetCritChance(DamageClass.Ranged) += 10;
                    player.GetCritChance(DamageClass.Summon) += 10;
                    player.maxMinions++;
                    player.moveSpeed += 0.15f;
                }
            }
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "PirateArmor")
            {
                player.setBonus = pirateBonus.Value;
                player.GetModPlayer<CritPlayer>().pirateArmor = true;
            }
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if ((head.type == ItemID.PirateHat && body.type == ItemID.PirateShirt && legs.type == ItemID.PiratePants) && CritRework.pirateArmorRework)
            {
                return "PirateArmor";
            }

            return base.IsArmorSet(head, body, legs);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            CritPlayer critPlayer = Main.LocalPlayer.GetModPlayer<CritPlayer>();
            CritType usedCritType = critType;

            if (item.DamageType == DamageClass.Summon && critPlayer.summonCrit != null)
            {
                usedCritType = critPlayer.summonCrit;
            }

            int index = tooltips.FindIndex(x => x.Name == "CritChance");

            if (critPlayer.slotMachineCritCrafting)
            {
                critPlayer.slotMachineItem = item;
            }

            //Add pirate armor tooltip
            if (CritRework.pirateArmorRework && !tooltips.Exists(x => x.Name == "Social"))
            {
                if (item.type == ItemID.PirateHat)
                {
                    tooltips.Insert(tooltips.FindIndex(x => x.Name == "Defense") + 1, new TooltipLine(Mod, "Tooltip", pirateHatTooltip.Value));
                }

                if (item.type == ItemID.PirateShirt)
                {
                    tooltips.Insert(tooltips.FindIndex(x => x.Name == "Defense") + 1, new TooltipLine(Mod, "Tooltip", pirateShirtTooltip.Value));
                }

                if (item.type == ItemID.PiratePants)
                {
                    tooltips.Insert(tooltips.FindIndex(x => x.Name == "Defense") + 1, new TooltipLine(Mod, "Tooltip", piratePantsTooltip.Value));
                }
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

            if (tooltips.Exists(x => x.Name == "CritChance") || usedCritType != null)
            {
                if (index != -1) tooltips.RemoveAt(index);

                if (CanHaveCrits(item) || usedCritType != null)
                {
                    if (usedCritType == null)
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

                            //Damage tooltip (slot machine)
                            if (tooltips.Exists(x => x.Name == "Damage"))
                            {
                                int i = tooltips.FindIndex(x => x.Name == "Damage");
                                List<string> words = tooltips[i].Text.Split(' ').ToList();

                                string damageExtra = Math.Round(Main.LocalPlayer.GetWeaponDamage(item, true) * CritType.CalculateActualCritMult(critPlayer.slotMachineCrit, item, Main.LocalPlayer)).ToString();

                                string colorHex = "fff88d";

                                if (CritRework.overrideCritColor)
                                {
                                    colorHex = CritRework.critColor.Hex3();
                                }

                                if (firstAmmo != null)
                                {
                                    string ammoExtra = "+" + Math.Round(Main.LocalPlayer.GetWeaponDamage(firstAmmo, true) * CritType.CalculateActualCritMult(critPlayer.slotMachineCrit, item, Main.LocalPlayer));
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
                        }
                        else
                        {
                            TooltipLine line = new TooltipLine(Mod, "NoCrit", unknownTooltip.Value);
                            line.OverrideColor = Color.Gray;

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

                            string damageExtra = Math.Round(Main.LocalPlayer.GetWeaponDamage(item, true) * CritType.CalculateActualCritMult(usedCritType, item, Main.LocalPlayer)).ToString();

                            string colorHex = "fff88d";

                            if (CritRework.overrideCritColor)
                            {
                                colorHex = CritRework.critColor.Hex3();
                            }

                            if (firstAmmo != null)
                            {
                                string ammoExtra = "+" + Math.Round(Main.LocalPlayer.GetWeaponDamage(firstAmmo, true) * CritType.CalculateActualCritMult(usedCritType, item, Main.LocalPlayer));
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

                        //string ammoExtra = firstAmmo != null ? (" + " + Math.Round(Main.LocalPlayer.GetWeaponDamage(firstAmmo, true) * CritType.CalculateActualCritMult(usedCritType, item, Main.LocalPlayer))) : "";
                        //TooltipLine line = new TooltipLine(Mod, "CritDamage", Math.Round(Main.LocalPlayer.GetWeaponDamage(item, true) * CritType.CalculateActualCritMult(usedCritType, item, Main.LocalPlayer)) + ammoExtra + " critical strike damage");

                        //tooltips.Insert(index, line);
                    }
                }
            }

            if (!CanHaveCrits(item) && item.ammo == AmmoID.None && usedCritType == null && item.damage > 0)
            {
                TooltipLine line = new TooltipLine(Mod, "NoCrit", unableTooltip.Value);
                line.OverrideColor = Color.Gray;

                tooltips.Add(line);
            }

            if (item.type == ModContent.ItemType<WiseCracker>() && usedCritType == null)
            {
                TooltipLine line = new TooltipLine(Mod, "NoCrit", unknownTooltip.Value);
                line.OverrideColor = Color.Gray;

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

            if (usedCritType != null)
            {
                TooltipLine line = new TooltipLine(Mod, "CritDescription", usedCritType.description.Value);
                line.OverrideColor = usedCritType.Color;

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
