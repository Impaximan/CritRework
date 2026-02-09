using System.Collections.Generic;
using Terraria.Audio;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class Boomerangs : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            List<int> allAffected = new()
            {
                ItemID.WoodenBoomerang,
                ItemID.EnchantedBoomerang,
                ItemID.IceBoomerang,
                ItemID.Flamarang,
                ItemID.Shroomerang,
                ItemID.Trimarang
            };
            return allAffected.Contains(item.type);
        }

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && projectile != null)
            {
                SoundEngine.PlaySound(SoundID.Item56, projectile.Center);
                projectile.velocity = player.DirectionTo(projectile.Center) * (8f + item.knockBack * target.knockBackResist);
            }
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile.ai[0] == 1;
        }
    }
}

