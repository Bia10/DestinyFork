using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Destiny.IO;
using Destiny.Network.Common;
using Destiny.Security;

namespace Destiny.Network.ServerHandler
{
    public abstract class ServerHandler<TReceiveOP, TSendOP, TCryptograph> : NetworkConnector<TReceiveOP, TSendOP, TCryptograph>, IDisposable where TCryptograph : Cryptograph, new()
    {
        private string Title { get; }

        protected abstract void StopServer();
        protected virtual void Initialize(params object[] args) { }

        protected ServerHandler(IPEndPoint remoteEP, string title = "Server", params object[] args)
        {
            Title = title;

            Log.Inform("Connecting to {0} at {1}...", Title.ToLower(), remoteEP.Address);

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Cryptograph = new TCryptograph();
            IsAlive = true;

            Prepare();

            bool connected = false;
            int tries = 0;

            while (!connected && tries < 5)
            {
                try
                {
                    if (tries > 0)
                    {
                        Thread.Sleep(2000);
                    }

                    Socket.Connect(remoteEP);
                    connected = true;
                }
                catch
                {
                    Log.Warn("Could not connect to {0} at {1}.", Title.ToLower(), remoteEP.ToString());
                    tries++;
                }
            }

            if (!connected)
            {
                Log.SkipLine();
                throw new NetworkException(string.Format("{0} connection failed.", Title));
            }

            Log.SkipLine();
            Log.Success("Connected to {0} on thread {1}.", Title.ToLower(), Thread.CurrentThread.ManagedThreadId);
            Log.SkipLine();

            Initialize(args);
        }

        protected void Loop()
        {
            while (IsAlive)// && IsServerAlive)
            {
                try
                {
                    ReceiveDone.Reset();

                    ByteBuffer buffer = new ByteBuffer();

                    Socket.BeginReceive(buffer.Array, buffer.Position, buffer.Capacity, SocketFlags.None, new AsyncCallback(OnReceive), buffer);

                    ReceiveDone.WaitOne();
                }

                catch (Exception e)
                {
                    Log.SkipLine();
                    Log.Error(e);
                    Log.SkipLine();
                    throw;
                }
            }
            Dispose();

            StopServer();
        }

        private void OnReceive(IAsyncResult ar)
        {
            if (!IsAlive) return;

            using (ByteBuffer buffer = (ByteBuffer)ar.AsyncState)
            {
                buffer.Position = 0;

                try
                {
                    buffer.Limit = Socket.EndReceive(ar);

                    if (buffer.Limit == 0)
                    {
                        Stop();
                    }
                    else
                    {
                        using (Packet inPacket = new Packet(Cryptograph.Decrypt(buffer.GetContent())))
                        {
                            // parse information from packet
                            short packetOPCODE = inPacket.OperationCode;
                            string packetName = Enum.GetName(typeof(TReceiveOP), packetOPCODE);
                            byte[] packetByteArray = inPacket.Array;
                            string currentTime = DateTime.Now.ToString("HH:mm:ss");

                            if (Enum.IsDefined(typeof(TReceiveOP), inPacket.OperationCode))
                            {
                                switch (Packet.LogLevel)
                                {
                                    case LogLevel.Name:
                                        Log.Inform("[{0}][ServerHandler]: \n Received [{1}] packet.", currentTime, packetName);
                                        break;

                                    case LogLevel.Full:
                                        Log.Hex("Received {0} packet: ", packetByteArray, packetName);
                                        break;

                                    case LogLevel.None:
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                            else
                            {
                                Log.SkipLine();
                                Log.Hex("Received unknown (0x{0:X2}) packet: ", packetByteArray, packetOPCODE);
                                Log.SkipLine();
                            }

                            Dispatch(inPacket);
                        }
                    }

                    ReceiveDone.Set();
                }
                catch (Exception e)
                {
                    Log.SkipLine();
                    Log.Error("Uncatched fatal error on {0}: ", e, Title.ToLower());
                    Stop();
                }
            }
        }

        protected void Send(Packet outPacket)
        {
            short packetOPCODE = outPacket.OperationCode;
            string packetName = Enum.GetName(typeof(TSendOP), packetOPCODE);
            byte[] packetByteArray = outPacket.Array;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");

            outPacket.SafeFlip();
            Socket.Send(Cryptograph.Encrypt(outPacket.GetContent()));

            if (Enum.IsDefined(typeof(TSendOP), outPacket.OperationCode))
            {
                switch (Packet.LogLevel)
                {
                    case LogLevel.Name:
                        Log.Inform("[{0}][ServerHandler]: \n Sent [{1}] packet.", currentTime, packetName);
                        break;

                    case LogLevel.Full:
                        Log.Hex("Sent {0} packet: ", outPacket.GetContent(), Enum.GetName(typeof(TSendOP), outPacket.OperationCode));
                        break;

                    case LogLevel.None:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                Log.Hex("Sent unknown ({0:X2}) packet: ", packetByteArray, packetOPCODE);
            }
        }

        public void Dispose()
        {
            Terminate();

            Socket.Dispose();

            Cryptograph.Dispose();
            ReceiveDone.Dispose(); // TODO: Figure out why this crashes.

            CustomDispose();

            Log.Inform("{0} disposed.", Title);
        }
    }
}
