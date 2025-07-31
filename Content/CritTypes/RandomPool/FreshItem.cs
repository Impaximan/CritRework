using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class FreshItem : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.3f;

        public override string GetDescription() => "Critically strikes for 1 second after the first use of this weapon" +
            "\nReset after using another weapon";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.GetModPlayer<CritPlayer>().freshItemTime <= 60;
        }
    }
}
