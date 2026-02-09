namespace CritRework.Content.Items.Weapons
{
    public class RabidClaw : ModItem
    {
        public override void SetDefaults()
        {
            Item.SetWeaponValues(12, 2f, 25);
            Item.width = 24;
            Item.height = 16;
            Item.noMelee = false;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.scale = 1.25f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                player.AddBuff(ModContent.BuffType<RabidFrenzy>(), 60 * 5, false);
            }
        }
    }

    public class RabidFrenzy : ModBuff
    {
        public override void SetStaticDefaults()
        {

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.3f;
        }
    }
}
