namespace CritRework.Content.CritTypes.RandomPool
{
    internal class PlayerNohit : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.33f;

        public override bool ShowWhenActive => true;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.GetModPlayer<Common.ModPlayers.CritPlayer>().timeSinceLastHit >= 60 * 15;
        }
    }
}
