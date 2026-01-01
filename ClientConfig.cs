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

        [DefaultValue(true)]
        public bool showActiveCrits = false;

        [DefaultValue(false)]
        public bool overrideCritColor = false;

        [ColorHSLSlider]
        public Color critColor = Color.White;

        [DefaultValue(true)]
        public bool critSoundEffect = true;

        [DefaultValue(false)]
        public bool hideGloveDescription = false;

        public override void OnChanged()
        {
            CritRework.showActiveCrits = showActiveCrits;
            CritRework.critColor = critColor;
            CritRework.overrideCritColor = overrideCritColor;
            CritRework.critSounds = critSoundEffect;
            CritRework.hideGloveDescription = hideGloveDescription;
        }
    }
}
