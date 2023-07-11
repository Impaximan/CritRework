global using Terraria.ModLoader;
global using Terraria;
global using Terraria.ID;
global using CritRework.Common;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;

namespace CritRework
{
	public class CritRework : Mod
    {
        public static bool critSounds = true;

        public static List<CritType> loadedCritTypes = new();
        public static List<CritType> forcedCritTypes = new();
        public static List<CritType> randomCritPool = new();

        public static CritType GetCrit<T>() where T : CritType
        {
            return loadedCritTypes.Find(x => x is T);
        }

        public static CritType GetCrit(string typeAsString)
        {
            return loadedCritTypes.Find(x => x.GetType().ToString() == typeAsString);
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
            ModContent.Request<SoundEffect>("CritRework/Assets/Sounds/Crit", AssetRequestMode.ImmediateLoad);
        }
    }
}