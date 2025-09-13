using Microsoft.Xna.Framework;

namespace CritRework.Content.Items.Bronze
{
    public class BronzePickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.damage = 10;
            Item.knockBack = 3f;
            Item.width = 30;
            Item.height = 30;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item1;
            Item.pick = 59;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.BronzeAlloy>(12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BronzeHamaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.damage = 10;
            Item.knockBack = 3f;
            Item.width = 30;
            Item.height = 30;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item1;
            Item.hammer = 60;
            Item.axe = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.BronzeAlloy>(12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
