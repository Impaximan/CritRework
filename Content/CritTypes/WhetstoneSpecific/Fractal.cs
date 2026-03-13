using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    public class Fractal : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override int WhetstoneItemType => ModContent.ItemType<FractalWhetstone>();

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return target.boss && (Main.BestiaryTracker.Kills.GetKillCount(target) >= 1 || (specialPrefix && target.life > target.lifeMax / 2));
        }
    }

}
