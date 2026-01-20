namespace CritRework.Content.Items.Equipable.Accessories
{
    public class ThawingCloth : ModItem
    {
        public static float damageBonusPerDebuff = 0f;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 30)
                .AddIngredient(ItemID.IceBlock, 50)
                .AddIngredient(ItemID.IceTorch, 15)
                .AddTile(TileID.Loom)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.defense = 2;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 50, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<ThawingCloth>();
            damageBonusPerDebuff = 0.3f;
        }
    }
}
