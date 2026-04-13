namespace CritRework.Content.Items.Equipable.Accessories
{
    public class CrownShard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<CrownShard>();
        }
    }
}
