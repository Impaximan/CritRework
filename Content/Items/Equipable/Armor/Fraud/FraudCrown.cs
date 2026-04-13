using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Equipable.Armor.Fraud
{
    [AutoloadEquip(EquipType.Head)]
    public class FraudCrown : ModItem
    {
        public static LocalizedText SetBonus { get; private set; }

        public override void SetStaticDefaults()
        {
            SetBonus = Mod.GetLocalization($"Items.{nameof(FraudCrown)}.SetBonus");
            int slot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[slot] = true;
            ArmorIDs.Head.Sets.DrawHatHair[slot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 8;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == Type && body.type == ModContent.ItemType<FraudVest>() && legs.type == ModContent.ItemType<FraudPants>();
        }

        public int burstJumpCounter = 0;
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonus.Value;

            player.AddEquip<FraudCrown>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Lie>(4)
                .AddIngredient(ItemID.Obsidian, 10)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}