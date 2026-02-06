using CritRework.Common.ModPlayers;
using Terraria.Audio;

namespace CritRework.Content.Items.Equipable.Accessories
{
    public class FireInABottle : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<FireInABottle>();
            player.GetJumpState<FireJump>().Enable();

            if (player.GetJumpState<FireJump>().Active)
            {
                player.jumpSpeedBoost += 5f;
            }
        }
    }

    public class FireJump : ExtraJump
    {
        public override Position GetDefaultPosition()
        {
            return AfterBottleJumps;
        }

        public override void OnStarted(Player player, ref bool playSound)
        {
            SoundEngine.PlaySound(SoundID.Item62, player.Center);
        }

        public override bool CanStart(Player player)
        {
            if (player.TryGetModPlayer(out CritPlayer critPlayer))
            {
                return critPlayer.fireJumpCounter <= 0;
            }
            return false;
        }

        public override void OnRefreshed(Player player)
        {
            if (player.TryGetModPlayer(out CritPlayer critPlayer))
            {
                critPlayer.fireJumpCounter = 1;
            }
        }

        public override void ShowVisuals(Player player)
        {
            float verticalSpeed = 5f;

            if (player.TryGetModPlayer(out CritPlayer critPlayer))
            {
                critPlayer.fireJumpCounter = 5;
            }

            //Torch dust
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(player.BottomLeft + new Vector2(-5f, 0f), player.width + 10, 2, DustID.Torch, 0, 15);
                dust.noGravity = true;
                dust.velocity.X = player.velocity.X;
                dust.velocity.Y = player.velocity.Y;
                dust.scale = 1.5f;
            }

            for (int i = 0; i < 1; i++)
            {
                Dust dust = Dust.NewDustDirect(player.BottomLeft + new Vector2(-5f, -5f), player.width + 10, 2, DustID.Torch, 0, 15);
                dust.noGravity = true;
                dust.velocity.X = -player.velocity.X;
                dust.velocity.Y = verticalSpeed;
                dust.scale = 1.5f;
            }

            //Smoke dust
            for (int i = 0; i < 6; i++)
            {
                Dust dust = Dust.NewDustDirect(player.BottomLeft + new Vector2(-5f, -5f), player.width + 10, 2, DustID.Smoke, 0, 15);
                dust.noGravity = true;
                dust.velocity.X = -player.velocity.X;
                dust.velocity.Y = verticalSpeed * Main.rand.NextFloat(0.8f, 1.2f);
                dust.scale = 1.5f;
                dust.color = Color.DarkGray;
            }

            //Smoke dust
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(player.BottomLeft + new Vector2(-5f, 0f), player.width + 10, 2, DustID.Smoke, 0, 15);
                dust.noGravity = true;
                dust.velocity.X = player.velocity.X;
                dust.velocity.Y = player.velocity.Y * Main.rand.NextFloat(0.8f, 1.2f);
                dust.scale = 1.5f;
                dust.color = Color.DarkGray;
            }

            for (int i = 0; i < 1; i++)
            {
                Dust dust = Dust.NewDustDirect(player.BottomLeft + new Vector2(-5f, -5f), player.width + 10, 2, DustID.Torch, 0, 15);
                dust.noGravity = true;
                dust.velocity.X = -player.velocity.X;
                dust.velocity.Y = verticalSpeed;
                dust.scale = 1.5f;
            }
        }

        public override void UpdateHorizontalSpeeds(Player player)
        {
            player.runAcceleration *= 4f;
        }

        public override float GetDurationMultiplier(Player player)
        {
            return 2f;
        }
    }
}
