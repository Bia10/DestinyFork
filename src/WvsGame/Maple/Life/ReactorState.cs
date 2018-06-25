using System;

using Destiny.Data;
using Destiny.Constants;

namespace Destiny.Maple.Life
{
    public sealed class ReactorState
    {
        public ReactorConstants.ReactorEventType Type { get; private set; }
        public byte State { get; private set; }
        public byte NextState { get; private set; }
        public int Timeout { get; private set; }
        public int ItemId { get; private set; }
        public short Quantity { get; private set; }
        public Rectangle Boundaries { get; private set; }

        public ReactorState(Datum datum)
        {
            Type = Timeout > 0 ? ReactorConstants.ReactorEventType.Timeout : (ReactorConstants.ReactorEventType)Enum.Parse(typeof(ReactorConstants.ReactorEventType), datum["event_type"].ToString().ToCamel().Replace("_", ""));
            State = (byte)(sbyte)datum["state"];
            NextState = (byte)(sbyte)datum["next_state"];
            Timeout = (int)datum["timeout"];
            ItemId = (int)datum["itemid"];
            Quantity = (short)datum["quantity"];
            Boundaries = new Rectangle(new Point((short)datum["ltx"], (short)datum["lty"]), new Point((short)datum["ltx"], (short)datum["lty"]));
        }
    }
}
