using CritRework.Common.ModPlayers;
using Terraria.Audio;

namespace CritRework.Content.Items.Equipable.Tokens
{
    public class MetalPipeToken : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Gray;
            Item.value = -100;
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
                c.uniqueCritSound = new("CritRework/Assets/Sounds/MetalPipeCrit")
                {
                    PitchVariance = 1f,
                    Pitch = 0f,
                    SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                    MaxInstances = 1,
                    Volume = 1.5f
                };
            }
        }
    }
}
