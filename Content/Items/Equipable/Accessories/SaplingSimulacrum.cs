using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class SaplingSimulacrum : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ContaminatedSapling>()
                .AddIngredient(ItemID.EyeoftheGolem)
                .AddIngredient<Greenprint>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 12, 0, 0);
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<ContaminatedSapling>() || equippedItem.type == ItemID.EyeoftheGolem
                || incomingItem.type == ModContent.ItemType<ContaminatedSapling>() || incomingItem.type == ItemID.EyeoftheGolem)
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public static float damagePerAugmentation = 10f;

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 10;
            damagePerAugmentation = 19f;
            player.AddEquip<ContaminatedSapling>();
        }
    }
}
