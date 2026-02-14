using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Starter : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.15f;

        public override bool ShowWhenActive => true;

        public override int WhetstoneItemType => ModContent.ItemType<StarterWhetstone>();

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.statLifeMax2 < (specialPrefix ? 300 : 200);
        }
    }
}
