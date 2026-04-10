using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class SkywardSling : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BronzeAlloy>(3)
                .AddIngredient(ItemID.Feather, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CritPlayer>().maxAugmentations++;
            player.GetCritChance(DamageClass.Generic) += 5;
        }
    }
}
