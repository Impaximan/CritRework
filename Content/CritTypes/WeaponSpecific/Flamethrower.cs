using System.Collections.Generic;
using Terraria.DataStructures;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class Flamethrower : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Flamethrower || item.type == ItemID.ElfMelter;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0)
            {
                for (int i = 0; i < Main.rand.Next(20, 30); i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(60f, 60f);
                    int p = Projectile.NewProjectile(new EntitySource_OnHit(player, target), target.Center, velocity, ProjectileID.SlimeGun, hit.SourceDamage, 0f, player.whoAmI);
                }
            }

            if (projectile != null && hit.Crit)
            {
                projectile.penetrate = -1;
                if (projectile.velocity.Length() <= 50f) projectile.velocity *= 1.25f;
            }
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            List<int> countedBuffs = new()
            {
                BuffID.Slimed,
                320,
                BuffID.Oiled
            };

            foreach (int id in countedBuffs)
            {
                if (target.HasBuff(id)) return true;
            }

            return false;
        }
    }
}

