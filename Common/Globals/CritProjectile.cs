using CritRework.Common.ModPlayers;
using CritRework.Content.CritTypes.WeaponSpecific;
using CritRework.Content.CritTypes.WhetstoneSpecific;
using CritRework.Content.Items.Equipable.Accessories;
using CritRework.Content.Items.Weapons.Gloves;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CritRework.Common.Globals
{
    public class CritProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public CritType critType = null;
        public Item ogItem = null;
        public int targetsHit = 0;
        public int wallBounces = 0;
        public int timeActive = 0;
        public int timeSinceHit = 0;
        public List<NPC> npcsHit = new();
        public bool consumedAmmo = false;
        public bool fromNecromantic = false;
        public int targetsKilled = 0;
        public bool blowgunCrit = false;
        public bool thrownUpward = false;
        public float highestPoint = 0;
        public bool fromPoisonedHand = false;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0)
            {
                targetsKilled++;
            }

            targetsHit++;
            timeSinceHit = 0;

            Player owner = Main.player[projectile.owner];

            if (!npcsHit.Contains(target))
            {
                npcsHit.Add(target);
            }

            if (fromNecromantic && hit.Crit && !owner.moonLeech)
            {
                owner.Heal(Content.Prefixes.Weapon.Necromantic.healAmount);
            }

            if (fromPoisonedHand && hit.Crit)
            {
                target.AddBuff(BuffID.Poisoned, 1200);
            }

            if (ModLoader.HasMod("OrchidMod"))
            {
                OnHitNPC_Orchid(projectile, target, hit, damageDone);
            }


            if (ogItem != null)
            {
                if (ogItem.IsSpecial() && ogItem.TryGetCritType(out CritType critType))
                {
                    critType.SpecialPrefixOnHitNPC(ogItem, owner, projectile, target, hit, damageDone);

                    if (ogItem.type == ItemID.Harpoon && hit.Crit && owner.controlUseItem)
                    {
                        Projectile p = Projectile.NewProjectileDirect(new EntitySource_ItemUse(owner, ogItem), owner.Center, ogItem.shootSpeed * owner.DirectionTo(Main.MouseWorld), ogItem.shoot, owner.GetWeaponDamage(ogItem), owner.GetWeaponKnockback(ogItem), owner.whoAmI);
                        p.netUpdate = true;

                        SoundEngine.PlaySound(ogItem.UseSound, owner.Center);
                    }

                    if (ogItem.type == ItemID.TheEyeOfCthulhu)
                    {
                        if (hit.Crit)
                        {
                            if (projectile.idStaticNPCHitCooldown > 5)
                            {
                                SoundEngine.PlaySound(SoundID.ForceRoarPitched, projectile.Center);
                            }
                            projectile.idStaticNPCHitCooldown = 5;
                        }
                        else
                        {
                            projectile.idStaticNPCHitCooldown = 10;
                        }
                    }
                }
            }
        }

        [JITWhenModsEnabled("OrchidMod")]
        public void OnHitNPC_Orchid(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.ModProjectile is OrchidMod.Content.Shapeshifter.OrchidModShapeshifterProjectile p && Main.player[projectile.owner].TryGetModPlayer(out CritPlayer c))
            {
                switch (p.ShapeshifterShapeshiftType)
                {
                    case OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.None:
                        break;
                    case OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Predator:
                        c.timeSincePredatorHit = 0;
                        break;
                    case OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Symbiote:
                        c.timeSinceSymbioteHit = 0;
                        break;
                    case OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Sage:
                        c.timeSinceSageHit = 0;
                        break;
                    case OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Warden:
                        c.timeSinceWardenHit = 0;
                        break;
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ProjectileID.Chik && ogItem != null && ogItem.IsSpecial())
            {
                modifiers.CritDamage *= 1f + (npcsHit.Count - 2) * 0.3f;
            }

            if (projectile.type == ProjectileID.Kraken && ogItem != null && ogItem.IsSpecial())
            {
                modifiers.CritDamage *= projectile.scale;
            }

            if ((projectile.type == ProjectileID.Code1 || projectile.type == ProjectileID.Code2) && ogItem != null && ogItem.IsSpecial())
            {
                modifiers.CritDamage *= 1f + (targetsHit - 10) * 0.05f;
            }
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            wallBounces++;
            if (ogItem != null && ogItem.type == ItemID.ShadowbeamStaff && ogItem.IsSpecial())
            {
                foreach (int i in projectile.localNPCImmunity)
                {
                    projectile.localNPCImmunity[i] = 0;
                }
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }

        public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
        {
            if (projectile.type == ProjectileID.Kraken && ogItem != null && ogItem.IsSpecial())
            {
                Point center = hitbox.Center;
                hitbox.Width = (int)(hitbox.Width * projectile.scale);
                hitbox.Height = (int)(hitbox.Height * projectile.scale);
                hitbox.X = center.X - hitbox.Width / 2;
                hitbox.Y = center.Y - hitbox.Height / 2;
            }
        }

        public override bool PreAI(Projectile projectile)
        {
            return base.PreAI(projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            timeActive++;
            timeSinceHit++;

            if (Main.player[projectile.owner].TryGetModPlayer(out CritPlayer cPlayer))
            {
                if (projectile.minion)
                {
                    if (Main.player[projectile.owner].HasEquip<WiseCracker>())
                    {
                        critType = cPlayer.summonCrit;
                    }
                    else
                    {
                        critType = null;
                    }   
                }
            }

            if (ogItem != null && ogItem.IsSpecial())
            {
                if (ogItem.type == ModContent.ItemType<GalacticGauntlet>() && projectile.velocity.Y > 0)
                {
                    int target = projectile.FindTargetWithLineOfSight(800);

                    if (target != -1)
                    {
                        projectile.velocity = projectile.velocity.Length() * projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.npc[target].Center).ToRotation(), MathHelper.Pi / 180f).ToRotationVector2();
                    }
                }

                if (ogItem.TryGetCritType(out DemonScytheShadowflameBow _) && targetsKilled > 0)
                {
                    int target = projectile.FindTargetWithLineOfSight(800);

                    if (target != -1)
                    {
                        projectile.velocity = projectile.velocity.Length() * projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.npc[target].Center).ToRotation(), MathHelper.Pi / 120f).ToRotationVector2();
                    }
                }

            }

            Player player = Main.player[projectile.owner];
            //Grapple to enemies
            if (player.HeldItem != null && player.HeldItem.IsSpecial() && player.HeldItem.TryGetCritType(out WebCovered _) && Main.projHook[projectile.type])
            {
                foreach (NPC target in Main.npc)
                {
                    if (target != null && target.active && target.getRect().Contains(projectile.Center.ToPoint()))
                    {
                        if (!hasGrappled)
                        {
                            SoundEngine.PlaySound(target.HitSound, target.Center);
                        }

                        if (player.getRect().Modified((int)player.velocity.X, (int)player.velocity.Y, 0, 0).Intersects(target.getRect()) && hasGrappled)
                        {
                            projectile.timeLeft = 0;
                            player.velocity = player.DirectionTo(target.Center) * -5f;
                            player.velocity += target.velocity;
                            player.immune = true;
                            player.immuneTime += 10;
                            SoundEngine.PlaySound(target.HitSound, target.Center);
                        }
                        else
                        {
                            SetGrapple(target.Center, projectile);
                        }

                        break;
                    }
                }
            }

            if (projectile.Center.Y < highestPoint) highestPoint = projectile.Center.Y;
        }

        /// <summary>
        /// Makes a grappling hook think it's grappled onto an object.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="grapple"></param>
        public void SetGrapple(Vector2 position, Projectile grapple)
        {
            hasGrappled = true;
            grapple.ai[0] = 2;
            grapple.position = position;
            grapple.position -= grapple.Size / 2;
            Main.player[grapple.owner].grappling[Main.player[grapple.owner].grapCount] = grapple.whoAmI;
            Main.player[grapple.owner].grapCount++;
            grapple.velocity = Vector2.Zero;
            grapple.netUpdate = true;
        }

        void InheritItemCrit(Projectile projectile, Item item, Player? player = null)
        {
            if (item.TryGetGlobalItem(out CritItem cItem))
            {
                critType = item.GetGlobalItem<CritItem>().critType;
            }


            if (ModLoader.TryGetMod("CalamityMod", out Mod Calamity))
            {
                if (projectile.TryGetGlobalProjectile(Calamity.Find<GlobalProjectile>("CalamityGlobalProjectile"), out var p))
                {
                    if (p.GetType().GetField("stealthStrike").GetValue(p) is bool b && b)
                    {
                        if (player != null && player.TryGetModPlayer(out CritPlayer critPlayer))
                        {
                            critPlayer.timeSinceStealthStrike = 0;
                        }
                    }
                }
            }

            ogItem = item;

            if (item.type == ModContent.ItemType<PoisonedHand>())
            {
                fromPoisonedHand = true;
            }

            if (item.prefix == ModContent.PrefixType<Content.Prefixes.Weapon.Necromantic>())
            {
                fromNecromantic = true;
            }

            if (item.type == ItemID.Trident && item.IsSpecial())
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 8;
            }
            
            if (ogItem.IsSpecial())
            {
                switch (item.type)
                {
                    case ItemID.ShadowFlameKnife:
                        projectile.penetrate = -1;
                        projectile.usesLocalNPCImmunity = true;
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ItemID.ShadowbeamStaff:
                        projectile.usesLocalNPCImmunity = true;
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ItemID.TheEyeOfCthulhu:
                        projectile.usesIDStaticNPCImmunity = true;
                        projectile.idStaticNPCHitCooldown = 10;
                        break;
                    case ItemID.HelFire:
                        projectile.usesIDStaticNPCImmunity = true;
                        projectile.idStaticNPCHitCooldown = 7;
                        break;
                    case ItemID.Amarok:
                        timeSinceHit = 120;
                        break;
                    default:
                        break;
                }
            }

            if (ogItem.IsSpecial() && item.type == ItemID.ShadowbeamStaff)
            {
            }

            if (player != null)
            {
                if (player.TryGetModPlayer(out CritPlayer cPlayer))
                {
                    if (player.HasEquip<WiseCracker>() && projectile.minion && critType == null)
                    {
                        critType = cPlayer.summonCrit;
                    }

                    if (item.type == ItemID.Blowpipe || item.type == ItemID.Blowgun)
                    {
                        if (cPlayer.timeSinceBlowpipe > 180)
                        {
                            blowgunCrit = true;
                        }
                        cPlayer.timeSinceBlowpipe = 0;
                    }
                }
            }
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            targetsHit = 0;
            npcsHit = new();
            wallBounces = 0;
            timeActive = 0;
            targetsKilled = 0;
            blowgunCrit = false;
            timeSinceHit = 0;
            thrownUpward = projectile.velocity.Y < 0;
            highestPoint = projectile.Center.Y;
            fromPoisonedHand = false;

            if (source is EntitySource_ItemUse itemSource)
            {
                InheritItemCrit(projectile, itemSource.Item, itemSource.Player);

                if (itemSource is EntitySource_ItemUse_WithAmmo ammoSource)
                {
                    if (ammoSource.AmmoItemIdUsed != 0)
                    {
                        if (itemSource.Player.GetModPlayer<CritPlayer>().ammoUsedThisFrame)
                        {
                            consumedAmmo = true;
                        }
                        else
                        {
                            consumedAmmo = false;
                        }
                    }
                }
                else
                {
                    consumedAmmo = false;
                }
            }
            else if (source is EntitySource_Parent parentSource)
            {
                if (parentSource.Entity is Projectile parent)
                {
                    if (parent.TryGetGlobalProjectile(out CritProjectile crit))
                    {
                        critType = crit.critType;
                        ogItem = crit.ogItem;
                        consumedAmmo = crit.consumedAmmo;
                        wallBounces = crit.wallBounces;
                        targetsHit = crit.targetsHit;
                        timeActive = crit.timeActive;
                    }

                    if (ogItem != null && ogItem.IsSpecial() && (projectile.type == ProjectileID.CrystalVileShardHead || projectile.type == ProjectileID.VilethornTip))
                    {
                        Vector2 ogCenter = projectile.Center;
                        projectile.scale *= 2.5f;
                        projectile.Center = ogCenter + parent.rotation.ToRotationVector2().RotatedBy(-MathHelper.PiOver2) * 25f;
                    }
                }

                if (parentSource.Entity is Item item)
                {
                    InheritItemCrit(projectile, item, projectile.owner != 255 ? Main.player[projectile.owner] : null);
                }
            }

            //Hook on spawn
            hasGrappled = false;
        }

        bool hasGrappled = false;

        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}
