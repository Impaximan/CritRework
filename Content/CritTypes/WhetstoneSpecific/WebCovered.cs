using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class WebCovered : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override string GetDescription() => "Critically strikes for 1 second after grappling yourself to a block, or while grappled";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceHook <= 60;
        }
    }
}
