using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeHasFourDebuffs : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.75f;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            float chance = 20f * (1f + player.luck / 2f);

            if (Main.rand.NextFloat(100) < chance)
            {
                int buffType = Main.rand.Next(4) switch
                {
                    0 => Main.hardMode ? BuffID.OnFire3 : BuffID.OnFire,
                    1 => Main.hardMode ? BuffID.Venom : BuffID.Poisoned,
                    2 => BuffID.Confused,
                    3 => Main.hardMode ? BuffID.Frostburn2 : BuffID.Frostburn,
                    4 => BuffID.Oiled,
                    _ => BuffID.OnFire,
                };


                target.AddBuff(buffType, Main.rand.Next(60, 480));
            }
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            int num = 0;

            List<int> oddExceptions = new()
                {
                    BuffID.OnFire3,
                    BuffID.Frostburn2
                };


            foreach (int type in target.buffType)
            {
                if (type > 0 && (Main.debuff[type] || oddExceptions.Contains(type)) //FSR some debuffs from the base game are not marked as debuffs. This aims to patch some of those.
                    && target.buffTime[target.FindBuffIndex(type)] > 0)
                {
                    num++;
                }
            }

            if (num >= 4)
            {
                return true;
            }

            return false;
        }
    }
}

