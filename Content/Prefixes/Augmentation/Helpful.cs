namespace CritRework.Content.Prefixes.Augmentation
{
    public class Helpful : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            critDamageMult = 1.05f;
            nonCritDamageMult = 1.03f;
            valueMult *= 1.01f;
        }
    }
}
