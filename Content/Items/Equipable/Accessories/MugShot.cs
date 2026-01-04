namespace CritRework.Content.Items.Equipable.Accessories
{
    public class MugShot : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<MugShot>();
            player.GetCritChance(DamageClass.Ranged) += 10;
        }
    }
}
