using Destiny.IO;
using Destiny.Network.ServerHandler;

namespace Destiny.Network.Common
{
    public class Packet : ByteBuffer
    {
        public static LogLevel LogLevel { get; set; }

        public short OperationCode { get; private set; }

        public Packet(byte[] data)
            : base(data)
        {
            this.OperationCode = this.ReadShort();
        }

        public Packet(short operationCode) 
            : base()
        {
            this.OperationCode = operationCode;

            this.WriteShort(this.OperationCode);
        }

        public Packet(ServerOperationCode operationCode) : this((short)operationCode) { }
        public Packet(InteroperabilityOperationCode operationCode) : this((short)operationCode) { }
    }
}
