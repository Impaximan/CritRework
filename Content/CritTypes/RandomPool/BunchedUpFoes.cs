using CritRework.Common.ModPlayers;
using System.Collections.Generic;
using Terraria.Audio;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class BunchedUpFoes : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Grenade;
        }

        public override float GetDamageMult(Player Player, Item Item) => 2f;

        const int minNumber = 4;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.TryGetModPlayer(out CritPlayer critPlayer) && critPlayer.timeSinceGravityWell > 60)
            {
                critPlayer.timeSinceGravityWell = 0;
                GravityWell(target.Center, hit.Crit ? 750 : 500, hit.Crit ? 8 : 6, hit, damageDone, player);
            }
        }

        public void GravityWell(Vector2 location, float radius, float pull, NPC.HitInfo hit, int damageOriginallyDone, Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/EVILCrit")
            {
                PitchVariance = 0.5f    
            }, location);

            for (int d = 0; d < Main.rand.Next(150, 200); d++)
            {
                float theta = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 position = location + theta.ToRotationVector2() * Main.rand.NextFloat(0.9f, 1.1f) * radius;
                Vector2 velocity = theta.ToRotationVector2() * Main.rand.NextFloat(0.5f, 1.5f) * -radius / 15f;
                Dust dust = Dust.NewDustPerfect(position, hit.Crit ? DustID.RedTorch : DustID.PurpleTorch, velocity);
                dust.noGravity = true;
                dust.noLight = true;
                if (hit.Crit) dust.scale *= 2f;
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Center != location && npc.getRect().ClosestPointInRect(location).Distance(location) < radius)
                {
                    npc.velocity += npc.knockBackResist * pull * npc.DirectionTo(location) * System.Math.Clamp(npc.Distance(location), 0f, radius / 2f) / 100f;
                    if (!npc.noGravity) npc.velocity.Y -= 10f * npc.knockBackResist;

                    if (hit.Crit && !npc.dontTakeDamage)
                    {
                        float damageReduction = 1f;

                        List<int> wormBosses = new()
                        {
                            NPCID.EaterofWorldsBody,
                            NPCID.EaterofWorldsHead,
                            NPCID.EaterofWorldsTail,
                            NPCID.TheDestroyer,
                            NPCID.TheDestroyerBody,
                            NPCID.TheDestroyerTail
                        };

                        if (wormBosses.Contains(npc.type))
                        {
                            damageReduction *= 0.3f;
                        }


                        player.addDPS(npc.SimpleStrikeNPC((int)(hit.SourceDamage * damageReduction), 0, true, 0f, hit.DamageType, true, player.luck));
                    }

                    npc.netUpdate = true;
                }
            }
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            int numberOfEnemies = 0;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(target.Center) <= 175 && npc.lifeMax >= 10)
                {
                    numberOfEnemies++;
                    if (numberOfEnemies >= minNumber)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
