using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;
using Terraria;
using Terraria.Audio;

namespace CritRework.Content.Items.Augmentations
{
    public class Buckler : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BronzeAlloy>(3)
                .AddRecipeGroup(RecipeGroupID.IronBar, 6)
                .AddRecipeGroup(RecipeGroupID.Wood, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.UseSound = equipSound;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

         public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return !player.HasBuff<BucklerRetaliation>();
        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            NPC.HitModifiers modifiers = new NPC.HitModifiers();

            if (projectile == null)
            {
                return;
            }

            if (critCondition && !player.HasBuff<BucklerRetaliation>() && !player.HasBuff<BucklerDepletion>())
            {
                float damageMult = CritType.CalculateActualCritMult(critType, item, player);
                CritPlayer critPlayer = player.GetModPlayer<CritPlayer>();
                critPlayer.timeSinceCrit = 0;
                if (critPlayer.bucklerPower < damageMult - 1f)
                {
                    critPlayer.bucklerPower = damageMult - 1f;

                    SoundEngine.PlaySound(SoundID.Item37, player.Center);
                    CombatText.NewText(player.getRect(), new Color(220, 220, 240), GetDefense(critPlayer.bucklerPower));
                }

                player.AddBuff(ModContent.BuffType<BucklerDefense>(), 60);
            }
        }

        public static int GetDefense(float bucklerPower)
        {
            return 10 + (int)(bucklerPower * 15);
        }

        public static float GetDamageBoost(float bucklerPower)
        {
            return bucklerPower / 2f;
        }
    }

    public class BucklerDefense : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += Buckler.GetDefense(player.GetModPlayer<CritPlayer>().bucklerPower);
        }
    }

    public class BucklerRetaliation : ModBuff
    {
        public override bool RightClick(int buffIndex)
        {
            return false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += Buckler.GetDamageBoost(player.GetModPlayer<CritPlayer>().bucklerPower);

            if (player.buffTime[buffIndex] <= 0)
            {
                player.AddBuff(ModContent.BuffType<BucklerDepletion>(), 600);
                SoundEngine.PlaySound(SoundID.Item8, player.Center);
                CombatText.NewText(player.getRect(), Color.Pink, "Recharging...", false, true);
            }
        }
    }

    public class BucklerDepletion : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance -= 0.3f;
        }
    }
}
