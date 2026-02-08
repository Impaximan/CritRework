
namespace CritRework.Content.Items.Equipable.Accessories
{
    [AutoloadEquip(EquipType.Balloon)]
    public class MagmaHorseshoeBalloon : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FireInABalloon>()
                .AddIngredient(ItemID.LuckyHorseshoe)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            //Add to recipe group
            if (RecipeGroup.recipeGroups.TryGetValue(RecipeGroupID.SandstormBalloons, out RecipeGroup group))
            {
                group.ValidItems.Add(Type);
            }
        }

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 48;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<FireInABottle>();
            player.GetJumpState<FireJump>().Enable();

            if (player.GetJumpState<FireJump>().Active)
            {
                player.jumpSpeedBoost += 5f;
            }

            player.jumpBoost = true;
            player.noFallDmg = true;
        }
    }
}
