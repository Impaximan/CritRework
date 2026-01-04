using CritRework.Common.Globals;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Ammo : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.25f;

        //public override string GetDescription() => "Critically strikes when not consuming ammo";

        public override bool CanApplyTo(Item item)
        {
            return item.useAmmo != AmmoID.None;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return !Projectile.GetGlobalProjectile<CritProjectile>().consumedAmmo;
        }
    }
}
