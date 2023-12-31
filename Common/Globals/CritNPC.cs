﻿using Terraria.Audio;

namespace CritRework.Common.Globals
{
    public class CritNPC : GlobalNPC
    {

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            SoundStyle style = new("CritRework/Assets/Sounds/Crit")
            {
                PitchVariance = 0.5f,
                Pitch = -0.3f,
                SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                MaxInstances = 10,
                Volume = 1.75f
            };

            if (CritRework.critSounds && hit.Crit && player.whoAmI == Main.myPlayer) SoundEngine.PlaySound(style);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            SoundStyle style = new("CritRework/Assets/Sounds/Crit")
            {
                PitchVariance = 0.5f,
                Pitch = -0.3f,
                SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                MaxInstances = 10,
                Volume = 1.75f
            };

            if (CritRework.critSounds && hit.Crit && projectile.owner == Main.myPlayer) SoundEngine.PlaySound(style);
        }

    }
}
