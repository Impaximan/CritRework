using CritRework.Common.Globals;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class MagicMissile : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.MagicMissile;
        }

        public override float GetDamageMult(Player Player, Item Item) => 2.5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (Projectile != null && !Player.CanHit(target))
            {
                return true;
            }

            return false;
        }
    }
}