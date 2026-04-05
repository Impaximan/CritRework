using CritRework.Common.ModPlayers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Augmentations
{
    public class CursersQuill : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 30)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 34;
            Item.UseSound = equipSound;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            return true;
        }

        static float counter = 0f;
        public int max = 10;

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target)
        {
            NPC.HitModifiers modifiers = new NPC.HitModifiers();

            max = 10;
            if (Item.prefix == ModContent.PrefixType<Hoarding>())
            {
                max = 15;
            }

            if (player.GetModPlayer<CritPlayer>().ShouldNormallyCrit(item, projectile, new NPC.HitModifiers(), critType, target) && (projectile == null || projectile.type != ModContent.ProjectileType<CriticalCurse>()))
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<CriticalCurse>()] < max)
                {
                    int p = Projectile.NewProjectile(new EntitySource_ItemUse(player, item), target.Center, Vector2.Zero, ModContent.ProjectileType<CriticalCurse>(), hit.SourceDamage, 2f, player.whoAmI);
                    Main.projectile[p].ai[1] = Main.rand.NextFloat(100f);
                    Main.projectile[p].netUpdate = true;
                    Main.projectile[p].DamageType = hit.DamageType;
                    Main.projectile[p].SetAsAugmentCrit();

                    player.GetModPlayer<CritPlayer>().criticalCurses.Add(Main.projectile[p]);
                    SoundEngine.PlaySound(SoundID.NPCHit36, target.Center);

                    bool maxed = player.ownedProjectileCounts[ModContent.ProjectileType<CriticalCurse>()] + 1 == max;
                    CombatText.NewText(target.getRect(), maxed ? Color.LightCyan : Color.DarkCyan, player.ownedProjectileCounts[ModContent.ProjectileType<CriticalCurse>()] + 1, maxed);
                    if (maxed) player.DoManaRechargeEffect();
                }
            }
        }
    }

    public class Hoarding : SpecialAugmentationPrefix<CursersQuill>
    {
        public override void AddExtraTooltipLines(Item item, ref List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ModifierHoarding", tooltip.Value)
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

    class CriticalCurse : ModProjectile
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

            Vector2 drawOrigin = new(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Lerp(Color.White, Color.Blue, 1f - ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length))) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 14;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.OffsetSize(-8, 0);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] == 0)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity * 0.75f;
            Projectile.tileCollide = false;

            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);
            Projectile.ai[0] = 3;

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(Projectile.Center, 42, 15));
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] != 3)
            {
                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(Projectile.Center, 42, 15));
                }
                Projectile.ai[0] = 3;
            }
        }

        public override void AI()
        {
            Projectile.netUpdate = true;
            Player player = Main.player[Projectile.owner];

            Lighting.AddLight(Projectile.Center, new Vector3(255, 135, 255) / 1200f);

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (player.dead)
            {
                Projectile.active = false;
            }

            if (Projectile.ai[0] == 0)
            {
                Projectile.timeLeft = 600;
                Projectile.ai[1]++;

                Vector2 target = player.Center + new Vector2(player.direction * -25f, -30f) + (float)Math.Sin(Projectile.ai[1] / 14f) * 50f * (Projectile.ai[1] / 50f).ToRotationVector2();
                Projectile.velocity = (target - Projectile.Center) / 10f;
            }
            else if (Projectile.ai[0] == 1)
            {
                SoundEngine.PlaySound(SoundID.Item73, Projectile.Center);
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(Projectile.Center, 2, 73));
                }

                if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.Center = player.Center;
                    Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * Projectile.velocity.Length();
                    Projectile.netUpdate = true;
                }

                Projectile.tileCollide = true;
                Projectile.ai[0]++;
            }
            else if (Projectile.ai[0] == 3)
            {
                Projectile.velocity *= 0.93f;
                Projectile.alpha += 5;

                if (Projectile.alpha > 255)
                {
                    Projectile.active = false;
                }
            }
        }
    }
}
