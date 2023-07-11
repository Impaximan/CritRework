using Microsoft.Xna.Framework;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class ShadowbeamStaff : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(out int itemType)
        {
            itemType = ItemID.ShadowbeamStaff;
            return true;
        }

        public override float GetDamageMult(Player Player, Item Item) => 2.5f;

        public override string GetDescription() => "Critically strikes after bouncing off a wall";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().wallBounces >= 1;
            }
            return false;
        }
    }
}
