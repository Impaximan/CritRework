using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace CritRework.Common.Globals
{
    public class CritProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public CritType critType = null;
        public Item ogItem = null;
        public int targetsHit = 0;
        public int wallBounces = 0;
        public int timeActive = 0;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            targetsHit++;
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            wallBounces++;
            return base.OnTileCollide(projectile, oldVelocity);
        }

        public override void PostAI(Projectile projectile)
        {
            timeActive++;
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            targetsHit = 0;
            wallBounces = 0;
            timeActive = 0;
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
