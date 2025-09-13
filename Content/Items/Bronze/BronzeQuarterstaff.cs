using Terraria.DataStructures;

namespace CritRework.Content.Items.Bronze
{
    public class BronzeQuarterstaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.damage = 20;
            Item.crit = 25;
            Item.knockBack = 3f;
            Item.width = 50;
            Item.height = 50;
            Item.shootSpeed = 0f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item152;
            Item.shoot = ModContent.ProjectileType<BronzeQuarterstaffProjectile>();
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
        }

        //public override bool? UseItem(Player player)
        //{
        //    return true;
        //}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.BronzeAlloy>(12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BronzeQuarterstaffProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 144;
            Projectile.height = 144;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.arrow = true;
            Projectile.ai[0] = 1f;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.Modified(-20, -20, 40, 40);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                Projectile.ai[0] += 0.08f;

                if (Projectile.ai[0] > 2f) Projectile.ai[0] = 2f;

                Projectile.ai[1] = 90f;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.Center = player.MountedCenter + player.direction * new Vector2(10f, 0).RotatedBy(-MathHelper.Pi * 0.65f * player.itemTime / player.itemTimeMax * player.direction) - player.velocity + new Vector2(0f, 1f);
            Projectile.velocity = player.velocity;
            Projectile.rotation += MathHelper.Pi / 22f * player.direction * Projectile.ai[0];

            if (Projectile.ai[1] > 0f)
            {
                Projectile.ai[1]--;
                Projectile.netUpdate = true;
            }
            else
            {
                if (Projectile.ai[0] < 1.01f)
                {
                    Projectile.ai[0] = 1f;
                }
                else
                {
                    Projectile.ai[0] *= 0.995f;
                    Projectile.netUpdate = true;
                }
            }

            Projectile.localNPCHitCooldown = (int)(20 / Projectile.ai[0]);

            Projectile.timeLeft = 300;

            player.heldProj = Projectile.whoAmI;

            //player.compositeFrontArm.enabled = true;
            //player.compositeFrontArm.rotation = player.DirectionTo(Projectile.Center).ToRotation();

            if (!player.channel && player.ItemTimeIsZero || player.dead)
            {
                Projectile.active = false;
            }
        }
    }
}
