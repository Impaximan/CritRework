namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class ThrowingKnife : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.ThrowingKnife;
        }

        public override float GetDamageMult(Player Player, Item Item) => 1.2f;

        //public override string GetDescription() => "Critically strikes when falling";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().timeActive >= 20;
            }
            return false;
        }
    }
}
