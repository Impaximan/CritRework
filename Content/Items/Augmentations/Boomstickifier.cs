using CritRework.Common.ModPlayers;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Augmentations
{
    public class Boomstickifier : Augmentation
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 34;
            Item.UseSound = equipSound;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 4, 0, 0);
        }

        public override bool CanApplyTo(Item weapon)
        {
            return ItemID.Sets.Spears[weapon.type];
        }

         public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return true;
        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            NPC.HitModifiers modifiers = new NPC.HitModifiers();

            if (projectile == null)
            {
                return;
            }

            if (projectile.type != ModContent.ProjectileType<BigBoom>() && critCondition)
            {
                Projectile explosion = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, item), projectile.Center, projectile.velocity, ModContent.ProjectileType<BigBoom>(), hit.SourceDamage * 3, projectile.knockBack * 3f, projectile.owner);
                explosion.SetAsAugmentCrit();
                player.Hurt(PlayerDeathReason.ByPlayerItem(player.whoAmI, Item), 30, -Math.Sign(projectile.velocity.X), false, false, -1, true, 999, knockback: 15f);
                projectile.timeLeft = 0;
                target.immune[player.whoAmI] = 0;
            }
        }
    }

    class BigBoom : ModProjectile
    {
        public override string Texture => "CritRework/nothing";

        public override void SetDefaults()
        {
            Projectile.width = 250;
            Projectile.height = 250;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }

        int timeActive = 0;
        public override void AI()
        {
            if (timeActive == 1)
            {
                for (int i = 0; i < 100; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                    dust.velocity = (dust.position - Projectile.Center) / 30f;
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }

                SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
            }

            timeActive++;
        }
    }
}
