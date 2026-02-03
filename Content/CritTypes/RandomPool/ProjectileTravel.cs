namespace CritRework.Content.CritTypes.WeaponSpecific
{
    internal class ProjectileTravel : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.2f;

        public override bool CanApplyTo(Item item)
        {
            return item.shoot != ProjectileID.None && !ItemID.Sets.Spears[item.type] && item.useStyle != ItemUseStyleID.Rapier && item.type != ModContent.ItemType<Items.Equipable.Accessories.WiseCracker>() && !item.channel;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (Projectile != null)
            {
                int t = Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().timeActive;
                return t >= 60 && (t < 240 || specialPrefix);
            }
            return false;
        }
    }
}
