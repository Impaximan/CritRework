using CritRework.Common.ModPlayers;
using CritRework.Content.CritTypes.RandomPool;
using CritRework.Content.CritTypes.WeaponSpecific;
using CritRework.Content.CritTypes.WhetstoneSpecific;
using CritRework.Content.Items;
using CritRework.Content.Items.Augmentations;
using CritRework.Content.Items.Equipable.Accessories;
using CritRework.Content.Items.Whetstones;
using CritRework.Content.Prefixes.Weapon;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ModLoader.IO;
using Terraria.GameContent.UI.Chat;
using Terraria.Utilities;
using CritRework.Content.Prefixes.Augmentation;
using Microsoft.Xna.Framework.Input;

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

        public static LocalizedText prostheticTooltip;

        public static LocalizedText removeAugmentation;

        public bool forceCanCrit = false;
        public bool recoverableArrow = false;
        public bool harpoonFireAgain = false;

        public Augmentation? augmentation = null;
        public Augmentation? augmentation2 = null;

        public override bool CanStack(Item destination, Item source)
        {
            if (destination.TryGetCritType(out CritType crit1) && source.TryGetCritType(out CritType crit2))
            {
                if (crit1 != crit2)
                {
                    return false;
                }
            }

            return base.CanStack(destination, source);
        }

        public override void SetStaticDefaults()
        {
            necromanticTooltip = Mod.GetLocalization($"NecromanticTooltip");
            unknownTooltip = Mod.GetLocalization($"UnknownTooltip");
            unableTooltip = Mod.GetLocalization($"UnableTooltip");
            pirateHatTooltip = Mod.GetLocalization($"PirateHat");
            pirateShirtTooltip = Mod.GetLocalization($"PirateShirt");
            piratePantsTooltip = Mod.GetLocalization($"PiratePants");
            pirateBonus = Mod.GetLocalization($"PirateSetBonus");
            prostheticTooltip = Mod.GetLocalization($"ProstheticTooltip");
            removeAugmentation = Mod.GetLocalization($"RemoveAugmentation");
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(critType != null);
            if (critType != null)
            {
                writer.Write(critType.InternalName);
            }

            writer.Write(augmentation != null); //Does this item have an augmentation

            if (augmentation != null)
            {
                writer.Write(augmentation.Type);
                writer.Write(augmentation.Item.prefix);
                ItemIO.SendModData(augmentation.Item, writer);
            }


            writer.Write(augmentation2 != null); //Does this item have a second augmentation

            if (augmentation2 != null)
            {
                writer.Write(augmentation2.Type);
                writer.Write(augmentation2.Item.prefix);
                ItemIO.SendModData(augmentation2.Item, writer);
            }
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            if (reader.ReadBoolean()) //Has crit type
            {
                critType = CritRework.GetCrit(reader.ReadString());
            }

            if (reader.ReadBoolean()) //Has augmentation
            {
                int type = reader.ReadInt32();
                int pre = reader.ReadInt32();

                Item instance = new Item(type, 1, pre);

                ItemIO.ReceiveModData(instance, reader);

                if (instance.ModItem is Augmentation a)
                {
                    augmentation = a;
                }
            }

            if (reader.ReadBoolean()) //Has augmentation 2
            {
                int type = reader.ReadInt32();
                int pre = reader.ReadInt32();

                Item instance = new Item(type, 1, pre);

                ItemIO.ReceiveModData(instance, reader);

                if (instance.ModItem is Augmentation a)
                {
                    augmentation2 = a;
                }
            }
        }

        public override float UseSpeedMultiplier(Item item, Player player)
        {
            float mult = 1f;
            if (item.TryGetGlobalItem(out CritItem critItem))
            {
                if (player.HasEquip<SharpenedWrench>())
                {
                    if (critItem.critType != null && critItem.critType.ShowWhenActive && (item.pick > 0 || item.axe > 0 || item.hammer > 0) && critItem.critType.ShouldCrit(player, item, null, null, new NPC.HitModifiers(), item.prefix == ModContent.PrefixType<Special>()))
                    {
                        mult *= 1.3f;
                    }
                }
            }

            if (augmentation != null && PrefixLoader.GetPrefix(augmentation.Item.prefix) is AugmentationPrefix prefix)
            {
                if (AugmentationActive(item, player))
                {
                    float c = 1f;
                    float n = 1f;
                    float useTimeMult = 1f;
                    float v = 1f;
                    prefix.SetStats(ref c, ref n, ref useTimeMult, ref v);

                    mult /= useTimeMult;
                }

            }
            if (augmentation2 != null && PrefixLoader.GetPrefix(augmentation2.Item.prefix) is AugmentationPrefix prefix2)
            {
                if (AugmentationActive(item, player))
                {
                    float c = 1f;
                    float n = 1f;
                    float useTimeMult = 1f;
                    float v = 1f;
                    prefix2.SetStats(ref c, ref n, ref useTimeMult, ref v);

                    mult /= useTimeMult;
                }
            }

            return base.UseSpeedMultiplier(item, player) * mult;
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.IsSpecial(player) && critType is TitaniumTrident)
            {
                velocity *= 1.5f;
            }
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if (player.HasEquip<SharpenedWrench>() && (item.pick > 0 || item.axe > 0 || item.hammer > 0))
            {
                crit += 25;
            }
        }


        public override void SaveData(Item item, TagCompound tag)
        {
            if (critType != null) tag.Add("critType", critType.InternalName);

            if (augmentation != null)
            {
                tag.Add("augmentation", true);
                TagCompound itemSave = ItemIO.Save(augmentation.Item);

                foreach (KeyValuePair<string, object> pair in itemSave)
                {
                    tag.Add(pair.Key + "_augmentation", pair.Value);
                }
            }

            if (augmentation2 != null)
            {
                tag.Add("2augmentation", true);
                TagCompound itemSave = ItemIO.Save(augmentation2.Item);

                foreach (KeyValuePair<string, object> pair in itemSave)
                {
                    tag.Add(pair.Key + "_2augmentation", pair.Value);
                }
            }
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.ContainsKey("critType")) critType = CritRework.GetCrit(tag.GetString("critType"));

            if (tag.ContainsKey("augmentation"))
            {
                TagCompound itemTag = new();

                foreach (KeyValuePair<string, object> pair in tag.Where(x => x.Key.Contains("_augmentation")))
                {
                    itemTag.Add(pair.Key.Replace("_augmentation", ""), pair.Value);
                }

                Item aug = ItemIO.Load(itemTag);
                if (aug.ModItem is Augmentation obj)
                {
                    augmentation = obj;
                }
            }

            if (tag.ContainsKey("2augmentation"))
            {
                TagCompound itemTag = new();

                foreach (KeyValuePair<string, object> pair in tag.Where(x => x.Key.Contains("_2augmentation")))
                {
                    itemTag.Add(pair.Key.Replace("_2augmentation", ""), pair.Value);
                }

                Item aug = ItemIO.Load(itemTag);
                if (aug.ModItem is Augmentation obj)
                {
                    augmentation2 = obj;
                }
            }
        }

        int counter = 0;
        float alphaMult = 0f;
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!CritRework.showActiveCrits || item.type == ModContent.ItemType<BasicWhetstone>()) return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);

            if (item.TryGetGlobalItem(out CritItem c) && c.critType != null && c.critType.ShowWhenActive && Main.LocalPlayer.inventory.Contains(item))
            {
                Texture2D texture = ModContent.Request<Texture2D>("CritRework/Shine1").Value;

                try
                {
                    counter++;

                    float baseScale = 30f;
                    if (Main.LocalPlayer.HeldItem == item && !Main.playerInventory) baseScale = 80f;

                    if (c.critType.ShouldCrit(Main.LocalPlayer, item, null, null, new NPC.HitModifiers(), item.prefix == ModContent.PrefixType<Special>()))
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

                if (CritRework.randomHijackSound && CanHaveCrits(item))
                {
                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Hijack")
                    {
                        Volume = 0.5f
                    });
                }
            }
        }

        public override bool CanBeConsumedAsAmmo(Item ammo, Item weapon, Player player)
        {
            return base.CanBeConsumedAsAmmo(ammo, weapon, player);
        }

        public override void PreReforge(Item item)
        {
            if (augmentation != null || augmentation2 != null)
            {
                RemoveAugmentation(Main.LocalPlayer);
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

            if (Main.HoverItem.type == item.type && Main.HoverItem.GetGlobalItem<CritItem>().augmentation != null && Main.HoverItem.GetGlobalItem<CritItem>().augmentation == augmentation)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt) && Main.mouseRight)
                {
                    RemoveAugmentation(player);
                }
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

            if (item.IsSpecial(player) && item.TryGetCritType(out CritType critType))
            {
                critType.SpecialPrefixOnHitNPC(item, player, null, target, hit, damageDone);
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

            if (player.TryGetModPlayer(out CritPlayer cPlayer))
            {
                if (item.damage > 0 && item.pick == 0 && item.axe == 0 && item.hammer == 0 && cPlayer.criticalCurses.Count > 0 && !item.TryGetAugmentation<CursersQuill>(out _) && !item.TryGetAugmentation2<CursersQuill>(out _))
                {
                    cPlayer.fireCriticalCurse = true;
                }
            }

            return base.UseItem(item, player);
        }

        public override void HoldItem(Item item, Player player)
        {
            if (critType == null)
            {
                if (item.DamageType == DamageClass.Summon && player.GetModPlayer<CritPlayer>().summonSpecial)
                {
                    player.GetModPlayer<CritPlayer>().summonCrit.SpecialPrefixHoldItem(item, player);
                }
                AddCritType(item);
            }
            else if (item.IsSpecial(player))
            {
                critType.SpecialPrefixHoldItem(item, player);
            }

            if (augmentation != null)
            {
                augmentation.HoldItem(player);
            }

            if (augmentation2 != null)
            {
                augmentation2.HoldItem(player);
            }
        }

        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            if (item.IsSpecial(player) && item.TryGetCritType(out CritType c))
            {
                if (c is TipOfTheWeapon)
                {
                    scale *= 3f;
                }
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

            if (source is EntitySource_Loot lootSource)
            {
                if (item.type == ModContent.ItemType<BasicWhetstone>())
                {
                    item.GetGlobalItem<CritItem>().AddCritType(item);
                }
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

            int rerolls = 0;

            SelectCrit:

            if (item.type == ModContent.ItemType<BasicWhetstone>())
            {
                critType = Main.rand.Next(CritRework.randomCritPool);
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

            if (critType != null && (item.pick > 0 || item.axe > 0 || item.hammer > 0 || item.type == ModContent.ItemType<ShadowDonut>()))
            {
                if (!critType.ShowWhenActive)
                {
                    rerolls++;

                    if (rerolls <= 2 || item.type == ModContent.ItemType<ShadowDonut>()) //Reroll up to 2 times for a different crit
                    {
                        goto SelectCrit;
                    }
                }
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

            if (weapon.IsSpecial(player) && weapon.TryGetCritType(out Ammo crit) && Main.rand.NextBool(5))
            {
                return false;
            }

            return base.CanConsumeAmmo(weapon, ammo, player);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            if (critType == null)
            {
                AddCritType(item);
            }

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

        public bool AugmentationActive(Item item, Player player, NPC? npc = null)
        {
            if (augmentation == null)
            {
                return false;
            }

            if (augmentation != null && PrefixLoader.GetPrefix(augmentation.Item.prefix) is AugmentationPrefix prefix)
            {
                if (prefix.DeactivateAugmentation(item, augmentation.Item, player, npc))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Augmentation2Active(Item item, Player player, NPC? npc = null)
        {
            if (augmentation2 == null)
            {
                return false;
            }

            if (augmentation2 != null && PrefixLoader.GetPrefix(augmentation2.Item.prefix) is AugmentationPrefix prefix)
            {
                if (prefix.DeactivateAugmentation(item, augmentation2.Item, player, npc))
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveAugmentation(Player player)
        {
            if (augmentation != null)
            {
                player.QuickSpawnItem(new EntitySource_ItemOpen(player, augmentation.Item.type, "Remove augmentation"), augmentation.Item);
                SoundEngine.PlaySound(SoundID.Item37);
                augmentation = null;
            }

            if (augmentation2 != null)
            {
                player.QuickSpawnItem(new EntitySource_ItemOpen(player, augmentation2.Item.type, "Remove augmentation"), augmentation2.Item);
                augmentation2 = null;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<BasicWhetstone>())
            {
                return;
            }

            CritPlayer critPlayer = Main.LocalPlayer.GetModPlayer<CritPlayer>();
            CritType usedCritType = critType;

            if (item.DamageType == DamageClass.Summon && critPlayer.summonCrit != null)
            {
                usedCritType = critPlayer.summonCrit;
            }

            if (item.prefix == ModContent.PrefixType<Special>() || item.type == ModContent.ItemType<DivineShard>())
            {
                string sparkle = "[i:" + ModContent.ItemType<SparkleIcon>() + "]";

                TooltipLine name = tooltips.Find(x => x.Name == "ItemName");
                name.Text = sparkle + " " + name.Text + " " + sparkle;
                name.OverrideColor = Color.Lerp(ItemRarity.GetColor(item.rare), Color.LightGoldenrodYellow, (float)(Math.Sin(Main._drawInterfaceGameTime.TotalGameTime.TotalSeconds * 6f) + 1f) / 2f);
            }

            if (augmentation != null)
            {
                if (!CritRework.abbreviateAugmentationTooltip) tooltips.Insert(1, new TooltipLine(Mod, "Augmentation", "Hold Left Alt for more info")
                {
                    IsModifier = true
                });
                if (augmentation2 != null) tooltips.Insert(1, new TooltipLine(Mod, "Augmentation2", "Has augmentation: " + ItemTagHandler.GenerateTag(augmentation2.Item) + " " + $" [c/{ItemRarity.GetColor(augmentation2.Item.rare).Hex3()}:" + augmentation2.Item.AffixName() + "]"));
                tooltips.Insert(1, new TooltipLine(Mod, "Augmentation", "Has augmentation: " + ItemTagHandler.GenerateTag(augmentation.Item) + " " + $" [c/{ItemRarity.GetColor(augmentation.Item.rare).Hex3()}:" + augmentation.Item.AffixName() + "]"));

                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    tooltips.Clear();

                    Augmentation usedAugmentation = augmentation;

                    if (augmentation2 != null && Main.gameTimeCache.TotalGameTime.TotalSeconds % 6 <= 3f)
                    {
                        usedAugmentation = augmentation2;
                    }

                    tooltips.Add(new TooltipLine(Mod, "AugmentationName", $"[c/{ItemRarity.GetColor(item.rare).Hex3()}:Augmentation: ]" + ItemTagHandler.GenerateTag(usedAugmentation.Item) + " " + $"[c/{ItemRarity.GetColor(usedAugmentation.Item.rare).Hex3()}:" + usedAugmentation.Item.AffixName() + "]"));

                    tooltips.Add(new TooltipLine(Mod, "AugmentationCanBeAppliedTo", Mod.GetLocalization($"Items.{usedAugmentation.GetType().Name}.CanBeApplied").Value));

                    for (int i = 0; i < usedAugmentation.Item.ToolTip.Lines; i++)
                    {
                        string line = usedAugmentation.Item.ToolTip.GetLine(i);
                        tooltips.Add(new TooltipLine(Mod, "AugmentationTooltip" + i, line));
                    }

                    if (PrefixLoader.GetPrefix(usedAugmentation.Item.prefix) is AugmentationPrefix prefix)
                    {
                        tooltips.AddRange(prefix.GetTooltipLines(item));
                    }

                    tooltips.Add(new TooltipLine(Mod, "AugmentationRemoval", removeAugmentation.Value)
                    {
                        OverrideColor = Color.Yellow
                    });

                    return;
                }
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


            //Convert chance tooltips
            if (item.DamageType != DamageClass.SummonMeleeSpeed)
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
            }
            else
            {
                foreach (Tuple<string, string> conversion in CritRework.tooltipConversions)
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains(conversion.Item1, StringComparison.OrdinalIgnoreCase))
                        {
                            int n = tooltips.IndexOf(tooltip);
                            tooltips.Insert(n + 1, new TooltipLine(Mod, "TooltipTagCrit", "Bonus crits enabled by this whip have a damage mult of 2x"));
                            break;
                        }
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

            if (forceCanCrit && usedCritType == null)
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
                List<TooltipLine> lines = new();

                TooltipLine line = new TooltipLine(Mod, "CritDescription", usedCritType.description.Value);
                line.OverrideColor = usedCritType.Color;

                TooltipLine prostheticCrit = null;

                if (critPlayer.prostheticCrit != null && item.type != ModContent.ItemType<ProstheticArm>() && critPlayer.prostheticCrit != usedCritType && (item.type != ModContent.ItemType<ShadowDonut>()))
                {
                    prostheticCrit = new TooltipLine(Mod, "CritDescription2", critPlayer.prostheticCrit.description.Value);
                    prostheticCrit.OverrideColor = critPlayer.prostheticCrit.Color;

                    lines.Add(new TooltipLine(Mod, "ProstheticTooltip", prostheticTooltip.Value)
                    {
                        OverrideColor = Color.Lerp(Color.OrangeRed, Color.White, 0.5f)
                    });
                }

                lines.Add(line);

                if (prostheticCrit != null) lines.Add(prostheticCrit);

                int indexA = tooltips.FindLastIndex(x => x.Name.Contains("Tooltip"));
                int indexB = tooltips.FindIndex(x => x.Name == "Consumable");
                int indexC = tooltips.FindIndex(x => x.Name == "UseMana");
                int indexD = tooltips.FindIndex(x => x.Name == "Knockback");

                if (indexA != -1)
                {
                    tooltips.InsertRange(indexA + 1, lines);
                }
                else if (indexB != -1)
                {
                    tooltips.InsertRange(indexB + 1, lines);
                }
                else if (indexC != -1)
                {
                    tooltips.InsertRange(indexC + 1, lines);
                }
                else if (indexD != -1)
                {
                    tooltips.InsertRange(indexD + 1, lines);
                }
                else
                {
                    tooltips.AddRange(lines);
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

            if (critPlayer.summonSpecial && item.DamageType == DamageClass.Summon)
            {
                tooltips.Add(new TooltipLine(Mod, "SpecialModifier1_Summon", critPlayer.summonCrit.specialPrefixTooltip1.Value)
                {
                    IsModifier = true,
                });
                tooltips.Add(new TooltipLine(Mod, "SpecialModifier2_Summon", critPlayer.summonCrit.specialPrefixTooltip2.Value)
                {
                    IsModifier = true,
                });
            }
        }
    }
}
