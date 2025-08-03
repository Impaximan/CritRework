namespace CritRework.Content.Prefixes.Weapon
{
    public class Quality : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1f;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            critBonus = 60;
            damageMult = 0.8f;
        }
    }
}
