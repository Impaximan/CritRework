namespace CritRework.Content.Items.Materials
{
    class Greenprint : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 2;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.width = 34;
            Item.height = 40;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }
    }
}
