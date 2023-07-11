using Microsoft.Xna.Framework;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class BelowFoe : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item)
        {
            if (Item.shoot == ProjectileID.None)
            {
                return 1.5f;
            }
            else
            {
                return 1.15f;
            }
        }

        public override string GetDescription() => "Critically strikes while you are below the target";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.position.Y > target.position.Y + target.height;
        }
    }
}
