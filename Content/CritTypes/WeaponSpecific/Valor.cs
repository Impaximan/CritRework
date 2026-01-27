using System.Collections.Generic;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class Valor : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Valor;
        }

        public override float GetDamageMult(Player Player, Item Item) => 3f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile.ai[0] == -1f;
        }
    }
}

