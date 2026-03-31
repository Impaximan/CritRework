namespace CritRework.Content.Prefixes.Augmentation
{
    public class Helpful : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {
            critDamageMult = 1.05f;
            nonCritDamageMult = 1.03f;
            valueMult *= 1.05f;
        }
    }
}
