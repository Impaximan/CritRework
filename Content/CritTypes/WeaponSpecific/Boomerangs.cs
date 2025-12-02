using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class Boomerangs : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            List<int> allAffected = new()
            {
                ItemID.WoodenBoomerang,
                ItemID.EnchantedBoomerang,
                ItemID.IceBoomerang,
                ItemID.Flamarang,
                ItemID.Shroomerang,
                ItemID.Trimarang
            };
            return allAffected.Contains(item.type);
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Projectile.ai[0] == 1;
        }
    }
}

