using CritRework.Content.Items.Augmentations;

namespace CritRework.Content.Prefixes.Augmentation
{
    public class Panicked : AugmentationPrefix
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            PrefixID.Sets.ReducedNaturalChance[Type] = true;
        }

        public override bool DeactivateAugmentation(Item weapon, Item augmentation, Player player, int index, NPC npc = null)
        {
            return player.statLife > player.statLifeMax2 * 0.3f;
        }

        public override bool ConditionPrefix => true;

        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            critDamageMult = 1.1f;
            valueMult *= 0.9f;
        }

        public override float RollChance(Item item)
        {
            return base.RollChance(item) * conditionalWeight;
        }
    }
}
