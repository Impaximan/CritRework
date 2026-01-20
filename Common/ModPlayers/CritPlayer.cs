using CritRework.Common.Globals;
using CritRework.Content.Buffs;
using CritRework.Content.CritTypes.WeaponSpecific;
using CritRework.Content.CritTypes.WhetstoneSpecific;
using CritRework.Content.Items.Bronze.BronzeArmor;
using CritRework.Content.Items.Equipable.Accessories;
using CritRework.Content.Items.Weapons.Gloves;
using CritRework.Content.Items.Whetstones;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Common.ModPlayers
{
    public class CritPlayer : ModPlayer
    {
        public int timeSinceLastHit = 0;
        public bool slotMachineCritCrafting = false;
        public CritType slotMachineCrit = null;
        public Item slotMachineItem = null;

        int slotTime = 0;
        public int currentSlotTime = maxCurrentSlotTime;
        public const int maxCurrentSlotTime = 30;
        public const int minCurrentSlotTime = 5;
        public int freshItemTime = 0;
        public int timeSinceLastTooltipShown = 0;
        public int timeSinceGoldPickup = 0;
        public int timeSinceHeal = 0;
        public int timeSinceDeath = 0;
        public int timeSinceHook = 0;
        public int timeSinceCrit = 0;
        public int timeFalling = 0;
        public int timeSinceDaggerBonus = 600;
        public int timeSinceArrowPickup = 0;
        public int noxiousEyeCooldown = 0;
        public int timeSinceBlowpipe = 0;
        public bool pirateArmor = false;
        public bool allowNewChakram = false;

        //Thorium mod only
        public int lastTechPoints = 0;
        public int timeSinceTechnique = 600;
        public int lastTechDecharge = 0;

        //Orchid mod only
        public int timeSinceShawlDash = 600;
        public bool hasSkillBonus = false;
        public int timeSincePredatorHit = 600;
        public int timeSinceSymbioteHit = 600;
        public int timeSinceSageHit = 600;
        public int timeSinceWardenHit = 600;

        public int crystalShieldDefense = 0;
        int lastShawlCooldown = -1;

        public Item lastWeaponUsed = null;

        public bool ammoUsedThisFrame = false;

        public List<string> accessoryEffects = new List<string>();

        public static LocalizedText scallywagText;

        //Extra crit types
        public CritType? summonCrit = null;
        public CritType? prostheticCrit = null;

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> list = new();

            if (!mediumCoreDeath)
            {
                list.Add(new Item(ModContent.ItemType<StarterWhetstone>(), 2));
            }

            return list;
        }

        public override void Load()
        {
            scallywagText = Mod.GetLocalization($"ScallywagText");
        }

        public override void ResetEffects()
        {
            slotMachineCritCrafting = false;
            pirateArmor = false;
            accessoryEffects.Clear();
            summonCrit = null;
            prostheticCrit = null;
        }

        int crystalLossCounter = 0;
        public override void UpdateEquips()
        {
            if (Player.HasEquip<CrystalShield>())
            {
                Player.statDefense += crystalShieldDefense;

                if (crystalShieldDefense > 0)
                {
                    crystalLossCounter++;

                    if (crystalLossCounter > 180f - 3.3f * crystalShieldDefense)
                    {
                        crystalLossCounter = 0;
                        crystalShieldDefense--;
                        CombatText.NewText(Player.getRect(), Color.LightPink, -1, false, true);
                    }
                }
            }
        }

        public override void PostUpdate()
        {
            timeSinceLastHit++;
            freshItemTime++;
            timeSinceGoldPickup++;
            timeSinceHeal++;
            timeSinceHook++;
            timeSinceDeath++;
            timeFalling++;
            timeSinceCrit++;
            timeSinceDaggerBonus++;
            timeSinceArrowPickup++;
            timeSinceBlowpipe++;
            if (noxiousEyeCooldown > 0) noxiousEyeCooldown--;
            UpdateSlotMachine();

            if (ModLoader.HasMod("OrchidMod"))
            {
                PostUpdate_Orchid();
            }

            if (ModLoader.TryGetMod("ThoriumMod", out Mod Thorium))
            {
                timeSinceTechnique++;

                ModPlayer mp = Player.GetModPlayer(Thorium.Find<ModPlayer>("ThoriumPlayer"));

                if (mp.GetType().GetField("techPoints").GetValue(mp) is int techPoints && mp.GetType().GetField("techDecharge").GetValue(mp) is int techDecharge)
                {
                    int diff = techPoints - lastTechPoints;
                    lastTechPoints = techPoints;

                    if (diff < 0 && techDecharge == 0 && techDecharge == lastTechDecharge)
                    {
                        timeSinceTechnique = 0;
                    }

                    lastTechDecharge = techDecharge;
                }
            }
        }

        [JITWhenModsEnabled("OrchidMod")]
        public void PostUpdate_Orchid()
        {
            timeSinceShawlDash++;
            timeSincePredatorHit++;
            timeSinceWardenHit++;
            timeSinceSymbioteHit++;
            timeSinceSageHit++;

            if (Player.TryGetModPlayer(out OrchidMod.OrchidShapeshifter s))
            {
                if (s.ShapeshifterShawlCooldown > lastShawlCooldown && lastShawlCooldown == 0)
                {
                    timeSinceShawlDash = 0;
                }

                lastShawlCooldown = s.ShapeshifterShawlCooldown;
            }
        }

        public override void UpdateAutopause()
        {
            UpdateSlotMachine();
        }

        public override void UpdateBadLifeRegen()
        {
            if (Player.HasBuff<Repentence>())
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                }

                Player.lifeRegen -= 30;
            }
        }

        public void UpdateSlotMachine()
        {
            timeSinceLastTooltipShown++;

            if (!slotMachineCritCrafting)
            {
                return;
            }

            slotTime--;
            if (slotTime <= 0)
            {
                slotTime = currentSlotTime;

                if (timeSinceLastTooltipShown > 300)
                {
                    currentSlotTime = (int)MathHelper.Lerp(currentSlotTime, maxCurrentSlotTime, 0.15f);
                }
                else if (timeSinceLastTooltipShown <= 2)
                {
                    currentSlotTime = (int)MathHelper.Lerp(currentSlotTime, minCurrentSlotTime, 0.075f);
                }

                if (slotMachineItem != null)
                {
                    if (slotMachineItem.damage == -1 || slotMachineItem.ammo != AmmoID.None)
                    {
                        return;
                    }

                    foreach (CritType crit in CritRework.loadedCritTypes)
                    {
                        if (crit.ForceOnItem(slotMachineItem))
                        {
                            slotMachineCrit = crit;
                            return;
                        }
                    }


                    if (CritRework.randomCritPool.Count <= 0 || slotMachineItem.consumable)
                    {
                        return;
                    }


                    if (!slotMachineItem.DamageType.CountsAsClass(DamageClass.Summon) && CritRework.randomCritPool.Exists(x => x.CanApplyTo(slotMachineItem)))
                    {
                        CritType appliedType = Main.rand.Next(CritRework.randomCritPool);
                        while (!appliedType.CanApplyTo(slotMachineItem))
                        {
                            appliedType = Main.rand.Next(CritRework.randomCritPool);
                        }
                        slotMachineCrit = appliedType;
                    }
                }
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            timeSinceLastHit = 0;
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            timeSinceLastHit = 0;
        }

        int lastHealth = 1000;
        public override void PostUpdateMiscEffects()
        {
            int diff = Player.statLife - lastHealth;
            lastHealth = Player.statLife;

            if (diff >= 20)
            {
                timeSinceHeal = 0;
            }

            if (Player.grapCount > 0)
            {
                timeSinceHook = 0;
            }


            if (Player.velocity.Y <= 0 ||
                Player.wingTime > 0 && Player.controlJump
                || Player.rocketTime > 0 && Player.controlJump)
            {
                timeFalling = 0;
            }

            ammoUsedThisFrame = false;
        }

        public override void OnConsumeAmmo(Item weapon, Item ammo)
        {
            ammoUsedThisFrame = true;
        }

        public override void ExtraJumpVisuals(ExtraJump jump)
        {
            timeFalling = 0;
        }

        public override void UpdateDead()
        {
            timeSinceDeath = 0;
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            CritItem critItem = item.GetGlobalItem<CritItem>();

            if (critItem == null)
            {
                modifiers.DisableCrit();
                return;
            }

            modifiers = ApplyModifiers(item, null, modifiers, critItem.critType, target);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            CritProjectile critProj = proj.GetGlobalProjectile<CritProjectile>();

            if (critProj == null)
            {
                modifiers.DisableCrit();
                return;
            }

            modifiers = ApplyModifiers(critProj.ogItem, proj, modifiers, critProj.critType, target);
        }

        private NPC.HitModifiers ApplyModifiers(Item item, Projectile? projectile, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            if (critType != null && critType.ShouldCrit(Player, item, projectile, target, modifiers) && (prostheticCrit == null || prostheticCrit.ShouldCrit(Player, item, projectile, target, modifiers)))
            {
                modifiers.SetCrit();
                modifiers.SourceDamage *= CritType.CalculateActualCritMult(critType, item, Player);

                if (timeSinceDaggerBonus >= 900 && Player.HasEquip<AssassinsDagger>())
                {
                    modifiers.SourceDamage *= 2;
                    timeSinceDaggerBonus = 0;
                    CombatText.NewText(Player.getRect(), new Color(172, 44, 77), "Assassin's Strike!", true);
                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/BloodSlash")
                    {
                        PitchVariance = 0.5f,
                        Volume = 1f
                    }, target.Center);
                }

                if (Player.HasEquip<EternalGuillotine>() && target.life == target.lifeMax)
                {
                    modifiers.CritDamage *= 2f;
                }

                if (Player.HasEquip<ThawingCloth>())
                {
                    int num = 0;

                    List<int> oddExceptions = new()
                    {
                        BuffID.OnFire3
                    };

                    foreach (int type in target.buffType)
                    {
                        if (type > 0 && (Main.debuff[type] || oddExceptions.Contains(type)) //FSR some debuffs from the base game are not marked as debuffs. This aims to patch some of those.
                            && target.buffTime[target.FindBuffIndex(type)] > 0)
                        {
                            num++;
                        }
                    }

                    if (num > 0)
                    {
                        modifiers.CritDamage *= 1f + ThawingCloth.damageBonusPerDebuff * num;
                    }
                }

                modifiers.FinalDamage *= 0.5f;
                if (CritRework.overrideCritColor) modifiers.HideCombatText();
            }
            else
            {
                if (item != null && item.TryGetGlobalItem(out CritItem cItem))
                {
                    if (item.DamageType.UseStandardCritCalcs)
                    {
                        modifiers.DisableCrit();
                    }
                }
            }

            return modifiers;
        }


        public override void OnHurt(Player.HurtInfo info)
        {
            if (crystalShieldDefense > 0)
            {
                if (Player.HasEquip<CrystalShield>())
                {
                    SoundEngine.PlaySound(SoundID.Shatter, Player.Center);
                    CombatText.NewText(Player.getRect(), Color.Pink, -crystalShieldDefense);
                }


                crystalShieldDefense = 0;
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && item.TryGetGlobalItem(out CritItem critItem) && critItem.critType is not CritWithAnother)
            {
                timeSinceCrit = 0;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && proj.TryGetGlobalProjectile(out CritProjectile crit) && crit.critType is not CritWithAnother)
            {
                timeSinceCrit = 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Player.HasEquip<MugShot>() && hit.Crit && target.type != NPCID.TargetDummy)
            {
                if (Main.rand.NextFloat() <= 0.2f + 0.1f * Player.luck)
                {
                    if (Main.rand.NextBool(20))
                    {
                        if (Main.rand.NextBool(100))
                        {
                            Player.QuickSpawnItem(new EntitySource_OnHit(Player, target, "MugShotHit"), ItemID.PlatinumCoin, 1);
                        }
                        else
                        {
                            Player.QuickSpawnItem(new EntitySource_OnHit(Player, target, "MugShotHit"), ItemID.GoldCoin, Main.rand.Next(1, 3));
                        }
                    }
                    else
                    {
                        Player.QuickSpawnItem(new EntitySource_OnHit(Player, target, "MugShotHit"), ItemID.SilverCoin, Main.rand.Next(3, 11));
                    }

                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/CoinCrit")
                    {
                        Volume = 1f,
                        PitchVariance = 0.35f
                    });
                }
            }

            if (pirateArmor && hit.Crit && Player.mount.Active)
            {
                if (!Player.HasBuff<Scallywag>())
                {
                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/PirateLaugh")
                    {
                        Volume = 1f,
                        PitchVariance = 0.25f
                    }, Player.Center);

                    CombatText.NewText(Player.getRect(), Color.Beige, scallywagText.Value, true);
                }

                Player.AddBuff(ModContent.BuffType<Scallywag>(), 300);
            }


            bool cleansed = false;

            if (Player.HasEquip<ThawingCloth>() && hit.Crit)
            {
                List<int> oddExceptions = new()
                {
                    BuffID.OnFire3
                };

                int numCleansed = 0;
                for (int i = 0; i < target.buffType.Length; i++)
                {
                    if (Main.debuff[target.buffType[i]] || oddExceptions.Contains(target.buffType[i]))
                    {
                        numCleansed++;
                        target.buffTime[i] = 0;
                        cleansed = true;
                    }
                }

                if (cleansed)
                {
                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Cleanse")
                    {
                        Volume = 0.65f,
                        Pitch = 0.5f,
                        PitchVariance = 0.5f,
                        MaxInstances = 0,
                    }, target.Center);

                    CombatText.NewText(target.getRect(), Color.SkyBlue, "Thawed x" + numCleansed, true, true);
                }

            }

            if (Player.HasEquip<NoxiousEye>() && hit.Crit && noxiousEyeCooldown <= 0)
            {
                if (!cleansed) target.AddBuff(BuffID.Poisoned, 30);

                if (Main.rand.NextBool(10) && target.type != NPCID.TargetDummy)
                {
                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Gas")
                    {
                        Volume = 1.5f,
                        PitchVariance = 0.25f
                    }, target.Center);

                    CombatText.NewText(target.getRect(), Color.Crimson, "Exsanguinated", false, true);

                    noxiousEyeCooldown = 180;

                    int i = Item.NewItem(new EntitySource_OnHit(Player, target, "NoxiousEyeHit"), target.getRect(), new Item(ItemID.Heart, 1));
                    if (Main.netMode == NetmodeID.MultiplayerClient) NetMessage.SendData(MessageID.SyncItem, number: i, number2: 1);
                }
            }

            if (Player.HasEquip<SparkingSludge>() && hit.Crit && !cleansed)
            {
                if (!target.HasBuff(BuffID.OnFire))
                {
                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/FireIgnite")
                    {
                        Volume = 0.4f,
                        Pitch = 0f,
                        PitchVariance = 0.5f,
                        MaxInstances = 0,
                        SoundLimitBehavior = SoundLimitBehavior.IgnoreNew,
                    }, target.Center);
                }

                target.AddBuff(BuffID.OnFire, 60 * 3);
            }

            if (Player.HasEquip<CrystalShield>() && hit.Crit)
            {
                crystalShieldDefense++;

                CombatText.NewText(Player.getRect(), Color.LightBlue, 1, false, true);
                crystalLossCounter = 0;

                if (crystalShieldDefense > CrystalShield.maxDefense)
                {
                    crystalShieldDefense = CrystalShield.maxDefense;
                }
            }

            if (Player.HasEquip<BronzeHelm>() && hit.DamageType.CountsAsClass(DamageClass.Melee) && hit.Crit)
            {
                Player.ManaEffect(5);
                Player.statMana += 5;
            }
        }
    }
}
