using System.ComponentModel;
using System.Reflection;
using MonoMod.RuntimeDetour;
using CritRework.Common.Globals;
using CritRework.Content.Prefixes.Weapon;
using CritRework.Common.ModPlayers;
using CritRework.Content.CritTypes.WeaponSpecific;
using CritRework.Content.Items.Whetstones;

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

            if (Main.LocalPlayer.TalkNPC != null)
            {
                if (Main.LocalPlayer.TalkNPC.type == NPCID.Pirate && CritRework.pirateHijack)
                {
                    button2 = pirateButtonText.Value;

                    if (Main.LocalPlayer.HeldItem != null && CritItem.CanHaveCrits(Main.LocalPlayer.HeldItem))
                    {
                        button2 += " " + Main.LocalPlayer.HeldItem.Name + " (" + Main.ValueToCoins(CritNPC.GetItemHijackCost(Main.LocalPlayer.HeldItem)) + ")";
                    }
                }

                if (Main.LocalPlayer.TalkNPC.type == NPCID.TravellingMerchant && CritRework.travellingMerchantWhetstones)
                {
                    button2 = travellingMerchantButtonText.Value;
                    if (!Main.LocalPlayer.TalkNPC.GetGlobalNPC<CritNPC>().travellingMerchantGivenWhetstone)
                    {
                        button2 += " (" + Main.ValueToCoins(Item.buyPrice(0, 5, 0, 0)) + ")";
                    }
                }
            }
        }

        public static string AffixName(On_Item.orig_AffixName orig, Item self)
        {
            if (self != null && !self.IsAir && self.TryGetGlobalItem(out CritItem c) && c.critType != null)
            {
                if (self.type == ModContent.ItemType<BasicWhetstone>())
                {
                    return c.critType.basicWhetstonePrefix.Value + " " + self.Name;
                }

                if (self.prefix == ModContent.PrefixType<Special>())
                {
                    return c.critType.specialPrefixName.Value + " " + self.Name;
                }
            }

            return orig(self);
        }

        public static int StrikeNPC(On_NPC.orig_StrikeNPC_HitInfo_bool_bool orig, NPC self, NPC.HitInfo hit, bool fromNet = false, bool noPlayerInteraction = false)
        {
            if (fromNet && hit.Crit)
            {
                if (Main.LocalPlayer.TryGetModPlayer(out CritPlayer critPlayer))
                {
                    if (Main.LocalPlayer.HeldItem != null && Main.LocalPlayer.HeldItem.IsSpecial() && Main.LocalPlayer.HeldItem.TryGetCritType(out CritType c) && c is CritWithAnother)
                    {
                        critPlayer.timeSinceCrit = 0;
                    }
                }
            }

            return orig(self, hit, fromNet, noPlayerInteraction);
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
        public static LocalizedText travellingMerchantButtonText;

        public static void Load()
        {
            pirateButtonText = CritRework.instance.GetLocalization($"PirateHijack");
            travellingMerchantButtonText = CritRework.instance.GetLocalization($"TravellingMerchantGiveWhetstone");
            
            SetChatButtonsHook = new Hook(typeof(NPCLoader).GetMethod("SetChatButtons"), SetChatButtons);
            SetChatButtonsHook.Apply();

            On_NPC.HitModifiers.SetCrit += SetCrit;
            On_NPC.HitModifiers.DisableCrit += DisableCrit;
            On_Item.AffixName += AffixName;
            On_NPC.StrikeNPC_HitInfo_bool_bool += StrikeNPC;
        }

        public static void Unload()
        {
            SetChatButtonsHook.Undo();
            On_NPC.HitModifiers.SetCrit -= SetCrit;
            On_NPC.HitModifiers.DisableCrit -= DisableCrit;
            On_Item.AffixName -= AffixName;
        }
    }
}
