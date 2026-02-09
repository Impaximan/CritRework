namespace CritRework.Content.Items.Weapons.Gloves
{
    public class TungstenGauntlet : GloveWeapon
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 5)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.TungstenBow);
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
