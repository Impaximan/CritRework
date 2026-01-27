namespace CritRework.Content.CritTypes.RandomPool
{
    internal class BunchedUpFoes : CritType
    {
        public override bool InRandomPool => false;

        public override bool ForceOnItem(Item item)
        {
            return item.type == ItemID.Grenade;
        }

        public override float GetDamageMult(Player Player, Item Item) => 2f;

        const int minNumber = 4;
        //public override string GetDescription() => "Critically strikes when there are many enemies in close proximity to each other";

        public override bool ShouldCrit(Player Player, Item Item, Projectile? Projectile, NPC target, NPC.HitModifiers modifiers, bool specialPrefix)
        {
            int numberOfEnemies = 0;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(target.Center) <= 175 && npc.lifeMax >= 10)
                {
                    numberOfEnemies++;
                    if (numberOfEnemies >= minNumber)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
