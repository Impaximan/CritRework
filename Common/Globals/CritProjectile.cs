using CritRework.Common.ModPlayers;
using CritRework.Content.Items.Equipable.Accessories;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
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

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
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
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            wallBounces++;
            return base.OnTileCollide(projectile, oldVelocity);
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
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            targetsHit = 0;
            npcsHit = new();
            wallBounces = 0;
            timeActive = 0;
            timeSinceHit = 0;

            if (source is EntitySource_ItemUse itemSource)
            {
                if (itemSource.Item.TryGetGlobalItem(out CritItem cItem))
                {
                    critType = itemSource.Item.GetGlobalItem<CritItem>().critType;
                    ogItem = itemSource.Item;
                }

                if (itemSource.Item.prefix == ModContent.PrefixType<Content.Prefixes.Weapon.Necromantic>())
                {
                    fromNecromantic = true;
                }

                if (itemSource.Player.TryGetModPlayer(out CritPlayer cPlayer))
                {
                    if (itemSource.Player.HasEquip<WiseCracker>() && projectile.minion && critType == null)
                    {
                        critType = cPlayer.summonCrit;
                    }
                }

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

            if (source is EntitySource_Parent parentSource)
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
                }
            }
        }
    }
}
