using CritRework.Common.Globals;
using Terraria;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class SharpenedWrench : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 15)
                .AddIngredient(ItemID.SilverBar, 10)
                .AddTile(TileID.Sawmill)
                .Register();
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 15)
                .AddIngredient(ItemID.TungstenBar, 10)
                .AddTile(TileID.Sawmill)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 44;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            if (player.HeldItem != null)
            {
                if (player.HeldItem.TryGetGlobalItem(out CritItem critItem))
                {
                    if (critItem.critType != null && critItem.critType.ShowWhenActive && (player.HeldItem.pick > 0 || player.HeldItem.axe > 0 || player.HeldItem.hammer > 0) && critItem.critType.ShouldCrit(player, player.HeldItem, null, null, new NPC.HitModifiers()))
                    {
                        player.pickSpeed -= 0.30f;
                    }
                }
            }
            player.AddEquip<SharpenedWrench>();
        }
    }
}
