
using CritRework.Common.Globals;

namespace CritRework.Content.CritTypes.RandomPool
{
    [JITWhenModsEnabled("CalamityMod")]
    internal class StealthStrike : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("CalamityMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod Calamity))
            {
                if (item.DamageType.CountsAsClass(Calamity.Find<DamageClass>("RogueDamageClass")))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod Calamity))
            {
                if (Projectile == null)
                {
                    ModPlayer mp = Player.GetModPlayer(Calamity.Find<ModPlayer>("CalamityPlayer"));

                    if (mp.GetType().GetField("rogueStealth").GetValue(mp) is float a && mp.GetType().GetField("rogueStealthMax").GetValue(mp) is float b)
                    {
                        return a >= b;
                    }
                }
                if (Projectile.TryGetGlobalProjectile(Calamity.Find<GlobalProjectile>("CalamityGlobalProjectile"), out var p))
                {
                    if (p.GetType().GetField("stealthStrike").GetValue(p) is bool b)
                    {
                        return b;
                    }
                }
            }
            return false;
        }
    }
}
