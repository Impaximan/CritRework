using CritRework.Common.ModPlayers;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class DemetersWrath : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PoseidonsGrace>()
                .AddIngredient(ItemID.FrozenTurtleShell)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 8, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CritPlayer>().whirlpoolDuration = 900;
            player.GetModPlayer<CritPlayer>().whirlpoolType = ModContent.ProjectileType<DemetersFury>();
            player.AddEquip<PoseidonsGrace>();
            
            if (player.statLife <= player.statLifeMax2 * 0.5f)
            {
                player.AddBuff(BuffID.IceBarrier, 2);
            }
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

    public class DemetersFury : ModProjectile
    {
        public override string Texture => "CritRework/Content/Items/Equipable/Accessories/Whirlpool";

        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 600;
            Projectile.height = 600;
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

            float alphaMult = 0.25f;

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

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Blue, Color.White, l) * alphaMult * (1f - (float)Projectile.alpha / 255f), rotation * rotationMult, texture.Size() / 2f, i * Projectile.width / (float)texture.Width * Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 60 * 5);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0f;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.type == NPCID.TheDestroyerBody)
            {
                modifiers.SourceDamage /= 2;
            }
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

            float targetScale = 1f;

            Player player = Main.player[Projectile.owner];

            targetScale += (float)player.statDefense * 0.005f;
            targetScale *= (1f + player.endurance * 1.5f);

            Projectile.scale = MathHelper.Lerp(Projectile.scale, targetScale, 0.05f);

            Projectile.Center = Main.player[Projectile.owner].Center;

            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3()* (1f - (float)Projectile.alpha / 255f));

            if (Main.player[Projectile.owner].TryGetModPlayer(out CritPlayer critPlayer) && Projectile.owner == Main.myPlayer)
            {
                Projectile.damage = DemetersWrath.CalculateWhirlpoolDamage(critPlayer);
                Projectile.timeLeft = critPlayer.WhirlpoolTimeLeft();
                Projectile.knockBack = 0.01f;
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int modify = (int)(Projectile.width * (Projectile.scale - 1f));

            hitbox.OffsetSize(modify, modify);

            //hitbox.X -= modify / 2;
            //hitbox.Y -= modify / 2;
        }
    }
}
