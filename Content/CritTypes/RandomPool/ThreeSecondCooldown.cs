using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class ThreeSecondCooldown : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 2f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceCrit >= 60 * 3;
        }
    }
}
