global using Terraria.ModLoader;
global using Terraria;
global using Terraria.Localization;
global using Terraria.ID;
global using CritRework.Common;
global using System;
global using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using CritRework.Content.CritTypes;
using CritRework.Common.ModPlayers;
using CritRework.Content.Prefixes.Weapon;
using CritRework.Common.Globals;
using CritRework.Content.Items.Equipable.Accessories;
using CritRework.Content.Items.Equipable.Accessories.Crackers;
using CritRework.Content.Items.Augmentations;
using Terraria.Audio;

namespace CritRework
{
    public partial class CritRework : Mod
    {
        public static bool critSounds = true;

        public static List<CritType> loadedCritTypes = new();
        public static List<CritType> randomCritPool = new();

        public static bool overrideCritColor = false;
        public static Color critColor = Color.White;

        public static bool abbreviateWhetstoneTooltip = false;
        public static bool abbreviateAugmentationTooltip = false;
        public static float critPower = 1f;
        public static float randomHijackChance = 0.25f;
        public static bool randomHijackSound = true;
        public static bool pirateHijack = true;
        public static bool pirateArmorRework = true;
        public static bool travellingMerchantWhetstones = true;
        public static bool showActiveCrits = true;
        public static bool hideGloveDescription = true;

        public static string critScaling = "Balanced";

        public static DamageClass gloveDamageClass = DamageClass.Ranged;

        public static CritRework instance;

        public static float bossLife = 1f;
        public static float enemyLife = 1f;


        public static CritType GetCrit<T>() where T : CritType
        {
            return loadedCritTypes.Find(x => x is T);
        }

        public static CritType GetCrit(string typeAsString)
        {
            return loadedCritTypes.Find(x => x.InternalName == typeAsString) ?? loadedCritTypes.Find(x => x.GetType().ToString() == typeAsString);
        }

        public static List<Tuple<string, string>> tooltipConversions = new()
        {
            new("critical strike chance", "critical strike power"),
            new("crit chance", "crit power"),
            new("critical chance", "critical power"),
            new("chance to inflict crit", "power for critical strikes")
        };

        public override void Load()
        {
            instance = this;

            ModContent.Request<SoundEffect>("CritRework/Assets/Sounds/Crit", AssetRequestMode.ImmediateLoad);

            Detours.Load();
        }

        public override void Unload()
        {
            Detours.Unload();
        }

        public override object Call(params object[] args)
        {
            if (args[0] is string call)
            {
                switch (call)
                {
                    case "AddCritType":
                        try
                        {
                            ModCalledCritType critType = new ModCalledCritType((Mod)args[1], //Mod
                                (bool)args[2], //In random pool
                                (Func<Player, Item, Projectile, NPC, NPC.HitModifiers, bool>)args[3], //Should apply
                                (Func<Player, Item, float>)args[4], //Damage mult function
                                (Func<Item, bool>)args[5],//Force on item
                                (Func<Item, bool>)args[6], //Whether or not the crit condition can apply to an item
                                (LocalizedText)args[7], //Description
                                (string)args[8]); //Internal name (to be found with GetCrit(string))

                            loadedCritTypes.Add(critType);

                            if (critType.InRandomPool)
                            {
                                randomCritPool.Add(critType);
                            }
                        }
                        catch
                        {
                            throw new Exception($"Improper arguments used for mod call. See steam workshop discussion for Critical Strikes Overhaul on cross-compatibility for info on how to use.");
                        }
                        break;
                    default:
                        return base.Call(args);
                }
            }

            return base.Call(args);
        }

        //EXAMPLE CUSTOM CRIT TYPE VIA MOD CALL

        //public override void PostSetupContent()
        //{
        //    if (ModLoader.TryGetMod("CritRework", out Mod critMod))
        //    {
        //        critMod.Call("AddCritType", //Neccessary argument to indicate what call you're actually using
        //            this, //The instance of your mod
        //            true, //Whether or not the crit type can appear randomly on items. In this case, it can
        //            (Player player, Item item, Projectile projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix) => // A function determining whether or not a given hit should critically strike on the enemy.
        //            {
        //                return Main.rand.NextFloat() <= 0.25f + 0.25f * player.luck; //In this example, the crit will happen 25% of the time with default luck
        //            },
        //            (Player player, Item item) => 1.4f, //The crit multiplier of this crit condition. In this case, 1.4x
        //            (Item item) => item.type == ItemID.WoodenSword, //Whether this crit condition should force itself onto a given item. In this case, wooden swords will always have the crit type.
        //            (Item item) => item.type % 2 == 0, //Whether or not this crit condition can be applied to a given item. In this case, the crit type can only appear on even-numbered item types.
        //            GetLocalization($"ExampleCritDescription"), //The localization key for the crit condition's description (ie what indicates how you get the crit)
        //            "ExampleCrit"); //Lastly, the crit's internal name. Used for saving, loading, multiplayer syncing etc. Also what you (or another modder) would use to access this CritType.
        //    }
        //}
    }

    public static class Extensions
    {
        public static void AddPotency(this Player player, float amount)
        {
            if (player.TryGetModPlayer(out CritPlayer critPlayer))
            {
                critPlayer.potency += amount;
            }
        }

        public static float GetPotency(this Player player, Item weapon)
        {
            if (player.TryGetModPlayer(out CritPlayer critPlayer))
            {
                return critPlayer.potency * weapon.GetPotency(player);
            }
            return 1f;
        }

        public static float GetPotency(this Item item, Player player)
        {
            if (item.TryGetGlobalItem(out CritItem critItem))
            {
                return critItem.Potency(item, player);
            }
            return 1f;
        }

        public static void DoManaRechargeEffect(this Player player)
        {
            if (player.whoAmI == Main.myPlayer) SoundEngine.PlaySound(SoundID.MaxMana);
            for (int i = 0; i < 5; i++)
            {
                int num3 = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                Main.dust[num3].noLight = true;
                Main.dust[num3].noGravity = true;
                Dust obj = Main.dust[num3];
                obj.velocity *= 0.5f;
            }
        }

        public static void SetAsAugmentCrit(this Projectile projectile)
        {
            if (projectile.TryGetGlobalProjectile(out CritProjectile c))
            {
                c.critAugment = true;
            }
        }

        public static bool IsCritAugment(this Projectile projectile)
        {
            if (projectile != null && projectile.TryGetGlobalProjectile(out CritProjectile c) && c.critAugment)
            {
                return true;
            }
            return false;
        }

        public static int MaxAugmentations(this Item item, Player player)
        {
            if (item.TryGetGlobalItem(out CritItem critItem))
            {
                return critItem.MaxAugmentations(item, player);
            }

            return 0;
        }

        public static bool TryGetAugmentation<T>(this Item item, out T augmentation) where T : Augmentation
        {
            if (item.TryGetGlobalItem(out CritItem critItem) && critItem.TryGetAugmentation(out T a))
            {
                augmentation = a;
                return true;
            }

            augmentation = null;
            return false;
        }

        public static bool IsCracker(this Item item)
        {
            return new List<int>()
            {
                ModContent.ItemType<WiseCracker>(),
                ModContent.ItemType<Beautificracker>(),
                ModContent.ItemType<Deificracker>(),
            }.Contains(item.type);
        }

        public static bool IsGrapple(this Item item)
        {
            return Main.projHook[item.type];
        }

        public static void AddEquip<T>(this Player player) where T : ModItem
        {
            if (player.TryGetModPlayer(out CritPlayer cPlayer))
            {
                cPlayer.accessoryEffects.Add(typeof(T).Name);
            }
        }

        public static bool HasEquip<T>(this Player player) where T : ModItem
        {
            if (player.TryGetModPlayer(out CritPlayer cPlayer))
            {
                return cPlayer.accessoryEffects.Contains(typeof(T).Name);
            }

            return false;
        }

        public static void AddConsecutiveCritDamage(this Player player, float bonus)
        {
            player.GetModPlayer<CritPlayer>().consecutiveCriticalStrikeDamage += bonus;
        }

        public static bool IsVersatile(this Item item)
        {
            return item.prefix == ModContent.PrefixType<Versatile>();
        }

        public static bool IsSpecial(this Item item, Player? owner = null)
        {
            return item.prefix == ModContent.PrefixType<Special>() || (owner != null && item.DamageType == DamageClass.Summon && owner.TryGetModPlayer(out CritPlayer critPlayer) && critPlayer.summonSpecial);
        }

        public static CritType? GetCritType(this Item item)
        {
            if (item.TryGetGlobalItem(out CritItem c))
            {
                return c.critType;
            }
            return null;
        }

        public static bool TryGetCritType<T>(this Item item, out T critType) where T : CritType
        {
            CritType c = GetCritType(item);
            critType = null;

            if (c != null && c is T var)
            {
                critType = var;
                return true;
            }
            return false;
        }
    }
}