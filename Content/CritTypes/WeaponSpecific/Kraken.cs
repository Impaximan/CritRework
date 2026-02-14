using CritRework.Common.Globals;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class Kraken : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Kraken;
        }

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && projectile != null)
            {
                projectile.scale += 0.2f;
            }
        }

        public override float GetDamageMult(Player Player, Item Item) => 3f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile.TryGetGlobalProjectile(out CritProjectile p) && !p.npcsHit.Contains(target);
        }
    }
}

