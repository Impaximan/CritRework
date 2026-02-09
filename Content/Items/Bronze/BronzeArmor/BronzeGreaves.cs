using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Bronze.BronzeArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class BronzeGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            //ArmorIDs.Body.Sets.HidesTopSkin[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 5;
            player.GetCritChance(DamageClass.Magic) += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BronzeAlloy>(18)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}