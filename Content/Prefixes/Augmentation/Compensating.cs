namespace CritRework.Content.Prefixes.Augmentation
{
    public class Compensating : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            nonCritDamageMult = 1.08f;
        }
    }
}
