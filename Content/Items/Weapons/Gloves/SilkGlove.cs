namespace CritRework.Content.Items.Weapons.Gloves
{
    public class SilkGlove : GloveWeapon
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.Loom)
                .Register();
        }

        public override void GloveDefaults()
        {
            Item.SetWeaponValues(1, //Damage
                1f, //Knockback
                0); //Bonus crit

            Item.width = 20;
            Item.height = 22;
            Item.shootSpeed = 4.5f;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            gloveWeight = 0;
        }
    }
}
