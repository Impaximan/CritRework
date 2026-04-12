namespace CritRework.Content.Prefixes.Augmentation
{
    public class Particular : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            potencyMult += 0.2f;
            critDamageMult -= 0.2f;
            nonCritDamageMult -= 0.05f;
            valueMult *= 1.05f;
        }
    }
}
