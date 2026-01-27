namespace CritRework.Content.CritTypes.RandomPool
{
    internal class PlayerNohit : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => (Item.prefix == ModContent.PrefixType<Prefixes.Weapon.Special>() && Player.GetModPlayer<Common.ModPlayers.CritPlayer>().timeSinceLastHit >= 60 * 45) ? 2.66f : 1.33f;

        public override bool ShowWhenActive => true;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.GetModPlayer<Common.ModPlayers.CritPlayer>().timeSinceLastHit >= 60 * 15;
        }
    }
}
