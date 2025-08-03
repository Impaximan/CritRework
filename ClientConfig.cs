using System.ComponentModel;
using Terraria.ModLoader.Config;
using Microsoft.Xna.Framework;
using System.IO;

namespace CritRework
{
    internal class ClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header($"AestheticHeader")]

        [DefaultValue(false)]
        public bool overrideCritColor = false;

        [ColorHSLSlider]
        public Color critColor = Color.White;

        [DefaultValue(true)]
        public bool critSoundEffect = true;


        public override void OnChanged()
        {
            CritRework.critColor = critColor;
            CritRework.overrideCritColor = overrideCritColor;
            CritRework.critSounds = critSoundEffect;
        }
    }
}
