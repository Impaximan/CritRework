using Terraria.DataStructures;

namespace CritRework.Common.Globals
{
    public class CritProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public CritType critType = null;
        public Item ogItem = null;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse itemSource)
            {
                if (itemSource.Item.GetGlobalItem<CritItem>() != null)
                {
                    critType = itemSource.Item.GetGlobalItem<CritItem>().critType;
                    ogItem = itemSource.Item;
                }
            }

            if (source is EntitySource_Parent parentSource)
            {
                if (parentSource.Entity is Projectile parent)
                {
                    if (parent.GetGlobalProjectile<CritProjectile>() != null)
                    {
                        critType = parent.GetGlobalProjectile<CritProjectile>().critType;
                        ogItem = parent.GetGlobalProjectile<CritProjectile>().ogItem;
                    }
                }
            }
        }
    }
}
