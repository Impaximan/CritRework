namespace CritRework.Content.Items.Equipable.Accessories
{
    public class ContaminatedSapling : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RichMahogany, 10)
                .AddIngredient(ItemID.JungleSpores, 6)
                .AddIngredient(ItemID.GlowingMushroom, 25)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public static float damagePerAugmentation = 12f;

        public override void UpdateEquip(Player player)
        {
            damagePerAugmentation = 12f;
            player.AddEquip<ContaminatedSapling>();
        }
    }
}
