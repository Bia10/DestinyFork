using System;
using System.IO;
using System.Net;

namespace Destiny.IO
{
    public class ByteBuffer : IDisposable
    {
        private int position;
        private MemoryStream Stream { get; set; }
        private BinaryWriter Writer { get; set; }
        private BinaryReader Reader { get; set; }

        public byte[] Array { get; private set; }
        public int Offset { get; private set; }
        public int Capacity { get; private set; }
        public int Limit { get; set; }
        public bool HasFlipped { get; private set; }

        public int Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                Stream.Position = Position + Offset;
            }
        }

        public int Remaining
        {
            get
            {
                return Limit - Position;
            }
        }

        public ByteBuffer(int capacity = Application.DefaultBufferSize)
        {
            Capacity = capacity;
            Array = new byte[Capacity];

            Stream = new MemoryStream(Array);
            Writer = new BinaryWriter(Stream);
            Reader = new BinaryReader(Stream);

            Limit = Capacity;
            Offset = 0;
            Position = 0;
        }

        public ByteBuffer(byte[] data)
        {
            Capacity = data.Length;
            Array = data;

            Stream = new MemoryStream(Array);
            Writer = new BinaryWriter(Stream);
            Reader = new BinaryReader(Stream);

            Limit = Capacity;
            Offset = 0;
            Position = 0;
        }

        private ByteBuffer(byte[] array, int offset, int capacity)
        {
            Array = array;

            Stream = new MemoryStream(Array);
            Writer = new BinaryWriter(Stream);
            Reader = new BinaryReader(Stream);

            Offset = offset;
            Capacity = capacity;
            Limit = Capacity;
            Position = 0;
        }

        public byte this[int index]
        {
            get
            {
                return Array[index];
            }
            set
            {
                Array[index] = value;
            }
        }

        public byte[] GetContent()
        {
            byte[] ba = new byte[Remaining];
            Buffer.BlockCopy(Array, Position + Offset, ba, 0, Remaining);
            return ba;
        }

        public ByteBuffer Skip(int count)
        {
            Position += count;
            
            return this;
        }

        public void Flip()
        {
            Limit = Position;
            Position = 0;
            HasFlipped = true;
        }

        public void SafeFlip()
        {
            if (!HasFlipped)
            {
                Flip();
            }
        }

        public ByteBuffer Slice()
        {
            return new ByteBuffer(Array, Position, Remaining);
        }

        public void Dispose()
        {
            Reader.Dispose();
            Writer.Dispose();
            Stream.Dispose();
        }

        public ByteBuffer WriteBytes(params byte[] collection)
        {
            Writer.Write(collection);
            Position += collection.Length;

            return this;
        }

        public ByteBuffer WriteByte(byte item = 0)
        {
            Writer.Write(item);
            Position += sizeof(byte);

            return this;
        }

        public ByteBuffer WriteSByte(sbyte item = 0)
        {
            Writer.Write(item);
            Position += sizeof(sbyte);

            return this;
        }

        public ByteBuffer WriteShort(short item = 0)
        {
            Writer.Write(item);
            Position += sizeof(short);

            return this;
        }

        public ByteBuffer WriteUShort(ushort item = 0)
        {
            Writer.Write(item);
            Position += sizeof(ushort);

            return this;
        }

        public ByteBuffer WriteInt(int item = 0)
        {
            Writer.Write(item);
            Position += sizeof(int);

            return this;
        }

        public ByteBuffer WriteUInt(uint item = 0)
        {
            Writer.Write(item);
            Position += sizeof(uint);

            return this;
        }

        public ByteBuffer WriteLong(long item = 0)
        {
            Writer.Write(item);
            Position += sizeof(long);

            return this;
        }

        public ByteBuffer WriteFloat(float item = 0)
        {
            Writer.Write(item);
            Position += sizeof(float);

            return this;
        }

        public ByteBuffer WriteBool(bool item)
        {
            Writer.Write(item);
            Position += sizeof(bool);

            return this;
        }

        public ByteBuffer WriteString(string item, params object[] args)
        {
            item = item ?? string.Empty;

            if (item != null)
            {
                item = string.Format(item, args);
            }

            Writer.Write((short)item.Length);

            foreach (char c in item)
            {
                Writer.Write(c);
            }

            Position += item.Length + sizeof(short);

            return this;
        }

        public ByteBuffer WriteStringFixed(string item, int length)
        {
            foreach (char c in item)
            {
                Writer.Write(c);
            }

            for (int i = item.Length; i < length; i++)
            {
                Writer.Write((byte)0);
            }

            Position += length;

            return this;
        }

        public ByteBuffer WriteDateTime(DateTime item)
        {
            Writer.Write((long)(item.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
            Position += sizeof(long);

            return this;
        }

        public ByteBuffer WriteKoreanDateTime(DateTime item)
        {
            Writer.Write((long)(item.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds * 10000 + 116444592000000000L);
            Position += sizeof(long);

            return this;
        }
        
        public ByteBuffer WriteIPAddress(IPAddress value)
        {
            Writer.Write(value.GetAddressBytes());
            Position += 4;

            return this;
        }

        public byte[] ReadBytes(int count)
        {
            byte[] result = Reader.ReadBytes(count);
            Position += count;
            return result;
        }

        public byte[] ReadBytes()
        {
            return ReadBytes(Remaining);
        }

        public byte ReadByte()
        {
            byte result = Reader.ReadByte();
            Position += sizeof(byte);
            return result;
        }

        public sbyte ReadSByte()
        {
            sbyte result = Reader.ReadSByte();
            Position += sizeof(sbyte);
            return result;
        }

        public short ReadShort()
        {
            short result = Reader.ReadInt16();
            Position += sizeof(short);
            return result;
        }

        public ushort ReadUShort()
        {
            ushort result = Reader.ReadUInt16();
            Position += sizeof(ushort);
            return result;
        }

        public int ReadInt()
        {
           /*#if DEBUG
            var count = Reader.BaseStream.Length / sizeof(int);
            for (var i = 0; i < count; i++)
            {
                int v = Reader.ReadInt32();
            }
            Log.Inform("ByteBuffer-ReadInt() count of int sized lengths in reader stream: {0}", count);
            #endif*/

            int result = Reader.ReadInt32();
            Position += sizeof(int);
            return result;
        }

        public uint ReadUInt()
        {
            uint result = Reader.ReadUInt32();
            Position += sizeof(uint);
            return result;
        }

        public long ReadLong()
        {
            long result = Reader.ReadInt64();
            Position += sizeof(long);
            return result;
        }

        public float ReadFloat()
        {
            float result = Reader.ReadSingle();
            Position += sizeof(float);
            return result;
        }

        public bool ReadBool()
        {
            bool result = Reader.ReadBoolean();
            Position += sizeof(bool);
            return result;
        }

        public string ReadString()
        {
            short count = Reader.ReadInt16();

            char[] result = new char[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = (char)Reader.ReadByte();
            }

            Position += count + sizeof(short);

            return new string(result);
        }

        public IPAddress ReadIPAddress()
        {
            IPAddress result = new IPAddress(Reader.ReadBytes(4));
            Position += 4;
            return result;
        }
    }
}
