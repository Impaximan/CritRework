using CritRework.Content.Items.Whetstones;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using CritRework.Content.Items.Equipable.Accessories;

namespace CritRework.Common.Globals
{
    public class CritNPC : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add<GreedyWhetstone>();
            }

            if (shop.NpcType == NPCID.Demolitionist)
            {
                shop.Add<VolatileWhetstone>();
            }

            if (shop.NpcType == NPCID.ArmsDealer)
            {
                shop.Add<AmmoWhetstone>();
            }

            if (shop.NpcType == NPCID.GoblinTinkerer)
            {
                shop.Add<PreparedWhetstone>();
            }

            if (shop.NpcType == NPCID.Pirate)
            {
                shop.Add<WiseCracker>();
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            SoundStyle style = new("CritRework/Assets/Sounds/Crit")
            {
                PitchVariance = 0.5f,
                Pitch = -0.3f,
                SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                MaxInstances = 10,
                Volume = 1.75f
            };

            if (CritRework.critSounds && hit.Crit)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    SoundEngine.PlaySound(style);
                }

                if (CritRework.overrideCritColor)
                {
                    CombatText.NewText(npc.getRect(), CritRework.critColor, hit.Damage, true);
                }
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            SoundStyle style = new("CritRework/Assets/Sounds/Crit")
            {
                PitchVariance = 0.5f,
                Pitch = -0.3f,
                SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                MaxInstances = 10,
                Volume = 1.75f
            };

            if (CritRework.critSounds && hit.Crit)
            {
                if (projectile.owner == Main.myPlayer)
                {
                    SoundEngine.PlaySound(style);
                }

                if (CritRework.overrideCritColor)
                {
                    CombatText.NewText(npc.getRect(), CritRework.critColor, hit.Damage, true);
                }
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.PirateDeadeye)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MugShot>(), 15));
            }

            if (npc.type == NPCID.GoblinThief)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AssassinsDagger>(), 6));
            }
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (npc.type == NPCID.Pirate)
            {
                if (!firstButton)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);

                    if (Main.LocalPlayer.HeldItem != null)
                    {
                        Item item = Main.LocalPlayer.HeldItem;
                        if (CritItem.CanHaveCrits(item) && item.TryGetGlobalItem(out CritItem cItem))
                        {
                            if (Main.LocalPlayer.BuyItem(GetItemHijackCost(item)))
                            {
                                string before = ItemTagHandler.GenerateTag(item);

                                cItem.AddCritType(item);
                                SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Hijack")
                                {
                                    Volume = 0.5f
                                });
                                SoundEngine.PlaySound(SoundID.Coins);

                                string after = ItemTagHandler.GenerateTag(item);

                                Main.npcChatText = GetPirateSuccessDialogue() + "\n" + before + " [c/ffff00:->] " + after +
                                    "\n[c/fff78b:" + cItem.critType.description.Value + "]";
                                //Main.NewText(before + " hijacked into " + after, Color.Yellow);

                            }
                            else
                            {
                                Main.npcChatText = GetPirateBrokeDialogue();
                            }
                        }
                        else
                        {
                            Main.npcChatText = GetPirateNoCanDoDialogue();
                        }
                    }
                    else
                    {
                        Main.npcChatText = GetPirateNoCanDoDialogue();
                    }
                }
            }
        }

        public static int GetItemHijackCost(Item item)
        {
            int num = item.GetStoreValue() / 3;
            if (num > 100) num -= num % 100;

            if (num > Item.buyPrice(0, 5, 0, 0))
            {
                num -= num % 10000;
            }

            return num;
        }

        public string GetPirateNoCanDoDialogue()
        {
            List<string> potentialDialogues = new();

            int num = 1;
            while (Language.Exists("Mods.CritRework.PirateNoCanDo" + num.ToString()))
            {
                potentialDialogues.Add(Language.GetTextValue("Mods.CritRework.PirateNoCanDo" + num.ToString()));

                num++;
            }

            return Main.rand.Next(potentialDialogues);
        }

        public string GetPirateBrokeDialogue()
        {
            List<string> potentialDialogues = new();

            int num = 1;
            while (Language.Exists("Mods.CritRework.PirateBroke" + num.ToString()))
            {
                potentialDialogues.Add(Language.GetTextValue("Mods.CritRework.PirateBroke" + num.ToString()));

                num++;
            }

            return Main.rand.Next(potentialDialogues);
        }

        public string GetPirateSuccessDialogue()
        {
            List<string> potentialDialogues = new();

            int num = 1;
            while (Language.Exists("Mods.CritRework.PirateSuccessful" + num.ToString()))
            {
                potentialDialogues.Add(Language.GetTextValue("Mods.CritRework.PirateSuccessful" + num.ToString()));

                num++;
            }

            return Main.rand.Next(potentialDialogues);
        }
    }
}
