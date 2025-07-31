namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class SpikyBall : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(out int itemType)
        {
            itemType = ItemID.SpikyBall;
            return true;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override string GetDescription() => "Critically strikes after being present for more than 5 seconds";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().timeActive >= 300;
            }
            return false;
        }
    }
}
