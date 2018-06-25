﻿using Destiny.Data;

namespace Destiny.Maple.Maps
{
    public sealed class Foothold 
    {
        public short ID { get; private set; }
        public Line Line { get; private set; }
        public short DragForce { get; private set; }
        public bool ForbidDownwardJump { get; private set; }

        public Foothold(Datum datum)
        {
            ID = (short)(int)datum["id"];
            Line = new Line(new Point((short)datum["x1"], (short)datum["y1"]), new Point((short)datum["x2"], (short)datum["y2"]));
            DragForce = (short)datum["drag_force"];
            ForbidDownwardJump = ((string)datum["flags"]).Contains("forbid_downward_jump");
        }
    }
}
