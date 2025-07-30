using Microsoft.Xna.Framework;

namespace CritRework.Common
{
    public abstract class CritType : ILoadable
    {
        public static float CalculateActualCritMult(CritType critType, Item Item, Player Player)
        {
            if (Item == null || Player == null)
            {
                return 1f;
            }
            return critType.GetDamageMult(Player, Item) * (1f + (Player.GetCritChance(Item.DamageType) + Player.GetCritChance<GenericDamageClass>() + Item.crit) / 100f);
        }

        public virtual bool InRandomPool => false;

        public virtual bool CanApplyTo(Item item)
        {
            return true;
        }

#nullable enable
        public abstract bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target);
#nullable disable

        public abstract float GetDamageMult(Player Player, Item Item);

        public abstract string GetDescription();

        public virtual Color Color => new Color(255, 255, 181);


        public virtual bool ForceOnItem(out int itemType)
        {
            itemType = 0;
            return false;
        }

        public void Load(Mod mod)
        {
            CritRework.loadedCritTypes.Add(this);
            if (ForceOnItem(out int itemType))
            {
                CritRework.forcedCritTypes.Add(this);
            }
            else if (InRandomPool)
            {
                CritRework.randomCritPool.Add(this);
            }
        }

        public void Unload()
        {
            CritRework.loadedCritTypes.Remove(this);
            if (ForceOnItem(out int itemType))
            {
                CritRework.forcedCritTypes.Remove(this);
            }
            else if (InRandomPool)
            {
                CritRework.randomCritPool.Remove(this);
            }
        }
    }
}
