using CritRework.Common.ModPlayers;
using System;
using Terraria.Audio;

namespace CritRework.Content.Items.Augmentations
{
    public class ManalyticConverter_Copper : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBar, 15)
                .AddIngredient(ItemID.FallenStar, 10)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.CopperBow);
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 36;
            Item.UseSound = new SoundStyle("CritRework/Assets/Sounds/Zap")
            {
                Volume = 1f,
                PitchVariance = 0.25f
            };
            Item.rare = ItemRarityID.Blue;
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

                SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Zap")
                {
                    PitchVariance = 0.5f,
                    Volume = 0.65f
                }, target.Center);

                counter += (damageMult - 1f) * 10f; //The amount gained per crit

                if ((int)counter > 0)
                {
                    player.ManaEffect((int)counter);
                    player.statMana += (int)counter;

                    if (Main.netMode != NetmodeID.SinglePlayer) NetMessage.SendData(MessageID.PlayerMana, number: player.whoAmI, number2: (int)counter);

                    counter -= (int)counter;
                }
            }
        }
    }

    public class ManalyticConverter_Tin : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TinBar, 15)
                .AddIngredient(ItemID.FallenStar, 10)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.TinBow);
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 36;
            Item.UseSound = new SoundStyle("CritRework/Assets/Sounds/Zap")
            {
                Volume = 1f,
                PitchVariance = 0.25f
            };
            Item.rare = ItemRarityID.Blue;
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

                SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Zap")
                {
                    PitchVariance = 0.5f,
                    Volume = 0.65f
                }, target.Center);

                counter += (damageMult - 1f) * 10f; //The amount gained per crit

                if ((int)counter > 0)
                {
                    player.ManaEffect((int)counter);
                    player.statMana += (int)counter;

                    if (Main.netMode != NetmodeID.SinglePlayer) NetMessage.SendData(MessageID.PlayerMana, number: player.whoAmI, number2: (int)counter);

                    counter -= (int)counter;
                }
            }
        }
    }
}
