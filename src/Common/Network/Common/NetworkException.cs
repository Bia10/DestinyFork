using System;

namespace Destiny.Network.Common
{
    public class NetworkException : Exception
    {
        public NetworkException() : base("A network error occured.") { }

        public NetworkException(string message) : base(message) { }
    }
}
