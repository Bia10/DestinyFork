﻿using System.Collections.Generic;

using Destiny.Data;
using Destiny.IO;

namespace Destiny.Maple.Data
{
    public sealed class CachedSkills : Dictionary<int, Dictionary<byte, Skill>>
    {
        public CachedSkills() : base()
        {
            using (Log.Load("Skills"))
            {
                foreach (Datum datum in new Datums("skill_player_data").Populate())
                {
                    Add((int)datum["skillid"], new Dictionary<byte, Skill>());
                }

                foreach (Datum datum in new Datums("skill_player_level_data").Populate())
                {
                    this[(int)datum["skillid"]].Add((byte)(short)datum["skill_level"], new Skill(datum));
                }
            }
        }
    }
}
