﻿using Destiny.Core.IO;
using Destiny.Maple.Characters;
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

            if (item.Expiry != null)
            {
                item.Expiry.Cancel();
            }

            item.Expiry = new Delay(Drop.ExpiryTime, () =>
            {
                if (item.Map == this.Map)
                {
                    this.Remove(item);
                }
            });

            item.Expiry.Execute();

            lock (this.Map.Characters)
            {
                if (item.Owner == null)
                {
                    foreach (Character character in this.Map.Characters)
                    {
                        using (OutPacket oPacket = item.GetCreatePacket(character))
                        {
                            character.Client.Send(oPacket);
                        }
                    }
                }
                else
                {
                    foreach (Character character in this.Map.Characters)
                    {
                        using (OutPacket oPacket = item.GetCreatePacket())
                        {
                            character.Client.Send(oPacket);
                        }
                    }
                }
            }
        }

        protected override void RemoveItem(int index)
        {
            Drop item = base.Items[index];

            if (item.Expiry != null)
            {
                item.Expiry.Cancel();
            }

            using (OutPacket oPacket = item.GetDestroyPacket())
            {
                this.Map.Broadcast(oPacket);
            }

            base.RemoveItem(index);
        }
    }
}