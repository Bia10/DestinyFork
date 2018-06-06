using Destiny.Maple.Data;
using Destiny.Maple.Life;
using Destiny.Network.Common;

namespace Destiny.Maple.Maps
{
    public sealed class MapNpcs : MapObjects<Npc>
    {
        public MapNpcs(Map map) : base(map) { }

        protected override void InsertItem(int index, Npc item)
        {
            base.InsertItem(index, item);

            if (!DataProvider.IsInitialized) return;

            using (Packet oPacket = item.GetCreatePacket())
            {
                Map.Broadcast(oPacket);
            }

            item.AssignController();
        }

        protected override void RemoveItem(int index)
        {
            if (DataProvider.IsInitialized)
            {
                Npc item = Items[index];

                item.Controller.ControlledNpcs.Remove(index);

                using (Packet oPacket = item.GetDestroyPacket())
                {
                    Map.Broadcast(oPacket);
                }
            }

            base.RemoveItem(index);
        }
    }
}
