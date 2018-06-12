using System;
using System.Net.Sockets;

using Destiny.IO;
using Destiny.Network.ClientHandler;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;
using Destiny.Security;

namespace Destiny.Network.MapleClientHandler
{
    public abstract class MapleClientHandler : ClientHandler<ClientOperationCode, ServerOperationCode, MapleCryptograph>
    {
        protected MapleClientHandler(Socket socket) : base(socket, "Client") { }

        protected override void Initialize()
        {
            byte[] initialization = Cryptograph.Initialize();

            try
            {
                Socket.Send(initialization);
            }

            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }

            string currentTime = DateTime.Now.ToString("HH:mm:ss");

            switch (Packet.LogLevel)
            {
                case LogLevel.Name:
                    Log.Inform("[{0}][MapleClientHandler]: \n Sent Initialization packet (unencrypted).", currentTime);
                    break;

                case LogLevel.Full:
                    Log.Hex("Sent Initialization packet (unencrypted): ", initialization);
                    break;

                case LogLevel.None:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
