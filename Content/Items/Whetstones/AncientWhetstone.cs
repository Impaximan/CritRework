namespace CritRework.Content.Items.Whetstones
{
    public class AncientWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.Ancient>();

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
                .AddIngredient(ItemID.FossilOre, 10)
                .AddIngredient(ItemID.StoneBlock, 12)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}
