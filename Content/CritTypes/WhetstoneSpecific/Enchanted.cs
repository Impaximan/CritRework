namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Enchanted : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override bool ShowWhenActive => true;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.statMana <= Player.statManaMax2 * 0.3f;
        }
    }
}
