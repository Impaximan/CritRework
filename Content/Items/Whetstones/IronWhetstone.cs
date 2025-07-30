using CritRework.Content.CritTypes.RandomPool;

namespace CritRework.Content.Items.Whetstones
{
    public class IronWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<MediumChance>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
