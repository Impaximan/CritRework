using CritRework.Content.Items.Whetstones;

namespace CritRework.Content.CritTypes.WhetstoneSpecific
{
    public class Prepared : CritType
    {
        public override bool InRandomPool => false;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override int WhetstoneItemType => ModContent.ItemType<PreparedWhetstone>();

        public override void SpecialPrefixOnHitNPC(Item item, Player player, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                player.AddBuff(ModContent.BuffType<Preparation>(), 60);
            }
        }

        public override bool CanApplyTo(Item item)
        {
            return item.type != ModContent.ItemType<Items.Equipable.Accessories.WiseCracker>() && item.type != ModContent.ItemType<Items.Bronze.BronzeQuarterstaff>();
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (Projectile != null)
            {
                return Projectile.GetGlobalProjectile<Common.Globals.CritProjectile>().timeActive >= 300;
            }
            return false;
        }
    }

    public class Preparation : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 15;
        }
    }
}
