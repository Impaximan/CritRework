using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Equipable.Armor.Fraud
{
    [AutoloadEquip(EquipType.Body)]
    public class FraudVest : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Body.Sets.HidesTopSkin[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Lie>(2)
                .AddIngredient(ItemID.Obsidian, 10)
                .AddIngredient(ItemID.DemoniteBar, 6)
                .AddTile(TileID.DemonAltar)
                .Register();
            CreateRecipe()
                .AddIngredient<Lie>(2)
                .AddIngredient(ItemID.Obsidian, 10)
                .AddIngredient(ItemID.CrimtaneBar, 6)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}