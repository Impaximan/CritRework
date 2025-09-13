using CritRework.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CritRework.Content.Items.Bronze.BronzeArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class BronzeBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Body.Sets.HidesTopSkin[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 7;
            player.GetCritChance(DamageClass.Magic) += 7;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BronzeAlloy>(22)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}