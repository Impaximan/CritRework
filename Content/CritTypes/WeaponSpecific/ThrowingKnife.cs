namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class ThrowingKnife : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(out int itemType)
        {
            itemType = ItemID.ThrowingKnife;
            return true;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.2f;

        public override string GetDescription() => "Critically strikes when falling";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().timeActive >= 20;
            }
            return false;
        }
    }
}
