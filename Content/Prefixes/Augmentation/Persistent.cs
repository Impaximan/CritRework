namespace CritRework.Content.Prefixes.Augmentation
{
    public class Persistent : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {
            critDamageMult = 1.03f;
            nonCritDamageMult = 1.05f;
            valueMult *= 1.05f;
        }
    }
}
