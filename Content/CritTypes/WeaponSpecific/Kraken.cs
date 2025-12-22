using CritRework.Common.Globals;
using System.Collections.Generic;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class Kraken : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Kraken;
        }

        public override float GetDamageMult(Player Player, Item Item) => 3f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Projectile.TryGetGlobalProjectile(out CritProjectile p) && !p.npcsHit.Contains(target);
        }
    }
}

