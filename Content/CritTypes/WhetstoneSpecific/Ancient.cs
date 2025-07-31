using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Ancient : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.4f;

        public override string GetDescription() => "Critically strikes for 60 seconds after you respawn";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceDeath <= 60 * 60;
        }
    }
}
