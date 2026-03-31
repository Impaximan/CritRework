using CritRework.Common.Globals;
using CritRework.Content.Items.Augmentations;
using System.Collections.Generic;
using System;

namespace CritRework.Content.Prefixes.Augmentation
{
    public abstract class AugmentationPrefix : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.ModItem is Items.Augmentations.Augmentation;
        }

        public sealed override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
        }

        public virtual void SetStats(ref float critDamageMult, ref float nonCritDamageMult, ref float useTimeMult, ref float valueMult)
        {

        }

        public sealed override void ModifyValue(ref float valueMult)
        {
            float critDamageMult = 1f;
            float nonCritDamageMult = 1f;
            float useTimeMult = 1f;

            SetStats(ref critDamageMult, ref nonCritDamageMult, ref useTimeMult, ref valueMult);

            valueMult *= critDamageMult;
            valueMult *= nonCritDamageMult;
            valueMult /= useTimeMult;
        }


        public sealed override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            if (item.TryGetGlobalItem(out CritItem c))
            {
                List<TooltipLine> lines = new();

                float critDamageMult = 1f;
                float nonCritDamageMult = 1f;
                float useTimeMult = 1f;
                float valuemult = 1f;

                SetStats(ref critDamageMult, ref nonCritDamageMult, ref useTimeMult, ref valuemult);

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
