using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    internal class Holy : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 4.5f;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.netMode != NetmodeID.SinglePlayer && hit.Crit)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)CritRework.MessageType.SpecialHolyHit);
                packet.Send();
            }
        }

        public override bool ShowWhenActive => true;

        public override int WhetstoneItemType => ModContent.ItemType<HolyWhetstone>();

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.immune;
        }
    }
}
