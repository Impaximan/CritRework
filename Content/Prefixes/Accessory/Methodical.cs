namespace CritRework.Content.Prefixes.Accessory
{
    public class Methodical : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.55f;
        }

        public override void ApplyAccessoryEffects(Player player)
        {
            player.GetCritChance<GenericDamageClass>() += 8;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            critBonus = 8;
        }
    }
}
