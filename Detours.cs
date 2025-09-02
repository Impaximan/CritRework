using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using Terraria.ModLoader;
using System.Linq.Expressions;
using CritRework.Common.Globals;
using Terraria.GameContent.UI.Chat;

namespace CritRework
{
    public class Detours
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate void orig_SetChatButtons(ref string button, ref string button2);

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public delegate void hook_SetChatButtons(orig_SetChatButtons orig, ref string button, ref string button2);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate void orig_SetCrit(NPC.HitModifiers hitModifiers);

        public static void SetChatButtons(orig_SetChatButtons orig, ref string button, ref string button2)
        {
            orig(ref button, ref button2);

            if (Main.LocalPlayer.TalkNPC != null && Main.LocalPlayer.TalkNPC.type == NPCID.Pirate && CritRework.pirateHijack)
            {
                button2 = pirateButtonText.Value;

                if (Main.LocalPlayer.HeldItem != null && CritItem.CanHaveCrits(Main.LocalPlayer.HeldItem))
                {
                    button2 += " " + Main.LocalPlayer.HeldItem.Name + " (" + Main.ValueToCoins(CritNPC.GetItemHijackCost(Main.LocalPlayer.HeldItem)) + ")";
                }
            }
        }

        #region Allow other mods to set crits
        public static void SetCrit(On_NPC.HitModifiers.orig_SetCrit orig, ref NPC.HitModifiers self)
        {
            //Main.NewText("Set crit");
            orig(ref self);

            if ((bool?)typeof(NPC.HitModifiers).GetField("_critOverride", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self) == true)
            {
                if (CritRework.overrideCritColor) self.HideCombatText();
                //Main.NewText("Set crit successful");
            }
        }

        public static void DisableCrit(On_NPC.HitModifiers.orig_DisableCrit orig, ref NPC.HitModifiers self)
        {
            bool? value = (bool?)typeof(NPC.HitModifiers).GetField("_critOverride", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self);
            if (value == null)
            {
                //Main.NewText("Disable crit");
                orig(ref self);
            }

        }
        #endregion

        public static Hook SetChatButtonsHook;
        public static LocalizedText pirateButtonText;

        public static void Load()
        {
            pirateButtonText = CritRework.instance.GetLocalization($"PirateHijack");
            
            SetChatButtonsHook = SetChatButtonsHook = new Hook(typeof(NPCLoader).GetMethod("SetChatButtons"), SetChatButtons);
            SetChatButtonsHook.Apply();

            On_NPC.HitModifiers.SetCrit += SetCrit;
            On_NPC.HitModifiers.DisableCrit += DisableCrit;
        }

        public static void Unload()
        {
            SetChatButtonsHook.Undo();
            On_NPC.HitModifiers.SetCrit -= SetCrit;
            On_NPC.HitModifiers.DisableCrit -= DisableCrit;
        }
    }
}
