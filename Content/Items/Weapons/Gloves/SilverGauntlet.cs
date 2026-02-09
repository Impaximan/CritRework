namespace CritRework.Content.Items.Weapons.Gloves
{
    public class SilverGauntlet : GloveWeapon
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 5)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.SilverBow);
        }

        public override void GloveDefaults()
        {
            Item.SetWeaponValues(6, //Damage
                1f, //Knockback
                0); //Bonus crit

            Item.width = 20;
            Item.height = 22;
            Item.shootSpeed = 1.5f;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            gloveWeight = 4;
        }
    }
}
