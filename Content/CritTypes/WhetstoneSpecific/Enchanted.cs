namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Enchanted : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        //public override string GetDescription() => "Critically strikes if you have less than 30% of your maximum mana";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.statMana <= Player.statManaMax2 * 0.3f;
        }
    }
}
