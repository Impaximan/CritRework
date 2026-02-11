using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Necromantic : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.75f;

        public override bool ShowWhenActive => true;

        public override int WhetstoneItemType => ModContent.ItemType<NecromanticWhetstone>();

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            foreach (Player player in Main.player)
            {
                if (player.active && player.dead)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
