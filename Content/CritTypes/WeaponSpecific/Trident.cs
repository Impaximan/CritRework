using CritRework.Common.Globals;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class Trident : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Trident;
        }

        public override float GetDamageMult(Player Player, Item Item) => 3f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile != null && Projectile.TryGetGlobalProjectile(out CritProjectile c) && c.targetsHit >= 2;
        }
    }

    internal class TitaniumTrident : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.TitaniumTrident || item.type == ItemID.AdamantiteGlaive;
        }

        public override float GetDamageMult(Player Player, Item Item) => 5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile != null && Projectile.TryGetGlobalProjectile(out CritProjectile c) && (c.npcsHit.Count >= 2 || (c.npcsHit.Count == 1 && !c.npcsHit.Contains(target)));
        }
    }
}
