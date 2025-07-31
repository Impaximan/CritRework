namespace CritRework.Content.Items.Whetstones
{
    public class EnchantedWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.Enchanted>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<IronWhetstone>()
                .AddIngredient(ItemID.FallenStar, 6)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe()
                .AddIngredient<LeadWhetstone>()
                .AddIngredient(ItemID.FallenStar, 6)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
