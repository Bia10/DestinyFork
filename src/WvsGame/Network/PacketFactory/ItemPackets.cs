using Destiny.Constants;
using Destiny.Maple;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network.PacketFactory
{
    public class ItemPackets : PacketFactoryManager
    {
        public static Packet UpdateItems(Item item)
        {
            Packet updateItemsPacket = new Packet(ServerOperationCode.InventoryOperation);

            updateItemsPacket
                .WriteBool(true)
                .WriteByte(1)
                .WriteByte((byte)ItemConstants.InventoryOperationType.ModifyQuantity)
                .WriteByte((byte)item.ItemType)
                .WriteShort(item.Slot)
                .WriteShort(item.Quantity);

            return updateItemsPacket;
        }

        public static Packet EquipOrUnequipItems(Item item, short sourceSlot, short destinationSlot)
        {
            Packet equipOrUnequipItemsPacket = new Packet(ServerOperationCode.InventoryOperation);

            equipOrUnequipItemsPacket
                .WriteBool(true)
                .WriteByte(1)
                .WriteByte((byte)ItemConstants.InventoryOperationType.ModifySlot)
                .WriteByte((byte)item.ItemType)
                .WriteShort(sourceSlot)
                .WriteShort(destinationSlot)
                .WriteByte(1);

            return equipOrUnequipItemsPacket;
        }

        public static Packet MoveItem(Item item, short sourceSlot, short destinationSlot)
        {
            Packet moveItemPacket = new Packet(ServerOperationCode.InventoryOperation);

            moveItemPacket
                .WriteBool(true)
                .WriteByte(1)
                .WriteByte((byte) ItemConstants.InventoryOperationType.ModifySlot)
                .WriteByte((byte) item.ItemType)
                .WriteShort(sourceSlot)
                .WriteShort(destinationSlot);

            return moveItemPacket;
        }

        public static Packet RemoveItem(Item item)
        {
            Packet removeItemPacket = new Packet(ServerOperationCode.InventoryOperation);

            removeItemPacket
                .WriteBool(true)
                .WriteByte(1)
                .WriteByte((byte)ItemConstants.InventoryOperationType.RemoveItem)
                .WriteByte((byte)item.ItemType)
                .WriteShort(item.Slot);

                if (item.IsEquipped)
                {
                    removeItemPacket.WriteByte(1);
                }

            return removeItemPacket;
        }

        public static Packet ModifyItemsQuantity(Item item)
        {
            Packet modifyItemsQuantityPacket = new Packet(ServerOperationCode.InventoryOperation);

            modifyItemsQuantityPacket
                .WriteBool(true)
                .WriteByte(1)
                .WriteByte((byte)ItemConstants.InventoryOperationType.ModifyQuantity)
                .WriteByte((byte)item.ItemType)
                .WriteShort(item.Slot)
                .WriteShort(item.Quantity);

            return modifyItemsQuantityPacket;
        }

        public static Packet StackItemsFullStack(Item itemFirst, Item itemSecond)
        {
            Packet stackItemsFullStackPacket = new Packet(ServerOperationCode.InventoryOperation);

            stackItemsFullStackPacket
                .WriteBool(true)
                .WriteByte(2)
                .WriteByte((byte)ItemConstants.InventoryOperationType.ModifyQuantity)
                .WriteByte((byte)itemFirst.ItemType)
                .WriteShort(itemFirst.Slot)
                .WriteShort(itemFirst.Quantity)
                .WriteByte((byte)ItemConstants.InventoryOperationType.ModifyQuantity)
                .WriteByte((byte)itemSecond.ItemType)
                .WriteShort(itemSecond.Slot)
                .WriteShort(itemSecond.Quantity);

            return stackItemsFullStackPacket;
        }

        public static Packet StackItems(Item itemFirst, Item itemSecond)
        {
            Packet stackItemsPacket = new Packet(ServerOperationCode.InventoryOperation);

            stackItemsPacket
                .WriteBool(true)
                .WriteByte(2)
                .WriteByte((byte)ItemConstants.InventoryOperationType.RemoveItem)
                .WriteByte((byte)itemFirst.ItemType)
                .WriteShort(itemFirst.Slot)
                .WriteByte((byte)ItemConstants.InventoryOperationType.ModifyQuantity)
                .WriteByte((byte)itemSecond.ItemType)
                .WriteShort(itemSecond.Slot)
                .WriteShort(itemSecond.Quantity);

            return stackItemsPacket;
        }

    }
}