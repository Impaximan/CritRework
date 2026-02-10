using CritRework.Common.Globals;
using CritRework.Content.CritTypes.RandomPool;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Whetstones
{
    public class BasicWhetstone : Whetstone
    {
        public override CritType AssociatedCritType
        {
            get
            {
                if (Item.TryGetCritType(out CritType critType))
                {
                    return critType;
                }
                else
                {
                    return null;
                }
            }
        }

        public override void OnCreated(ItemCreationContext context)
        {
            if (context is not InitializationItemCreationContext)
            {
                if (!Item.TryGetCritType(out CritType _))
                {
                    Item.GetGlobalItem<CritItem>().AddCritType(Item);
                }
            }
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.maxStack = 20;
            Item.GetGlobalItem<CritItem>().forceCanCrit = true;
        }
    }
}
