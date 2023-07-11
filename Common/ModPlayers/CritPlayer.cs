using CritRework.Common.Globals;
using Terraria;

namespace CritRework.Common.ModPlayers
{
    public class CritPlayer : ModPlayer
    {
        public int timeSinceLastHit = 0;

        public override void PostUpdate()
        {
            timeSinceLastHit++;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            timeSinceLastHit = 0;
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            timeSinceLastHit = 0;
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            CritItem critItem = item.GetGlobalItem<CritItem>();

            if (critItem == null)
            {
                modifiers.DisableCrit();
                return;
            }

            modifiers = ApplyModifiers(item, modifiers, critItem.critType, target);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            CritProjectile critProj = proj.GetGlobalProjectile<CritProjectile>();

            if (critProj == null)
            {
                modifiers.DisableCrit();
                return;
            }

            modifiers = ApplyModifiers(critProj.ogItem, modifiers, critProj.critType, target);
        }

        private NPC.HitModifiers ApplyModifiers(Item item, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            if (critType != null && critType.ShouldCrit(Player, item, target))
            {
                modifiers.SetCrit();
                modifiers.SourceDamage *= CritType.CalculateActualCritMult(critType, item, Player);
                modifiers.FinalDamage *= 0.5f;
            }
            else
            {
                modifiers.DisableCrit();
            }

            return modifiers;
        }

    }
}
