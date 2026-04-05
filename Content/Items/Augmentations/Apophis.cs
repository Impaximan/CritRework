using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Augmentations
{
    public class Apophis : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 20)
                .AddIngredient(ItemID.HellstoneBar, 5)
                .AddIngredient(ItemID.FallenStar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 48;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item119;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public const int playerDamage = 35;

        static int cooldown = 0;
        public override void HoldItem(Player player)
        {
            if (cooldown > 0)
            {
                cooldown--;
            }
        }

        public override void AugmentationOnHitNPC(Player Player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target)
        {
            if (hit.Crit && (projectile == null || projectile.type != ModContent.ProjectileType<ApophisAsteroid>()) && cooldown <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item88, target.Center);
                cooldown = 30;

                int damage = (int)(hit.SourceDamage * 2.5f);
                if (item.useTime < cooldown)
                {
                    damage = (int)(damage * (float)cooldown / item.useTime);
                }

                if (item.useTime > cooldown * 2)
                {
                    damage = (int)(damage * (float)(cooldown * 2f) / item.useTime);
                }

                int num = Main.rand.Next(1, 3);

                if (Item.prefix == ModContent.PrefixType<Apocalyptic>())
                {
                    num++;
                }

                for (int i = 0; i < num; i++)
                {
                    Projectile p = Projectile.NewProjectileDirect(new EntitySource_ItemUse(Player, Item), Player.Center + new Vector2(Main.rand.Next(-1000, 1000) + Player.velocity.X * 35, -Main.rand.Next(600, 1000)), new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(10, 15)), ModContent.ProjectileType<ApophisAsteroid>(), damage, 10f, Player.whoAmI);
                    p.ai[0] = target.position.Y;
                    p.ai[1] = Main.rand.NextFloat(-30f, 30f);
                    p.netUpdate = true;
                }
            }
        }
    }

    public class Apocalyptic : SpecialAugmentationPrefix<Apophis>
    {
        public override void AddExtraTooltipLines(Item item, ref List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ModfierApocalyptic", "• " + tooltip.Value)
            {
                IsModifier = true,
                OverrideColor = new Color(102, 166, 226)
            });
        }
    }

    public class ApophisAsteroid : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D texture = ModContent.Request<Texture2D>(Texture + "Trail").Value;
            Vector2 drawOrigin = new(texture.Width * 0.65f, texture.Height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                Color color = Projectile.GetAlpha(Color.Lerp(Color.White, Color.Blue, 1f - ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length))) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(), color * (1f - Projectile.alpha / 255f), Projectile.velocity.ToRotation(), drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            return true;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= (float)Apophis.playerDamage / Projectile.damage;
            modifiers.SetMaxDamage(target.statLifeMax2 / 2);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.penetrate--;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Default;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.alpha = 255;
            Projectile.ai[2] = -1;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
            }

            Projectile.tileCollide = Projectile.Bottom.Y > Projectile.ai[0];
            Projectile.rotation += MathHelper.Pi / Projectile.ai[1];
        }


        public override bool CanHitPlayer(Player target)
        {
            if (Projectile.alpha > 0)
            {
                return false;
            }
            return base.CanHitPlayer(target);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[2] = target.whoAmI;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile explosion = Projectile.NewProjectileDirect(new EntitySource_Parent(Projectile), Projectile.Center, Projectile.velocity / 10f, ModContent.ProjectileType<TheExplosionThatKilledTheDinosaurs>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            explosion.ai[0] = Projectile.ai[2];
        }
    }

    class TheExplosionThatKilledTheDinosaurs : ModProjectile
    {
        public override string Texture => "CritRework/nothing";

        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.DamageType = DamageClass.Default;
            Projectile.timeLeft = 5;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= (float)Apophis.playerDamage / Projectile.damage;
            modifiers.SetMaxDamage(target.statLifeMax2 / 3);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)Projectile.ai[0])
            {
                return false;
            }

            return base.CanHitNPC(target);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            List<int> resistantEnemies = new()
            {
                NPCID.TheDestroyer,
                NPCID.TheDestroyerBody,
                NPCID.TheDestroyerTail,
            };

            if (resistantEnemies.Contains(target.type))
            {
                modifiers.SourceDamage /= 12f;
            }
        }

        int timeActive = 0;
        public override void AI()
        {
            if (timeActive == 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch);
                    dust.velocity = (dust.position - Projectile.Center) / 30f;
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                    dust.noLight = true;
                }

                SoundStyle sound = SoundID.Item89;
                sound.MaxInstances = -1;
                sound.PitchVariance = 0.5f;

                SoundEngine.PlaySound(sound, Projectile.Center);
            }

            timeActive++;
        }
    }
}
