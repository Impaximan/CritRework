using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Ancient : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.4f;

        public override bool ShowWhenActive => true;

        public override int WhetstoneItemType => ModContent.ItemType<AncientWhetstone>();

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceDeath <= 60 * 60;
        }
    }
}
