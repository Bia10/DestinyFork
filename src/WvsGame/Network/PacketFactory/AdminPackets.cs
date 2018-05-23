using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network.PacketFactory
{
    public class AdminPackets : PacketFactoryManager
    {
        #region AdminShop
        public static Packet ShowAdminShop(int npcToShow)
        {
            Packet showAdminShopPacket = new Packet(ServerOperationCode.AdminShop);

            showAdminShopPacket
                .WriteInt(npcToShow)
                .WriteShort((short) NPCsConstants.AdminShopItems.Count);

            foreach (var item in NPCsConstants.AdminShopItems)
            {
                showAdminShopPacket
                    .WriteInt(item.Item1)
                    .WriteInt(item.Item2)
                    .WriteInt(item.Item3)
                    .WriteByte() // NOTE: Unknown.
                    .WriteShort(item.Item4);
            }

            // NOTE: If enabled, when you exit the shop the NPC will ask you if you were looking for something that was missing.
            // If you press yes, a search box with all the items in game will pop up and you can select an item to "register".
            // Once you register an item, a packet will be sent to the server with it's ID so it can be added to the shop.
            showAdminShopPacket.WriteBool(true);

            return showAdminShopPacket;
        }
        #endregion
    }
}