using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    public class Prepared : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().timeActive >= 300;
            }
            return false;
        }
    }
}
