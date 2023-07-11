using Microsoft.Xna.Framework;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class CloseToFoe : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.33f;

        public override string GetDescription() => "Critically strikes while you are closer than 6 tiles away from the target";

        public override bool CanApplyTo(Item item)
        {
            return !item.DamageType.CountsAsClass(DamageClass.Melee);
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.Distance(target.getRect().ClosestPointInRect(Player.Center)) <= 96;
        }
    }
}
