using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class StandingStill : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.75f;

        public override bool ShowWhenActive => true;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.velocity.Length() <= 2f || (specialPrefix && Player.GetModPlayer<CritPlayer>().timeSinceMovingSlow < 60);
        }
    }
}
