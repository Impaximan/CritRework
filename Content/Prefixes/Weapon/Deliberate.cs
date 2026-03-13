
namespace CritRework.Content.Prefixes.Weapon
{
    public class Deliberate : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            if (item.channel)
            {
                return false;
            }
            return base.CanRoll(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            critBonus = 50;
            useTimeMult = 1.25f;
        }
    }
}
