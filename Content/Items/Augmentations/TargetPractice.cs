using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Augmentations
{
    public class TargetPractice : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 10)
                .AddIngredient<BronzeAlloy>(5)
                .AddIngredient(ItemID.MeteoriteBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.UseSound = equipSound;
        }

        public override bool CanApplyTo(Item weapon)
        {
            return ItemID.Sets.Yoyo[weapon.type];
        }

        public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return true;
        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            if (critCondition && (projectile == null || projectile.type != ModContent.ProjectileType<Thorn>()) && ! projectile.IsCritAugment())
            {
                Vector2 position = target.Center + Main.rand.NextVector2Circular(300 + target.width, 300 + target.height);

                int num = 0;
                while (Collision.SolidCollision(position - new Vector2(22), 44, 44) || position.Distance(target.Center) < 100f - num * 10)
                {
                    position = target.Center + Main.rand.NextVector2Circular(300 + target.width, 300 + target.height);

                    num++;

                    if (num > 50)
                    {
                        break;
                    }
                }

                Projectile t = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, item), position, Vector2.Zero, ModContent.ProjectileType<TargetPracticeTarget>(), hit.SourceDamage, 0f, player.whoAmI);
                t.SetAsAugmentCrit();

                if (Item.prefix == ModContent.PrefixType<Rapid>())
                {
                    t.extraUpdates = 1;
                }
            }
        }
    }

    public class Rapid : SpecialAugmentationPrefix<TargetPractice>
    {
        public override void AddExtraTooltipLines(Item item, ref List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "PrefixRapid", tooltip.Value)
            {
                IsModifier = true,
            });
        }

        public override void ModifyValue(ref float valueMult)
        {
            base.ModifyValue(ref valueMult);

            valueMult *= 1.1f;
        }
    }

    public class TargetPracticeTarget : ModProjectile
    {
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.75f;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 600;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 44;
            Projectile.height = 44;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item82, Projectile.Center);
            Projectile.ai[0] = 0.1f;
            Projectile.scale = 0f;
        }

        public override void OnKill(int timeLeft)
        {
            for (int d = 0; d < 25f; d++)
            {
                float theta = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 position = Projectile.Center + theta.ToRotationVector2() * 22;
                Vector2 velocity = theta.ToRotationVector2() * 5f;
                Dust dust = Dust.NewDustPerfect(position, DustID.RedTorch, velocity);
                dust.noGravity = true;
                dust.scale *= 1f;
            }
        }

        public override void AI()
        {
            Projectile.scale += Projectile.ai[0];

            if (Projectile.scale > 1f || (Projectile.scale <= 1f && Projectile.ai[0] < 1f))
            {
                Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 1f - Projectile.scale, 0.2f);
            }

            foreach (Projectile yoyo in Main.projectile)
            {
                if (yoyo.type != Projectile.type && yoyo.active && yoyo.TryGetGlobalProjectile(out CritProjectile critProjectile) && critProjectile.ogItem != null && ItemID.Sets.Yoyo[critProjectile.ogItem.type] && Main.player[yoyo.owner].heldProj == yoyo.whoAmI)
                {
                    if (yoyo.getRect().Intersects(Projectile.getRect()))
                    {
                        Projectile.active = false;

                        SoundStyle sound = SoundID.Item71;
                        sound.Pitch = 0.5f;
                        sound.PitchVariance = 0.25f;
                        sound.MaxInstances = 0;

                        SoundEngine.PlaySound(sound, Projectile.Center);

                        Projectile saw = Projectile.NewProjectileDirect(new EntitySource_Parent(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TargetSaw>(), Projectile.damage / 4, 0f, Projectile.owner);
                        saw.SetAsAugmentCrit();
                        saw.ai[0] = yoyo.whoAmI;
                        saw.extraUpdates = Projectile.extraUpdates;

                        if (saw.extraUpdates > 0)
                        {
                            saw.penetrate /= 2;
                        }

                        yoyo.usesLocalNPCImmunity = true;

                        return;
                    }
                }
            }
        }
    }

    public class TargetSaw : ModProjectile
    {
        const int rotationTime = 30;

        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = rotationTime / 2;
            Projectile.penetrate = 8;
        }

        public override void OnKill(int timeLeft)
        {
            for (int d = 0; d < 10f; d++)
            {
                float theta = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 position = Projectile.Center + theta.ToRotationVector2() * 11;
                Vector2 velocity = theta.ToRotationVector2() * 5f;
                Dust dust = Dust.NewDustPerfect(position, DustID.RedTorch, velocity);
                dust.noGravity = true;
                dust.scale *= 1f;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[1] = Main.rand.NextFloat(30f, 100f);
            Projectile.ai[2] = Main.rand.NextFloat(MathHelper.TwoPi);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Vector2 drawOrigin = new(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Lerp(Color.White, Color.Crimson, 1f - ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length))) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void AI()
        {
            Projectile yoyo = Main.projectile[(int)Projectile.ai[0]];

            if (!yoyo.active || yoyo.timeLeft <= 0)
            {
                Projectile.timeLeft = 0;
                return;
            }

            Projectile.direction = (int)Projectile.ai[1] % 2 == 0 ? 1 : -1;

            Projectile.ai[2] += MathHelper.TwoPi * Projectile.direction / rotationTime;
            Projectile.Center = yoyo.Center + Projectile.ai[2].ToRotationVector2() * Projectile.ai[1];

            Projectile.rotation += MathHelper.TwoPi / rotationTime * 5f;
        }
    }
}
