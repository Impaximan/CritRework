namespace CritRework.Content.Items.Equipable.Accessories
{
    public class NoxiousEye : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 2, 25, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<NoxiousEye>();
            player.GetCritChance(DamageClass.Generic) += 5;
        }
    }
}
