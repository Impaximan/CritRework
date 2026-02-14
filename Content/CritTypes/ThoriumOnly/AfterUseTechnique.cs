using CritRework.Common.ModPlayers;

namespace CritRework.Content.CritTypes.RandomPool
{
    [JITWhenModsEnabled("ThoriumMod")]
    internal class AfterUseTechnique : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("ThoriumMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.5f;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod Thorium))
            {
                if (item.DamageType.CountsAsClass(DamageClass.Throwing))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (Player.TryGetModPlayer(out CritPlayer c))
            {
                return c.timeSinceTechnique <= (specialPrefix ? 120 : 60);
            }
            return false;
        }
    }
}
