namespace CritRework.Content.Items.Equipable.Accessories
{
    public class BattlemageEmblem : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WarriorEmblem)
                .AddIngredient(ItemID.SorcererEmblem)
                .AddIngredient(ItemID.SoulofMight, 3)
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
            player.GetCritChance(DamageClass.Melee) += 15;
            player.GetCritChance(DamageClass.Magic) += 15;
        }
    }
}
