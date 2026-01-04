
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
            player.GetCritChance(DamageClass.Generic) += 15;

            if (!player.mount.Active)
            {
                player.buffTime[buffIndex] = 0;
            }
        }
    }
}
