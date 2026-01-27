using CritRework.Common.Globals;
using System.Collections.Generic;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class Chik : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Chik;
        }

        public override float GetDamageMult(Player Player, Item Item) => 2f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile.TryGetGlobalProjectile(out CritProjectile p) && (p.npcsHit.Count >= 2 || (p.npcsHit.Count == 1 && !p.npcsHit.Contains(target)));
        }
    }
}

