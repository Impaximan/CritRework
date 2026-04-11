using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Materials;

namespace CritRework.Content.Items.Equipable.Armor.Construct
{
    [AutoloadEquip(EquipType.Head)]
    public class ConstructHelmet : ModItem
    {
        public static LocalizedText SetBonus { get; private set; }

        public override void SetStaticDefaults()
        {
            SetBonus = Mod.GetLocalization($"{nameof(ConstructHelmet)}.SetBonus");
            ArmorIDs.Head.Sets.DrawHead[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 29;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CritPlayer>().augmentedWeaponCritBoost += 15f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == Type && body.type == ModContent.ItemType<ConstructPlating>() && legs.type == ModContent.ItemType<ConstructGreaves>();
        }

        public int burstJumpCounter = 0;
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonus.Value;

            player.GetModPlayer<CritPlayer>().maxAugmentations++;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Greenprint>()
                .AddIngredient(ItemID.LunarTabletFragment, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}