namespace CritRework.Content.CritTypes.RandomPool
{
    internal class HighChance : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.3f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            float chance = specialPrefix ? 0.3f : 0.2f;
            return Main.rand.NextFloat() <= chance + Player.luck * chance;
        }
    }
}
