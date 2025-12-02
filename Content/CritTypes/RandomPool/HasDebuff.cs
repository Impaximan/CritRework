using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Equipable.Accessories;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class HasDebuff : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 2f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            for (int i = 0; i < Player.buffType.Length; i++)
            {
                int type = Player.buffType[i];

                if (Main.debuff[type] && !Main.buffNoTimeDisplay[type] && Main.TryGetBuffTime(i, out int time) && time > 5)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
