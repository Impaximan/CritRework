namespace CritRework.Content.Items.Equipable.Accessories
{
    public class CrystalShield : ModItem
    {
        public static int maxDefense = 50;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrystalShard, 25)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.UnicornHorn, 1)
                .AddIngredient(ItemID.TitaniumBar, 7)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.CrystalShard, 25)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.UnicornHorn, 1)
                .AddIngredient(ItemID.AdamantiteBar, 7)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 42;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 5;
            Item.value = Item.sellPrice(0, 6, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 5;
            if (!player.HasBuff<CrystalShieldCooldown>())
            {
                player.AddEquip<CrystalShield>();
            }

        }
    }

    public class CrystalShieldCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
    }
}
