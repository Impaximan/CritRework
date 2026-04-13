namespace CritRework.Content.Items.Equipable.Accessories
{
    public class TiaraShard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public static float potencyAdded = 0.2f;

        public override void UpdateEquip(Player player)
        {
            potencyAdded = 0.2f;
            player.AddEquip<TiaraShard>();
        }
    }
}
