using System;

namespace CritRework.Content.Items
{
    class SparkleIcon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 26;
            Item.rare = ItemRarityID.White;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.White * 0.75f, Color.White, (float)(Math.Sin(Main._drawInterfaceGameTime.TotalGameTime.TotalSeconds) + 1f) / 2f);
        }
    }
}
