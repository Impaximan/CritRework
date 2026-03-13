using Terraria.Audio;

namespace CritRework.Content.Items.Bronze
{
    public class BronzeBullet : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.rare = ItemRarityID.Orange;
            Item.value = 20;
            Item.damage = 10;
            Item.knockBack = 3f;
            Item.shoot = ModContent.ProjectileType<BronzeBulletProjectile>();
            Item.width = 18;
            Item.height = 42;
            Item.shootSpeed = 15f;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void AddRecipes()
        {
            CreateRecipe(200)
                .AddIngredient<Materials.BronzeAlloy>()
                .AddIngredient(ItemID.MusketBall, 200)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BronzeBulletProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 10;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = 6;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.light = 0.1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hit.Crit)
            {
                Projectile.penetrate = 0;
            }
            else
            {
                Projectile.velocity *= 1.1f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.OffsetSize(2, -6);
        }
    }
}
