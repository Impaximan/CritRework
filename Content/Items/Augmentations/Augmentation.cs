using CritRework.Common.Globals;
using CritRework.Content.Prefixes.Augmentation;
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

        public static readonly SoundStyle equipSound = new SoundStyle("CritRework/Assets/Sounds/EquipAugmentation")
        {
            PitchVariance = 0.25f,
            Volume = 0.5f
        };

        public override bool ReforgePrice(ref int reforgePrice, ref bool canApplyDiscount)
        {
            reforgePrice /= 4;
            return base.ReforgePrice(ref reforgePrice, ref canApplyDiscount);
        }

        public override void SetStaticDefaults()
        {
            tooltip1 = Mod.GetLocalization($"{typeof(Augmentation).Name}.Tooltip1");
            tooltip2 = Mod.GetLocalization($"{typeof(Augmentation).Name}.Tooltip2");
            applicableTooltip = Mod.GetLocalization($"Items.{GetType().Name}.CanBeApplied");
        }

        public virtual bool OverrideNormalCritBehavior(Player player, Item item, Projectile? projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return false;
        }

        public virtual void AugmentationOnHitNPC(Player player, Item item, Projectile? projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {

        }

        public override bool AllowPrefix(int pre)
        {
            if (PrefixLoader.GetPrefix(pre) != null)
            {
                return PrefixLoader.GetPrefix(pre) is AugmentationPrefix;
            }
            return false;
        }

        public sealed override bool WeaponPrefix()
        {
            return true;
        }

        public virtual bool CanApplyTo(Item weapon)
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            if (Main.mouseItem != null && Main.mouseItem.damage > 0 && Main.mouseItem.useStyle != ItemUseStyleID.None && !Main.mouseItem.accessory && CanApplyTo(Main.mouseItem) && CritItem.CanHaveCrits(Main.mouseItem))
            {
                int maxAugmentations = Main.mouseItem.MaxAugmentations(player);

                if (Main.mouseItem.TryGetGlobalItem(out CritItem critTarget))
                {
                    if (critTarget.augmentations.Exists(x => x.Type == Type) || critTarget.augmentations.Count >= maxAugmentations)
                    {
                        critTarget.RemoveAugmentation(player);
                    }
                }
                Apply(Main.mouseItem, player);
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
            target.TryGetGlobalItem(out CritItem critTarget);

            critTarget.augmentations.Add(this);

            SoundEngine.PlaySound(Item.UseSound);
            Main.NewText("Applied " + ItemTagHandler.GenerateTag(Item) + " to " + ItemTagHandler.GenerateTag(target), Color.Yellow);

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
            tooltips.Insert(startingIndex + 1, new TooltipLine(Mod, "AugmentationTooltip3", Mod.GetLocalization($"Items.{GetType().Name}.CanBeApplied").Value));
            if (!CritRework.abbreviateAugmentationTooltip) tooltips.Insert(startingIndex + 2, new TooltipLine(Mod, "AugmentationTooltip2", tooltip2.Value));
        }
    }

    public abstract class SpecialAugmentationPrefix<T> : AugmentationPrefix where T : Augmentation
    {
        public LocalizedText tooltip;
        public LocalizedText tooltip2;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            tooltip = Mod.GetLocalization($"Prefixes.{GetType().Name}.tooltip");
            tooltip2 = Mod.GetLocalization($"Prefixes.{GetType().Name}.tooltip2");
        }

        public override bool CanRoll(Item item)
        {
            return base.CanRoll(item) && item.ModItem is T;
        }

        public override float RollChance(Item item)
        {
            return 2f;
        }

        public sealed override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            List<TooltipLine> tooltips = [.. base.GetTooltipLines(item)];
            AddExtraTooltipLines(item, ref tooltips);
            return tooltips;
        }

        public virtual void AddExtraTooltipLines(Item item, ref List<TooltipLine> tooltips)
        {

        }
    }
}
