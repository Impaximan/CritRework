namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeAtHighHP : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.85f;

        //public override string GetDescription() => "Critically strikes while the target has more than 85% of its health left";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return target.life >= target.lifeMax * 0.85f;
        }
    }
}
