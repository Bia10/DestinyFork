using Destiny.IO;
using Destiny.Network.ServerHandler;

namespace Destiny.Network.Common
{
    public class Packet : ByteBuffer
    {
        public static LogLevel LogLevel { get; set; }

        public short OperationCode { get; }

        public Packet(byte[] data) : base(data)
        {
           OperationCode = ReadShort();
        }

        public Packet(short operationCode) 
            : base()
        {
            OperationCode = operationCode;

            WriteShort(OperationCode);
        }

        public Packet(ServerOperationCode operationCode) : this((short)operationCode) { }
        public Packet(InteroperabilityOperationCode operationCode) : this((short)operationCode) { }
    }
}
