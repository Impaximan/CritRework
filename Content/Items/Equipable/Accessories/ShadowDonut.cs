using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;
using System.Collections.Generic;
using System;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class ShadowDonut : ModItem
    {
        public static LocalizedText finalTooltipLine;

        public override void SetStaticDefaults()
        {
            finalTooltipLine = Mod.GetLocalization($"Items.ShadowDonut.LastTooltipLine");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 3, 50, 0);
            Item.GetGlobalItem<CritItem>().forceCanCrit = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<ShadowDonut>();

            if (player.TryGetModPlayer(out CritPlayer critPlayer) && Item.TryGetGlobalItem(out CritItem critItem))
            {
                critPlayer.EVILCrit = critItem.critType;
                critPlayer.shadowDonut = this;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int i = tooltips.FindLastIndex(x => x.Name.Contains("Tooltip"));

            string dmgString = "[c/808080:???]";

            if (Item.TryGetGlobalItem(out CritItem cItem) && cItem.critType != null)
            {
                dmgString = Math.Round(cItem.critType.GetDamageMult(Main.LocalPlayer, Item) * 100f).ToString() + "%";
            }

            if (i > 2)
            {
                tooltips.Insert(i - 2, new TooltipLine(Mod, "EVILTooltip", finalTooltipLine.Value.Replace("[dmg]", dmgString))
                {
                    OverrideColor = new Color(255, 115, 115)
                });
            }
        }
    }

    public class ShadowFrenzy : ModBuff
    {
        public override void SetStaticDefaults()
        {

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 20;
            player.GetAttackSpeed(DamageClass.Generic) += 0.2f;
            player.lifeRegen += 15;
            player.moveSpeed += 0.75f;

            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.Shadowflame, player.velocity.X, player.velocity.Y);
            }
        }
    }
}
