//using CritRework.Common.ModPlayers;
//using CritRework.Content.Items.Equipable.Accessories;

//namespace CritRework.Content.CritTypes.WeaponSpecific
//{
//    internal class PickUpAmmo : CritType
//    {
//        public override bool InRandomPool => true;

//        public override float GetDamageMult(Player Player, Item Item) => 1.4f;

//        public override bool CanApplyTo(Item item)
//        {
//            return item.useAmmo == AmmoID.Arrow;
//        }

//        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
//        {
//            return Player.GetModPlayer<CritPlayer>().timeSinceArrowPickup <= 180;
//        }
//    }
//}
