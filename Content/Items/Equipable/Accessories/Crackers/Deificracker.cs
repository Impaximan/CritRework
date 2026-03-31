using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;
using System.Collections.Generic;

namespace CritRework.Content.Items.Equipable.Accessories.Crackers
{
    public class Deificracker : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 40;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.GetGlobalItem<CritItem>().forceCanCrit = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WiseCracker>()
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddIngredient<DivineShard>(2)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<WiseCracker>();
            player.AddEquip<Deificracker>();

            if (Item.TryGetGlobalItem(out CritItem cItem) && cItem.critType != null)
            {
                player.GetModPlayer<CritPlayer>().summonCrit = cItem.critType;
            }

            if (Item.IsSpecial())
            {
                player.GetModPlayer<CritPlayer>().summonSpecial = true;
            }
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !equippedItem.IsCracker() || !incomingItem.IsCracker();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips.FindAll(x => x.Name.Contains("Tooltip")))
            {
                line.Text = line.Text.Replace("[icon]", "[i:" + ModContent.ItemType<SparkleIcon>() + "]");
            }
        }
    }
}
