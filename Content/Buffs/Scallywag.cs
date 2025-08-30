
namespace CritRework.Content.Buffs
{
    public class Scallywag : ModBuff
    {
        public override void SetStaticDefaults()
        {

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 10;
            player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
            player.GetCritChance(DamageClass.Generic) += 30;

            if (!player.mount.Active)
            {
                player.buffTime[buffIndex] = 0;
            }
        }
    }
}
