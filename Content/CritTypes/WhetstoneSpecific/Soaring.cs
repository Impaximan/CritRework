using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    public class Soaring : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.65f;

        public override void SpecialPrefixHoldItem(Item item, Player player)
        {
            player.AddBuff(BuffID.Featherfall, 2);
        }

        public override bool ShowWhenActive => true;

        public override int WhetstoneItemType => ModContent.ItemType<SoaringWhetstone>();

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.GetModPlayer<CritPlayer>().timeFalling >= 60 * 4;
        }
    }
}
