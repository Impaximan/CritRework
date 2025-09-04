using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria.DataStructures;
using Terraria.Audio;

namespace CritRework.Content.Items.Bronze
{
    public class BronzeArrow : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.ammo = AmmoID.Arrow;
            Item.rare = ItemRarityID.Orange;
            Item.value = 20;
            Item.damage = 22;
            Item.knockBack = 5f;
            Item.shoot = ModContent.ProjectileType<BronzeArrowProjectile>();
            Item.width = 18;
            Item.height = 42;
            Item.shootSpeed = 5f;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void AddRecipes()
        {
            CreateRecipe(200)
                .AddIngredient<Materials.BronzeAlloy>()
                .AddIngredient(ItemID.WoodenArrow, 200)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BronzeArrowProjectile : ModProjectile
    {
        public override string Texture => "CritRework/Content/Items/Bronze/BronzeArrow";

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 42;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            if (!Projectile.noDropItem && Main.rand.NextBool(5))
            {
                Item.NewItem(new EntitySource_DropAsItem(Projectile), Projectile.getRect(), ModContent.ItemType<BronzeArrow>());
            }
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.OffsetSize(2, -22);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.NonCritDamage *= 0;
            modifiers.CritDamage *= 1.3f;
        }
    }
}
