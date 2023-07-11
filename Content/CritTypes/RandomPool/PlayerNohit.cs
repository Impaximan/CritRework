using Microsoft.Xna.Framework;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class PlayerNohit : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.33f;

        public override string GetDescription() => "Critically strikes if you haven't taken damage in more than 15 seconds";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.GetModPlayer<Common.ModPlayers.CritPlayer>().timeSinceLastHit >= 60 * 15;
        }
    }
}
