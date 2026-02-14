using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    public class Adaptive : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item)
        {
            if (Item.IsSpecial() && Player.GetModPlayer<CritPlayer>().timeSinceHeal < 300)
            {
                return 1.5f * Player.GetModPlayer<CritPlayer>().healPowerMult;
            }

            return 1.5f;
        }

        public override int WhetstoneItemType => ModContent.ItemType<AdaptiveWhetstone>();

        public override bool ShowWhenActive => true;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceHeal <= 300;
        }
    }
}
