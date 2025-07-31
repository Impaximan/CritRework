namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Starter : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.15f;

        public override string GetDescription() => "Critically strikes if you have less than 200 max life";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.statLifeMax2 < 200;
        }
    }
}
