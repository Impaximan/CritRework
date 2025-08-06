namespace CritRework.Content.CritTypes.RandomPool
{
    internal class MediumChance : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 2f;

        //public override string GetDescription() => "Critically strikes 10% of the time";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Main.rand.NextFloat() <= 0.1f + Player.luck * 0.1f;
        }
    }
}
