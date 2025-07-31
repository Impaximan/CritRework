using CritRework.Common.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Greedy : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override string GetDescription() => "Critically strikes for 3 seconds after you pick up money";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceGoldPickup <= 60 * 3;
        }
    }
}
