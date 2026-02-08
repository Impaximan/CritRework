using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoePoisoned : CritType
    {
        public override bool InRandomPool => true;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile? projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                target.AddBuff(BuffID.Venom, 60 * 10);
            }
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.2f;

        //public override string GetDescription() => "Critically strikes while the target is poisoned";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            List<int> countedBuffs = new()
            {
                BuffID.Poisoned,
                BuffID.Venom
            };

            foreach (int id in countedBuffs)
            {
                if (target.HasBuff(id)) return true;
            }

            return false;
        }
    }
}

