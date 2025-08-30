using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class WiseCracker : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 40;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.GetGlobalItem<CritItem>().forceCanCrit = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<WiseCracker>();

            if (Item.TryGetGlobalItem(out CritItem cItem) && cItem.critType != null)
            {
                player.GetModPlayer<CritPlayer>().summonCrit = cItem.critType;
            }
        }
    }
}
