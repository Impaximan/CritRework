using CritRework.Common.ModPlayers;
using Terraria.Audio;

namespace CritRework.Content.Items.Equipable.Tokens
{
    public class OrchestraToken : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.vanity = true;
        }

        public override void UpdateEquip(Player player)
        {
            UpdateSoundEffect(player);
        }

        public override void UpdateVanity(Player player)
        {
            UpdateSoundEffect(player);
        }

        public void UpdateSoundEffect(Player player)
        {
            if (player.TryGetModPlayer(out CritPlayer c))
            {
                c.uniqueCritSound = new("CritRework/Assets/Sounds/OrchestraCrit")
                {
                    PitchVariance = 0.5f,
                    Pitch = 0f,
                    SoundLimitBehavior = SoundLimitBehavior.IgnoreNew,
                    MaxInstances = 8,
                    Volume = 0.25f
                };
            }
        }
    }
}
