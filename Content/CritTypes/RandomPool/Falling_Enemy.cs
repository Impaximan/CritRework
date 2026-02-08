namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FallingEnemy : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.6f;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                target.velocity.Y = -20f * target.knockBackResist;
            }
        }

        public override bool CanApplyTo(Item item)
        {
            return true;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return target.velocity.Y > 0f && !target.noGravity;
        }
    }
}
