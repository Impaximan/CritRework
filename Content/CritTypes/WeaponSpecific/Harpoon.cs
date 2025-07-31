namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class Harpoon : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(out int itemType)
        {
            itemType = ItemID.Harpoon;
            return true;
        }

        public override float GetDamageMult(Player Player, Item Item) => 4f;

        public override string GetDescription() => "Critically strikes while the target is at least 26 tiles away";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.Distance(target.getRect().ClosestPointInRect(Player.Center)) >= 416;
        }
    }
}
