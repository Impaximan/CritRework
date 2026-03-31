using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;

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
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override bool CanApplyTo(Item weapon)
        {
            return weapon.DamageType == DamageClass.Ranged;
        }

        public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            modifiers.DisableCrit();
            return true;
        }

        static int cooldown = 0;
        public override void HoldItem(Player player)
        {
            if (cooldown > 0)
            {
                cooldown--;
            }
        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target)
        {
            NPC.HitModifiers modifiers = new NPC.HitModifiers();
            if (cooldown <= 0 && critType.ShouldCrit(player, item, projectile, target, modifiers, item.IsSpecial()))
            {
                cooldown = 60;

                float damageMult = critType.GetDamageMult(player, item);

                SoundEngine.PlaySound(SoundID.Item61, player.Center);
            }
        }
    }
}
