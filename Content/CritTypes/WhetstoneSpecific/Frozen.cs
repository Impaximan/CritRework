using CritRework.Common.Globals;
using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Frozen : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override int WhetstoneItemType => ModContent.ItemType<FrozenWhetstone>();

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return target.velocity.Length() <= 2f || (specialPrefix && target.GetGlobalNPC<CritNPC>().timeSinceSlow < 180);
        }
    }
}
