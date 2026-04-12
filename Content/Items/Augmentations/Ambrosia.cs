using CritRework.Common.ModPlayers;
using System;
using Terraria.Audio;

namespace CritRework.Content.Items.Augmentations
{
    public class Ambrosia : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PixieDust, 30)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.Bottle)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.UseSound = SoundID.Item106;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 8, 0, 0);
        }

        public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {
            if (cooldown > 0)
            {
                cooldown--;
            }
        }

        int cooldown = 0;
        static float counter = 0f;

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            if (target.type == NPCID.TargetDummy || target.SpawnedFromStatue)
            {
                return;
            }

            NPC.HitModifiers modifiers = new NPC.HitModifiers();
            if (critCondition)
            {
                float power = CritType.CalculateActualCritMult(critType, item, player) - 1f;

                player.GetModPlayer<CritPlayer>().timeSinceCrit = 0;

                SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Ambrosia")
                {
                    PitchVariance = 0.5f,
                    Volume = MathHelper.Lerp(0.5f, 1f, power) * 0.75f,
                    MaxInstances = 10,
                }, player.Center);

                ReduceAllDebuffs(player, (int)(power * 10f));
            }
        }

        public static void ReduceAllDebuffs(Player player, int amount)
        {
            for (int i = 0; i < amount / 5; i++)
            {
                Dust sparkle = Dust.NewDustDirect(player.position, player.width, player.height, DustID.TreasureSparkle);
            }

            for (int i = 0; i < player.buffTime.Length; i++)
            {
                int type = player.buffType[i];

                if (Main.debuff[type])
                {
                    player.buffTime[i] -= amount;

                    if (type == BuffID.PotionSickness)
                    {
                        player.potionDelay -= amount;
                    }
                }
            }

            if (player.TryGetModPlayer(out CritPlayer critPlayer))
            {
                if (critPlayer.ambrosiaTextCounter <= 0)
                {
                    critPlayer.ambrosiaTextCounter = 60;

                }
                
                if (critPlayer.ambrosiaTotal + amount > 60 && critPlayer.ambrosiaTextCounter > 0)
                {
                    critPlayer.ambrosiaTextCounter = 1;
                }

                critPlayer.ambrosiaTotal += amount;
            }
        }
    }
}
