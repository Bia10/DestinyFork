using System;

using Destiny.Data;
using Destiny.IO;

namespace Destiny.Maple
{
    public sealed class Memo
    {
        public int ID { get; }
        public string Sender { get; }
        public string Message { get; }
        public DateTime Received { get; }

        public Memo(Datum datum)
        {
            ID = (int)datum["ID"];
            Sender = (string)datum["Sender"];
            Message = (string)datum["Message"];
            Received = (DateTime)datum["Received"];
        }

        public void Delete()
        {
            Database.Delete("memos", "ID = {0}", ID);
        }

        public byte[] ToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteInt(ID)
                    .WriteString(Sender + " ") // NOTE: Space is intentional.
                    .WriteString(Message)
                    .WriteDateTime(Received)
                    .WriteByte(3); // TODO: Memo kind (0 - None, 1 - Fame, 2 - Gift).

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }
    }
}
