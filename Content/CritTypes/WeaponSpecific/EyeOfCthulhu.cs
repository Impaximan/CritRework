using System.Collections.Generic;

namespace CritRework.Content.CritTypes.WeaponSpecific
{
    public class EyeOfCthulhu : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.TheEyeOfCthulhu;
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Player.statLife <= Player.statLifeMax2 / 2;
        }
    }
}

