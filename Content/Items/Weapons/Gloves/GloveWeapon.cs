using System.Collections.Generic;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;

namespace CritRework.Content.Items.Weapons.Gloves
{
    public abstract class GloveWeapon : ModItem
    {
        public static LocalizedText gloveDescription;
        public static LocalizedText gloveUsing;
        public static LocalizedText gloveCritMult;
        public static LocalizedText gloveShootSpeed;
        public static LocalizedText gloveBonus;

        public override void SetStaticDefaults()
        {
            gloveDescription = Mod.GetLocalization($"GloveDescription");
            gloveUsing = Mod.GetLocalization($"GloveUsing");
            gloveCritMult = Mod.GetLocalization($"GloveCritMult");
            gloveShootSpeed = Mod.GetLocalization($"GloveShootSpeed");
            gloveBonus = Mod.GetLocalization($"GloveBonus");
        }

        public sealed override void SetDefaults()
        {
            Item.DamageType = CritRework.gloveDamageClass;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.shootSpeed = 1f;
            Item.shoot = ProjectileID.Shuriken;
            Item.noMelee = true;
            GloveDefaults();
        }

        public override void UpdateInventory(Player player)
        {
            if (GetThrownItem(player, out Item throwable))
            {
                UpdateStatsForThrownItem(player, throwable);
            }
        }

        public override void HoldItem(Player player)
        {
            if (GetThrownItem(player, out Item throwable))
            {
                UpdateStatsForThrownItem(player, throwable);

                if (throwable.ModItem != null)
                {
                    throwable.ModItem.HoldItem(player);
                }
            }
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            if (GetThrownItem(player, out Item throwable))
            {
                if (throwable.ModItem != null)
                {
                    throwable.ModItem.GrabRange(player, ref grabRange);
                }
            }
        }

        public override void UseAnimation(Player player)
        {
            if (GetThrownItem(player, out Item throwable))
            {
                if (throwable.ModItem != null)
                {
                    throwable.ModItem.UseAnimation(player);
                }
            }
        }

        public void UpdateStatsForThrownItem(Player player, Item throwable)
        {
            Item.useTime = throwable.useTime;
            Item.useAnimation = throwable.useAnimation;
            Item.UseSound = throwable.UseSound;
            Item.useStyle = throwable.useStyle;
            Item.reuseDelay = throwable.reuseDelay;
            Item.autoReuse = throwable.autoReuse;
            Item.shoot = throwable.shoot;
        }

        public virtual void GloveDefaults()
        {

        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player) && GetThrownItem(player, out Item throwable) && (throwable.ModItem == null || throwable.ModItem.CanUseItem(player));
        }

        public override bool? UseItem(Player player)
        {
            if (GetThrownItem(player, out Item throwable))
            {
                if (throwable.ModItem != null)
                {
                    return throwable.ModItem.UseItem(player);
                }
                else
                {
                    return base.UseItem(player);
                }
            }
            else
            {
                return base.UseItem(player);
            }
        }

        public override void UseItemFrame(Player player)
        {
            if (GetThrownItem(player, out Item throwable))
            {
                if (throwable.ModItem != null)
                {
                   throwable.ModItem.UseItemFrame(player);
                }
            }
        }

        public bool GetThrownItem(Player player, out Item item)
        {
            for (int i = 0; i <= 58; i++)
            {
                item = player.inventory[i];

                if (item.consumable && item.damage > 0 && item.ammo == AmmoID.None && item.stack > 0)
                {
                    return true;
                }
            }

            item = null;
            return false;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (GetThrownItem(player, out Item throwable))
            {
                damage += player.GetWeaponDamage(throwable);
                knockback += player.GetWeaponKnockback(throwable);
                type = throwable.shoot;
                velocity += velocity.ToRotation().ToRotationVector2() * throwable.shootSpeed;

                if (throwable.ModItem != null)
                {
                    throwable.ModItem.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
                }
            }
        }

        public override bool CanShoot(Player player)
        {
            if (GetThrownItem(player, out Item throwable))
            {
                if (throwable.ModItem != null)
                {
                    return throwable.ModItem.CanShoot(player);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (GetThrownItem(player, out Item throwable))
            {
                if (throwable.ModItem != null)
                {
                    if (throwable.ModItem.ConsumeItem(player))
                    {
                        throwable.stack--;
                    }

                    return throwable.ModItem.Shoot(player, source, position, velocity, type, damage, knockback);
                }
                else
                {
                    throwable.stack--;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.RemoveAll(x => x.Name == "Speed");
            tooltips.RemoveAll(x => x.Name == "Knockback");

            tooltips.Insert(2, new TooltipLine(Mod, "GloveTooltip", gloveDescription.Value));



            if (GetThrownItem(Main.LocalPlayer, out Item throwable))
            {
                tooltips.Insert(3, new TooltipLine(Mod, "GloveUsing", gloveUsing.Value + " " + ItemTagHandler.GenerateTag(throwable) + $" [c/{ItemRarity.GetColor(throwable.rare).Hex3()}:" + throwable.Name + "]"));
            }
            else
            {
                tooltips.Insert(3, new TooltipLine(Mod, "GloveUsing", gloveUsing.Value + " [c/ff4b4b:None]"));
            }

            TooltipLine damageLine = tooltips.Find(x => x.Name == "Damage");
            damageLine.Text = damageLine.Text.Insert(damageLine.Text.IndexOf(' '), " " + gloveBonus.Value);

            tooltips.Insert(tooltips.IndexOf(damageLine) + 1, new TooltipLine(Mod, "GloveShootSpeed", System.Math.Round(Item.shootSpeed, 1) + " " + gloveShootSpeed.Value));
            if (Item.TryGetGlobalItem(out Common.Globals.CritItem c))
            {
                if (c.critType != null)
                {
                    tooltips.Insert(tooltips.IndexOf(damageLine) + 1, new TooltipLine(Mod, "GloveCritMult", System.Math.Round(c.critType.GetDamageMult(Main.LocalPlayer, Item), 1) + gloveCritMult.Value));
                }
                else
                {
                    tooltips.Insert(tooltips.IndexOf(damageLine) + 1, new TooltipLine(Mod, "GloveCritMult", "???" + gloveCritMult.Value)
                    {
                        OverrideColor = Color.Gray
                    });
                }
            }
        }
    }
}
