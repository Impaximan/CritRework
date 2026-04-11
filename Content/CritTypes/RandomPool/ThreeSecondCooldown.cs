using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Equipable.Accessories;

namespace CritRework.Content.CritTypes.RandomPool
{
    //No longer 3 seconds by defualt but I'm keeping the name because changing it would cause more problems than it's worth
    internal class ThreeSecondCooldown : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override bool CanApplyTo(Item item)
        {
            return item.type != ModContent.ItemType<ProstheticArm>();
        }

        public override bool ShowWhenActive => true;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceCrit >= 60 * (specialPrefix ? 3 : 5);
        }
    }
}
