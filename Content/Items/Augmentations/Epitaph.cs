using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Augmentations
{
    public class Epitaph : Augmentation
    {
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.DD2_DarkMageSummonSkeleton;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            if (hit.Crit && target.life <= 0)
            {
                Vector2 position = target.Center - new Vector2(30, 34) / 2;
                for (int i = 0; i < 1000; i++)
                {
                    position.Y++;

                    if (Collision.SolidCollision(position, 30, 34))
                    {
                        break;
                    }
                }

                float range = 700f * player.GetPotency(item);

                if (item.prefix == ModContent.PrefixType<Grieving>())
                {
                    range *= 1.5f;
                }

                SoundEngine.PlaySound(SoundID.DD2_SkeletonSummoned, target.Center);
                Projectile.NewProjectile(new EntitySource_ItemUse(player, item), position - new Vector2(0, 34), Vector2.Zero, ModContent.ProjectileType<DoomTomb>(), 0, 0f, player.whoAmI, range);
            }
        }
    }

    public class Grieving : SpecialAugmentationPrefix<Epitaph>
    {
        public override void AddExtraTooltipLines(Item item, ref List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ModifierGrieving", tooltip.Value)
            {
                IsModifier = true,
            });
        }

        public override void ModifyValue(ref float valueMult)
        {
            base.ModifyValue(ref valueMult);

            valueMult *= 1.1f;
        }
    }

    public class DoomTomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.width = 30;
            Projectile.height = 34;
            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        float range => Projectile.ai[0];

        public override void AI()
        {
            Projectile.velocity.Y += 0.5f;

            foreach (NPC target in Main.npc)
            {
                if (target.active && target.getRect().ClosestPointInRect(Projectile.Center).Distance(Projectile.Center) < range)
                {
                    target.AddBuff(ModContent.BuffType<Doom>(), 60);
                }
            }

            foreach (Player target in Main.player)
            {
                if (target.getRect().ClosestPointInRect(Projectile.Center).Distance(Projectile.Center) < range)
                {
                    target.AddBuff(ModContent.BuffType<Doom>(), 60);
                }
            }

            if (Projectile.timeLeft < 17)
            {
                Projectile.alpha += 15;
            }

            Lighting.AddLight(Projectile.Center, Color.MediumPurple.ToVector3() / 2);
        }
    }

    public class Doom : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.Next(3) == 0)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.PurpleTorch);
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity.Y = 4;
                dust.velocity.X = 0;
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (Main.rand.Next(3) == 0)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.PurpleTorch);
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity.Y = 4;
                dust.velocity.X = 0;
            }

            player.statDefense -= 20;
        }
    }
}
