using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class Flamethrower : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(out int itemType)
        {
            itemType = ItemID.Flamethrower;
            return true;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override string GetDescription() => "Critically strikes while the target is covered in slime";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
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

    internal class ElfMelter : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(out int itemType)
        {
            itemType = ItemID.ElfMelter;
            return true;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override string GetDescription() => "Critically strikes while the target is covered in slime";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
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

