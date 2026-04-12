namespace CritRework.Content.Prefixes.Augmentation
{
    public class Critical : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            critDamageMult = 1.08f;
            valueMult *= 1.05f;
        }
    }
}
