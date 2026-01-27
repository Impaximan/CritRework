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

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            float chance = specialPrefix ? 0.02f : 0.01f;
            return Main.rand.NextFloat() <= chance + Player.luck * chance;
        }
    }
}
