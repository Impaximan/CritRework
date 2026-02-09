namespace CritRework.Content.Items.Weapons.Gloves
{
    public class LeatherGlove : GloveWeapon
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Leather, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override void GloveDefaults()
        {
            Item.SetWeaponValues(2, //Damage
                1f, //Knockback
                0); //Bonus crit

            Item.width = 20;
            Item.height = 22;
            Item.shootSpeed = 0.5f;
            Item.value = Item.buyPrice(0, 0, 35, 0);
            gloveWeight = 1;
        }
    }
}
