namespace CritRework.Content.CritTypes.RandomPool
{
    internal class MovingFast : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.25f;

        public override bool ShowWhenActive => true;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.velocity.Length() >= 12f;
        }
    }
}
