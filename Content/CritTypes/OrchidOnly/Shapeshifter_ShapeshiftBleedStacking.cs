using System.Collections.Generic;

namespace CritRework.Content.CritTypes.RandomPool
{
    internal class Shapeshifter_ShapeshiftBleedStackingMedium : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("OrchidMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.4f;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidModI))
            {
                return item.DamageType.CountsAsClass(OrchidModI.Find<DamageClass>("ShapeshifterDamageClass")) && item.ModItem is OrchidMod.Content.Shapeshifter.OrchidModShapeshifterShapeshift s && s.ShapeshiftType == OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Predator;
            }

            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidMod))
            {
                int stacks = 0;

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && (npc.type != NPCID.TargetDummy || (target != null && target.type == NPCID.TargetDummy)) && (!npc.SpawnedFromStatue || (target != null && target.SpawnedFromStatue)))
                    {
                        if (npc.TryGetGlobalNPC(OrchidMod.Find<GlobalNPC>("ShapeshifterGlobalNPC"), out GlobalNPC g))
                        {
                            if (g.GetType().GetField("BleedSpecific").GetValue(g) is List<OrchidMod.Content.Shapeshifter.ShapeshifterBleed> BleedSpecific)
                            {
                                foreach (OrchidMod.Content.Shapeshifter.ShapeshifterBleed sb in BleedSpecific)
                                {
                                    if (sb.Owner == Player.whoAmI)
                                    {
                                        stacks += sb.BleedCount;
                                    }
                                }
                            }

                            if (g.GetType().GetField("BleedGeneral").GetValue(g) is List<OrchidMod.Content.Shapeshifter.ShapeshifterBleed> BleedGeneral)
                            {
                                foreach (OrchidMod.Content.Shapeshifter.ShapeshifterBleed gb in BleedGeneral)
                                {
                                    if (gb.Owner == Player.whoAmI)
                                    {
                                        stacks += gb.BleedCount;
                                    }
                                }
                            }

                            if (stacks >= 8)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }

    internal class Shapeshifter_ShapeshiftBleedStackingHigh : CritType
    {
        public override bool InRandomPool => true;

        public override bool ShouldLoad()
        {
            return ModLoader.HasMod("OrchidMod");
        }

        public override bool ShowWhenActive => true;

        public override float GetDamageMult(Player Player, Item Item) => 3f;

        public override bool CanApplyTo(Item item)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidModI))
            {
                return item.DamageType.CountsAsClass(OrchidModI.Find<DamageClass>("ShapeshifterDamageClass")) && item.ModItem is OrchidMod.Content.Shapeshifter.OrchidModShapeshifterShapeshift s && s.ShapeshiftType == OrchidMod.Content.Shapeshifter.ShapeshifterShapeshiftType.Predator;
            }

            return false;
        }

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            if (ModLoader.TryGetMod("OrchidMod", out Mod OrchidMod))
            {
                int stacks = 0;

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && (npc.type != NPCID.TargetDummy || (target != null && target.type == NPCID.TargetDummy)) && (!npc.SpawnedFromStatue || (target != null && target.SpawnedFromStatue)))
                    {
                        if (npc.TryGetGlobalNPC(OrchidMod.Find<GlobalNPC>("ShapeshifterGlobalNPC"), out GlobalNPC g))
                        {
                            if (g.GetType().GetField("BleedSpecific").GetValue(g) is List<OrchidMod.Content.Shapeshifter.ShapeshifterBleed> BleedSpecific)
                            {
                                foreach (OrchidMod.Content.Shapeshifter.ShapeshifterBleed sb in BleedSpecific)
                                {
                                    if (sb.Owner == Player.whoAmI)
                                    {
                                        stacks += sb.BleedCount;
                                    }
                                }
                            }

                            if (g.GetType().GetField("BleedGeneral").GetValue(g) is List<OrchidMod.Content.Shapeshifter.ShapeshifterBleed> BleedGeneral)
                            {
                                foreach (OrchidMod.Content.Shapeshifter.ShapeshifterBleed gb in BleedGeneral)
                                {
                                    if (gb.Owner == Player.whoAmI)
                                    {
                                        stacks += gb.BleedCount;
                                    }
                                }
                            }

                            if (stacks >= 20)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
