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
    [JITWhenModsEnabled("ThoriumMod")]
    internal class LowInspiration : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("ThoriumMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.3f;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod Thorium))
            {
                if (item.DamageType.CountsAsClass(Thorium.Find<DamageClass>("BardDamage")))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod Thorium))
            {
                ModPlayer mp = Player.GetModPlayer(Thorium.Find<ModPlayer>("ThoriumPlayer"));

                if (mp.GetType().GetField("bardResource").GetValue(mp) is int inspiration && mp.GetType().GetField("bardResourceMax").GetValue(mp) is int inspirationMax)
                {
                    return inspiration <= inspirationMax * 0.3f;
                }
            }
            return false;
        }
    }
}
