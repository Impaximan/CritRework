namespace CritRework.Content.CritTypes.RandomPool
{
	[ReinitializeDuringResizeArrays]
    internal class Backstab : CritType
    {
		static readonly bool[] CanCrit = NPCID.Sets.Factory.CreateNamedSet("Backstab_CanCrit")
		.Description("The Backstab crit type can crit against enemies in this set, regardless of ai style")
		.RegisterBoolSet();
		static readonly bool[] UsesDirection = NPCID.Sets.Factory.CreateNamedSet("Backstab_UsesDirection")
		.Description("The Backstab crit type will use NPC.direction instead of NPC.spriteDirection against enemies in this set")
		.RegisterBoolSet();
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item)
        {
            if (Item.shoot == ProjectileID.None || ContentSamples.ProjectilesByType[Item.shoot].aiStyle == ProjAIStyleID.ShortSword)
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
			switch (target.aiStyle) {
				case NPCAIStyleID.Fighter:
				case NPCAIStyleID.DD2Fighter:
				case NPCAIStyleID.Caster:
				case NPCAIStyleID.DemonEye:
				case NPCAIStyleID.Flying:
				case NPCAIStyleID.Bat:
				case NPCAIStyleID.Vulture:
				case NPCAIStyleID.HoveringFighter:
				case NPCAIStyleID.Unicorn:
				case NPCAIStyleID.Snowman:
				case NPCAIStyleID.Rider:
				case NPCAIStyleID.Mothron:
				case NPCAIStyleID.Mimic:
				case NPCAIStyleID.BiomeMimic:
				case NPCAIStyleID.DD2Flying:
				break;
				default:
				if (CanCrit[target.type]) break;
				return false;
			}

            int direction = target.spriteDirection;
            if (UsesDirection[target.type]) direction = target.direction;

            if (Projectile != null) {
				if (Player.heldProj == Projectile.whoAmI && !ItemID.Sets.Yoyo[Item.type]) {
					return Math.Sign(target.Center.X - Player.Center.X) == direction;
				} else {
					return (Projectile.velocity.Length() <= 2f && Math.Sign(target.Center.X - Projectile.Center.X) == direction)
						|| Math.Sign(Projectile.velocity.X) == direction;
				}
			}

			return Math.Sign(target.Center.X - Player.Center.X) == direction;
        }
    }
}
