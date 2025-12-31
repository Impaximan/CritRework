namespace CritRework.Content.CritTypes.RandomPool
{
    internal class LowChance : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 15f;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.ZapinatorGray || item.type == ItemID.ZapinatorOrange;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Main.rand.NextFloat() <= 0.01f + Player.luck * 0.01f;
        }
    }
}
