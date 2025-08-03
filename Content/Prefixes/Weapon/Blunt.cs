namespace CritRework.Content.Prefixes.Weapon
{
    public class Blunt : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            critBonus = -20;
            damageMult = 1.15f;
        }
    }
}
