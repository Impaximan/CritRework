using CritRework.Common.ModPlayers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.Items.Weapons
{
    public class LeadChakram : ModItem
    {
        public override void SetDefaults()
        {
            Item.SetWeaponValues(9, 1f, 15);
            Item.width = 24;
            Item.height = 24;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 0, 1, 80);
            Item.rare = ItemRarityID.White;
            Item.shoot = ModContent.ProjectileType<LeadChakramProjectile>();
            Item.shootSpeed = 15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 4)
                .AddRecipeGroup(RecipeGroupID.Wood, 2)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.LeadBow);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.TryGetModPlayer(out CritPlayer c) && c.allowNewChakram)
            {
                c.allowNewChakram = false;
                return base.CanUseItem(player);
            }

            if (player.ownedProjectileCounts[Item.shoot] >= 1)
            {
                return false;
            }

            return base.CanUseItem(player);
        }
    }

    public class LeadChakramProjectile : ModProjectile
    {
        public override string Texture => "CritRework/Content/Items/Weapons/LeadChakram";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && Main.player[Projectile.owner].TryGetModPlayer(out CritPlayer c))
            {
                c.allowNewChakram = true;
            }

            Projectile.ai[1]++;

            Projectile.netUpdate = true;

            if (Projectile.ai[1] > 3 && Projectile.ai[0] < 20f)
            {
                Projectile.ai[0] = 300f;
                Projectile.velocity = Vector2.Zero;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X == 0) Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y == 0) Projectile.velocity.Y = -oldVelocity.Y;

            return false;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.Pi / 15f;

            Projectile.ai[0]++;

            if (Projectile.ai[0] > 20f)
            {
                Projectile.velocity *= 0.91f;
                Projectile.velocity += Projectile.DirectionTo(Main.player[Projectile.owner].Center) * 1.3f;

                if (Projectile.Distance(Main.player[Projectile.owner].Center) < Projectile.Size.Length())
                {
                    Projectile.active = false;
                }

                Projectile.tileCollide = false;
            }
        }
    }
}
