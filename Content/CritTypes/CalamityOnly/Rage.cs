using CritRework.Common.ModPlayers;
using MonoMod.RuntimeDetour;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using Terraria;
using CritRework.Common.Globals;

namespace CritRework.Content.CritTypes.RandomPool
{
    [JITWhenModsEnabled("CalamityMod")]
    internal class Rage : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("CalamityMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.3f;

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod Calamity))
            {
                ModPlayer mp = Player.GetModPlayer(Calamity.Find<ModPlayer>("CalamityPlayer"));

                if (mp.GetType().GetField("rageModeActive").GetValue(mp) is bool adrenaline)
                {
                    return adrenaline;
                }
            }
            return false;
        }
    }
}
