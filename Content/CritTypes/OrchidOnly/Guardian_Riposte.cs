using CritRework.Common.ModPlayers;
using MonoMod.RuntimeDetour;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System;
using CritRework.Common.Globals;
using OrchidMod;
using System.Reflection;
using OrchidMod.Content.Guardian;

namespace CritRework.Content.CritTypes.RandomPool
{
    [JITWhenModsEnabled("OrchidMod")]
    internal class Guardian_Riposte : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("OrchidMod");
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate void orig_OnParry(OrchidModGuardianParryItem self, Player player, OrchidGuardian guardian, Entity aggressor, Projectile anchor);

        public void OnParry(orig_OnParry orig, OrchidModGuardianParryItem self, Player player, OrchidGuardian guardian, Entity aggressor, Projectile anchor)
        {
            if (player.TryGetModPlayer(out CritPlayer c) && self.Item.TryGetGlobalItem(out CritItem critItem))
            {
                if (critItem.critType is Guardian_Riposte)
                {
                    c.parryCrit = true;
                }
            }

            orig(self, player, guardian, aggressor, anchor);
        }

        public static List<Hook> OnParryHooks;

        public override void OnLoad(Mod mod)
        {
            OnParryHooks = new List<Hook>();

            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidMod))
            {
                foreach (Type type in OrchidMod.Code.GetTypes().Where(x => x.IsSubclassOf(typeof(OrchidModGuardianParryItem))))
                {
                    MethodInfo? method = type.GetMethod("OnParry");
                    if (method != null)
                    {
                        Hook parryHook = new Hook(method, OnParry);
                        parryHook.Apply();
                        OnParryHooks.Add(parryHook);
                    }
                }
            }
        }

        public override void OnUnload()
        {
            foreach (Hook hook in OnParryHooks)
            {
                hook.Undo();
            }

            OnParryHooks.Clear();
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.85f;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidMod))
            {
                return item.DamageType.CountsAsClass(OrchidMod.Find<DamageClass>("GuardianDamageClass"));
            }

            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            if (Player.TryGetModPlayer(out CritPlayer c) && c.parryCrit)
            {
                if (target != null)
                {
                    if (!specialPrefix || Main.rand.NextBool(2))
                    {
                        c.parryCrit = false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
