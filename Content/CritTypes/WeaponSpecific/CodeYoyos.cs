using CritRework.Common.Globals;
using System.Collections.Generic;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class Code1 : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Code1;
        }

        public override float GetDamageMult(Player Player, Item Item) => 2.5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile.TryGetGlobalProjectile(out CritProjectile p) && p.targetsHit >= 10;
        }
    }

    public class Code2 : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Code2;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile.TryGetGlobalProjectile(out CritProjectile p) && p.targetsHit >= 20;
        }
    }
}

