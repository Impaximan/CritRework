using System.ComponentModel;
using Terraria.ModLoader.Config;
using Microsoft.Xna.Framework;
using System.IO;

namespace CritRework
{
    internal class ServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header($"BalanceHeader")]

        [DefaultValue(1f)]
        [Increment(0.05f)]
        [Range(0f, 2f)]
        public float critPower = 1f;

        [DefaultValue(0.25f)]
        [Increment(0.01f)]
        [Range(0f, 1f)]
        public float randomHijackChance = 0.25f;

        [DefaultValue(true)]
        public bool randomHijackSound = true;

        [Header($"ContentHeader")]

        [DefaultValue(true)]
        public bool pirateHijack = true;

        public override void OnChanged()
        {
            CritRework.critPower = critPower;
            CritRework.randomHijackChance = randomHijackChance;
            CritRework.pirateHijack = pirateHijack;
            CritRework.randomHijackSound = randomHijackSound;
        }
    }
}
