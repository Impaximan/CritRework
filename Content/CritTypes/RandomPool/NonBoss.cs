using CritRework.Common.ModPlayers;
using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class NonBoss : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.2f;

        public override bool ShowWhenActive => false;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return !target.boss && !new List<int>() { NPCID.EaterofWorldsHead, 
                NPCID.EaterofWorldsBody, 
                NPCID.EaterofWorldsTail, 
                NPCID.Creeper }.Contains(target.type);
        }
    }
}
