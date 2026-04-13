namespace CritRework.Content.Items.Materials
{
    class Lie : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
            ItemID.Sets.ItemNoGravity[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            ItemID.Sets.ItemIconPulse[Type] = true;
            Item.width = 18;
            Item.height = 14;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 10, 0);
        }
    }
}
