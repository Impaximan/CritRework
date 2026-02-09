using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Equipable.Accessories;
using CritRework.Content.Items.Weapons.Gloves;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;

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

            if (!npcsHit.Contains(target))
            {
                npcsHit.Add(target);
            }

            if (fromNecromantic && hit.Crit && !Main.player[projectile.owner].moonLeech)
            {
                Main.player[projectile.owner].Heal(Content.Prefixes.Weapon.Necromantic.healAmount);
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
                    critType.SpecialPrefixOnHitNPC(ogItem, Main.player[projectile.owner], projectile, target, hit, damageDone);
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
            base.ModifyHitNPC(projectile, target, ref modifiers);
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            wallBounces++;
            return base.OnTileCollide(projectile, oldVelocity);
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

            if (projectile.Center.Y < highestPoint) highestPoint = projectile.Center.Y;
        }

        void InheritItemCrit(Projectile projectile, Item item, Player? player = null)
        {
            if (item.TryGetGlobalItem(out CritItem cItem))
            {
                critType = item.GetGlobalItem<CritItem>().critType;
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

        }


    }
}
