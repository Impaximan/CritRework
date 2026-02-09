using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.CritTypes.RandomPool
{
    public class FoeIsWet : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && Main.rand.NextBool(5))
            {
                Vector2 position = target.Center + Main.rand.NextVector2Circular(150 + target.width, 150f + target.height);

                int p = Projectile.NewProjectile(new EntitySource_ItemUse(player, item), position, new Vector2(0f, -1f), ModContent.ProjectileType<WetBubble>(), 0, 0f, player.whoAmI);
                Main.projectile[p].timeLeft = 180;
                Main.projectile[p].netUpdate = true;
            }

        }

        int bubbleCounter = 0;
        static Projectile? bubble = null;
        public override void SpecialPrefixHoldItem(Item item, Player player)
        {
            if (player.whoAmI != Main.myPlayer)
            {
                return;
            }

            if (bubble != null && !bubble.active) bubble = null;

            bubbleCounter++;

            if (bubbleCounter > 280 || bubble == null)
            {
                bubbleCounter = 0;

                Vector2 position = player.Center + Main.rand.NextVector2Circular(300f, 300f);
                for (int i = 0; i < 10; i++)
                {
                    if (!Collision.CanHitLine(player.Center, 1, 1, position, 1, 1))
                    {
                        position = player.Center + Main.rand.NextVector2Circular(300f, 300f);
                    }
                    else
                    {
                        break;
                    }
                }

                position.Y += 100;

                int p = Projectile.NewProjectile(new EntitySource_ItemUse(player, item), position, new Vector2(0f, -1f), ModContent.ProjectileType<WetBubble>(), 0, 0f, player.whoAmI);
                bubble = Main.projectile[p];
            }

            if (bubble.Distance(player.Center) > 500)
            {
                bubble.timeLeft = 20;
                bubble = null;
            }
        }

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.ThunderSpear;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return target.wet || target.HasBuff(BuffID.Wet) || (Player.ZoneOverworldHeight && Main.raining);
        }
    }

    public class WetBubble : ModProjectile
    {
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 68;
            Projectile.height = 68;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
            Projectile.ai[0] = 0.1f;
            Projectile.scale = 0f;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item54, Projectile.Center);

            for (int d = 0; d < 25f; d++)
            {
                float theta = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 position = Projectile.Center + theta.ToRotationVector2() * 34;
                Vector2 velocity = theta.ToRotationVector2() * Main.rand.NextFloat(5f, 10f);
                Dust dust = Dust.NewDustPerfect(position, DustID.Water, velocity);
                dust.noGravity = true;
                dust.scale *= 2f;
            }
        }

        public override void AI()
        {
            Projectile.scale += Projectile.ai[0];

            if (Projectile.scale > 1f || (Projectile.scale <= 1f && Projectile.ai[0] < 1f))
            {
                Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 1f - Projectile.scale, 0.1f);
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.getRect().Intersects(Projectile.getRect()))
                {
                    npc.AddBuff(BuffID.Wet, 90);
                }
            }
        }
    }
}

