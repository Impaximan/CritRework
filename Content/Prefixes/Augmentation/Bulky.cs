namespace CritRework.Content.Prefixes.Augmentation
{
    public class Bulky : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {
            critDamageMult = 1.3f;
            useTimeMult *= 1.3f;
        }
    }
}
