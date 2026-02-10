using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CritRework
{
    internal class ServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header($"BalanceHeader")]

        [DefaultValue("Balanced")]
        [OptionStrings(
        [
            "Balanced",
            "Simple"
        ])]
        public string critScaling = "Balanced";

        [DefaultValue(1f)]
        [Increment(0.05f)]
        [Range(0f, 2f)]
        public float critPower = 1f;

        [DefaultValue(1f)]
        [Increment(0.1f)]
        [Range(1f, 2f)]
        public float bossLife = 1f;

        [DefaultValue(1f)]
        [Increment(0.1f)]
        [Range(1f, 2f)]
        public float enemyLife = 1f;

        [DefaultValue(0.25f)]
        [Increment(0.01f)]
        [Range(0f, 1f)]
        public float randomHijackChance = 0.25f;

        [DefaultValue(true)]
        public bool randomHijackSound = true;

        [Header($"ContentHeader")]

        [DefaultValue(true)]
        public bool pirateHijack = true;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool pirateArmorRework = true;

        [DefaultValue(true)]
        public bool travellingMerchantWhetstones = true;

        [DefaultValue("Ranged")]
        [OptionStrings(
        [
            "Ranged",
            "Throwing",
            "Rogue (CALAMITY ONLY)"
        ])]
        [ReloadRequired]
        public string gloveDamageType = "Ranged";

        public override void OnChanged()
        {
            CritRework.critPower = critPower;
            CritRework.critScaling = critScaling;
            CritRework.bossLife = bossLife;
            CritRework.enemyLife = enemyLife;
            CritRework.randomHijackChance = randomHijackChance;

            CritRework.pirateHijack = pirateHijack;
            CritRework.randomHijackSound = randomHijackSound;
            CritRework.pirateArmorRework = pirateArmorRework;

            CritRework.travellingMerchantWhetstones = travellingMerchantWhetstones;

            switch (gloveDamageType)
            {
                default:
                    CritRework.gloveDamageClass = DamageClass.Ranged;
                    break;
                case "Ranged":
                    CritRework.gloveDamageClass = DamageClass.Ranged;
                    break;
                case "Throwing":
                    CritRework.gloveDamageClass = DamageClass.Throwing;
                    break;
                case "Rogue (CALAMITY ONLY)":
                    if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                    {
                        CritRework.gloveDamageClass = calamity.Find<DamageClass>("RogueDamageClass");
                    }
                    else
                    {
                        CritRework.gloveDamageClass = DamageClass.Ranged;
                    }
                    break;
            }
        }
    }
}
