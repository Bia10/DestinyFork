using Destiny.Network.Common;

namespace Destiny.Maple
{
    public interface ISpawnable
    {
        Packet GetCreatePacket();
        Packet GetSpawnPacket();
        Packet GetDestroyPacket();
    }
}
