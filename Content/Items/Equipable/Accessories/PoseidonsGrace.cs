using CritRework.Common.ModPlayers;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class PoseidonsGrace : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CritPlayer>().whirlpoolDuration = 600;
            player.GetModPlayer<CritPlayer>().whirlpoolType = ModContent.ProjectileType<PoseidonsProtection>();
            player.AddEquip<PoseidonsGrace>();
        }

        public static int CalculateWhirlpoolDamage(CritPlayer critPlayer)
        {
            float damage = 0f;
            float timeDenominator = critPlayer.whirlpoolDuration / 30;

            foreach (CritPlayer.WhirlpoolStack whirlpoolStack in critPlayer.whirlpoolStacks)
            {
                damage += whirlpoolStack.damage / timeDenominator / 3f;
            }

            if (damage < 1f)
            {
                return 1;
            }
            else
            {
                return (int)damage;
            }
        }
    }

    public class PoseidonsProtection : ModProjectile
    {
        public override string Texture => "CritRework/Content/Items/Equipable/Accessories/Whirlpool";

        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 30;
        }

        float rotation = 0f;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            float smallest = 1f;
            float largest = 2f;
            rotation += MathHelper.PiOver2 / 30;

            float alphaMult = 0.4f;

            if (Main.player[Projectile.owner].TryGetModPlayer(out CritPlayer critPlayer))
            {
                if (critPlayer.timeSinceWhirlpoolStrike < 30)
                {
                    alphaMult += 0.1f * ((30 - critPlayer.timeSinceWhirlpoolStrike) / 30f);
                }
            }

            for (float i = smallest; i < largest; i += 0.1f)
            {
                float l = (i - smallest) / (largest - smallest);

                float rotationMult = 1f + l * 0.2f;

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Cyan, Color.Purple, l) * alphaMult * (1f - (float)Projectile.alpha / 255f), rotation * rotationMult, texture.Size() / 2f, i * Projectile.width / (float)texture.Width * Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0f;
        }

        public override void OnKill(int timeLeft)
        {

        }

        public override void AI()
        {
            if (Projectile.timeLeft <= 30)
            {
                Projectile.alpha += 255 / 30;
            }
            else if (Projectile.alpha > 0 && Projectile.alpha < 255)
            {
                Projectile.alpha -= 255 / 30;
            }
            else
            {
                Projectile.alpha = 0;
            }

            Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.1f);

            Projectile.Center = Main.player[Projectile.owner].Center;

            Lighting.AddLight(Projectile.Center, Color.Cyan.ToVector3() * (1f / 2f) * (1f - (float)Projectile.alpha / 255f));

            if (Main.player[Projectile.owner].TryGetModPlayer(out CritPlayer critPlayer) && Projectile.owner == Main.myPlayer)
            {
                Projectile.damage = PoseidonsGrace.CalculateWhirlpoolDamage(critPlayer);
                Projectile.timeLeft = critPlayer.WhirlpoolTimeLeft();
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int modify = (int)(Projectile.width * (Projectile.scale - 1f));

            hitbox.OffsetSize(modify, modify);
            hitbox.X -= modify / 2;
            hitbox.Y -= modify / 2;
        }
    }
}
