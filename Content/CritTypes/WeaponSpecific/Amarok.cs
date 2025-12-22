using CritRework.Common.Globals;
using System.Collections.Generic;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class Amarok : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Amarok;
        }

        public override float GetDamageMult(Player Player, Item Item) => 5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Projectile.TryGetGlobalProjectile(out CritProjectile p) && p.timeSinceHit > 120;
        }
    }
}

