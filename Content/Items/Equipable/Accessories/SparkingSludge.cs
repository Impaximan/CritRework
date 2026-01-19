namespace CritRework.Content.Items.Equipable.Accessories
{
    public class SparkingSludge : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Gel, 25)
                .AddIngredient(ItemID.Torch, 50)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 20, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<SparkingSludge>();
            player.GetCritChance(DamageClass.Generic) += 6;
        }
    }
}
