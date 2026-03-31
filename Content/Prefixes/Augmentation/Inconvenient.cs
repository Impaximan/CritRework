namespace CritRework.Content.Prefixes.Augmentation
{
    public class Inconvenient : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {
            useTimeMult *= 1.3f;
        }
    }
}
