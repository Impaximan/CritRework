using CritRework.Common.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.Items.Potions
{
    public class ReclawPotion : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteOre, 3)
                .AddIngredient(ItemID.Fireblossom)
                .AddTile(TileID.Bottles)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneOre, 3)
                .AddIngredient(ItemID.Fireblossom)
                .AddTile(TileID.Bottles)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IronskinPotion);
            Item.width = 20;
            Item.height = 32;
            Item.rare = ItemRarityID.Blue;
            Item.buffType = ModContent.BuffType<Reclaw>();
            Item.buffTime = 60 * 5 * 60;
        }
    }

    public class Reclaw : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CritPlayer>().consecutiveCriticalStrikeDamage += 0.2f;
        }
    }
}
