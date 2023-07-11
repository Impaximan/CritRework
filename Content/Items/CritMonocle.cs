using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace CritRework.Content.Items
{
    class CritMonocle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.GetModPlayer<Common.ModPlayers.CritPlayer>().slotMachineCritCrafting = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Lens, 3)
                .AddIngredient(ItemID.BlackLens)
                .AddIngredient(ItemID.DemoniteBar, 2)
                .AddTile(TileID.DemonAltar)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Lens, 3)
                .AddIngredient(ItemID.BlackLens)
                .AddIngredient(ItemID.CrimtaneBar, 2)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
