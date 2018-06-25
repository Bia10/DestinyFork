using Destiny.Data;

namespace Destiny.Maple.Life
{
    public sealed class Loot
    {
        public int MapleID { get; private set; }
        public int MinimumQuantity { get; private set; }
        public int MaximumQuantity { get; private set; }
        public int QuestID { get; private set; }
        public int Chance { get; private set; }
        public bool IsMeso { get; private set; }

        public Loot(Datum datum)
        {
            MapleID = (int)datum["itemid"];
            MinimumQuantity = (int)datum["minimum_quantity"];
            MaximumQuantity = (int)datum["maximum_quantity"];
            QuestID = (int)datum["questid"];
            Chance = (int)datum["chance"];
            IsMeso = ((string)datum["flags"]).Contains("is_mesos");
        }
    }
}
