
namespace CritRework.Content.Prefixes.Accessory
{
    public class Accurate : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.44f;
        }

        public override void ApplyAccessoryEffects(Player player)
        {
            player.GetCritChance<GenericDamageClass>() += 6;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            critBonus = 6;
        }
    }
}
