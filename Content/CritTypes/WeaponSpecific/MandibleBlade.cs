namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class MandibleBlade : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.AntlionClaw;
        }

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                player.immune = true;
                player.immuneTime = 30;
                player.AddBuff(BuffID.Panic, 60);
            }
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (target.velocity == Vector2.Zero) return false;

            Vector2 v = target.velocity;
            v.Normalize();

            Vector2 d = Player.Center - target.Center;
            d.Normalize();

            return v.Distance(d) <= 0.5f;
        }
    }
}
