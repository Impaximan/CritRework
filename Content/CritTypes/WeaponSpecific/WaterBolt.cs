namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class WaterBolt : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.WaterBolt;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().wallBounces >= 1;
            }
            return false;
        }
    }
}
