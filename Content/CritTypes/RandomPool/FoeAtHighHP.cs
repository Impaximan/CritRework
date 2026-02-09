using CritRework.Common.ModPlayers;
using Terraria.Audio;
using System;

namespace CritRework.Content.CritTypes.RandomPool
{
    public class FoeAtHighHP : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item)
        {
            if (Item != null && Item.IsSpecial() && Player.TryGetModPlayer(out CritPlayer c))
            {
                return 2f * c.highHpCritMult;
            }
            return 2f;
        }

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.TryGetModPlayer(out CritPlayer c))
            {
                if (hit.Crit && target.life <= 0)
                {
                    if (!target.CountsAsACritter)
                    {
                        c.highHpCritMult += 0.3f;
                        SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/SpecialReforge")
                        {
                            Pitch = Math.Clamp((c.highHpCritMult - 1f) / 8f, 0f, 1f)
                        });
                        CombatText.NewText(player.getRect(), Color.LightBlue, Math.Round(((c.highHpCritMult - 1f) * 100f)).ToString() + "%", true);
                    }
                }
                else if (c.highHpCritMult > 1f)
                {
                    c.highHpCritMult = 1f;
                    CombatText.NewText(target.getRect(), Color.Crimson, "Desperado!", true);
                }
            }
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return target.life >= target.lifeMax * 0.85f;
        }
    }
}
