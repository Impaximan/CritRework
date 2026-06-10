namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class MagicHarp : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.MagicalHarp;
        }

        public override float GetDamageMult(Player Player, Item Item)
        {
            return 1.35f;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().harpCrit;
            }
            return false;
        }
    }
}
