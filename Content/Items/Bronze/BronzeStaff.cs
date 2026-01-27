using Microsoft.Xna.Framework;

namespace CritRework.Content.Items.Bronze
{
    public class BronzeStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 3, 50, 0);
            Item.damage = 30;
            Item.mana = 15;
            Item.crit = 30;
            Item.knockBack = 3f;
            Item.width = 50;
            Item.height = 50;
            Item.shootSpeed = 15f;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.UseSound = SoundID.Item109;
            Item.shoot = ProjectileID.DiamondBolt;
            Item.noMelee = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.BronzeAlloy>(15)
                .AddIngredient(ItemID.Diamond, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BronzeStaffCrit : CritType
    {
        public override float GetDamageMult(Player Player, Item Item) => 3f;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ModContent.ItemType<BronzeStaff>();
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.statMana <= 20;
        }
    }
}
