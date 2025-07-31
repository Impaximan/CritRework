namespace CritRework.Content.Items.Whetstones
{
    public class AdaptiveWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.Adaptive>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 1, 30, 0);
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<IronWhetstone>()
                .AddIngredient(ItemID.JungleSpores, 7)
                .AddIngredient(ItemID.RichMahogany, 12)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe()
                .AddIngredient<LeadWhetstone>()
                .AddIngredient(ItemID.JungleSpores, 7)
                .AddIngredient(ItemID.RichMahogany, 12)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
