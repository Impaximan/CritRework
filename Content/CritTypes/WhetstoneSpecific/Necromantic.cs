using CritRework.Content.Items.Whetstones;
using Terraria.ModLoader.IO;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    public class Necromantic : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.75f;

        public override bool ShowWhenActive => true;

        public override int WhetstoneItemType => ModContent.ItemType<NecromanticWhetstone>();

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.netMode != NetmodeID.SinglePlayer && hit.Crit)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)CritRework.MessageType.SpecialNecromanticHit);
                packet.Send();
            }
        }

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
