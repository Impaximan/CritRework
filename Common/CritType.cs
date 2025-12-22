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

            return MathHelper.Lerp(1f, critType.GetDamageMult(Player, Item) * (1f + (Player.GetWeaponCrit(Item)) / 100f), CritRework.critPower);
        }

        public static T Get<T>() where T : CritType
        {
            return CritRework.loadedCritTypes.Find(x => x is T) as T;
        }

        public static CritType Get(string typeAsString)
        {
            return CritRework.GetCrit(typeAsString);
        }

        public LocalizedText description;
        public LocalizedText name;

        public virtual string InternalName => GetType().Name;
        public virtual bool InRandomPool => false;

        public virtual bool ShowWhenActive => false;

        public virtual bool CanApplyTo(Item item)
        {
            return true;
        }

#nullable enable
        public abstract bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers);
#nullable disable

        public abstract float GetDamageMult(Player Player, Item Item);

        public virtual Color Color => new Color(255, 255, 181);


        public virtual bool ForceOnItem(Item item)
        {
            return false;
        }

        public void Load(Mod mod)
        {
            description = mod.GetLocalization($"CritTypes.{GetType().Name}.Description");

            CritRework.loadedCritTypes.Add(this);
            if (InRandomPool)
            {
                CritRework.randomCritPool.Add(this);
            }
        }

        public void Unload()
        {
            CritRework.loadedCritTypes.Remove(this);
            if (InRandomPool)
            {
                CritRework.randomCritPool.Remove(this);
            }
        }
    }
}
