namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class ClassyCane : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.TaxCollectorsStickOfDoom;
        }

        public override float GetDamageMult(Player Player, Item Item) => 100f;

        public override bool ShowWhenActive => true;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            int totalValue = 0;

            for (int i = 50; i <= 53; i++)
            {
                if (Player.inventory[i] != null && !Player.inventory[i].IsAir)
                {
                    switch (Player.inventory[i].type)
                    {
                        case ItemID.CopperCoin:
                            totalValue += 1 * Player.inventory[i].stack;
                            break;
                        case ItemID.SilverCoin:
                            totalValue += 100 * Player.inventory[i].stack;
                            break;
                        case ItemID.GoldCoin:
                            totalValue += 10000 * Player.inventory[i].stack;
                            break;
                        case ItemID.PlatinumCoin:
                            totalValue += 1000000 * Player.inventory[i].stack;
                            break;
                    }
                }
            }

            return totalValue >= 50000000;
        }
    }
}
