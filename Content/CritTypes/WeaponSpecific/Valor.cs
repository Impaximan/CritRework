namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class Valor : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Valor;
        }

        public override float GetDamageMult(Player Player, Item Item) => 3f;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                player.immune = true;
                player.immuneTime = 60;
            }
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile.ai[0] == -1f;
        }
    }
}

