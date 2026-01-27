using CritRework.Common.Globals;

namespace CritRework.Content.Items.Weapons.Gloves
{
    public class GalacticGauntlet : GloveWeapon
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 16)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.SpaceGun);
        }

        public override void GloveDefaults()
        {
            Item.SetWeaponValues(18, //Damage
                1f, //Knockback
                0); //Bonus crit

            Item.width = 20;
            Item.height = 22;
            Item.shootSpeed = 3f;
            Item.value = Item.sellPrice(0, 0, 12, 0);
            Item.rare = ItemRarityID.Blue;
            gloveWeight = 14;
        }
    }

    public class GalacticCrit : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 2.5f;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ModContent.ItemType<GalacticGauntlet>();
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile.TryGetGlobalProjectile(out CritProjectile c) && c.thrownUpward && Projectile.Center.Y - c.highestPoint >= 16 * 10;
        }
    }
}
