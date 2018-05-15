using Destiny.Network.Common;

namespace Destiny.Maple
{
    public interface IControllable
    {
        Packet GetControlRequestPacket();
        Packet GetControlCancelPacket();
    }
}
