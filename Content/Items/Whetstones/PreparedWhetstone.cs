namespace CritRework.Content.Items.Whetstones
{
    public class PreparedWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<CritTypes.WhetstoneSpecific.Prepared>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 12, 50, 0);
            Item.maxStack = 20;
        }
    }
}
