using CritRework.Common.Globals;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class DemonScytheShadowflameBow : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.DemonScythe || item.type == ItemID.ShadowFlameBow;
        }

        public override float GetDamageMult(Player Player, Item Item) => 10f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Projectile != null && Projectile.TryGetGlobalProjectile(out CritProjectile c) && c.targetsKilled >= 1;
        }
    }
}
