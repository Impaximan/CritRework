using CritRework.Content.Items.Whetstones;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent.UI.Chat;
using Terraria.GameContent.ItemDropRules;
using CritRework.Content.Items.Equipable.Accessories;
using CritRework.Content.Items.Weapons;
using CritRework.Content.Items.Weapons.Gloves;
using CritRework.Common.ModPlayers;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using System.IO;
using CritRework.Content.Items;

namespace CritRework.Common.Globals
{
    public class CritNPC : GlobalNPC
    {
        public bool travellingMerchantGivenWhetstone = false;

        public override bool InstancePerEntity => true;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (npc.type == NPCID.TravellingMerchant)
            {
                bitWriter.WriteBit(travellingMerchantGivenWhetstone);
            }
        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            if (npc.type == NPCID.TravellingMerchant)
            {
                travellingMerchantGivenWhetstone = bitReader.ReadBit();
            }
        }

        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add<LeatherGlove>();
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

            if (shop.NpcType == NPCID.SkeletonMerchant)
            {
                shop.Add<WhetstoneExtractor>();
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

            if (player.TryGetModPlayer(out CritPlayer c) && c.uniqueCritSound.HasValue)
            {
                style = c.uniqueCritSound.Value;
            }

            if (hit.Crit && (CritRework.critSounds || (c != null && c.uniqueCritSound != null)))
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

            if (Main.player[projectile.owner].TryGetModPlayer(out CritPlayer c) && c.uniqueCritSound.HasValue)
            {
                style = c.uniqueCritSound.Value;
            }

            if (hit.Crit && (CritRework.critSounds || (c != null && c.uniqueCritSound != null)))
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

        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {

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

            if (npc.type == NPCID.BloodZombie)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RabidClaw>(), 10));
            }

            if (npc.type == NPCID.Drippler)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<NoxiousEye>(), 20));
            }

            if (npc.aiStyle == NPCAIStyleID.Slime && !npc.friendly && !npc.CountsAsACritter)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SparkingSludge>(), 125));
            }
        }

        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.Common(ModContent.ItemType<BasicWhetstone>(), 250));
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            //Travelling merchant basic whetstones
            if (npc.type == NPCID.TravellingMerchant)
            {
                if (!firstButton)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);

                    if (travellingMerchantGivenWhetstone)
                    {
                        Main.npcChatText = TravellingMerchantAlreadyGivenWhetstoneDialogue();
                    }
                    else if (Main.LocalPlayer.BuyItem(Item.buyPrice(0, 5, 0, 0)))
                    {
                        travellingMerchantGivenWhetstone = true;
                        SoundEngine.PlaySound(SoundID.Coins);
                        Main.npcChatText = TravellingMerchantGiveWhetstoneDialogue();

                        for (int i = 0; i < Main.rand.Next(2, 5); i++)
                        {
                            Item item = new Item(ModContent.ItemType<BasicWhetstone>());
                            item.GetGlobalItem<CritItem>().AddCritType(item);
                            Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc("TravellingMerchantGiveItem"), item);
                        }

                        npc.netUpdate = true;
                    }
                    else
                    {
                        Main.npcChatText = TravellingMerchantBrokeDialogue();
                    }
                }
            }

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
                                    Volume = 0.3f
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

        public string TravellingMerchantGiveWhetstoneDialogue()
        {
            List<string> potentialDialogues = new();

            int num = 1;
            while (Language.Exists("Mods.CritRework.TravellingMerchantGiveWhetstone" + num.ToString()))
            {
                potentialDialogues.Add(Language.GetTextValue("Mods.CritRework.TravellingMerchantGiveWhetstone" + num.ToString()));

                num++;
            }

            return Main.rand.Next(potentialDialogues);
        }

        public string TravellingMerchantBrokeDialogue()
        {
            List<string> potentialDialogues = new();

            int num = 1;
            while (Language.Exists("Mods.CritRework.TravellingMerchantBroke" + num.ToString()))
            {
                potentialDialogues.Add(Language.GetTextValue("Mods.CritRework.TravellingMerchantBroke" + num.ToString()));

                num++;
            }

            return Main.rand.Next(potentialDialogues);
        }


        public string TravellingMerchantAlreadyGivenWhetstoneDialogue()
        {
            List<string> potentialDialogues = new();

            int num = 1;
            while (Language.Exists("Mods.CritRework.TravellingMerchantAlreadyGiven" + num.ToString()))
            {
                potentialDialogues.Add(Language.GetTextValue("Mods.CritRework.TravellingMerchantAlreadyGiven" + num.ToString()));

                num++;
            }

            return Main.rand.Next(potentialDialogues);
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

        public override void SetDefaults(NPC NPC)
        {
            if (NPC.lifeMax == int.MaxValue)
            {
                return;
            }

            if (NPC.boss || new List<int>() { NPCID.EaterofWorldsHead,
                NPCID.EaterofWorldsBody,
                NPCID.EaterofWorldsTail,
                NPCID.Creeper,
                NPCID.PlanterasTentacle,
                NPCID.TheHungry,
                NPCID.TheHungryII,
                NPCID.GolemFistLeft,
                NPCID.GolemFistRight,
                NPCID.GolemHead}.Contains(NPC.type))
            {
                NPC.lifeMax = (int)(NPC.lifeMax * CritRework.bossLife);
            }
            else
            {
                NPC.lifeMax = (int)(NPC.lifeMax * CritRework.enemyLife);
            }
        }
    }
}
