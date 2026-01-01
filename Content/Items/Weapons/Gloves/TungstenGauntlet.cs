using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.Items.Weapons.Gloves
{
    public class TungstenGauntlet : GloveWeapon
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void GloveDefaults()
        {
            Item.SetWeaponValues(8, //Damage
                1f, //Knockback
                0); //Bonus crit

            Item.width = 20;
            Item.height = 22;
            Item.shootSpeed = 1f;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            gloveWeight = 6;
        }
    }
}
