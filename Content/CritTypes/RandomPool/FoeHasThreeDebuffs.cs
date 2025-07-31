﻿using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeHasThreeDebuffs : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.4f;

        public override string GetDescription() => "Critically strikes while the target is inflicted with three or more debuffs";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            int num = 0;

            List<int> oddExceptions = new()
                {
                    BuffID.OnFire3
                };


            foreach (int type in target.buffType)
            {
                if (type > 0 && (Main.debuff[type] || oddExceptions.Contains(type)) //FSR some debuffs from the base game are not marked as debuffs. This aims to patch some of those.
                    && target.buffTime[target.FindBuffIndex(type)] > 0)
                {
                    num++;
                }
            }

            if (num >= 3)
            {
                return true;
            }

            return false;
        }
    }
}

