using Microsoft.Xna.Framework;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeAtLowHP : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 2f;

        public override string GetDescription() => "Critically strikes while the target has less than 15% of its health left";

        public override bool ShouldCrit(Player Player, Item Item, NPC target)
        {
            return target.life <= target.lifeMax * 0.15f;
        }
    }
}
