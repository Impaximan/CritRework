global using Terraria.ModLoader;
global using Terraria;
global using Terraria.Localization;
global using Terraria.ID;
global using CritRework.Common;
global using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using CritRework.Content.CritTypes;
using CritRework.Common.ModPlayers;
using CritRework.Content.Prefixes.Weapon;
using CritRework.Common.Globals;
using System.IO;

namespace CritRework
{
	public class CritRework : Mod
    {
        public static bool critSounds = true;

        public static List<CritType> loadedCritTypes = new();
        public static List<CritType> randomCritPool = new();

        public static bool overrideCritColor = false;
        public static Color critColor = Color.White;

        public static float critPower = 1f;
        public static float randomHijackChance = 0.25f;
        public static bool randomHijackSound = true;
        public static bool pirateHijack = true;
        public static bool pirateArmorRework = true;
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
            new("critical strike chance", "critical strike damage"),
            new("crit chance", "crit damage"),
            new("critical chance", "critical damage"),
            new("chance to inflict crit", "damage for critical strikes")
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

        public static bool IsSpecial(this Item item)
        {
            return item.prefix == ModContent.PrefixType<Special>();
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