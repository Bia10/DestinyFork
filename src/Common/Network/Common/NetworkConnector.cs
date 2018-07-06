using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Destiny.Security;

namespace Destiny.Network.Common
{
    public abstract class NetworkConnector<TReceiveOP, TSendOP, TCryptograph> : MarshalByRefObject where TCryptograph : Cryptograph, new()
    {
        private bool isAlive;
        protected readonly ManualResetEvent ReceiveDone = new ManualResetEvent(false);

        protected Socket Socket { get; set; }
        protected TCryptograph Cryptograph { get; set; }

        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
            protected set
            {
                isAlive = value;

                if (!value)
                {
                    ReceiveDone.Set();
                }
            }
        }

        protected IPEndPoint RemoteEndPoint
        {
            get
            {
                return (IPEndPoint)Socket.RemoteEndPoint;
            }
        }

        protected virtual void Prepare(params object[] args) { }
        protected virtual void Initialize() { }
        protected virtual void Terminate() { }
        protected virtual void CustomDispose() { }

        protected abstract bool IsServerAlive { get; }
        protected abstract void Dispatch(Packet inPacket);

        public void Stop()
        {
            IsAlive = false;
        }
    }
}
