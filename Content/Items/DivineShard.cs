using CritRework.Common.Globals;
using CritRework.Content.CritTypes;
using CritRework.Content.Items.Whetstones;
using CritRework.Content.Prefixes.Weapon;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Chat;

namespace CritRework.Content.Items
{
    public class DivineShard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 16;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.maxStack = 9999;
        }

        public override void RightClick(Player player)
        {
            if (Main.mouseItem.prefix == ModContent.PrefixType<Special>())
            {
                Main.NewText(ItemTagHandler.GenerateTag(Main.mouseItem) + " is already enchanted", new Color(255, 25, 25));
                Item.stack++;
                return;
            }

            if (Main.mouseItem.TryGetCritType(out CritType critType))
            {
                if (Main.mouseItem.CanApplyPrefix(ModContent.PrefixType<Special>()) && Main.mouseItem.TryGetGlobalItem(out CritItem critItem) && CritItem.CanHaveCrits(Main.mouseItem))
                {
                    SoundEngine.PlaySound(SoundID.Item37);
                    Main.NewText("[i:" + ModContent.ItemType<SparkleIcon>() + "] " + ItemTagHandler.GenerateTag(Main.mouseItem) + " has been enchanted [i:" + ModContent.ItemType<SparkleIcon>() + "]", new Color(255, 230, 115));
                    Main.mouseItem.Prefix(ModContent.PrefixType<Special>());
                    return;
                }
            }

            Item.stack++;
            Main.NewText(ItemTagHandler.GenerateTag(Main.mouseItem) + " cannot be enchanted", new Color(255, 25, 25));
        }

        public override bool CanRightClick()
        {
            return Main.mouseItem != null && !Main.mouseItem.IsAir;
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips.FindAll(x => x.Name.Contains("Tooltip")))
            {
                line.Text = line.Text.Replace("[icon]", "[i:" + ModContent.ItemType<SparkleIcon>() + "]");
            }
        }
    }
}
