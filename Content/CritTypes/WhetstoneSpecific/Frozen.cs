namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Frozen : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        //public override string GetDescription() => "Critically strikes on targets moving less than 10 miles per hour";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return target.velocity.Length() <= 2f;
        }
    }
}
