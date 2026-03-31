using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.Prefixes.Augmentation
{
    public class Quality_Augment : AugmentationPrefix
    {
        public override void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {
            critDamageMult = 1.3f;
            nonCritDamageMult *= 0.8f;
        }
    }
}
