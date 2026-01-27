using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    public class Adaptive : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.85f;

        public override bool ShowWhenActive => true;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceHeal <= 60 * 3;
        }
    }
}
