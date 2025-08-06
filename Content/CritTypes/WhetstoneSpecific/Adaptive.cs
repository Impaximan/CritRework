using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    public class Adaptive : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.85f;

        //public override string GetDescription() => "Critically strikes for 3 seconds after healing for at least 20 hp (healing while at max doesn't count)";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceHeal <= 60 * 3;
        }
    }
}
