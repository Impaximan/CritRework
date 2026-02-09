using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class FreshItem : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.1f;

        public override bool ShowWhenActive => true;

        public override bool CanApplyTo(Item item)
        {
            return !item.accessory;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.GetModPlayer<CritPlayer>().freshItemTime <= (specialPrefix ? 120 : 30);
        }
    }
}
