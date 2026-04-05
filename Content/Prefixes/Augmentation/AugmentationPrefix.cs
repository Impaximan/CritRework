using CritRework.Common.Globals;
using System.Collections.Generic;
using System;
using CritRework.Content.Items.Augmentations;

namespace CritRework.Content.Prefixes.Augmentation
{
    public abstract class AugmentationPrefix : ModPrefix
    {
        public const float conditionalWeight = 0.5f;

        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.ModItem is Items.Augmentations.Augmentation;
        }

        LocalizedText disableTooltip;
        public override void SetStaticDefaults()
        {
            if (ConditionPrefix) disableTooltip = Mod.GetLocalization($"Prefixes.{GetType().Name}.DisableCondition");
        }

        public sealed override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
        }

        public virtual void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {

        }

        public override void ModifyValue(ref float valueMult)
        {
            float critDamageMult = 1f;
            float nonCritDamageMult = 1f;
            float useTimeMult = 1f;

            SetStats(ref critDamageMult, ref nonCritDamageMult, ref useTimeMult, ref valueMult);

            valueMult *= critDamageMult;
            valueMult *= nonCritDamageMult;
            valueMult /= useTimeMult;
        }

        /// <summary>
        /// NPC may be null. Return true when this prefix should disable the augmentation's effect
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        /// <param name="npc"></param>
        public virtual bool DeactivateAugmentation(Item weapon, Item augmentation, Player player, NPC? npc = null)
        {
            return false;
        }

        public virtual bool ConditionPrefix => false;


        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            if (item.TryGetGlobalItem(out CritItem c))
            {
                List<TooltipLine> lines = new();

                float critDamageMult = 1f;
                float nonCritDamageMult = 1f;
                float useTimeMult = 1f;
                float valuemult = 1f;

                SetStats(ref critDamageMult, ref nonCritDamageMult, ref useTimeMult, ref valuemult);

                if (ConditionPrefix)
                {
                    lines.Add(new TooltipLine(Mod, "PrefixCondition", "• " + disableTooltip.Value)
                    {
                        IsModifier = true,
                        OverrideColor = new Color(102, 166, 226)
                    });
                }

                if (critDamageMult > 1f)
                {
                    lines.Add(new TooltipLine(Mod, "PrefixDamage", "+" + Math.Round((critDamageMult - 1f) * 100f).ToString() + "% critical strike damage")
                    {
                        IsModifier = true
                    });
                }
                else if (critDamageMult < 1f)
                {
                    lines.Add(new TooltipLine(Mod, "PrefixDamage", "-" + Math.Round((critDamageMult - 1f) * -100f).ToString() + "% critical strike damage")
                    {
                        IsModifier = true,
                        IsModifierBad = true
                    });
                }

                if (nonCritDamageMult > 1f)
                {
                    lines.Add(new TooltipLine(Mod, "PrefixDamage", "+" + Math.Round((nonCritDamageMult - 1f) * 100f).ToString() + "% non-critical strike damage")
                    {
                        IsModifier = true
                    });
                }
                else if (nonCritDamageMult < 1f)
                {
                    lines.Add(new TooltipLine(Mod, "PrefixDamage", "-" + Math.Round((nonCritDamageMult - 1f) * -100f).ToString() + "% non-critical strike damage")
                    {
                        IsModifier = true,
                        IsModifierBad = true
                    });
                }

                if (useTimeMult > 1f)
                {
                    lines.Add(new TooltipLine(Mod, "PrefixDamage", "-" + Math.Round((useTimeMult - 1f) * 100f).ToString() + "% use speed")
                    {
                        IsModifier = true,
                        IsModifierBad = true
                    });
                }
                else if (useTimeMult < 1f)
                {
                    lines.Add(new TooltipLine(Mod, "PrefixDamage", "+" + Math.Round((useTimeMult - 1f) * -100f).ToString() + "% use speed")
                    {
                        IsModifier = true
                    });
                }

                return lines;
            }

            return base.GetTooltipLines(item);
        }
    }
}
