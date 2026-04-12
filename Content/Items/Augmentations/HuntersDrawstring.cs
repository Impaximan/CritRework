using CritRework.Common.ModPlayers;
using Terraria.DataStructures;

namespace CritRework.Content.Items.Augmentations
{
    public class HuntersDrawstring : Augmentation
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.UseSound = equipSound;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 4, 0, 0);
        }

        public override bool CanApplyTo(Item weapon)
        {
            return weapon.useAmmo == AmmoID.Arrow;
        }

         public override bool OverrideNormalCritBehavior(Player player, Item item, Projectile projectile, NPC.HitModifiers? modifiers, CritType critType, NPC target)
        {
            return true;
        }

        public override void AugmentationOnHitNPC(Player player, Item item, Projectile projectile, NPC.HitInfo hit, CritType critType, NPC target, bool critCondition)
        {
            NPC.HitModifiers modifiers = new NPC.HitModifiers();

            if (projectile == null)
            {
                return;
            }

            if (critCondition)
            {
                float damageMult = CritType.CalculateActualCritMult(critType, item, player);
                target.AddBuff(ModContent.BuffType<HuntersMark>(), (int)((10 + ((damageMult - 1f) * 120)) * player.GetPotency(item)));
            }
        }
    }

    public class HuntersMark : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.RedTorch);
                dust.noGravity = true;
                dust.velocity = npc.velocity;
                dust.scale = 1.25f;
            }
        }
    }
}
