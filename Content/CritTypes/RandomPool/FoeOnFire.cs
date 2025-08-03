using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeOnFire : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.2f;

        //public override string GetDescription() => "Critically strikes while the target is on fire";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
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

