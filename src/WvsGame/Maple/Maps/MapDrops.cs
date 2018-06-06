using System;

using Destiny.IO;
using Destiny.Maple.Characters;
using Destiny.Network.Common;
using Destiny.Threading;

namespace Destiny.Maple.Maps
{
    public sealed class MapDrops : MapObjects<Drop>
    {
        public MapDrops(Map map) : base(map) { }

        protected override void InsertItem(int index, Drop item)
        {
            item.Picker = null;

            base.InsertItem(index, item);

            item.Expiry?.Dispose();

            item.Expiry = new Delay(() =>
            {
                if (item.Map == Map)
                {
                    Remove(item);
                }
            }, Drop.ExpiryTime);

            lock (Map.Characters)
            {
                foreach (Character character in Map.Characters)
                {
                    using (Packet oPacket = item.GetCreatePacket(item.Owner == null ? character : null))
                    {
                        character.Client.Send(oPacket);
                    }
                }
            }
        }

        protected override void RemoveItem(int index)
        {
            Drop item = Items[index];

            item.Expiry?.Dispose();

            using (Packet oPacket = item.GetDestroyPacket())
            {
                Map.Broadcast(oPacket);
            }

            if (Items.Count <= index) return;

            try
            {
                base.RemoveItem(index);
            }

            catch (Exception e)
            {
                Log.SkipLine();
                Log.Inform("MapDrops-RemoveItem: exception occurred: {0}", e);
                Log.SkipLine();
            }
        }
    }
}