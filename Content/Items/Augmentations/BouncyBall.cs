using CritRework.Common.ModPlayers;
using System;
using Terraria.Audio;

namespace CritRework.Content.Items.Augmentations
{
    public class BouncyBall : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PinkGel, 20)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.UseSound = equipSound;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 50, 0);
        }

        public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {

        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            float power = CritType.CalculateActualCritMult(critType, item, player) - 1f;
            power *= 3f;
            power += 1f;

            if (target.knockBackResist != 0 && critCondition)
            {
                Vector2 direction;

                if (projectile != null && projectile.velocity != Vector2.Zero)
                {
                    direction = projectile.velocity;
                }
                else
                {
                    direction = player.DirectionTo(target.Center);
                }

                direction.Normalize();

                target.velocity = direction * power * hit.Knockback * target.knockBackResist;
                target.velocity.Y -= 0.5f * power * hit.Knockback * target.knockBackResist;
                SoundEngine.PlaySound(SoundID.Item56, target.Center);
            }
        }
    }
}
