using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Equipable.Armor.Fraud
{
    [AutoloadEquip(EquipType.Legs)]
    public class FraudPants : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 5;
            player.AddPotency(0.1f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Lie>(2)
                .AddIngredient(ItemID.Obsidian, 25)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}