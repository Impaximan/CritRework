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
    [JITWhenModsEnabled("OrchidMod")]
    internal class Shapeshifter_AfterSkillUse : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("OrchidMod");
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate void orig_ShapeshiftOnRightClick(OrchidMod.Content.Shapeshifter.OrchidModShapeshifterShapeshift self, Projectile projectile, OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftAnchor anchor, Player player, OrchidMod.OrchidShapeshifter shapeshifter);

        public void ShapeshiftOnRightClick(orig_ShapeshiftOnRightClick orig, OrchidMod.Content.Shapeshifter.OrchidModShapeshifterShapeshift self, Projectile projectile, OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftAnchor anchor, Player player, OrchidMod.OrchidShapeshifter shapeshifter)
        {
            if (player.TryGetModPlayer(out CritPlayer c) && self.Item.TryGetGlobalItem(out CritItem critItem))
            {
                if (critItem.critType is Shapeshifter_AfterSkillUse)
                {
                    c.hasSkillBonus = true;
                }
            }

            orig(self, projectile, anchor, player, shapeshifter);
        }

        public static List<Hook> ShapeshiftOnRightClickHooks;

        public override void OnLoad(Mod mod)
        {
            ShapeshiftOnRightClickHooks = new List<Hook>();

            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidMod))
            {
                foreach (Type type in OrchidMod.Code.GetTypes().Where(x => x.IsSubclassOf(typeof(OrchidMod.Content.Shapeshifter.OrchidModShapeshifterShapeshift))))
                {
                    Hook ShapeshiftOnRightClickHook = new Hook(type.GetMethod("ShapeshiftOnRightClick"), ShapeshiftOnRightClick);
                    ShapeshiftOnRightClickHook.Apply();
                    ShapeshiftOnRightClickHooks.Add(ShapeshiftOnRightClickHook);
                }
            }
        }

        public override void OnUnload()
        {
            foreach (Hook hook in ShapeshiftOnRightClickHooks)
            {
                hook.Undo();
            }

            ShapeshiftOnRightClickHooks.Clear();
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.65f;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidMod))
            {
                return item.DamageType.CountsAsClass(OrchidMod.Find<DamageClass>("ShapeshifterDamageClass"));
            }

            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            if (Player.TryGetModPlayer(out CritPlayer c) && c.hasSkillBonus)
            {
                if (target != null) c.hasSkillBonus = false;
                return true;
            }
            return false;
        }
    }
}
