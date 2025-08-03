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

        public override void OnChanged()
        {
            CritRework.critPower = critPower;
        }
    }
}
