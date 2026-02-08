using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeConfused : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && Main.rand.NextBool(20))
            {
                target.AddBuff(BuffID.Confused, 600);
            }
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
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

