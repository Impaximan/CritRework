namespace CritRework.Content.CritTypes.RandomPool
{
    internal class BelowFoe : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item)
        {
            if (Item.shoot == ProjectileID.None)
            {
                return 1.5f;
            }
            else
            {
                return 1.15f;
            }
        }

        public override bool CanApplyTo(Item item)
        {
            return item.OriginalRarity <= ItemRarityID.Orange;
        }

        public override void SpecialPrefixHoldItem(Item item, Player player)
        {
            if (player.controlDown && player.velocity.Y != 0 && player.jump == 0)
            {
                player.velocity.Y = player.maxFallSpeed;
            }

            player.noFallDmg = true;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.position.Y > target.position.Y + target.height;
        }
    }
}
