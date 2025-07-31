using CritRework.Content.CritTypes.WhetstoneSpecific;

namespace CritRework.Content.Items.Whetstones
{
    public class StarterWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<Starter>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 10)
                .AddRecipeGroup(RecipeGroupID.Wood, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
