namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FarFromFoe : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.2f;

        public override string GetDescription() => "Critically strikes while you are further than 40 tiles away from the target";

        public override bool CanApplyTo(Item item)
        {
            return !item.DamageType.CountsAsClass(DamageClass.Melee);
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.Distance(target.getRect().ClosestPointInRect(Player.Center)) >= 40 * 16;
        }
    }
}
