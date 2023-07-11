using Microsoft.Xna.Framework;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class Sniper : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(out int itemType)
        {
            itemType = ItemID.SniperRifle;
            return true;
        }

        public override float GetDamageMult(Player Player, Item Item) => 10f;

        public override string GetDescription() => "Critically strikes while the target is at least 75 tiles away";

        public override bool ShouldCrit(Player Player, Item Item, NPC target)
        {
            return Player.Distance(target.getRect().ClosestPointInRect(Player.Center)) >= 1200;
        }
    }
}
