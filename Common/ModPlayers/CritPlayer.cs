using CritRework.Common.Globals;
using CritRework.Content.Buffs;
using CritRework.Content.CritTypes.WeaponSpecific;
using CritRework.Content.Items.Augmentations;
using CritRework.Content.Items.Bronze.BronzeArmor;
using CritRework.Content.Items.Equipable.Accessories;
using CritRework.Content.Items.Equipable.Accessories.Crackers;
using CritRework.Content.Items.Weapons.Gloves;
using CritRework.Content.Items.Whetstones;
using CritRework.Content.Prefixes.Weapon;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader.IO;
using CritRework.Content.CritTypes.RandomPool;
using System.Linq;

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
        public float healPowerMult = 1f;
        public int timeSinceDeath = 0;
        public int timeSinceHook = 0;
        public int timeSinceCrit = 0;
        public int timeFalling = 0;
        public int timeSinceDaggerBonus = 600;
        public int timeSinceArrowPickup = 0;
        public int noxiousEyeCooldown = 0;
        public int timeSinceBlowpipe = 0;
        public int timeSinceMovingSlow = 0;
        public int timeSinceGravityWell = 0;
        public int fireJumpCounter = 0;
        public float highHpCritMult = 1f;
        public bool pirateArmor = false;
        public bool allowNewChakram = false;
        public float consecutiveCriticalStrikeDamage = 1f;
        public int clockworkCounter = 0;
        public float bucklerPower = 0f;
        public bool firstTimeSpawning = true;
        public Projectile sawProjectile = null;
        public float augmentedWeaponCritBoost = 0f;
        public float potency = 1f;
        public int ambrosiaTextCounter = 0;
        public int ambrosiaTotal = 0;

        public List<Projectile> criticalCurses = new();
        public bool fireCriticalCurse = false;

        public List<Projectile> approaches = new();
        public bool slashApproach = false;
        public int slashDamage = 0;

        private bool lastHitWasCrit = false;

        public int maxAugmentations = 0;

        //Tokens
        public SoundStyle? uniqueCritSound = null;

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

        //Calamity only
        public int timeSinceStealthStrike = 0;
        public int timeWithMaxAdrenaline = 0;

        public int crystalShieldDefense = 0;
        int lastShawlCooldown = -1;

        public Item lastWeaponUsed = null;

        public bool ammoUsedThisFrame = false;

        public List<string> accessoryEffects = new List<string>();

        public static LocalizedText scallywagText;
        public static LocalizedText startingMessage;

        //Extra crit types
        public CritType? summonCrit = null;
        public bool summonSpecial = false;
        public CritType? prostheticCrit = null;
        public CritType? EVILCrit = null;
        public ShadowDonut? shadowDonut = null;

        public Item? bucklerWeapon = null;


        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> list = new();

            if (NameContains("Noelle"))
            {
                list.Add(new Item(ModContent.ItemType<ThornRing>()));
            }

            if (NameContains("Apophis") || NameContains("Cult"))
            {
                list.Add(new Item(ModContent.ItemType<Apophis>()));
            }

            if (NameContains("Benjamin Franklin") || NameContains("Ben Franklin"))
            {
                list.Add(new Item(ModContent.ItemType<PocketLightningRod>()));
            }

            if ((NameContains("Dark") && NameContains("Soul")) || NameContains("Undead"))
            {
                list.Add(new Item(ModContent.ItemType<Buckler>()));
            }

            if (NameContains("2020") || NameContains("2021") || NameContains("2022"))
            {
                list.Add(new Item(ModContent.ItemType<ManalyticConverter_Copper>()));
            }

            if (NameContains("Light Yagami") || NameContains("Kira ") || Player.name.ToLower() == "kira")
            {
                list.Add(new Item(ModContent.ItemType<CursersQuill>()));
            }

            if (NameContains("Jerry") || NameContains("Seinfeld"))
            {
                list.Add(new Item(ModContent.ItemType<CursersQuill>()));
            }

            if ((NameContains("Ultra") && NameContains("Kill")) || NameContains("V1"))
            {
                list.Add(new Item(ModContent.ItemType<BloodCog>()));
            }

            if (NameContains("Vergil"))
            {
                list.Add(new Item(ModContent.ItemType<ImperfectStorm>()));
            }

            if (!mediumCoreDeath && list.Count == 0)
            {
                list.Add(new Item(ModContent.ItemType<StarterWhetstone>(), 2));
            }

            return list;
        }

        public bool NameContains(string name)
        {
            return Player.name.ToLower().Contains(name.ToLower());
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("firstTimeSpawning", firstTimeSpawning);
        }

        public override void LoadData(TagCompound tag)
        {
            firstTimeSpawning = tag.GetBool("firstTimeSpawning");
        }

        public override void Load()
        {
            scallywagText = Mod.GetLocalization($"ScallywagText");
            startingMessage = Mod.GetLocalization($"StartingMessage");
        }

        public override void ResetEffects()
        {
            slotMachineCritCrafting = false;
            pirateArmor = false;
            accessoryEffects.Clear();
            summonCrit = null;
            EVILCrit = null;
            shadowDonut = null;
            prostheticCrit = null;
            uniqueCritSound = null;
            summonSpecial = false;
            consecutiveCriticalStrikeDamage = 1f;
            potency = 1f;
            maxAugmentations = 0;
            augmentedWeaponCritBoost = 0f;

            if (!Player.HasBuff<BucklerDefense>() && !Player.HasBuff<BucklerRetaliation>())
            {
                bucklerPower = 0f;
            }
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
            timeSinceGravityWell++;
            timeSinceMovingSlow++;
            clockworkCounter++;
            if (noxiousEyeCooldown > 0) noxiousEyeCooldown--;
            UpdateSlotMachine();

            if (sawProjectile != null && !sawProjectile.active)
            {
                sawProjectile = null;
            }

            if (ambrosiaTextCounter > 0)
            {
                ambrosiaTextCounter--;
                
                if (ambrosiaTextCounter <= 0)
                {
                    float seconds = ambrosiaTotal / 60f;

                    CombatText.NewText(Player.getRect(), Color.LightCoral, "-" + Math.Round(seconds, 1).ToString() + "s");

                    ambrosiaTotal = 0;
                }
            }

            if (firstTimeSpawning && Player.whoAmI == Main.myPlayer)
            {
                firstTimeSpawning = false;

                Main.NewText(startingMessage.Value, Color.Yellow);
            }

            if (Player.velocity.Length() <= 2f)
            {
                timeSinceMovingSlow = 0;
            }

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
            if (ModLoader.TryGetMod("CalamityMod", out Mod Calamity))
            {
                ModPlayer mp = Player.GetModPlayer(Calamity.Find<ModPlayer>("CalamityPlayer"));

                if (mp.GetType().GetField("adrenalineModeActive").GetValue(mp) is bool adrenalineActive && mp.GetType().GetField("adrenaline").GetValue(mp) is float adrenaline)
                {
                    if (adrenaline >= 100f)
                    {
                        timeWithMaxAdrenaline++;
                    }
                    else if (!adrenalineActive)
                    {
                        timeWithMaxAdrenaline = 0;
                    }
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

            if (Player.HasBuff<Overclocked>())
            {
                if (Player.HeldItem != null && Player.HeldItem.TryGetGlobalItem(out CritItem critItem))
                {
                    int stack = critItem.augmentations.Count - critItem.MaxAugmentations(Player.HeldItem, Player);

                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                        Player.lifeRegenTime = 0;
                    }

                    Player.lifeRegen -= 30 * stack;
                }
            }
        }

        public override void UpdateDead()
        {
            criticalCurses.Clear();
            timeSinceDeath = 0;
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
        int curseCounter = 0;
        int approachCounter = 0;
        public override void PostUpdateMiscEffects()
        {
            int diff = Player.statLife - lastHealth;
            lastHealth = Player.statLife;

            if (diff >= 20)
            {
                timeSinceHeal = 0;
                healPowerMult = 1f + (diff - 20f) / 100f;
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

            if (fireCriticalCurse && Player.whoAmI == Main.myPlayer)
            {
                curseCounter++;
                if (curseCounter >= 10)
                {
                    curseCounter = 0;

                    EndCurse:

                    if (criticalCurses.Count <= 0)
                    {
                        fireCriticalCurse = false;
                    }
                    else
                    {
                        while (criticalCurses.Count > 0 && (criticalCurses[0] == null || !criticalCurses[0].active))
                        {
                            criticalCurses.RemoveAt(0);
                        }

                        if (criticalCurses.Count <= 0)
                        {
                            goto EndCurse;
                        }

                        criticalCurses[0].ai[0] = 1;
                        criticalCurses[0].velocity = criticalCurses[0].DirectionTo(Main.MouseWorld) * 15f;
                        criticalCurses[0].netUpdate = true;

                        criticalCurses.RemoveAt(0);
                    }
                }
            }

            if (slashApproach && Player.whoAmI == Main.myPlayer)
            {
                approachCounter++;
                if (approachCounter >= 2)
                {
                    approachCounter = 0;

                    EndApproach:

                    if (approaches.Count <= 0)
                    {
                        slashApproach = false;
                    }
                    else
                    {
                        while (approaches.Count > 0 && (approaches[0] == null || !approaches[0].active))
                        {
                            approaches.RemoveAt(0);
                        }

                        if (approaches.Count <= 0)
                        {
                            goto EndApproach;
                        }

                        approaches[0].ai[0] = 1;
                        approaches[0].netUpdate = true;

                        float speed = 30f;
                        float rotation = approaches[0].rotation;

                        Projectile slash = Projectile.NewProjectileDirect(new EntitySource_Parent(approaches[0]), rotation.ToRotationVector2() * speed * -7.5f + approaches[0].Center,
                            rotation.ToRotationVector2() * speed,
                            ModContent.ProjectileType<Storm>(), approaches[0].damage, 0f, Player.whoAmI);

                        slash.SetAsAugmentCrit();
                        slash.netUpdate = true;

                        approaches.RemoveAt(0);
                    }
                }
            }
        }

        public override void OnConsumeAmmo(Item weapon, Item ammo)
        {
            ammoUsedThisFrame = true;
        }

        public override void ExtraJumpVisuals(ExtraJump jump)
        {
            timeFalling = 0;
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.TryGetAugmentation(out BloodCog _))
            {
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(sawProjectile != null && sawProjectile.ai[0] > 0 ? 40 : 15));
            }
        }

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.TryGetAugmentation(out BloodCog _))
            {
                if (sawProjectile == null)
                {
                    float rarityMult = 1f;

                    if (item.OriginalRarity > ItemRarityID.Orange)
                    {
                        rarityMult = 1.3f;
                    }

                    if (item.OriginalRarity > ItemRarityID.Lime)
                    {
                        rarityMult = 1.7f;
                    }

                    if (new List<int>() { 
                        ItemID.Shotgun, 
                        ItemID.Boomstick, 
                        ItemID.QuadBarrelShotgun, 
                        ItemID.OnyxBlaster }
                    .Contains(item.type))
                    {
                        rarityMult *= 2f;
                    }

                    if (item.GetGlobalItem<CritItem>().critType is CloseToFoe)
                    {
                        rarityMult /= 6f;
                    }

                    Projectile saw = Projectile.NewProjectileDirect(new EntitySource_ItemUse(Player, item), Player.Center, Vector2.Zero, ModContent.ProjectileType<SawedOn>(), (int)(damage * 12f * rarityMult / item.useTime), 2f, Player.whoAmI);
                    saw.DamageType = item.DamageType;
                    sawProjectile = saw;
                }
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
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

        public bool ShouldNormallyCrit(Item item, Projectile? projectile, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            return critType != null && critType.ShouldCrit(Player, item, projectile, target, modifiers, item.IsSpecial(Main.LocalPlayer)) && (prostheticCrit == null || prostheticCrit.ShouldCrit(Player, item, projectile, target, modifiers, item.prefix == ModContent.PrefixType<Special>()));
        }

        public bool hitWouldCrit = false;

        private NPC.HitModifiers ApplyModifiers(Item item, Projectile? projectile, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            bool augmentationOverride = false;

            if (item != null && item.TryGetGlobalItem(out CritItem critItem))
            {
                foreach (Augmentation a in critItem.augmentations)
                {

                    if (critItem.AugmentationActive(a, item, Player, target))
                    {
                        if (PrefixLoader.GetPrefix(a.Item.prefix) is Content.Prefixes.Augmentation.AugmentationPrefix prefix)
                        {
                            float critDamage = 1f;
                            float nonCritDamage = 1f;
                            float useTimeMult = 1f;
                            float v = 1f;
                            float p = 1f;

                            prefix.SetStats(ref critDamage, ref critDamage, ref useTimeMult, ref v, ref p);

                            modifiers.CritDamage *= critDamage;
                            modifiers.NonCritDamage *= nonCritDamage;
                        }

                        if (a.OverrideNormalCritBehavior(Player, item, projectile, modifiers, critType, target))
                        {
                            augmentationOverride = true;
                        }
                    }
                }
            }

            hitWouldCrit = false;

            if (critType != null && ShouldNormallyCrit(item, projectile, modifiers, critType, target) || (projectile != null && projectile.IsCritAugment()))
            {
                hitWouldCrit = true;

                if (!augmentationOverride || (projectile != null && projectile.IsCritAugment()))
                {
                    modifiers.FinalDamage *= 0.5f;
                    modifiers.SetCrit();

                    float finalBonusDamageMult = 1f;

                    finalBonusDamageMult *= CritType.CalculateActualCritMult(critType, item, Player);

                    if (timeSinceDaggerBonus >= 900 && Player.HasEquip<AssassinsDagger>())
                    {
                        finalBonusDamageMult *= 2;
                        timeSinceDaggerBonus = 0;
                        CombatText.NewText(Player.getRect(), new Color(172, 44, 77), "Assassin's Strike!", true);
                        SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/BloodSlash")
                        {
                            PitchVariance = 0.5f,
                            Volume = 1f
                        }, target.Center);
                    }

                    if (Player.HasEquip<ShortestStraw>())
                    {
                        finalBonusDamageMult *= MathHelper.Lerp(1f, 1.25f, 1f - target.life / (float)target.lifeMax);
                    }

                    if (lastHitWasCrit)
                    {
                        finalBonusDamageMult *= consecutiveCriticalStrikeDamage;
                    }

                    if (Player.HasEquip<EternalGuillotine>() && target.life == target.lifeMax)
                    {
                        finalBonusDamageMult *= 2f;
                    }

                    if (Player.HasEquip<ThawingCloth>() && !Player.HasBuff<ThawingClothCooldown>())
                    {
                        int num = 0;

                        List<int> oddExceptions = new()
                    {
                        BuffID.OnFire3,
                        BuffID.Frostburn2
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
                            finalBonusDamageMult *= 1f + ThawingCloth.damageBonusPerDebuff * num;
                        }
                    }


                    if (CritRework.overrideCritColor) modifiers.HideCombatText();

                    modifiers.SourceDamage *= finalBonusDamageMult;
                }
                else if (item != null)
                {
                    if (item.DamageType.UseStandardCritCalcs)
                    {
                        modifiers.DisableCrit();
                    }
                }
            }
            else
            {
                if (item != null)
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

                Player.AddBuff(ModContent.BuffType<CrystalShieldCooldown>(), 600);
            }

            if (Player.HasBuff<BucklerDefense>())
            {
                Player.ClearBuff(ModContent.BuffType<BucklerDefense>());
                Player.AddBuff(ModContent.BuffType<BucklerRetaliation>(), 180);
                SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/HeavyMetal"), Player.Center);
                CombatText.NewText(Player.getRect(), Color.Pink, "+" + Math.Round(Buckler.GetDamageBoost(bucklerPower, Player) * 100f) + "%", true);

                PunchCameraModifier modifier = new(Player.Center, new Vector2(info.HitDirection, 0f), 10f, 10f, 8, 500f);
                Main.instance.CameraModifiers.Add(modifier);
            }

            if (EVILCrit != null && EVILCrit.ShouldCrit(Player, shadowDonut.Item, null, null, new(), false))
            {
                Player.AddBuff(ModContent.BuffType<ShadowFrenzy>(), 60 * 5, false);
                CombatText.NewText(Player.getRect(), Color.MediumPurple, "CRITICAL STRIKE", true);

                SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/EVILCrit")
                {
                    PitchVariance = 0.5f,
                    Volume = 0.75f
                }, Player.Center);
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (EVILCrit != null && EVILCrit.ShouldCrit(Player, shadowDonut.Item, null, null, new(), false))
            {
                modifiers.FinalDamage *= EVILCrit.GetDamageMult(Player, shadowDonut.Item);
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.TryGetGlobalItem(out CritItem critItem))
            {
                if (hit.Crit && critItem.critType is not CritWithAnother)
                {
                    timeSinceCrit = 0;
                }

                List<Augmentation> overrides = critItem.augmentations.Where(x => x.OverrideNormalCritBehavior(Player, item, null, null, critItem.critType, target)).ToList();

                if (overrides.Count > 0)
                {
                    Augmentation overAug = Main.rand.Next(overrides);

                    overAug.AugmentationOnHitNPC(Player, item, null, hit, critItem.critType, target, hitWouldCrit);
                }

                foreach (Augmentation augmentation in critItem.augmentations.Where(x => !overrides.Contains(x)))
                {
                    augmentation.AugmentationOnHitNPC(Player, item, null, hit, critItem.critType, target, hitWouldCrit);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.TryGetGlobalProjectile(out CritProjectile crit))
            {
                if (hit.Crit && crit.critType is not CritWithAnother)
                {
                    timeSinceCrit = 0;
                }

                if (crit.ogItem != null && crit.ogItem.TryGetGlobalItem(out CritItem critItem))
                {
                    List<Augmentation> overrides = critItem.augmentations.Where(x => x.OverrideNormalCritBehavior(Player, crit.ogItem, proj, null, crit.critType, target)).ToList();

                    if (overrides.Count > 0)
                    {
                        Augmentation overAug = Main.rand.Next(overrides);

                        overAug.AugmentationOnHitNPC(Player, crit.ogItem, proj, hit, crit.critType, target,  hitWouldCrit);
                    }

                    foreach (Augmentation augmentation in critItem.augmentations.Where(x => !overrides.Contains(x)))
                    {
                        augmentation.AugmentationOnHitNPC(Player, crit.ogItem, proj, hit, crit.critType, target, hitWouldCrit);
                    }
                }
            }


            if (Player.HasEquip<Beautificracker>() && hit.Crit && proj.DamageType == DamageClass.Summon)
            {
                int orbColor = Beautificracker.GetOrbColor(summonCrit, Player, proj.GetGlobalProjectile<CritProjectile>().ogItem);

                float chance = orbColor switch
                {
                    0 => 0.15f,
                    1 => 0.2f,
                    2 => 0.5f,
                    _ => 0.1f
                };

                if (Main.rand.NextFloat() < chance)
                {
                    Projectile orb = Projectile.NewProjectileDirect(new EntitySource_OnHit(proj, target), proj.Center, Main.rand.NextVector2Circular(10f, 10f), ModContent.ProjectileType<BeautyOrb>(), 0, 0f, proj.owner, orbColor);
                    orb.netUpdate = true;
                }
            }
        }

        public override void PostUpdateRunSpeeds()
        {
            if (Player.HasBuff<WalkingWithTheWind>())
            {
                Player.accRunSpeed += 3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            lastHitWasCrit = hit.Crit;

            if (Player.HasEquip<WindWalker>() && hit.Crit)
            {
                if (!Player.HasBuff<WalkingWithTheWind>())
                {
                    CombatText.NewText(Player.getRect(), Color.LightCyan, "Zoom!", true);
                    SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Boost")
                    {
                        Volume = 1f,
                        PitchVariance = 0.5f,
                    }, Player.Center);

                }

                Player.AddBuff(ModContent.BuffType<WalkingWithTheWind>(), 60 * 3, false);
            }

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

            if (Player.HasEquip<ThawingCloth>() && hit.Crit && !Player.HasBuff<ThawingClothCooldown>())
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

                    target.netUpdate = true;
                    Player.AddBuff(ModContent.BuffType<ThawingClothCooldown>(), 60 * 5);
                }

            }

            if (Player.HasEquip<FireInABottle>() && hit.Crit && fireJumpCounter > 0 && Player.velocity.Y != 0)
            {
                fireJumpCounter--;

                if (fireJumpCounter <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Item76, Player.Center);
                    SoundEngine.PlaySound(SoundID.Item73, Player.Center);
                    for (int i = 0; i < 60; i++)
                    {
                        float theta = Main.rand.NextFloat(MathHelper.TwoPi);

                        float distanceMult = Main.rand.NextFloat(0.8f, 1.3f);
                        Vector2 velocity = theta.ToRotationVector2() * 6f * -distanceMult;
                        Vector2 position = Player.Center + theta.ToRotationVector2() * 65f * distanceMult;

                        Dust dust = Dust.NewDustPerfect(position, DustID.Torch, velocity);
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                    }

                    Player.GetJumpState<FireJump>().Available = true;
                }
            }

            if (Player.HasEquip<NoxiousEye>() && hit.Crit)
            {
                if (!cleansed) target.AddBuff(BuffID.Poisoned, 30);

                if (noxiousEyeCooldown <= 0 && Main.rand.NextBool(10) && target.type != NPCID.TargetDummy)
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
