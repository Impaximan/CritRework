using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.CritTypes
{
    public class ModCalledCritType : CritType {
		public override string InternalName => $"{Mod.Name}/{internalName}";
		public override bool InRandomPool => inRandomPool;

        public Mod Mod;

        public string internalName;

        public bool inRandomPool;

        public Func<Player, Item, Projectile, NPC, NPC.HitModifiers, bool> shouldCrit;

        public Func<Player, Item, float> getDamageMult;

        public Func<Item, bool> forceOnItem;

        public Func<Item, bool> canApplyTo;

        public ModCalledCritType(Mod mod, bool inRandomPool, Func<Player, Item, Projectile, NPC, NPC.HitModifiers, bool> shouldCrit, Func<Player, Item, float> getDamageMult, Func<Item, bool> forceOnItem, Func<Item, bool> canApplyTo, LocalizedText description, string internalName)
        {
            Mod = mod;
            this.inRandomPool = inRandomPool;
            this.shouldCrit = shouldCrit;
            this.getDamageMult = getDamageMult;
            this.description = description;
            this.internalName = internalName;
            this.forceOnItem = forceOnItem;
            this.canApplyTo = canApplyTo;
        }

        public override bool CanApplyTo(Item item)
        {
            return canApplyTo(item);
        }

        public override bool ForceOnItem(Item item)
        {
            return forceOnItem(item);
        }

        public override float GetDamageMult(Player Player, Item Item)
        {
            return getDamageMult(Player, Item);
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return shouldCrit(Player, Item, Projectile, target, modifiers);
        }
    }
}
