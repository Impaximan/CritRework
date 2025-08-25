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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate void hook_SetChatButtons(orig_SetChatButtons orig, ref string button, ref string button2);

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

        public static Hook SetChatButtonsHook;
        public static LocalizedText pirateButtonText;

        public static void Load()
        {
            //NPCLoader.SetChatButtons
            pirateButtonText = CritRework.instance.GetLocalization($"PirateHijack");
            
            SetChatButtonsHook = SetChatButtonsHook = new Hook(typeof(NPCLoader).GetMethod("SetChatButtons"), SetChatButtons);
            SetChatButtonsHook.Apply();
        }

        public static void Unload()
        {
            SetChatButtonsHook.Undo();
        }
    }
}
