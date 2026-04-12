namespace CritRework.Content.Prefixes.Augmentation
{
    public class Detrimental : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            critDamageMult = 0.85f;
        }
    }
}
