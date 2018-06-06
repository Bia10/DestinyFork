using Destiny.Maple.Interaction;
using Destiny.Network.Common;

namespace Destiny.Maple.Maps
{
    public sealed class MapPlayerShops : MapObjects<PlayerShop>
    {
        public MapPlayerShops(Map map) : base(map) { }

        protected override void InsertItem(int index, PlayerShop item)
        {
            lock (this)
            {
                base.InsertItem(index, item);

                using (Packet oPacket = item.GetCreatePacket())
                {
                    Map.Broadcast(oPacket);
                }
            }
        }

        protected override void RemoveItem(int index)
        {
            lock (this)
            {
                PlayerShop item = Items[index];

                using (Packet oPacket = item.GetDestroyPacket())
                {
                    Map.Broadcast(oPacket);
                }

                base.RemoveItem(index);
            }
        }
    }
}
