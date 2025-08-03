namespace CritRework.Content.Items.Whetstones
{
    public class AmmoWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.Ammo>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.maxStack = 20;
        }
    }
}
