namespace CritRework.Content.Prefixes.Augmentation
{
    public class Impotent : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult, ref float potencyMult)
        {
            potencyMult -= 0.2f;
        }
    }
}
