﻿using CritRework.Common.Globals;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Holy : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 4.5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.immune;
        }
    }
}
