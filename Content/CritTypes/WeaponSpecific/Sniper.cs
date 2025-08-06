namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class Sniper : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.SniperRifle;
        }

        public override float GetDamageMult(Player Player, Item Item) => 6f;

        //public override string GetDescription() => "Critically strikes while the target is at least 75 tiles away";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.Distance(target.getRect().ClosestPointInRect(Player.Center)) >= 1200;
        }
    }
}
