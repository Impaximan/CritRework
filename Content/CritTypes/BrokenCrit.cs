using System;

namespace CritRework.Content.CritTypes
{
    public class BrokenCrit : CritType
    {
        public override Color Color => Color.Gray;

        public override float GetDamageMult(Player Player, Item Item)
        {
            return 1f;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return false;
        }
    }
}
