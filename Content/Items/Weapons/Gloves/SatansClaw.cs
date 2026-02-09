namespace CritRework.Content.Items.Weapons.Gloves
{
    public class SatansClaw : GloveWeapon
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 9)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.MoltenFury);
        }

        public override void GloveDefaults()
        {
            Item.SetWeaponValues(12, //Damage
                1f, //Knockback
                0); //Bonus crit

            Item.width = 20;
            Item.height = 22;
            Item.shootSpeed = 8f;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Orange;
            gloveWeight = 1;
        }

        public override float AttackTimeMult => 0.5f;

        public override void HoldItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<Repentence>(), 2);
        }
    }

    public class Repentence : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.LongerExpertDebuff[Type] = false;
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            Dust.NewDust(player.position, player.width, player.height, DustID.Torch, player.velocity.X, player.velocity.Y);
        }
    }
}
