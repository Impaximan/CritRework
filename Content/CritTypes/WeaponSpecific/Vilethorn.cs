namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class Vilethorn : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Vilethorn;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        //public override string GetDescription() => "Critically strikes at the tip of the thorns";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Projectile != null && Projectile.type == ProjectileID.VilethornTip;
        }
    }

    internal class CrystalVileShard : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.CrystalVileShard;
        }

        public override float GetDamageMult(Player Player, Item Item) => 2.25f;

        //public override string GetDescription() => "Critically strikes at the tip";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Projectile != null && Projectile.type == ProjectileID.CrystalVileShardHead;
        }
    }
}
