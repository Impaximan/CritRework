using Microsoft.Xna.Framework;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class Shuriken : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(out int itemType)
        {
            itemType = ItemID.Shuriken;
            return true;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override string GetDescription() => "Critically strikes after hitting at least one target";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().targetsHit >= 1;
            }
            return false;
        }
    }
}
