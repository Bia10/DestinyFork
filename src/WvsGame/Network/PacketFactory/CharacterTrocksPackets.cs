using Destiny.Constants;
using Destiny.Maple.Characters;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network.PacketFactory
{
    public class CharacterTrocksPackets : PacketFactoryManager
    {
        public static Packet TrockInventoryUpdate(ItemConstants.TrockInventoryAction action, ItemConstants.TrockType type)
        {
            Packet trockInventoryUpdatePacket = new Packet(ServerOperationCode.MapTransferResult);

            trockInventoryUpdatePacket
                .WriteByte((byte) action)
                .WriteByte((byte) type)
                .WriteBytes(type == ItemConstants.TrockType.Regular
                    ? CharacterTrocks.RegularTrockToByteArray()
                    : CharacterTrocks.VIPTrockToByteArray());

            return trockInventoryUpdatePacket;
        }

        public static Packet TrockTransferResult(ItemConstants.TrockResult result, ItemConstants.TrockType type)
        {
            Packet trockTransferResultPacket = new Packet(ServerOperationCode.MapTransferResult);

            trockTransferResultPacket
                .WriteByte((byte) result)
                .WriteByte((byte) type);

            return trockTransferResultPacket;
        }
    }
}