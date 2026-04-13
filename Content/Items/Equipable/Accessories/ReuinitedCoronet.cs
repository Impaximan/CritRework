namespace CritRework.Content.Items.Equipable.Accessories
{
    public class ReuinitedCoronet : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CrownShard>()
                .AddIngredient(ItemID.LifeCrystal)
                .AddIngredient<TiaraShard>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            TiaraShard.potencyAdded = 0.16f;
            player.AddEquip<CrownShard>();
            player.AddEquip<TiaraShard>();
        }
    }
}
