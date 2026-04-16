using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;
using OrchidMod;
using OrchidMod.Content.Guardian;
using System;
using Terraria.Audio;

namespace CritRework.Content.Items.Augmentations.Orchid
{
    [JITWhenModsEnabled("OrchidMod")]
    public class ParryingDagger : Augmentation
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.HasMod("OrchidMod");
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BronzeAlloy>(3)
                .AddRecipeGroup(RecipeGroupID.IronBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.UseSound = equipSound;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return true;
        }

        public override bool CanApplyTo(Item weapon)
        {
            return weapon.ModItem != null && weapon.ModItem is OrchidModGuardianShield;
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

                counter += power * player.GetPotency(item);

                if (counter >= 1f)
                {
                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/ParryCrit")
                    {
                        PitchVariance = 0.5f,
                        Volume = 1f,
                        MaxInstances = 10,
                    }, player.Center);

                    OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
                    guardian.AddGuard((int)counter);
                    counter -= (int)counter;
                }
            }
        }
    }
}
