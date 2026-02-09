namespace CritRework.Content.Items.Bronze
{
    public class BronzeBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.damage = 22;
            Item.crit = 30;
            Item.knockBack = 5f;
            Item.width = 16;
            Item.height = 46;
            Item.shootSpeed = 15f;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item5;
            Item.shoot = 1;
            Item.noMelee = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly) type = ModContent.ProjectileType<BronzeArrowProjectile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.BronzeAlloy>(15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BronzeBowCrit : CritType
    {
        public override float GetDamageMult(Player Player, Item Item) => 2f;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ModContent.ItemType<BronzeBow>();
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            float dist = target.getRect().ClosestPointInRect(Player.Center).Distance(Player.Center);
            return dist > 16f * 15 && dist < 16 * 20f;
        }
    }
}
