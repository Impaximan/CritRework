using CritRework.Common.Globals;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Audio;
using System;

namespace CritRework.Content.Items.Whetstones
{
    public abstract class Whetstone : ModItem
    {
        static LocalizedText tooltip1;
        static LocalizedText tooltip2;
        static LocalizedText critDamageMult;
        public LocalizedText applicableTooltip;

        public override void SetStaticDefaults()
        {
            tooltip1 = Mod.GetLocalization($"{typeof(Whetstone).Name}.Tooltip1");
            tooltip2 = Mod.GetLocalization($"{typeof(Whetstone).Name}.Tooltip2");
            critDamageMult = Mod.GetLocalization($"{typeof(Whetstone).Name}.critDamageMult");
            applicableTooltip = Mod.GetLocalization($"Items.{GetType().Name}.CanBeApplied");
        }

        public override void RightClick(Player player)
        {
            if (AssociatedCritType.CanApplyTo(Main.mouseItem) && CritItem.CanHaveCrits(Main.mouseItem))
            {
                if (AssociatedCritType == Main.mouseItem.GetGlobalItem<CritItem>().critType)
                {
                    Main.NewText("[i:" + Main.mouseItem.type + "] already has [i:" + Item.type + "]", new Color(255, 25, 25));
                }
                else
                {
                    Apply(Main.mouseItem);
                }
            }
            else
            {
                Main.NewText("[i:" + Main.mouseItem.type + "] is not eligible for [i:" + Item.type + "]", new Color(255, 25, 25));
            }
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public virtual void Apply(Item target)
        {
            target.GetGlobalItem<CritItem>().critType = AssociatedCritType;
            SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Whetstone"));
            Main.NewText("Applied [i:" + Item.type + "] to [i:" + target.type + "]", Color.Yellow);

            Item.stack--;

            if (Item.stack <= 0)
            {
                Item.SetDefaults(ItemID.None);
            }
        }

        public override bool CanRightClick()
        {
            return Main.mouseItem != null && !Main.mouseItem.IsAir;
        }

        public abstract CritType AssociatedCritType { get; }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(1, new TooltipLine(Mod, "WhetstoneTooltip0", tooltip1.Value));
            tooltips.Insert(2, new TooltipLine(Mod, "WhetstoneTooltip1", " • \"" + AssociatedCritType.GetDescription() + "\"" + "\n • " + Math.Round(AssociatedCritType.GetDamageMult(Main.LocalPlayer, Item), 2) + "x " + critDamageMult.Value)
            {
                OverrideColor = Color.Lerp(Color.Yellow, Color.White, 0.3f)
            });
            tooltips.Insert(3, new TooltipLine(Mod, "WhetstoneTooltip3", Mod.GetLocalization($"Items.{GetType().Name}.CanBeApplied").Value));
            tooltips.Insert(4, new TooltipLine(Mod, "WhetstoneTooltip2", tooltip2.Value));
        }
    }
}
