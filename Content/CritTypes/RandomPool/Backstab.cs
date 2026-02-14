using System;
using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class Backstab : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item)
        {
            if (Item.shoot == ProjectileID.None)
            {
                return 3f;
            }
            else
            {
                return 2f;
            }
        }

        public override void SpecialPrefixHoldItem(Item item, Player player)
        {
            player.AddBuff(BuffID.Invisibility, 2);
        }

        //public override string GetDescription() => "Critically strikes while you are below the target";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            List<int> directionBased = new()
            {
                NPCAIStyleID.Fighter,
                NPCAIStyleID.DD2Fighter,
                NPCAIStyleID.Caster,
                NPCAIStyleID.Flying,
                NPCAIStyleID.Bat,
                NPCAIStyleID.DemonEye,
                NPCAIStyleID.Vulture,
                NPCAIStyleID.HoveringFighter,
                NPCAIStyleID.Unicorn,
                NPCAIStyleID.Snowman,
                NPCAIStyleID.Rider,
                NPCAIStyleID.Mothron,
                NPCAIStyleID.Mimic,
                NPCAIStyleID.BiomeMimic,
                NPCAIStyleID.DD2Flying,
            };

            List<int> spriteDirectionBased = new()
            {
                NPCAIStyleID.Fighter,
                NPCAIStyleID.DD2Fighter,
                NPCAIStyleID.Caster,
                NPCAIStyleID.DemonEye,
                NPCAIStyleID.Flying,
                NPCAIStyleID.Bat,
                NPCAIStyleID.Vulture,
                NPCAIStyleID.HoveringFighter,
                NPCAIStyleID.Unicorn,
                NPCAIStyleID.Snowman,
                NPCAIStyleID.Rider,
                NPCAIStyleID.Mothron,
                NPCAIStyleID.Mimic,
                NPCAIStyleID.BiomeMimic,
                NPCAIStyleID.DD2Flying,
            };

            int direction = target.direction;
            if (spriteDirectionBased.Contains(target.type)) direction = target.spriteDirection;

            if (Projectile != null)
            {
                if (directionBased.Contains(target.aiStyle))
                {
                    if (Player.heldProj == Projectile.whoAmI && !ItemID.Sets.Yoyo[Item.type])
                    {
                        return Math.Sign(target.Center.X - Player.Center.X) == direction;
                    }
                    else
                    {
                        return (Projectile.velocity.Length() <= 2f && Math.Sign(target.Center.X - Projectile.Center.X) == direction) 
                            || Math.Sign(Projectile.velocity.X) == direction;
                    }
                }
            }

            if (directionBased.Contains(target.aiStyle))
            {
                return Math.Sign(target.Center.X - Player.Center.X) == direction;
            }

            return false;
        }
    }
}
