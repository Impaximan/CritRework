namespace CritRework.Content.CritTypes.RandomPool
{
    internal class TipOfTheWeapon : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.15f;

        //public override string GetDescription() => "Critically strikes when hitting enemies at the tip of the weapon";

        public override bool CanApplyTo(Item item)
        {
            return item.noMelee == false && item.DamageType == DamageClass.Melee;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            return Projectile == null && Player.Distance(target.getRect().ClosestPointInRect(Player.Center)) - Item.Size.Length() * Player.GetAdjustedItemScale(Item) >= 0f;
        }
    }
}
