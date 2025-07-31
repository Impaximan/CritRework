using CritRework.Content.CritTypes.RandomPool;

namespace CritRework.Content.Items.Whetstones
{
    public class FrozenWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.Frozen>();

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
                .AddIngredient(ItemID.IceBlock, 15)
                .AddTile(TileID.IceMachine)
                .Register();
            CreateRecipe()
                .AddIngredient<LeadWhetstone>()
                .AddIngredient(ItemID.IceBlock, 15)
                .AddTile(TileID.IceMachine)
                .Register();
        }
    }
}
