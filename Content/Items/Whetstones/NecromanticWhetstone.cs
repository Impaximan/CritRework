namespace CritRework.Content.Items.Whetstones
{
    public class NecromanticWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.Necromantic>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<IronWhetstone>()
                .AddIngredient(ItemID.Bone, 35)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe()
                .AddIngredient<LeadWhetstone>()
                .AddIngredient(ItemID.Bone, 35)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
