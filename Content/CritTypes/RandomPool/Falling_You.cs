namespace CritRework.Content.CritTypes.RandomPool
{
    internal class Falling : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item)
        {
            if (Item.IsSpecial() && Player.velocity.Y > 0)
            {
                int fall = Player.fallStart;
                if (Player.fallStart2 < Player.fallStart) fall = Player.fallStart2;
                return 1.25f + 0.01f * (Player.position.Y - fall * 16);
            }

            return 1.25f;
        }

        public override bool ShowWhenActive => true;

        public override bool CanApplyTo(Item item)
        {
            return (item.shoot == ProjectileID.None || ItemID.Sets.Spears[item.type]) && item.DamageType == DamageClass.Melee && !item.accessory;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.velocity.Y > 0f;
        }
    }
}
