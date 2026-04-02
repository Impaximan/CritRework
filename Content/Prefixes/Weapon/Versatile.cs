
using CritRework.Common.Globals;
using System.Collections.Generic;

namespace CritRework.Content.Prefixes.Weapon
{
    public class Versatile : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public LocalizedText description;

        public override void SetStaticDefaults()
        {
            description = Mod.GetLocalization($"Prefixes.{GetType().Name}.Description");
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
            valueMult *= 1.25f;
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            List<TooltipLine> lines =
            [
                new TooltipLine(Mod, "PrefixSpecial", "• " + description.Value)
                {
                    IsModifier = true,
                    OverrideColor = new Color(102, 166, 226)
                },
            ];

            return lines;
        }
    }
}
