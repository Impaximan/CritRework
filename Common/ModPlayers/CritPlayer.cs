using CritRework.Common.Globals;
using CritRework.Content.Buffs;
using CritRework.Content.CritTypes.WeaponSpecific;
using CritRework.Content.CritTypes.WhetstoneSpecific;
using CritRework.Content.Items.Equipable.Accessories;
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
        public bool pirateArmor = false;

        public Item lastWeaponUsed = null;

        public bool ammoUsedThisFrame = false;

        public List<string> accessoryEffects = new List<string>();

        public static LocalizedText scallywagText;

        public CritType? summonCrit = null;

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
            UpdateSlotMachine();
        }

        public override void UpdateAutopause()
        {
            UpdateSlotMachine();
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


            if (Player.velocity.Y == 0 ||
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
            if (critType != null && critType.ShouldCrit(Player, item, projectile, target, modifiers))
            {
                modifiers.SetCrit();
                modifiers.SourceDamage *= CritType.CalculateActualCritMult(critType, item, Player);
                modifiers.FinalDamage *= 0.5f;
                if (CritRework.overrideCritColor) modifiers.HideCombatText();
            }
            else
            {
                if (item.TryGetGlobalItem(out CritItem cItem))
                {
                    if (cItem.critType != null && item.DamageType.UseStandardCritCalcs)
                    {
                        modifiers.DisableCrit();
                    }
                }
            }

            return modifiers;
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
                            Item.NewItem(new EntitySource_OnHit(Player, target, "MugShotHit"), Player.getRect(), new Item(ItemID.PlatinumCoin));
                        }
                        else
                        {
                            Item.NewItem(new EntitySource_OnHit(Player, target, "MugShotHit"), Player.getRect(), new Item(ItemID.GoldCoin, Main.rand.Next(1, 3)));
                        }
                    }
                    else
                    {
                        Item.NewItem(new EntitySource_OnHit(Player, target, "MugShotHit"), Player.getRect(), new Item(ItemID.SilverCoin, Main.rand.Next(3, 11)));
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
        }
    }
}
