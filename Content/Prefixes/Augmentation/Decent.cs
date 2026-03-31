namespace CritRework.Content.Prefixes.Augmentation
{
    public class Decent : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {
            critDamageMult = 1.07f;
            nonCritDamageMult *= 0.95f;
            valueMult *= 1.05f;
        }
    }
}
