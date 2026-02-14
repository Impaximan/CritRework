using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Whetstones;
using Terraria.DataStructures;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Greedy : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.Distance(target.getRect().ClosestPointInRect(player.Center)) < 100f)
            {
                player.QuickSpawnItem(new EntitySource_OnHit(player, target), ItemID.SilverCoin, 1);
            }
        }

        public override bool ShowWhenActive => true;

        public override int WhetstoneItemType => ModContent.ItemType<GreedyWhetstone>();

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.GetModPlayer<CritPlayer>().timeSinceGoldPickup <= 60 * 3;
        }
    }
}
