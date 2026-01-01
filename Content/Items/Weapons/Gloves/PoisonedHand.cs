using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.Items.Weapons.Gloves
{
    public class PoisonedHand : GloveWeapon
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Stinger, 4)
                .AddIngredient(ItemID.JungleSpores, 5)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.JungleYoyo);
        }

        public override void GloveDefaults()
        {
            Item.SetWeaponValues(6, //Damage
                1f, //Knockback
                0); //Bonus crit

            Item.width = 20;
            Item.height = 22;
            Item.shootSpeed = 3.5f;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Orange;
            gloveWeight = 5;
        }
    }
}
