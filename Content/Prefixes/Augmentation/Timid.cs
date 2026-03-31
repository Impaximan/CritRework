namespace CritRework.Content.Prefixes.Augmentation
{
    public class Timid : AugmentationPrefix
    {
        public override bool DeactivateAugmentation(Item item, Player player, NPC npc = null)
        {
            return player.statLife < player.statLifeMax2 * 0.85f;
        }

        public override bool ConditionPrefix => true;

        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {
            critDamageMult = 1.2f;
            valueMult *= 0.85f;
        }

        public override float RollChance(Item item)
        {
            return base.RollChance(item) * conditionalWeight;
        }
    }
}
