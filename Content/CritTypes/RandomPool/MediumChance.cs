namespace CritRework.Content.CritTypes.RandomPool
{
    internal class MediumChance : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.75f;

        //public override string GetDescription() => "Critically strikes 10% of the time";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            float chance = specialPrefix ? 0.15f : 0.1f;
            return Main.rand.NextFloat() <= chance + Player.luck * chance;
        }
    }
}
