using CritRework.Content.Items.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CritRework.Content.Items.Bronze.BronzeArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class BronzeHelm : ModItem
    {
        public static LocalizedText SetBonus { get; private set; }

        public override void SetStaticDefaults()
        {
            SetBonus = Mod.GetLocalization($"{nameof(BronzeHelm)}.SetBonus");
            ArmorIDs.Head.Sets.DrawHead[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 5;
            player.GetCritChance(DamageClass.Magic) += 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == Type && body.type == ModContent.ItemType<BronzeBreastplate>() && legs.type == ModContent.ItemType<BronzeGreaves>();
        }

        public int burstJumpCounter = 0;
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonus.Value;

            if (player.statMana < player.statManaMax2)
            {
                player.GetCritChance(DamageClass.Melee) += 35;
                player.GetDamage(DamageClass.Melee) += 0.35f;
            }

            player.AddEquip<BronzeHelm>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BronzeAlloy>(15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}