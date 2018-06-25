using Destiny.Maple.Characters;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;
using Destiny.Threading;

namespace Destiny.Maple.Maps
{
    public abstract class Drop : MapObject, ISpawnable
    {
        public const int ExpiryTime = 60 * 1000;

        public Character Owner { get; set; }
        public Character Picker { get; set; }
        public Point Origin { get; set; }
        public Delay Expiry { get; set; }

        private MapObject mDropper;

        public MapObject Dropper
        {
            get
            {
                return mDropper;
            }
            set
            {
                mDropper = value;

                Origin = mDropper.Position;
                Position = mDropper.Map.Footholds.FindFloor(mDropper.Position);
            }
        }

        public abstract Packet GetShowGainPacket();

        public Packet GetCreatePacket()
        {
            return GetInternalPacket(true, null);
        }

        public Packet GetCreatePacket(Character temporaryOwner)
        {
            return GetInternalPacket(true, temporaryOwner);
        }

        public Packet GetSpawnPacket()
        {
            return GetInternalPacket(false, null);
        }

        public Packet GetSpawnPacket(Character temporaryOwner)
        {
            return GetInternalPacket(false, temporaryOwner);
        }

        private Packet GetInternalPacket(bool dropped, Character temporaryOwner)
        {
            Packet oPacket = new Packet(ServerOperationCode.DropEnterField);

            oPacket
                .WriteByte((byte)(dropped ? 1 : 2)) // TODO: Other types; 3 = disappearing, and 0 probably is something as well.
                .WriteInt(ObjectID)
                .WriteBool(this is Meso);

            var meso = this as Meso;

            if (meso != null)
            {
                oPacket.WriteInt(meso.Amount);
            }
            else
            {
                var item = this as Item;

                if (item != null)
                {
                    oPacket.WriteInt(item.MapleID);
                }
            }

            oPacket
                .WriteInt(Owner?.ID ?? temporaryOwner.ID)
                .WriteByte() // TODO: Type implementation (0 - normal, 1 - party, 2 - FFA, 3 - explosive)
                .WriteShort(Position.X)
                .WriteShort(Position.Y)
                .WriteInt(Dropper.ObjectID);

            if (dropped)
            {
                oPacket
                    .WriteShort(Origin.X)
                    .WriteShort(Origin.Y)
                    .WriteShort(); // NOTE: Foothold, probably.
            }

            if (this is Item)
            {
                oPacket.WriteLong(); // NOTE: Item expiration.
            }

            oPacket.WriteByte(); // NOTE: Pet equip pick-up.

            return oPacket;
        }

        public Packet GetDestroyPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.DropLeaveField);

            oPacket
                .WriteByte((byte)(Picker == null ? 0 : 2))
                .WriteInt(ObjectID)
                .WriteInt(Picker?.ID ?? 0);

            return oPacket;
        }
    }
}