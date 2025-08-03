namespace CritRework.Content.Items.Whetstones
{
    public class SoaringWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.Soaring>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SunplateBlock, 35)
                .AddIngredient(ItemID.Feather, 3)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }
}
