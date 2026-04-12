using CritRework.Common.Globals;
using System.Collections.Generic;

namespace CritRework.Content.Prefixes.Weapon
{
    public class Potent : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        static LocalizedText tooltip;

        public const float potencyBonus = 0.3f;

        public override void SetStaticDefaults()
        {
            tooltip = Mod.GetLocalization($"Prefixes.{nameof(Potent)}.Tooltip");
        }

        public override bool CanRoll(Item item)
        {
            if (!CritItem.CanHaveCrits(item))
            {
                return false;
            }
            return base.CanRoll(item);
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1.1f;
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            return new List<TooltipLine>()
            {
                new TooltipLine(Mod, "ModifierPotent", tooltip.Value)
                {
                    IsModifier = true,
                }
            };
        }
    }
}
