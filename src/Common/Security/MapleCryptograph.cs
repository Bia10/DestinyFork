using System;

using Destiny.IO;

namespace Destiny.Security
{
    public class MapleCryptograph : Cryptograph
    {
        private AesCryptograph Encryptograph { get; set; }
        private AesCryptograph Decryptograph { get; set; }

        public byte[] Initialize()
        {
            byte[] receiveIV = BitConverter.GetBytes(Application.Random.Next());
            byte[] sendIV = BitConverter.GetBytes(Application.Random.Next());

            Encryptograph = new AesCryptograph(sendIV, unchecked((short)(0xFFFF - Application.MapleVersion)));
            Decryptograph = new AesCryptograph(receiveIV, Application.MapleVersion);

            using (ByteBuffer buffer = new ByteBuffer(16))
            {
                buffer.WriteShort(0x0E);
                buffer.WriteShort(Application.MapleVersion);
                buffer.WriteString(Application.PatchVersion);
                buffer.WriteBytes(receiveIV);
                buffer.WriteBytes(sendIV);
                buffer.WriteByte(8);

                return buffer.Array;
            }
        }

        public override byte[] Encrypt(byte[] data)
        {
            lock (this)
            {
                byte[] result = new byte[data.Length];
                Buffer.BlockCopy(data, 0, result, 0, data.Length);

                byte[] header = Encryptograph.GenerateHeader(result.Length);

                BlurCryptograph.Encrypt(result);
                Encryptograph.Crypt(result);

                using (ByteBuffer buffer = new ByteBuffer(data.Length + 4))
                {
                    buffer.WriteBytes(header);
                    buffer.WriteBytes(result);

                    return buffer.Array;
                }
            }
        }

        public override byte[] Decrypt(byte[] data)
        {
            lock (this)
            {
                using (ByteBuffer buffer = new ByteBuffer(data))
                {
                    byte[] header = buffer.ReadBytes(4);

                    if (!Decryptograph.IsValidPacket(header))
                    {
                        throw new CryptographyException("Invalid header.");
                    }

                    byte[] content = buffer.GetContent();

                    int length = AesCryptograph.RetrieveLength(header);

                    if (content.Length == length)
                    {
                        Decryptograph.Crypt(content);
                        BlurCryptograph.Decrypt(content);

                        return content;
                    }
                    else
                    {
                        throw new CryptographyException(string.Format("Packet length not matching ({0} != {1}).", content.Length, length));
                    }
                }
            }
        }

        protected override void CustomDispose()
        {
            Encryptograph.Dispose();
            Decryptograph.Dispose();
        }
    }
}
