using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.RandomPool
{
    [JITWhenModsEnabled("OrchidMod")]
    internal class Shapeshifter_ShapeshiftShawlDash : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("OrchidMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 2f;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidMod))
            {
                return item.DamageType.CountsAsClass(OrchidMod.Find<DamageClass>("ShapeshifterDamageClass"));
            }

            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceShawlDash < 60;
        }
    }
}
