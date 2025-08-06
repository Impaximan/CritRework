namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class ShadowflameKnife : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.ShadowFlameKnife;
        }

        public override float GetDamageMult(Player Player, Item Item) => 2.5f;

        //public override string GetDescription() => "Critically strikes after ricocheting off at least one target";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().targetsHit >= 1;
            }
            return false;
        }
    }
}
