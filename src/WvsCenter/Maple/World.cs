using System.Collections.ObjectModel;

using Destiny.Network;
using Destiny.Network.Common;

namespace Destiny.Maple
{
    public sealed class World : KeyedCollection<byte, CenterClient>
    {
        public byte ID { get;  }
        public string Name { get;  }
        public ushort Port { get;  }
        public ushort ShopPort { get;  }
        public byte Channels { get;  }
        public string TickerMessage { get;  }
        public bool AllowMultiLeveling { get;  }
        public int ExperienceRate { get;  }
        public int QuestExperienceRate { get;  }
        public int PartyQuestExperienceRate { get;  }
        public int MesoRate { get;  }
        public int DropRate { get;  }

        private CenterClient shop;

        public CenterClient Shop
        {
            get
            {
                return shop;
            }
            set
            {
                shop = value;

                if (value != null)
                {
                    Shop.Port = ShopPort;
                }
            }
        }

        public CenterClient RandomChannel
        {
            get
            {
                return this[Application.Random.Next(Count)];
            }
        }

        public bool IsFull
        {
            get
            {
                return Count == Channels;
            }
        }

        public bool HasShop
        {
            get
            {
                return Shop != null;
            }
        }

        internal World() : base() { }

        internal World(Packet inPacket) : this()
        {
            ID = inPacket.ReadByte();
            Name = inPacket.ReadString();
            Port = inPacket.ReadUShort();
            ShopPort = inPacket.ReadUShort();
            Channels = inPacket.ReadByte();
            TickerMessage = inPacket.ReadString();
            AllowMultiLeveling = inPacket.ReadBool();
            ExperienceRate = inPacket.ReadInt();
            QuestExperienceRate = inPacket.ReadInt();
            PartyQuestExperienceRate = inPacket.ReadInt();
            MesoRate = inPacket.ReadInt();
            DropRate = inPacket.ReadInt();
        }

        protected override void InsertItem(int index, CenterClient item)
        {
            item.ID = (byte)index;
            item.Port = (ushort)(Port + index);

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            foreach (CenterClient loopChannel in this)
            {
                if (loopChannel.ID <= index) continue;

                loopChannel.ID--;

                using (Packet Packet = new Packet(InteroperabilityOperationCode.UpdateChannelID))
                {
                    Packet.WriteByte(loopChannel.ID);

                    loopChannel.Send(Packet);
                }
            }
        }

        protected override byte GetKeyForItem(CenterClient item)
        {
            return item.ID;
        }
    }
}
