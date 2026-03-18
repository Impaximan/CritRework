namespace CritRework.Content.Items.Equipable.Accessories
{
    public class ShortestStraw : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 50, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<ShortestStraw>();
            player.AddConsecutiveCritDamage(0.05f);
        }
    }
}
