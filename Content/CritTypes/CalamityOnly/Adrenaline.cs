using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.RandomPool
{
    [JITWhenModsEnabled("CalamityMod")]
    internal class Adrenaline : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("CalamityMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item)
        {
            if (Item.IsSpecial())
            {
                if (Player.TryGetModPlayer(out CritPlayer critPlayer))
                {
                    return 1.6f + critPlayer.timeWithMaxAdrenaline * 0.075f / 60f;
                }
            }
            return 1.6f;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod Calamity))
            {
                ModPlayer mp = Player.GetModPlayer(Calamity.Find<ModPlayer>("CalamityPlayer"));

                if(mp.GetType().GetField("adrenalineModeActive").GetValue(mp) is bool adrenaline)
                {
                    return adrenaline;
                }
            }
            return false;
        }
    }
}
