using CritRework.Common.Globals;
using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Ammo : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.25f;

        public override int WhetstoneItemType => ModContent.ItemType<AmmoWhetstone>();

        public override bool CanApplyTo(Item item)
        {
            return item.useAmmo != AmmoID.None;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return !Projectile.GetGlobalProjectile<CritProjectile>().consumedAmmo;
        }
    }
}
