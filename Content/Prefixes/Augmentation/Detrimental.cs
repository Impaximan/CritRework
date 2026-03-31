namespace CritRework.Content.Prefixes.Augmentation
{
    public class Detrimental : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {
            critDamageMult = 0.85f;
            valueMult *= 0.85f;
        }
    }
}
