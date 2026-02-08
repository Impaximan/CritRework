using Terraria.DataStructures;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeAtLowHP : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 3f;

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && target.life <= 0)
            {
                int i = Item.NewItem(new EntitySource_OnHit(player, target, "RavenouseHit"), target.getRect(), new Item(ItemID.Heart, 1));
                if (Main.netMode == NetmodeID.MultiplayerClient) NetMessage.SendData(MessageID.SyncItem, number: i, number2: 1);
            }
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return target.life <= target.lifeMax * 0.25f;
        }
    }
}
