using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;
using System.Collections.Generic;
using System;
using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class GraftedSling : ModItem
    {

        public static LocalizedText finalTooltipLine;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ProstheticArm>()
                .AddIngredient<SkywardSling>()
                .AddIngredient(ItemID.ChlorophyteBar, 15)
                .AddIngredient<BronzeAlloy>(10)
                .AddIngredient<Greenprint>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            finalTooltipLine = Mod.GetLocalization($"Items.ProstheticArm.LastTooltipLine");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 40;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.GetGlobalItem<CritItem>().forceCanCrit = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<ProstheticArm>();

            if (Item.TryGetGlobalItem(out CritItem cItem) && cItem.critType != null)
            {
                player.GetModPlayer<CritPlayer>().prostheticCrit = cItem.critType;

                if (cItem.critType.ShowWhenActive && cItem.critType.ShouldCrit(player, Item, null, null, new NPC.HitModifiers(), Item.prefix == ModContent.PrefixType<Prefixes.Weapon.Special>()))
                {
                    player.AddBuff(ModContent.BuffType<ProstheticsActive>(), 2);
                }
            }


            player.GetModPlayer<CritPlayer>().maxAugmentations++;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<ProstheticArm>() || equippedItem.type == ModContent.ItemType<SkywardSling>()
                || incomingItem.type == ModContent.ItemType<ProstheticArm>() || incomingItem.type == ModContent.ItemType<SkywardSling>())
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int i = tooltips.FindLastIndex(x => x.Name.Contains("Tooltip"));

            string dmgString = "[c/808080:???]";

            if (Item.TryGetGlobalItem(out CritItem cItem) && cItem.critType != null)
            {
                dmgString = Math.Round((cItem.critType.GetDamageMult(Main.LocalPlayer, Item) - 1f) * 150f).ToString() + "%";
            }

            if (i != -1)
            {
                tooltips.Insert(i + 1, new TooltipLine(Mod, "Tooltip3", finalTooltipLine.Value.Replace("[dmg]", dmgString)));
            }
        }
    }
}
