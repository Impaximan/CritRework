namespace CritRework.Content.Prefixes.Augmentation
{
    public class Persistent : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            critDamageMult = 1.03f;
            nonCritDamageMult = 1.05f;
        }
    }
}
