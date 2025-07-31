using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Frozen : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.3f;

        public override string GetDescription() => "Critically strikes on targets moving less than 10 miles per hour";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return target.velocity.Length() <= 2f;
        }
    }
}
