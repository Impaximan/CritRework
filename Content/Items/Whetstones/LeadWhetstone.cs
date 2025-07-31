using CritRework.Content.CritTypes.RandomPool;

namespace CritRework.Content.Items.Whetstones
{
    public class LeadWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<MediumChance>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 6)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}
