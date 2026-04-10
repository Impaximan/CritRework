using CritRework.Common.Globals;
using CritRework.Content.CritTypes;
using CritRework.Content.Items.Whetstones;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Chat;

namespace CritRework.Content.Items
{
    public class GildedRemains : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Red;
        }


        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.AddEquip<GildedRemains>();
            }
        }
    }
}
