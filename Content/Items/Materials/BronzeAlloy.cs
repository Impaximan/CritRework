using Terraria.GameContent.Creative;

namespace CritRework.Content.Items.Materials
{
    class BronzeAlloy : ModItem
    {
        LocalizedText AnyCopper;
        LocalizedText AnySilver;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            AnyCopper = Mod.GetLocalization($"RecipeGroups.AnyCopper");
            AnySilver = Mod.GetLocalization($"RecipeGroups.AnySilver");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.width = 30;
            Item.height = 24;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 40, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddRecipeGroup(RecipeGroup.RegisterGroup("AnyCopper", new RecipeGroup(() => AnyCopper.Value, ItemID.CopperBar, ItemID.TinBar)), 2)
                .AddRecipeGroup(RecipeGroup.RegisterGroup("AnySilver", new RecipeGroup(() => AnySilver.Value, ItemID.SilverBar, ItemID.TungstenBar)))
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}
