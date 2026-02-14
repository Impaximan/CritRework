using CritRework.Content.CritTypes.WhetstoneSpecific;
using System.IO;

namespace CritRework
{
    public partial class CritRework : Mod
    {
        internal enum MessageType : byte
        {
            SpecialNecromanticHit,
            SpecialHolyHit,
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                case MessageType.SpecialNecromanticHit:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        if (Main.LocalPlayer.dead)
                        {
                            Main.LocalPlayer.respawnTimer -= 10;
                        }
                    }
                    else
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)MessageType.SpecialNecromanticHit);
                        packet.Send(ignoreClient: whoAmI);
                    }
                    break;
                case MessageType.SpecialHolyHit:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        if (Main.LocalPlayer.HeldItem == null || Main.LocalPlayer.HeldItem.IsAir || !Main.LocalPlayer.HeldItem.TryGetCritType(out Holy critType))
                        {
                            Main.LocalPlayer.immune = true;
                            Main.LocalPlayer.immuneTime = 120;
                        }
                    }
                    else
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)MessageType.SpecialHolyHit);
                        packet.Send(ignoreClient: whoAmI);
                    }
                    break;
            }
        }
    }
}
