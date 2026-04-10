using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;
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
    public class ImperfectStorm : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 10)
                .AddIngredient(ItemID.Feather, 8)
                .AddIngredient(ItemID.RainCloud, 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 15)
                .AddIngredient(ItemID.AdamantiteBar, 10)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 15)
                .AddIngredient(ItemID.TitaniumBar, 10)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 22;
            Item.UseSound = SoundID.Item103;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override bool CanApplyTo(Item weapon)
        {
            return weapon.DamageType.CountsAsClass(DamageClass.Melee);
        }

         public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return true;
        }

        const int max = 20;

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            NPC.HitModifiers modifiers = new NPC.HitModifiers();
            if ((critCondition || (projectile != null && projectile.IsCritAugment())) && (projectile == null || projectile.type != ModContent.ProjectileType<Storm>()))
            {
                player.GetModPlayer<CritPlayer>().timeSinceCrit = 0;
                if (player.GetModPlayer<CritPlayer>().approaches.Count > 0 && !player.GetModPlayer<CritPlayer>().slashApproach)
                {
                    player.GetModPlayer<CritPlayer>().slashApproach = true;
                    SoundEngine.PlaySound(SoundID.Item103);
                }
            }
            else if (player.TryGetModPlayer(out CritPlayer critPlayer) && (projectile == null || projectile.type != ModContent.ProjectileType<Storm>()))
            {
                int damage = (int)(hit.SourceDamage * 8.5f / max);
                Projectile approach = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, item), target.Center + Main.rand.NextVector2Circular(75f, 75f), Vector2.Zero, ModContent.ProjectileType<Approach>(), damage, 0f, player.whoAmI);
                approach.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                critPlayer.approaches.Add(approach);

                if (critPlayer.approaches.Count > max)
                {
                    critPlayer.approaches[0].ai[0] = 1;
                    critPlayer.approaches.RemoveAt(0);
                }
            }
        }
    }

    public class Approach : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.timeLeft = 900;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.alpha = 100;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.scale = 0f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, texture.Size() / 2, new Vector2(1f, Projectile.scale), SpriteEffects.None, 0f);

            return false;
        }

        bool madeSound = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!madeSound)
            {
                madeSound = true;
                SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
            }

            if (player.dead || !player.active || Projectile.ai[0] == 1 || Projectile.timeLeft < 10)
            {
                Projectile.scale -= 0.2f;

                if (Projectile.scale <= 0f)
                {
                    Projectile.active = false;

                    if (player.TryGetModPlayer(out CritPlayer critPlayer) && critPlayer.approaches.Contains(Projectile))
                    {
                        critPlayer.approaches.Remove(Projectile);
                    }
                }
            }
            else
            {
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.1f);
            }
        }
    }

    public class Storm : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 15;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.scale = 0f;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + Projectile.Size / 2;
                Color color = Projectile.GetAlpha(Color.Lerp(Color.White, Color.Blue, 1f - ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length))) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(1f, Projectile.scale), SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 180);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += 10;
        }

        bool madeSound = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!madeSound)
            {
                madeSound = true;

                SoundStyle sound = SoundID.Item71;
                sound.Pitch = 0.5f;
                sound.PitchVariance = 0.25f;
                sound.MaxInstances = 0;

                SoundEngine.PlaySound(sound, Projectile.Center);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.timeLeft < 5)
            {
                Projectile.scale -= 0.2f;
            }
            else if (Projectile.scale < 1f)
            {
                Projectile.scale += 0.2f;
            }
        }
    }
}