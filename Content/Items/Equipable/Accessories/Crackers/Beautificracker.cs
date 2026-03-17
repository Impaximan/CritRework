using CritRework.Common.Globals;
using CritRework.Common.ModPlayers;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;

namespace CritRework.Content.Items.Equipable.Accessories.Crackers
{
    public class Beautificracker : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 40;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.GetGlobalItem<CritItem>().forceCanCrit = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WiseCracker>()
                .AddIngredient(ItemID.ChlorophyteBar, 15)
                .AddIngredient(ItemID.TurtleShell, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.AddEquip<WiseCracker>();
            player.AddEquip<Beautificracker>();

            if (Item.TryGetGlobalItem(out CritItem cItem) && cItem.critType != null)
            {
                player.GetModPlayer<CritPlayer>().summonCrit = cItem.critType;
            }
        }

        public static int GetOrbColor(CritType critType, Player player, Item item)
        {
            float damageMult = critType.GetDamageMult(player, item);
            if (damageMult >= 2.5f)
            {
                return 2;
            }
            else if (damageMult >= 1.5f)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !equippedItem.IsCracker() || !incomingItem.IsCracker();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string orbText = "Unknown bonus orb effect";
            Color orbColor = Color.Gray;

            int usedOrbColor = 0;

            if (Item.TryGetGlobalItem(out CritItem cItem) && cItem.critType != null)
            {
                usedOrbColor = GetOrbColor(cItem.critType, Main.LocalPlayer, Item);
            }
            else
            {
                usedOrbColor = (int)(Main.gameTimeCache.TotalGameTime.TotalSeconds % 9) / 3;
            }

            switch (usedOrbColor)
            {
                case 0:
                    orbText = "Collecting the orb will boost consecutive critical strike damage by 25% for 4 seconds";
                    orbColor = Color.Lerp(Color.Red, Color.Pink, 0.5f);
                    break;
                case 1:
                    orbText = "Collecting the orb will greatly boost life regen for 7 seconds";
                    orbColor = Color.Lime;
                    break;
                case 2:
                    orbText = "Collecting the orb will grant a 2 second burst of movement speed";
                    orbColor = Color.Cyan;
                    break;
            }

            foreach (TooltipLine line in tooltips)
            {
                if (line.Text.Contains("[insert]"))
                {
                    line.Text = line.Text.Replace("[insert]", orbText);
                    line.OverrideColor = orbColor;
                }
            }
        }
    }

    public class BeautyOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        void DoAI(int dustType, int pickupDistance, SoundStyle pickupSound, int buffID, int buffDuration)
        {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType);
            d.velocity = Projectile.velocity;
            d.noGravity = true;

            float dist = pickupDistance;
            Player? p = null;

            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && player.Distance(Projectile.Center) <= dist)
                {
                    dist = player.Distance(Projectile.Center);
                    p = player;
                }
            }

            if (p != null)
            {
                float speed = 15f + p.velocity.Length() / 3f;
                Projectile.velocity = Projectile.DirectionTo(p.Center) * speed;

                if (dist <= speed)
                {
                    Projectile.timeLeft = 0;
                    p.AddBuff(buffID, buffDuration);
                    SoundEngine.PlaySound(pickupSound, p.Center);
                }
            }
            else
            {
                Projectile.velocity *= 0.98f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {

            switch (Projectile.ai[0])
            {
                case 0: //Red
                    lightColor = Color.Lerp(Color.Red, Color.Pink, 0.5f);
                    break;
                case 1: //Green
                    lightColor = Color.Lime;
                    break;
                case 2: //Blue
                    lightColor = Color.Cyan;
                    break;
            }
            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            switch (Projectile.ai[0])
            {
                case 0: //Red
                    DoAI(DustID.RedTorch, 300, SoundID.Item4, ModContent.BuffType<RedOrbBuff>(), 240);
                    break;
                case 1: //Green
                    DoAI(DustID.GreenTorch, 400, SoundID.Item4, ModContent.BuffType<GreenOrbBuff>(), 60 * 7);
                    break;
                case 2: //Blue
                    DoAI(DustID.BlueTorch, 600, SoundID.Item67, ModContent.BuffType<BlueOrbBuff>(), 120);
                    break;
            }
        }
    }

    public class RedOrbBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CritPlayer>().consecutiveCriticalStrikeDamage += 0.25f;
        }
    }

    public class GreenOrbBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 7;
        }
    }

    public class BlueOrbBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 4f;
        }
    }
}
