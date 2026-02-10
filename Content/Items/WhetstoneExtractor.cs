using CritRework.Common.Globals;
using CritRework.Content.CritTypes;
using CritRework.Content.Items.Whetstones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Chat;
using Terraria.UI.Chat;

namespace CritRework.Content.Items
{
    //TODO: Make this work with special whetstones
    public class WhetstoneExtractor : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 38;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Red;
        }

        public override void RightClick(Player player)
        {
            Item.stack++;
            if (Main.mouseItem.TryGetCritType(out CritType critType))
            {
                if (!critType.ForceOnItem(Main.mouseItem) && critType is not BrokenCrit)
                {
                    Item whetstone = new Item(ModContent.ItemType<BasicWhetstone>());
                    whetstone.GetGlobalItem<CritItem>().critType = critType;
                    player.QuickSpawnItem(new EntitySource_Misc("WhetstoneExtractor"), whetstone);
                    Main.mouseItem.Prefix(PrefixID.Broken);
                    Main.mouseItem.GetGlobalItem<CritItem>().critType = CritType.Get<BrokenCrit>();

                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Hijack")
                    {
                        Volume = 0.3f
                    });

                    return;
                }
            }

            Main.NewText("Cannot extract whetstone from " + ItemTagHandler.GenerateTag(Main.mouseItem), new Color(255, 25, 25));
        }

        public override bool CanRightClick()
        {
            return Main.mouseItem != null && !Main.mouseItem.IsAir;
        }
    }
}
