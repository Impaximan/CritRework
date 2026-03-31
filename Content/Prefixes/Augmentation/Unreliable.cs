namespace CritRework.Content.Prefixes.Augmentation
{
    public class Unreliable : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {
            nonCritDamageMult = 0.85f;
            valueMult *= 0.85f;
        }
    }
}