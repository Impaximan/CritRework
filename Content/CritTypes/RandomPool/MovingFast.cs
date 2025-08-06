namespace CritRework.Content.CritTypes.RandomPool
{
    internal class MovingFast : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.25f;

        //public override string GetDescription() => "Critically strikes while you are moving faster than 40 miles per hour";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.velocity.Length() >= 8f;
        }
    }
}
