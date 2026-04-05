using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;
using System.Collections.Generic;

namespace CritRework.Content.Items.Equipable.Accessories.Crackers
{
    public class Horrificracker : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 40;
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 40, 0, 0);
            Item.GetGlobalItem<CritItem>().forceCanCrit = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WiseCracker>()
                .AddIngredient(ItemID.NecromanticScroll)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<WiseCracker>();
            player.maxMinions++;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if ((equippedItem.type == ItemID.NecromanticScroll && incomingItem.type == Type)  || (equippedItem.type == Type && incomingItem.type == ItemID.NecromanticScroll))
            {
                return false;
            }
            return !equippedItem.IsCracker() || !incomingItem.IsCracker();
        }
    }
}
