namespace CritRework.Content.CritTypes.RandomPool
{
    internal class Falling : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.25f;

        public override string GetDescription() => "Critically strikes while you are falling";

        public override bool CanApplyTo(Item item)
        {
            return item.shoot == ProjectileID.None || ItemID.Sets.Spears[item.type];
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.velocity.Y > 0f;
        }
    }
}
