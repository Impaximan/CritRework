using CritRework.Common.Globals;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent.UI.Chat;

namespace CritRework.Content.Items.Augmentations
{
    public abstract class Augmentation : ModItem
    {
        static LocalizedText tooltip1;
        static LocalizedText tooltip2;
        static LocalizedText critDamageMult;
        public LocalizedText applicableTooltip;

        public override void SetStaticDefaults()
        {
            tooltip1 = Mod.GetLocalization($"{typeof(Augmentation).Name}.Tooltip1");
            tooltip2 = Mod.GetLocalization($"{typeof(Augmentation).Name}.Tooltip2");
            applicableTooltip = Mod.GetLocalization($"Items.{GetType().Name}.CanBeApplied");
        }

        public virtual bool CanApplyTo(Item weapon)
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            if (Main.mouseItem != null && Main.mouseItem.damage > 0 && !Main.mouseItem.accessory && CanApplyTo(Main.mouseItem))
            {
                if (Main.mouseItem.TryGetGlobalItem(out CritItem critTarget) && critTarget.augmentation != null && critTarget.augmentation.Item.type == Item.type)
                {
                    Main.NewText(ItemTagHandler.GenerateTag(Main.mouseItem) + " already has a " + ItemTagHandler.GenerateTag(Item), new Color(255, 25, 25));
                }
                else
                {
                    Apply(Main.mouseItem, player);
                }
            }
            else
            {
                Main.NewText(ItemTagHandler.GenerateTag(Main.mouseItem) + " is not eligible for " + ItemTagHandler.GenerateTag(Item), new Color(255, 25, 25));
            }
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public virtual void Apply(Item target, Player player)
        {
            Main.mouseItem.TryGetGlobalItem(out CritItem critTarget);
            critTarget.augmentation = this;

            SoundEngine.PlaySound(Item.UseSound);
            Main.NewText("Applied " + ItemTagHandler.GenerateTag(Item) + " to " + ItemTagHandler.GenerateTag(Item), Color.Yellow);

            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i] == Item)
                {
                    player.inventory[i] = new Item();
                }
            }
        }

        public override bool CanRightClick()
        {
            return Main.mouseItem != null && !Main.mouseItem.IsAir;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int startingIndex = 1;

            if (tooltips.Exists(x => x.Name == "FavoriteDesc"))
            {
                startingIndex = tooltips.FindIndex(x => x.Name == "FavoriteDesc") + 1;
            }

            tooltips.Insert(startingIndex, new TooltipLine(Mod, "AugmentationTooltip", tooltip1.Value));
            tooltips.Insert(startingIndex + 2, new TooltipLine(Mod, "AugmentationTooltip3", Mod.GetLocalization($"Items.{GetType().Name}.CanBeApplied").Value));
            if (!CritRework.abbreviateAugmentationTooltip) tooltips.Insert(startingIndex + 3, new TooltipLine(Mod, "AugmentationTooltip2", tooltip2.Value));
        }
    }
}
