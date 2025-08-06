namespace CritRework.Content.CritTypes.RandomPool
{
    internal class FoeIsWet : CritType
    {
        public override bool InRandomPool => true;

        public override float GetDamageMult(Player Player, Item Item) => 1.35f;

        //public override string GetDescription() => "Critically strikes while the target is wet or underwater";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers)
        {
            return target.wet || target.HasBuff(BuffID.Wet);
        }
    }
}

