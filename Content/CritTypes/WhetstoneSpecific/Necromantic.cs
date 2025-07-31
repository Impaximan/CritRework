namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Necromantic : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.75f;

        public override string GetDescription() => "Critically strikes while another player is dead";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            foreach (Player player in Main.player)
            {
                if (player.active && player.dead)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
