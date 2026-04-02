using CritRework.Common.ModPlayers;
using System;
using Terraria.Audio;

namespace CritRework.Content.Items.Augmentations
{
    public class HoneydippedStinger : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeeWax, 5)
                .AddIngredient(ItemID.Stinger, 3)
                .AddIngredient(ItemID.HoneyBlock, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 34;
            Item.UseSound = equipSound;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            return true;
        }

        static float counter = 0f;

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target)
        {
            if (target.type == NPCID.TargetDummy || target.SpawnedFromStatue)
            {
                return;
            }

            NPC.HitModifiers modifiers = new NPC.HitModifiers();
            if (player.GetModPlayer<CritPlayer>().ShouldNormallyCrit(item, projectile, new NPC.HitModifiers(), critType, target))
            {
                float damageMult = CritType.CalculateActualCritMult(critType, item, player);

                player.GetModPlayer<CritPlayer>().timeSinceCrit = 0;

                SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/SharpHit2")
                {
                    PitchVariance = 0.5f,
                    Volume = 1.5f
                }, target.Center);

                counter += (damageMult - 1f) * 2.5f; //The amount healed per crit

                if ((int)counter > 0)
                {
                    if (!player.moonLeech)
                    {
                        player.Heal((int)counter);
                        if (Main.netMode != NetmodeID.SinglePlayer) NetMessage.SendData(MessageID.PlayerHeal, number: player.whoAmI, number2: (int)counter);
                    }
                    counter -= (int)counter;
                }
            }
        }
    }
}
