using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

using Destiny.IO;
using Destiny.Network.Common;
using Destiny.Security;

namespace Destiny.Network.ClientHandler
{
    public abstract class ClientHandler<TReceiveOP, TSendOP, TCryptograph> : NetworkConnector<TReceiveOP, TSendOP, TCryptograph>, IDisposable where TCryptograph : Cryptograph, new()
    {
        protected string Title { get; set; }

        protected virtual void Register() { }
        protected virtual void Unregister() { }

        protected ClientHandler(Socket socket, string title = "Client", params object[] args)
        {
            Title = title;

            Log.Inform("Preparing {0}...", Title.ToLower());

            Socket = socket;
            Cryptograph = new TCryptograph();
            ReceivalBuffer = new ByteBuffer() { Limit = 0 };
            IsAlive = true;

            Prepare(args);

            Log.SkipLine();
            Log.Success(string.Format("{0} connected from {1}.", Title, RemoteEndPoint.Address));
            Log.SkipLine();

            Initialize();

            Register();

            while (IsAlive && IsServerAlive)
            {
                ReceiveDone.Reset();

                try
                {
                    Socket.BeginReceive(ReceivalBuffer.Array, ReceivalBuffer.Limit, ReceivalBuffer.Capacity - ReceivalBuffer.Limit, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    Stop();
                }

                ReceiveDone.WaitOne();
            }

            Dispose();
        }

        private ByteBuffer ReceivalBuffer { get; set; }

        private void Handle(byte[] rawPacket)
        {
            using (Packet inPacket = new Packet(Cryptograph.Decrypt(rawPacket)))
            {
                string packetName = Enum.GetName(typeof(TReceiveOP), inPacket.OperationCode);
                string packetLogData = string.Format("Received {0} packet from {1}.", packetName, Title);

                LogEvent lEvent = new LogEvent(packetLogData, DateTime.Now);

                BoundedLogEventQueue<LogEvent> que = new BoundedLogEventQueue<LogEvent>(100);

                que.EnqueueLogEvent(lEvent);

                bool isDuplicate = que.Contains(lEvent);

                if (isDuplicate) return;

                if (Enum.IsDefined(typeof(TReceiveOP), inPacket.OperationCode))
                {
                    switch (Packet.LogLevel)
                    {
                        case LogLevel.Name:
                            Log.Inform("Received {0} packet from {1}.", packetName, Title);
                            break;

                        case LogLevel.Full:
                            Log.Hex("Received {0} packet from {1}: ", inPacket.Array, packetName, Title);
                            break;

                        case LogLevel.None:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    Log.Hex("Received unknown (0x{0:X2}) packet from {1}: ", inPacket.Array, inPacket.OperationCode, Title);
                }

                Dispatch(inPacket);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            if (!IsAlive) return;

            try
            {
                int received = Socket.EndReceive(ar);

                if (received != 0)
                {
                    ReceivalBuffer.Limit += received;

                    if (Cryptograph is MapleCryptograph)
                    {
                        int processed = 0;
                        bool reset = false;

                        if (ReceivalBuffer.Remaining < 4)
                        {
                            Log.Error("TODO: Remaining < 4!");
                        }

                        while (ReceivalBuffer.Remaining >= 4)
                        {
                            int length = AesCryptograph.RetrieveLength(ReceivalBuffer.ReadBytes(4));

                            if (ReceivalBuffer.Remaining < length)
                            {
                                ReceivalBuffer.Position -= 4;

                                try
                                {
                                    Buffer.BlockCopy(ReceivalBuffer.Array, ReceivalBuffer.Position,
                                        ReceivalBuffer.Array, 0, ReceivalBuffer.Remaining);
                                }

                                catch (Exception e)
                                {
                                    Log.SkipLine();
                                    Tracer.TraceErrorMessage(e, "Failed to BlockCopy!");
                                    Log.SkipLine();
                                    throw;
                                }

                                reset = true;

                                break;
                            }

                            ReceivalBuffer.Position -= 4;

                            Handle(ReceivalBuffer.ReadBytes(length + 4));

                            processed += (length + 4);
                        }

                        ReceivalBuffer.Limit -= processed;

                        ReceivalBuffer.Position = reset ? 0 : ReceivalBuffer.Limit;
                    }

                    else
                    {
                        Handle(ReceivalBuffer.GetContent());
                        ReceivalBuffer.Limit = 0;
                        ReceivalBuffer.Position = 0;
                    }
                }

                else
                {
                    Stop();
                }

                ReceiveDone.Set();
            }

            catch (ArgumentNullException ex)
            {
                Log.SkipLine();
                Tracer.TraceErrorMessage(ex, "Argument is null!");
                Log.SkipLine();
                throw;
            }

            catch (ArgumentOutOfRangeException ex)
            {
                Log.SkipLine();
                Tracer.TraceErrorMessage(ex, "Argument is out of range!");
                Log.SkipLine();
                throw;
            }

            catch (SocketException ex)
            {
                Log.SkipLine();
                Tracer.TraceErrorMessage(ex, "Socket exception!");
                Log.SkipLine();
                throw;
            }

            catch (Exception ex)
            {
                Log.SkipLine();
                Log.Error("Uncatched fatal error on {0}: ", ex, Title.ToLower(), Thread.CurrentThread.ManagedThreadId);
                Stop();
            }
        }

        public void Send(Packet Packet)
        {
            lock (this)
            {
                if (IsAlive)
                {
                    Packet.SafeFlip();
                    Socket.Send(Cryptograph.Encrypt(Packet.GetContent()));

                    if (Enum.IsDefined(typeof(TSendOP), Packet.OperationCode))
                    {
                        switch (Packet.LogLevel)
                        {
                            case LogLevel.Name:
                                Log.Inform("Sent {0} packet to {1}.", Enum.GetName(typeof(TSendOP), Packet.OperationCode), Title);
                                break;

                            case LogLevel.Full:
                                Log.Hex("Sent {0} packet to {1}: ", Packet.GetContent(), Enum.GetName(typeof(TSendOP), Packet.OperationCode), Title);
                                break;

                            case LogLevel.None:
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        Log.Hex("Sent unknown (0x{0:X2}) packet to {1}: ", Packet.Array, Packet.OperationCode, Title);
                    }
                }
                else
                {
                    //Log.Warn("Tried to send {0} packet to dead client.", Enum.GetName(typeof(TSendOP), Packet.OperationCode));
                }
            }
        }

        public void Dispose()
        {
            try
            {
                Terminate();
            }
            catch (Exception ex)
            {
                Log.SkipLine();
                Log.Error("Termination error on {0}: ", ex, Title);
                Log.SkipLine();
            }

            Socket.Dispose();
            Cryptograph.Dispose();
            //ReceiveDone.Dispose(); // TODO: Figure why this crashes.
            ReceivalBuffer.Dispose();

            CustomDispose();

            Unregister();

            Log.Inform("{0} disposed.", Title);
        }
    }
}
