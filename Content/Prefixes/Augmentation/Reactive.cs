using CritRework.Common.ModPlayers;

namespace CritRework.Content.Prefixes.Augmentation
{
    public class Reactive : AugmentationPrefix
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            PrefixID.Sets.ReducedNaturalChance[Type] = true;
        }

        public override bool DeactivateAugmentation(Item weapon, Item augmentation, Player player, int index, NPC npc = null)
        {
            return player.TryGetModPlayer(out CritPlayer cPlayer) && (cPlayer.timeSinceCrit > 180 || cPlayer.timeSinceCrit == 0);
        }

        public override bool ConditionPrefix => true;

        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {

        }

        public override float RollChance(Item item)
        {
            return base.RollChance(item) * conditionalWeight;
        }
    }
}
