using CritRework.Common.Globals;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ModLoader.IO;
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
            if (AssociatedCritType == null)
            {
                return;
            }

            if (AssociatedCritType.CanApplyTo(Main.mouseItem) && CritItem.CanHaveCrits(Main.mouseItem))
            {
                if (AssociatedCritType == Main.mouseItem.GetGlobalItem<CritItem>().critType)
                {
                    Main.NewText("[i/d" + ItemIO.ToBase64(Main.mouseItem) + ":" + Main.mouseItem.type + "]" + " already has [i/s1/d" + ItemIO.ToBase64(Item) + ":" + Item.type + "]", new Color(255, 25, 25));
                }
                else
                {
                    Apply(Main.mouseItem);
                }
            }
            else
            {
                Main.NewText("[i/d" + ItemIO.ToBase64(Main.mouseItem) + ":" + Main.mouseItem.type + "] is not eligible for [i/s1/d" + ItemIO.ToBase64(Item) + ":" + Item.type + "]", new Color(255, 25, 25));
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
            Main.NewText("Applied [i/s1/d" + ItemIO.ToBase64(Item) + ":" + Item.type + "] to [i/d" + ItemIO.ToBase64(target) + ":" + target.type + "]", Color.Yellow);

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
            if (AssociatedCritType == null)
            {
                return;
            }

            int startingIndex = 1;

            if (tooltips.Exists(x => x.Name == "FavoriteDesc"))
            {
                startingIndex = tooltips.FindIndex(x => x.Name == "FavoriteDesc") + 1;
            }

            tooltips.Insert(startingIndex, new TooltipLine(Mod, "WhetstoneTooltip0", tooltip1.Value));
            tooltips.Insert(startingIndex + 1, new TooltipLine(Mod, "WhetstoneTooltip1", " • \"" + AssociatedCritType.description.Value + "\"" + "\n • " + Math.Round(AssociatedCritType.GetDamageMult(Main.LocalPlayer, Item), 2) + "x " + critDamageMult.Value)
            {
                OverrideColor = Color.Lerp(Color.Yellow, Color.White, 0.3f)
            });
            if (Item.type == ModContent.ItemType<BasicWhetstone>())
            {
                tooltips.Insert(startingIndex + 2, new TooltipLine(Mod, "WhetstoneTooltip3", AssociatedCritType.canBeAppliedToDesc.Value));
            }
            else
            {
                tooltips.Insert(startingIndex + 2, new TooltipLine(Mod, "WhetstoneTooltip3", Mod.GetLocalization($"Items.{GetType().Name}.CanBeApplied").Value));
            }
            if (!CritRework.abbreviateWhetstoneTooltip) tooltips.Insert(startingIndex + 3, new TooltipLine(Mod, "WhetstoneTooltip2", tooltip2.Value));
        }
    }
}
