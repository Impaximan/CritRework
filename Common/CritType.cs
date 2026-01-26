using CritRework.Common.ModPlayers;
using Microsoft.Xna.Framework;

namespace CritRework.Common
{
    public abstract class CritType : ILoadable
    {
        bool loaded = false;

        public static float CalculateActualCritMult(CritType critType, Item Item, Player Player)
        {
            if (Item == null || Player == null)
            {
                return 1f;
            }

            //Nerfed scaling
            float s = critType.GetDamageMult(Player, Item);

            if (Player.TryGetModPlayer(out CritPlayer player) && player.prostheticCrit != null && player.prostheticCrit != critType)
            {
                s *= (player.prostheticCrit.GetDamageMult(Player, Item) - 1f) * 1.5f + 1f;
            }

            float c = (100f + Player.GetWeaponCrit(Item)) / 100f - 1f;

            float finalValue;

            switch (CritRework.critScaling)
            {
                default:
                    finalValue = ((s - 1) * (c / 2 + 1) + 1) * (1 + c / 2);
                    break;
                case "Simple":
                    finalValue = critType.GetDamageMult(Player, Item) * (1f + Player.GetWeaponCrit(Item) / 100f);
                    break;
            }
           
            return MathHelper.Lerp(1f, finalValue, CritRework.critPower);
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
        public LocalizedText specialPrefixName;
        public LocalizedText specialPrefixTooltip1;
        public LocalizedText specialPrefixTooltip2;
        public LocalizedText name;

        public virtual string InternalName => GetType().Name;
        public virtual bool InRandomPool => false;

        public virtual bool ShowWhenActive => false;

        public virtual bool CanApplyTo(Item item)
        {
            return true;
        }

        public virtual bool ShouldLoad()
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
            if (!ShouldLoad())
            {
                return;
            }

            loaded = true;
            description = mod.GetLocalization($"CritTypes.{GetType().Name}.Description");
            specialPrefixName = mod.GetLocalization($"CritTypes.{GetType().Name}.SpecialPrefixName");
            specialPrefixTooltip1 = mod.GetLocalization($"CritTypes.{GetType().Name}.SpecialPrefixTooltip1");
            specialPrefixTooltip2 = mod.GetLocalization($"CritTypes.{GetType().Name}.SpecialPrefixTooltip2");

            CritRework.loadedCritTypes.Add(this);
            if (InRandomPool)
            {
                CritRework.randomCritPool.Add(this);
            }

            OnLoad(mod);
        }

        public virtual void OnLoad(Mod mod)
        {

        }

        public void Unload()
        {
            if (!loaded)
            {
                return;
            }

            loaded = false;
            CritRework.loadedCritTypes.Remove(this);
            if (InRandomPool)
            {
                CritRework.randomCritPool.Remove(this);
            }

            OnUnload();
        }

        public virtual void OnUnload()
        {

        }
    }
}
