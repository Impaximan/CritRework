namespace CritRework.Content.Items.Equipable.Accessories
{
    public class AssassinsDagger : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<AssassinsDagger>();
            player.GetCritChance(DamageClass.Melee) += 10;
        }
    }
}
