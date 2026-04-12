namespace CritRework.Content.Items.Equipable.Accessories
{
    public class LastStraw : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FatedFlame>()
                .AddIngredient<ShortestStraw>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 36;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<ShortestStraw>();
            player.AddConsecutiveCritDamage(0.05f);

            if (player.statLife <= player.statLifeMax2 * 0.5f)
            {
                player.AddPotency(0.3f);
            }
        }
    }
}
