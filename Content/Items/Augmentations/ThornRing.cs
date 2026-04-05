using CritRework.Common.ModPlayers;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Augmentations
{
    public class ThornRing : Augmentation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 5)
                .AddTile(TileID.DemonAltar)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.WoodenBow);
        }

        public override void OnCreated(ItemCreationContext context)
        {
            if (context is RecipeItemCreationContext recipe)
            {
                Main.LocalPlayer.Hurt(PlayerDeathReason.ByPlayerItem(Main.myPlayer, recipe.DestinationStack), 10, 0, dodgeable: false, armorPenetration: 999, knockback: 0f);
            }
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.UseSound = new SoundStyle("CritRework/Assets/Sounds/BloodSlash")
            {
                Volume = 1.5f,
                PitchVariance = 0.25f
            };
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 10, 0);
        }

        public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers modifiers, CritType critType, NPC target)
        {
            return true;
        }

        static float counter = 0f;

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target)
        {
            if (player.GetModPlayer<CritPlayer>().ShouldNormallyCrit(item, projectile, new NPC.HitModifiers(), critType, target) && (projectile == null || projectile.type != ModContent.ProjectileType<Thorn>()))
            {
                Projectile thorn = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, item), target.Center, Vector2.Zero, ModContent.ProjectileType<Thorn>(), hit.SourceDamage / 3, 0f, player.whoAmI);
                thorn.ai[0] = target.whoAmI;
                thorn.ai[1] = Main.rand.NextFloat(MathHelper.TwoPi);
                float maxDist = (target.width > target.height ? target.height : target.width) / 2;
                thorn.ai[2] = Main.rand.NextFloat(maxDist / 3f, maxDist);
                thorn.SetAsAugmentCrit();
                thorn.netUpdate = true;
                if (Item.prefix == ModContent.PrefixType<Gradual>())
                {
                    thorn.localNPCHitCooldown = 120;
                    thorn.ArmorPenetration = 5;
                }
            }
        }
    }

    public class Gradual : SpecialAugmentationPrefix<ThornRing>
    {
        public override void AddExtraTooltipLines(Item item, ref List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "PrefixGradual", tooltip.Value)
            {
                IsModifier = true,
            });

            tooltips.Add(new TooltipLine(Mod, "PrefixGradual", "• " + tooltip2.Value)
            {
                IsModifier = true,
                OverrideColor = new Color(102, 166, 226)
            });
        }

        public override void ModifyValue(ref float valueMult)
        {
            base.ModifyValue(ref valueMult);

            valueMult *= 1.1f;
        }
    }

    public class Thorn : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 10;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 3;
            Projectile.localNPCHitCooldown = 60;
            Projectile.usesLocalNPCImmunity = true;
        }

        int counter = 0;
        public override void AI()
        {
            if (Projectile.ai[0] == -1 || !Main.npc[(int)Projectile.ai[0]].active)
            {
                if (Projectile.ai[0] != -1)
                {
                    NPC npc = Main.npc[(int)Projectile.ai[0]];
                    Projectile.ai[0] = -1;

                    NPC newTarget = Projectile.FindTargetWithinRange(500, false);
                    if (newTarget != null)
                    {
                        Projectile.velocity = npc.DirectionTo(newTarget.Center);
                        Projectile.velocity *= Main.rand.Next(12, 16);
                    }
                    else
                    {

                        Projectile.velocity = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Projectile.velocity *= Main.rand.Next(7, 12);
                    }
                    Projectile.timeLeft = 300;
                    Projectile.usesLocalNPCImmunity = true;
                    Projectile.localNPCHitCooldown = -1;
                    Projectile.penetrate = 3;
                    Projectile.Center = npc.Center;
                    Projectile.tileCollide = !npc.noTileCollide;
                    Projectile.netUpdate = true;
                }

                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity.Y += 0.1f;

                return;
            }


            NPC target = Main.npc[(int)Projectile.ai[0]];

            while (!Projectile.getRect().Intersects(target.getRect()))
            {
                Projectile.ai[2]--;
                Projectile.Center = target.Center + Projectile.ai[1].ToRotationVector2().RotatedBy(target.rotation) * Projectile.ai[2];
            }

            Projectile.rotation = Projectile.ai[1] + target.rotation + MathHelper.Pi;
            Projectile.Center = target.Center + Projectile.ai[1].ToRotationVector2().RotatedBy(target.rotation) * Projectile.ai[2];
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] == -1 || !Main.npc[(int)Projectile.ai[0]].active)
            {
                return null;
            }

            if (target.whoAmI == (int)Projectile.ai[0])
            {
                return null;    
            }

            return false;
        }
    }
}
