using CritRework.Content.CritTypes.RandomPool;

namespace CritRework.Content.Items.Whetstones
{
    public class WebCoveredWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.WebCovered>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<IronWhetstone>()
                .AddIngredient(ItemID.Cobweb, 50)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe()
                .AddIngredient<LeadWhetstone>()
                .AddIngredient(ItemID.Cobweb, 50)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
