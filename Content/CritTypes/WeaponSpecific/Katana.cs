namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class Katana : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Katana;
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.8f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.statDefense <= 0;
        }
    }
}
