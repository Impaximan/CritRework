namespace CritRework.Content.Items.Equipable.Accessories
{
    public class TeacherEmblem : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SorcererEmblem)
                .AddIngredient(ItemID.SummonerEmblem)
                .AddIngredient(ItemID.SoulofLight)
                .AddIngredient(ItemID.SoulofNight)
                .AddIngredient(ItemID.SoulofFlight)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Summon) += 15;
            player.GetCritChance(DamageClass.Magic) += 15;
        }
    }
}
