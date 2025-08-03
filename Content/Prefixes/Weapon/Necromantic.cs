namespace CritRework.Content.Prefixes.Weapon
{
    public class Necromantic : ModPrefix
    {
        public const int healAmount = 2;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 2f;
        }

        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            critBonus = -20;
        }
    }
}
