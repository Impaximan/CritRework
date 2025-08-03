using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class JustHitByEnemy : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 2.35f;

        //public override string GetDescription() => "Critically strikes for 1.5 seconds after getting hit";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            if (Projectile != null)
            {
                return Player.GetModPlayer<CritPlayer>().timeSinceLastHit <= 90;
            }
            return false;
        }
    }
}
