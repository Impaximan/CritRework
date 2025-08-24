using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class CritWithAnother : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceCrit <= 60 * 3;
        }
    }
}
