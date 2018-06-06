using Destiny.Data;

namespace Destiny.Maple.Maps
{
    public sealed class Seat : MapObject
    {
        public short ID { get; }

        public Seat(Datum datum) : base()
        {
            ID = (short)datum["seatid"];
            Position = new Point((short)datum["x_pos"], (short)datum["y_pos"]);
        }
    }
}
