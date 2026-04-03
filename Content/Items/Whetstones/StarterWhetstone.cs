using CritRework.Content.CritTypes.WhetstoneSpecific;

namespace CritRework.Content.Items.Whetstones
{
    public class StarterWhetstone : Whetstone
    {
        public override CritType AssociatedCritType => CritType.Get<Starter>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.maxStack = 20;
        }
    }
}
