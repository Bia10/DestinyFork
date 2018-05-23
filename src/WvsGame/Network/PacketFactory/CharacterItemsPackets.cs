using Destiny.Constants;
using Destiny.Maple;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network.PacketFactory
{
    public class CharacterItemsPackets : PacketFactoryManager
    {
        #region AddItemToInventory
        public static Packet AddItemToInventoryPacket(Item item, bool fromDrop = false, bool autoMerge = true, bool forceGetSlot = false)
        {
            Packet addItemToInventoryPacket = new Packet(ServerOperationCode.InventoryOperation);

            addItemToInventoryPacket
                .WriteBool(fromDrop)
                .WriteByte(1)
                .WriteByte((byte)ItemConstants.InventoryOperationType.AddItem)
                .WriteByte((byte)item.ItemType)
                .WriteShort(item.Slot)
                .WriteBytes(item.ToByteArray(true)); // item data

            return addItemToInventoryPacket;
        }
        #endregion

        #region RemoveItemFromInventory
        public static Packet RemoveItemFromInventory(Item item, bool fromDrop = false, bool autoMerge = true, bool forceGetSlot = false)
        {
            Packet removeItemFromInventoryPacket = new Packet(ServerOperationCode.InventoryOperation);

            removeItemFromInventoryPacket
                .WriteBool(fromDrop)
                .WriteByte(1)
                .WriteByte((byte) ItemConstants.InventoryOperationType.RemoveItem)
                .WriteByte((byte) item.ItemType)
                .WriteShort(item.Slot);

            return removeItemFromInventoryPacket;
        }
        #endregion
    }
}