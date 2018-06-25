using Destiny.Network.Common;

namespace Destiny.Maple
{
    public sealed class Channel
    {
        public byte ID { get; set; }
        public ushort Port { get; set; }
        public int Population { get; set; }

        public Channel(Packet inPacket)
        {
            ID = inPacket.ReadByte();
            Port = inPacket.ReadUShort();
            Population = inPacket.ReadInt();
        }
    }
}
