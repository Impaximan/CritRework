using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.Items.Augmentations
{
    public class AMPL : Augmentation
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.UseSound = SoundID.Item149;
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
