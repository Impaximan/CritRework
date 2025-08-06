using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeConfused : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        //public override string GetDescription() => "Critically strikes while the target is confused";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            List<int> countedBuffs = new()
            {
                BuffID.Confused,
            };

            foreach (int id in countedBuffs)
            {
                if (target.HasBuff(id)) return true;
            }

            return false;
        }
    }
}

