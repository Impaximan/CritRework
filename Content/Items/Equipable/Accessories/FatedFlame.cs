namespace CritRework.Content.Items.Equipable.Accessories
{
    public class FatedFlame : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 16;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            if (player.statLife <= player.statLifeMax2 * 0.5f)
            {
                player.AddPotency(0.3f);
            }
        }
    }
}
