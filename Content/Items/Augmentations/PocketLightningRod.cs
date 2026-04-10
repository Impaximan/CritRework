using System.Collections.Generic;
using System.Security.Policy;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Augmentations
{
    public class PocketLightningRod : Augmentation
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 48;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = equipSound;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            if (hit.Crit && projectile.type != ModContent.ProjectileType<PocketLightning>())
            {
                SoundEngine.PlaySound(new SoundStyle("CritRework/Assets/Sounds/Zap")
                {
                    Volume = 0.65f,
                    PitchVariance = 0.5f
                }, target.Center);

                Projectile p = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, Item), player.Center, Vector2.Zero, ModContent.ProjectileType<PocketLightning>(), hit.Damage, 0f, player.whoAmI);
                p.ai[2] = target.whoAmI;
                (p.ModProjectile as PocketLightning).SetTargetPosition();
                player.AddBuff(BuffID.Electrified, 10);
            }
        }
    }

    /// <summary>
    /// ai 0 and 1 are for target position. ai 2 is for target enemy
    /// </summary>
    class PocketLightning : ModProjectile
    {
        public override string Texture => "CritRework/nothing";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 50;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        Vector2 currentTargetPosition
        {
            get
            {
                return new Vector2(Projectile.ai[0], Projectile.ai[1]);
            }
            set
            {
                Projectile.ai[0] = value.X;
                Projectile.ai[1] = value.Y;
            }
        }

        Vector2 finalTargetPosition
        {
            get
            {
                return finalTarget.Center;
            }
        }

        NPC finalTarget
        {
            get
            {
                return Main.npc[(int)Projectile.ai[2]];
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            List<int> resistantEnemies = new()
            {
                NPCID.TheDestroyer,
                NPCID.TheDestroyerBody,
                NPCID.TheDestroyerTail,
                NPCID.EaterofWorldsHead,
                NPCID.EaterofWorldsBody,
                NPCID.EaterofWorldsTail,
                NPCID.Creeper
            };

            if (resistantEnemies.Contains(target.type))
            {
                modifiers.SourceDamage /= 3f;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            SetTargetPosition();
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target == finalTarget)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        const float distanceJump = 100f;
        public void SetTargetPosition(bool moveTo = true)
        {
            if (Projectile.Distance(finalTargetPosition) <= distanceJump * 1.5f)
            {
                currentTargetPosition = finalTargetPosition;
            }
            else
            {
                float distance = Projectile.Distance(finalTargetPosition) - distanceJump;
                currentTargetPosition = finalTargetPosition + Projectile.DirectionFrom(finalTargetPosition) * distance;
                currentTargetPosition += new Vector2(Main.rand.NextFloat(-distanceJump * 0.75f, distanceJump * 0.75f), Main.rand.NextFloat(-distanceJump * 0.4f, distanceJump * 0.4f));
            }

            if (moveTo)
            {
                Projectile.velocity = Projectile.DirectionTo(currentTargetPosition) * 15;
            }
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Electric);
            dust.velocity = Vector2.Zero;
            dust.scale = 1f;
            dust.noGravity = true;

            if (Projectile.Distance(currentTargetPosition) < Projectile.velocity.Length() || currentTargetPosition == Vector2.Zero)
            {
                SetTargetPosition();
            }

            if (Projectile.Distance(finalTargetPosition) <= Projectile.velocity.Length())
            {
                Projectile.active = false;
            }
        }
    }
}
