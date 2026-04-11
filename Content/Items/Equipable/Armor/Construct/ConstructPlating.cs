using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Equipable.Armor.Construct
{
    [AutoloadEquip(EquipType.Body)]
    public class ConstructPlating : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Body.Sets.HidesTopSkin[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CritPlayer>().augmentedWeaponCritBoost += 12f;
            player.GetDamage(DamageClass.Generic) += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Greenprint>()
                .AddIngredient(ItemID.LunarTabletFragment, 7)
                .AddIngredient(ItemID.LihzahrdPowerCell)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}