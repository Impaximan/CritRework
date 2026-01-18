using CritRework.Common.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritRework.Content.CritTypes.OrchidOnly
{

    [JITWhenModsEnabled("OrchidMod")]
    public class Shapeshifter_PredatorHit : CritType
    {
        public const float _shapeshifterOtherSubclassHitMult = 1.3f;

        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("OrchidMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => _shapeshifterOtherSubclassHitMult;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidModI))
            {
                return item.DamageType.CountsAsClass(OrchidModI.Find<DamageClass>("ShapeshifterDamageClass")) && item.ModItem is OrchidMod.Content.Shapeshifter.OrchidModShapeshifterShapeshift s && s.ShapeshiftType != OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Predator;
            }

            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.TryGetModPlayer(out CritPlayer c) && c.timeSincePredatorHit <= 300;
        }
    }

    [JITWhenModsEnabled("OrchidMod")]
    public class Shapeshifter_SymbioteHit : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("OrchidMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => Shapeshifter_PredatorHit._shapeshifterOtherSubclassHitMult;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidModI))
            {
                return item.DamageType.CountsAsClass(OrchidModI.Find<DamageClass>("ShapeshifterDamageClass")) && item.ModItem is OrchidMod.Content.Shapeshifter.OrchidModShapeshifterShapeshift s && s.ShapeshiftType != OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Symbiote;
            }

            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.TryGetModPlayer(out CritPlayer c) && c.timeSinceSymbioteHit <= 300;
        }
    }

    [JITWhenModsEnabled("OrchidMod")]
    public class Shapeshifter_SageHit : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("OrchidMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => Shapeshifter_PredatorHit._shapeshifterOtherSubclassHitMult;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidModI))
            {
                return item.DamageType.CountsAsClass(OrchidModI.Find<DamageClass>("ShapeshifterDamageClass")) && item.ModItem is OrchidMod.Content.Shapeshifter.OrchidModShapeshifterShapeshift s && s.ShapeshiftType != OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Sage;
            }

            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.TryGetModPlayer(out CritPlayer c) && c.timeSinceSageHit <= 300;
        }
    }

    [JITWhenModsEnabled("OrchidMod")]
    public class Shapeshifter_WardenHit : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("OrchidMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => Shapeshifter_PredatorHit._shapeshifterOtherSubclassHitMult;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidModI))
            {
                return item.DamageType.CountsAsClass(OrchidModI.Find<DamageClass>("ShapeshifterDamageClass")) && item.ModItem is OrchidMod.Content.Shapeshifter.OrchidModShapeshifterShapeshift s && s.ShapeshiftType != OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Warden;
            }

            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return Player.TryGetModPlayer(out CritPlayer c) && c.timeSinceWardenHit <= 300;
        }
    }
}
