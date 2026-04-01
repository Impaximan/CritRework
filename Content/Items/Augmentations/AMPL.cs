using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Augmentations
{
    public class AMPL : Augmentation
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.UseSound = SoundID.Item149;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override bool CanApplyTo(Item weapon)
        {
            return weapon.DamageType == DamageClass.Ranged;
        }

        public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            modifiers.DisableCrit();
            return true;
        }

        static int cooldown = 0;
        public override void HoldItem(Player player)
        {
            if (cooldown > 0)
            {
                cooldown--;
            }
        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target)
        {
            NPC.HitModifiers modifiers = new NPC.HitModifiers();
            if (cooldown <= 0 && critType.ShouldCrit(player, item, projectile, target, modifiers, item.IsSpecial()))
            {
                if (projectile != null && projectile.IsCritAugment())
                {
                    return;
                }

                cooldown = 60;
                SoundEngine.PlaySound(SoundID.Item61, player.Center);
                Projectile missile = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, item), player.Center, new Vector2(0f, -12f), ModContent.ProjectileType<AutoMissile>(), hit.SourceDamage, 0f, player.whoAmI);
                missile.ai[0] = target.whoAmI;
                missile.netUpdate = true;
                missile.SetAsAugmentCrit();
            }
        }
    }

    public class AutoMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.timeLeft = 0;
            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Rectangle frame = new Rectangle(0, texture.Height / 5 * Projectile.frame, texture.Width, texture.Height / 5);

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, new Vector2(11.5f, 4.5f) * 2f, Projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glowTexture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, new Vector2(11.5f, 4.5f) * 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust dust = Dust.NewDustPerfect(Projectile.Center - Projectile.rotation.ToRotationVector2() * 6f, DustID.Torch);
            dust.velocity = Projectile.rotation.ToRotationVector2() * -10f + Projectile.velocity;
            dust.noGravity = true;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 1)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }
            }

            NPC target = null;

            if (Projectile.ai[0] != 0)
            {
                target = Main.npc[(int)Projectile.ai[0]];
            }
            else
            {
                target = Projectile.FindTargetWithinRange(1000f);
            }

            if (target != null)
            {
                if (Projectile.timeLeft < 280) Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(target.Center).ToRotation(), MathHelper.ToRadians(15f)).ToRotationVector2() * Projectile.velocity.Length();

                if (!target.active)
                {
                    Projectile.ai[0] = 0;
                }
            }
            else
            {
                Projectile.timeLeft = 0;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile explosion = Projectile.NewProjectileDirect(new EntitySource_Parent(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AutoMissileExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                explosion.netUpdate = true;
            }
        }
    }

    class AutoMissileExplosion : ModProjectile
    {
        public override string Texture => "CritRework/Content/Items/Augmentations/AutoMissile";

        public override void SetDefaults()
        {
            Projectile.width = 250;
            Projectile.height = 250;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            for (int i = 0; i < 100; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                dust.velocity = (dust.position - Projectile.Center) / 30f;
                dust.noGravity = true;
                dust.scale = 1.5f;
            }

            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
        }
    }
}
