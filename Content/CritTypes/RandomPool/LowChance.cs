using Microsoft.Xna.Framework;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class LowChance : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 10f;

        public override string GetDescription() => "Critically strikes 1% of the time";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target)
        {
            return Main.rand.NextFloat() <= 0.01f + Player.luck * 0.01f;
        }
    }
}
