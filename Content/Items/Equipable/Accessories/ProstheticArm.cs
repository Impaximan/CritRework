using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;
using System.Collections.Generic;
using Terraria.GameContent.UI.Chat;
using Terraria.GameContent.UI;
using System;
using Terraria;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class ProstheticArm : ModItem
    {

        public static LocalizedText finalTooltipLine;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddIngredient(ItemID.AdamantiteBar, 10)
                .AddIngredient(ItemID.SoulofFright, 15)
                .AddIngredient(ItemID.SoulofNight, 8)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddIngredient(ItemID.TitaniumBar, 10)
                .AddIngredient(ItemID.SoulofFright, 15)
                .AddIngredient(ItemID.SoulofNight, 8)
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
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.GetGlobalItem<CritItem>().forceCanCrit = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<ProstheticArm>();

            if (Item.TryGetGlobalItem(out CritItem cItem) && cItem.critType != null)
            {
                player.GetModPlayer<CritPlayer>().prostheticCrit = cItem.critType;

                if (cItem.critType.ShowWhenActive && cItem.critType.ShouldCrit(player, Item, null, null, new NPC.HitModifiers()))
                {
                    player.AddBuff(ModContent.BuffType<ProstheticsActive>(), 2);
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int i = tooltips.FindLastIndex(x => x.Name.Contains("Tooltip"));

            string dmgString = "[c/808080:???]";

            if (Item.TryGetGlobalItem(out CritItem cItem) && cItem.critType != null)
            {
                dmgString = Math.Round(cItem.critType.GetDamageMult(Main.LocalPlayer, Item) * 150f).ToString() + "%";
            }

            if (i != -1)
            {
                tooltips.Insert(i + 1, new TooltipLine(Mod, "Tooltip3", finalTooltipLine.Value.Replace("[dmg]", dmgString)));
            }
        }
    }

    public class ProstheticsActive : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
