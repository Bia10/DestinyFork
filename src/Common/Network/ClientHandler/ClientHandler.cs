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

        private ByteBuffer ReceivalBuffer { get; set; }

        protected ClientHandler(Socket socket, string title = "Client", params object[] args)
        {
            Title = title;

            Log.Inform("Preparing {0} ...", Title.ToLower());

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

                            HandleIncomingPacket(ReceivalBuffer.ReadBytes(length + 4));

                            processed += (length + 4);
                        }

                        ReceivalBuffer.Limit -= processed;

                        ReceivalBuffer.Position = reset ? 0 : ReceivalBuffer.Limit;
                    }

                    else
                    {
                        HandleIncomingPacket(ReceivalBuffer.GetContent());
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

        private void HandleIncomingPacket(byte[] rawPacket)
        {
            using (Packet inPacket = new Packet(Cryptograph.Decrypt(rawPacket)))
            {
                // parse information from packet
                short packetOPCODE = inPacket.OperationCode;
                string packetName = Enum.GetName(typeof(TReceiveOP), packetOPCODE);
                byte[] packetByteArray = inPacket.Array;
                string currentTime = DateTime.Now.ToString("HH:mm:ss");

                // create new log event out of inPacket information
                LogEventQue.LogEvent newLogEvent = new LogEventQue.LogEvent
                {
                    EventTime = currentTime,
                    EventName = packetName,
                    EventData = packetByteArray
                }; 
                              
                // check if packet operation code is known
                if (Enum.IsDefined(typeof(TReceiveOP), packetOPCODE))
                {
                    if (LogEventQue.NormalEventsLog.Count > 499)
                    {
                        LogEventQue.LogEvent lastLogEvent = LogEventQue.NormalEventsLog.FirstOrDefault();

                        LogEventQue.NormalEventsLog.Remove(lastLogEvent);
                        LogEventQue.NormalEventsLog.Add(newLogEvent);
                    }

                    LogEventQue.NormalEventsLog.Add(newLogEvent); 

                    switch (Packet.LogLevel)
                    {
                        case LogLevel.Name:
                            Log.Inform("[{0}][ClientHandler]: \n Received [{1}] packet from [{2}].", newLogEvent.EventTime, newLogEvent.EventName, Title);
                            break;

                        case LogLevel.Full:
                            Log.Hex("[{0}] Received [{1}] packet from [{2}]: ", newLogEvent.EventData, newLogEvent.EventTime, newLogEvent.EventName, Title);
                            break;

                        case LogLevel.None:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // else log as anomalous event
                else
                {
                    Log.SkipLine();
                    Log.Hex("Received unknown (0x{0:X2}) packet from {1}: ", packetByteArray, packetOPCODE, Title);
                    Log.SkipLine();

                    LogEventQue.AnomalousEventsLog.Add(newLogEvent);
                }

                // dispatch regardless if we know the packet
                Dispatch(inPacket);
            }
        }

        public void Send(Packet outPacket)
        {
            lock (this)
            {
                if (IsAlive)
                {
                    short packetOPCODE = outPacket.OperationCode;
                    string packetName = Enum.GetName(typeof(TSendOP), packetOPCODE);
                    byte[] packetByteArray = outPacket.Array;
                    string currentTime = DateTime.Now.ToString("HH:mm:ss");

                    // add new packet event to log que
                    LogEventQue.LogEvent newLogEvent = new LogEventQue.LogEvent
                    {
                        EventTime = currentTime,
                        EventName = packetName,
                        EventData = packetByteArray
                    };

                    outPacket.SafeFlip();
                    Socket.Send(Cryptograph.Encrypt(outPacket.GetContent()));
                                  
                    if (Enum.IsDefined(typeof(TSendOP), outPacket.OperationCode))
                    {
                        if (LogEventQue.NormalEventsLog.Count > 499)
                        {
                            LogEventQue.LogEvent lastLogEvent = LogEventQue.NormalEventsLog.FirstOrDefault();

                            LogEventQue.NormalEventsLog.Remove(lastLogEvent);
                            LogEventQue.NormalEventsLog.Add(newLogEvent);
                        }

                        LogEventQue.NormalEventsLog.Add(newLogEvent);

                        switch (Packet.LogLevel)
                        {
                            case LogLevel.Name:
                                Log.Inform("[{0}][ClientHandler]: \n Sent [{1}] packet to [{2}].", newLogEvent.EventTime, newLogEvent.EventName, Title);
                                break;

                            case LogLevel.Full:
                                Log.Hex("Sent {0} packet to {1}: ", outPacket.GetContent(), Enum.GetName(typeof(TSendOP), outPacket.OperationCode), Title);
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
                        Log.Hex("Sent unknown (0x{0:X2}) packet to {1}: ", outPacket.Array, outPacket.OperationCode, Title);
                        Log.SkipLine();

                        LogEventQue.AnomalousEventsLog.Add(newLogEvent);
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
