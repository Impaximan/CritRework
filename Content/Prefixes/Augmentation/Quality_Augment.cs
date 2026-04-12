namespace CritRework.Content.Prefixes.Augmentation
{
    public class Quality_Augment : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            critDamageMult = 1.15f;
            nonCritDamageMult *= 0.8f;
            valueMult *= 1.25f;
        }
    }
}
