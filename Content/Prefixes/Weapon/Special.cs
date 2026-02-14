
using CritRework.Common.Globals;
using System.Collections.Generic;

namespace CritRework.Content.Prefixes.Weapon
{
    public class Special : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return CritItem.CanHaveCrits(item);
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 2f;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {

        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            if (item.TryGetGlobalItem(out CritItem c) && c.critType != null)
            {
                List<TooltipLine> lines = [new TooltipLine(Mod, "SpecialModifier1", c.critType.specialPrefixTooltip1.Value) {
                    IsModifier = true,
                }];

                if (c.critType.specialPrefixTooltip2.Value != "" && c.critType.specialPrefixTooltip2.Value != " ")
                {
                    lines.Add(new TooltipLine(Mod, "SpecialModifier2", c.critType.specialPrefixTooltip2.Value)
                    {
                        IsModifier = true,
                    });
                }

                return lines;
            }

            return base.GetTooltipLines(item);
        }

        public override float RollChance(Item item)
        {
            return 0.5f;
        }
    }
}
