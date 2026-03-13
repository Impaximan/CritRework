namespace CritRework.Content.Items.Whetstones
{
    public class FractalWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.Fractal>();

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 26;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 5)
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddIngredient(ItemID.SpectreBar, 5)
                .AddIngredient(ItemID.ShroomiteBar, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
