using CritRework.Common.ModPlayers;

namespace CritRework.Content.Prefixes.Augmentation
{
    public class Clockwork : AugmentationPrefix
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            PrefixID.Sets.ReducedNaturalChance[Type] = true;
        }

        public override bool DeactivateAugmentation(Item weapon, Item augmentation, Player player, int index, NPC npc = null)
        {
            if (index % 2 == 1)
            {
                return player.TryGetModPlayer(out CritPlayer ca) && ca.clockworkCounter % 240 > 120;
            }
            return player.TryGetModPlayer(out CritPlayer cb) && cb.clockworkCounter % 240 <= 120;
        }

        public override bool ConditionPrefix => true;

        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            critDamageMult = 1.05f;
            valueMult *= 0.97f;
        }

        public override float RollChance(Item item)
        {
            return base.RollChance(item) * conditionalWeight;
        }
    }
}
