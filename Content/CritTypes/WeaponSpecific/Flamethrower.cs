using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class Flamethrower : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Flamethrower || item.type == ItemID.ElfMelter;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        //public override string GetDescription() => "Critically strikes while the target is covered in slime";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            List<int> countedBuffs = new()
            {
                BuffID.Slimed,
                320
            };

            foreach (int id in countedBuffs)
            {
                if (target.HasBuff(id)) return true;
            }

            return false;
        }
    }
}

