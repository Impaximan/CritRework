using CritRework.Common.Globals;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class WindWalker : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HermesBoots)
                .AddIngredient(ItemID.Feather, 15)
                .AddIngredient(ItemID.Cloud, 20)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.accessory = true;
            Item.defense = 2;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<WindWalker>();
        }
    }

    public class WalkingWithTheWind : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.65f;
        }
    }
}
