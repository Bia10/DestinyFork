using Destiny.Data;
using Destiny.Maple.Maps;

namespace Destiny.Maple.Life
{
    public abstract class LifeObject : MapObject
    {
        public int MapleID { get; }
        public short Foothold { get; }
        protected short MinimumClickX { get; }
        protected short MaximumClickX { get; }
        protected bool FacesLeft { get; }
        public int RespawnTime { get; }

        protected LifeObject(Datum datum)
        {
            MapleID = (int)datum["lifeid"];
            Position = new Point((short)datum["x_pos"], (short)datum["y_pos"]);
            Foothold = (short)datum["foothold"];
            MinimumClickX = (short)datum["min_click_pos"];
            MaximumClickX = (short)datum["max_click_pos"];
            FacesLeft = ((string)datum["flags"]).Contains("faces_left");
            RespawnTime = (int)datum["respawn_time"];
        }
    }
}
