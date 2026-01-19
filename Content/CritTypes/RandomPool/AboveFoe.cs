namespace CritRework.Content.CritTypes.RandomPool
{
    internal class AboveFoe : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.15f;

        public override bool CanApplyTo(Item item)
        {
            return item.shoot != ProjectileID.None && item.rare <= ItemRarityID.Orange;
        }

        //public override string GetDescription() => "Critically strikes while you are above the target";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.position.Y + Player.height < target.position.Y;
        }
    }
}
