using CritRework.Common.Globals;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class Amarok : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Amarok;
        }

        public override float GetDamageMult(Player Player, Item Item) => 3f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile.TryGetGlobalProjectile(out CritProjectile p) && p.timeSinceHit > 60;
        }
    }
}

