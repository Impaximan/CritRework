using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeOnFire : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.2f;

        public override string GetDescription() => "Critically strikes while the target is on fire";

        public override bool CanApplyTo(Item item)
        {
            List<int> blacklist = new()
            {
                ItemID.ShadowFlameKnife,
            };

            return item.useAmmo != AmmoID.Gel && !blacklist.Contains(item.type);
        }

        public override bool ShouldCrit(Player Player, Item Item, NPC target)
        {
            List<int> countedBuffs = new()
            {
                BuffID.OnFire,
                BuffID.OnFire3,
                BuffID.ShadowFlame,
                BuffID.CursedInferno,
                BuffID.Frostburn,
                BuffID.Frostburn2
            };

            foreach (int id in countedBuffs)
            {
                if (target.HasBuff(id)) return true;
            }

            return false;
        }
    }
}

