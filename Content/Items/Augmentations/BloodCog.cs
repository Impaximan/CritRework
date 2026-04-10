using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;

namespace CritRework.Content.Items.Augmentations
{
    public class BloodCog : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 10)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddIngredient(ModContent.ItemType<BronzeAlloy>(), 3)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 15)
                .AddIngredient(ItemID.AdamantiteBar, 10)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 15)
                .AddIngredient(ItemID.TitaniumBar, 10)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 34;
            Item.UseSound = SoundID.Item22;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override bool CanApplyTo(Item weapon)
        {
            return (weapon.useAmmo == AmmoID.Bullet || Item.staff[weapon.type] || ItemID.Sets.IsSpaceGun[Type] || weapon.type == ItemID.SpaceGun || weapon.type == ItemID.AquaScepter) && weapon.useStyle == ItemUseStyleID.Shoot && !weapon.channel;
        }

         public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return false;
        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            if (player.TryGetModPlayer(out CritPlayer critPlayer))
            {
                if (hit.Crit && critPlayer.sawProjectile != null)
                {
                    float damageMult = CritType.CalculateActualCritMult(critType, item, player);
                    if (critPlayer.sawProjectile.ai[0] <= 0) SoundEngine.PlaySound(SoundID.Item23, critPlayer.sawProjectile.Center);
                    critPlayer.sawProjectile.ai[0] = 100 * (damageMult - 1f) + item.useTime;
                    critPlayer.sawProjectile.netUpdate = true;
                }
            }
        }
    }

    public class SawedOn : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.penetrate = -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle frame = new Rectangle(0, texture.Height / 2 * Projectile.frame, texture.Width, texture.Height / 2);

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, new Vector2(Main.player[Projectile.owner].direction == 1 ? 48f : texture.Width - 48f, 10f), Projectile.scale, Main.player[Projectile.owner].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            PunchCameraModifier modifier = new(target.Center, (Projectile.rotation + (Projectile.direction == -1 ? MathHelper.Pi : 0f)).ToRotationVector2(), 2f, 10f, 10, 500f);
            Main.instance.CameraModifiers.Add(modifier);

            SoundStyle style = new("CritRework/Assets/Sounds/SharpHit2");
            style.PitchVariance = 0.35f;
            style.Pitch -= 0.35f;
            style.Volume = 0.75f;
            style.MaxInstances = 1;
            SoundEngine.PlaySound(style, target.Center);
        }

        int soundCounter = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.TryGetModPlayer(out CritPlayer critPlayer);
            if (player.HeldItem != Projectile.GetGlobalProjectile<CritProjectile>().ogItem || player.ItemTimeIsZero)
            {
                Projectile.active = false;
                critPlayer.sawProjectile = null;
                return;
            }

            Projectile.timeLeft = 30;

            critPlayer.sawProjectile = Projectile;

            Projectile.rotation = player.itemRotation + (Projectile.direction == -1 ? MathHelper.Pi : 0f);
            Projectile.Center = player.Center + (Projectile.rotation.ToRotationVector2() * (player.itemWidth + 20f) * player.direction).RotatedBy(MathHelper.ToRadians(-2 * player.direction));

            if (Projectile.ai[0] > 0)
            {
                Projectile.frameCounter++;

                if (Projectile.frameCounter > 2)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    if (Projectile.frame >= 2)
                    {
                        Projectile.frame = 0;
                    }
                }

                soundCounter++;
                if (soundCounter > 20)
                {
                    soundCounter = 0;
                    SoundEngine.PlaySound(SoundID.Item22, Projectile.Center);
                }

                Projectile.ai[0]--;
                Projectile.position += Main.rand.NextVector2Circular(2f, 2f);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] <= 0)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }
    }
}