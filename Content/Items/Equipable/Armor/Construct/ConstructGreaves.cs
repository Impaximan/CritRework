using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Equipable.Armor.Construct
{
    [AutoloadEquip(EquipType.Legs)]
    public class ConstructGreaves : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 17;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CritPlayer>().augmentedWeaponCritBoost += 10f;
            player.jumpSpeedBoost = 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Greenprint>()
                .AddIngredient(ItemID.LunarTabletFragment, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}