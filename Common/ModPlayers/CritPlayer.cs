using CritRework.Common.Globals;
using Microsoft.Xna.Framework;

namespace CritRework.Common.ModPlayers
{
    public class CritPlayer : ModPlayer
    {
        public int timeSinceLastHit = 0;
        public bool slotMachineCritCrafting = true;
        public CritType slotMachineCrit = null;
        public Item slotMachineItem = null;

        int slotTime = 0;
        public int currentSlotTime = maxCurrentSlotTime;
        public const int maxCurrentSlotTime = 30;
        public const int minCurrentSlotTime = 5;
        public int timeSinceLastTooltipShown = 0;
        public override void PostUpdate()
        {
            timeSinceLastHit++;

            UpdateSlotMachine();
        }

        public override void UpdateAutopause()
        {
            UpdateSlotMachine();
        }

        public void UpdateSlotMachine()
        {
            timeSinceLastTooltipShown++;

            if (!slotMachineCritCrafting)
            {
                return;
            }

            slotTime--;
            if (slotTime <= 0)
            {
                slotTime = currentSlotTime;

                if (timeSinceLastTooltipShown > 300)
                {
                    currentSlotTime = (int)MathHelper.Lerp(currentSlotTime, maxCurrentSlotTime, 0.15f);
                }
                else if (timeSinceLastTooltipShown <= 2)
                {
                    currentSlotTime = (int)MathHelper.Lerp(currentSlotTime, minCurrentSlotTime, 0.075f);
                }

                if (slotMachineItem != null)
                {
                    if (slotMachineItem.damage == -1 || slotMachineItem.ammo != AmmoID.None)
                    {
                        return;
                    }

                    foreach (CritType crit in CritRework.forcedCritTypes)
                    {
                        crit.ForceOnItem(out int itemType);

                        if (slotMachineItem.type == itemType)
                        {
                            slotMachineCrit = crit;
                            return;
                        }
                    }


                    if (CritRework.randomCritPool.Count <= 0 || slotMachineItem.consumable)
                    {
                        return;
                    }


                    if (!slotMachineItem.DamageType.CountsAsClass(DamageClass.Summon) && CritRework.randomCritPool.Exists(x => x.CanApplyTo(slotMachineItem)))
                    {
                        CritType appliedType = Main.rand.Next(CritRework.randomCritPool);
                        while (!appliedType.CanApplyTo(slotMachineItem))
                        {
                            appliedType = Main.rand.Next(CritRework.randomCritPool);
                        }
                        slotMachineCrit = appliedType;
                    }
                }
            }
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

            modifiers = ApplyModifiers(item, null, modifiers, critItem.critType, target);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            CritProjectile critProj = proj.GetGlobalProjectile<CritProjectile>();

            if (critProj == null)
            {
                modifiers.DisableCrit();
                return;
            }

            modifiers = ApplyModifiers(critProj.ogItem, proj, modifiers, critProj.critType, target);
        }

        private NPC.HitModifiers ApplyModifiers(Item item, Projectile? projectile, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            if (critType != null && critType.ShouldCrit(Player, item, projectile, target))
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
