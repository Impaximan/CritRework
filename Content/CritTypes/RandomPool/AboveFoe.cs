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

        public override void SpecialPrefixHoldItem(Item item, Player player)
        {
            if (player.controlJump)
            {
                if (player.velocity.Y > 0.1f) player.velocity.Y = 0.1f;
                player.fallStart = (int)player.position.Y;
                player.noFallDmg = true;
            }
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.position.Y + Player.height < target.position.Y;
        }
    }
}
