using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class Blowpipe : CritType
    {
        public override bool InRandomPool => false;

        public override bool ShowWhenActive => true;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Blowpipe || item.type == ItemID.Blowgun;
        }

        public override float GetDamageMult(Player Player, Item Item) => 3.5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (Projectile != null)
            {
                return Projectile.TryGetGlobalProjectile(out CritProjectile p) && p.blowgunCrit;
            }

            return Player.TryGetModPlayer(out CritPlayer c) && c.timeSinceBlowpipe > 180;
        }
    }
}

