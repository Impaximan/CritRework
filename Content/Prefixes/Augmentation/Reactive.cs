using CritRework.Common.ModPlayers;

namespace CritRework.Content.Prefixes.Augmentation
{
    public class Reactive : AugmentationPrefix
    {
        public override bool DeactivateAugmentation(Item item, Player player, NPC npc = null)
        {
            return player.TryGetModPlayer(out CritPlayer cPlayer) && cPlayer.timeSinceCrit > 180;
        }

        public override bool ConditionPrefix => true;

        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
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
