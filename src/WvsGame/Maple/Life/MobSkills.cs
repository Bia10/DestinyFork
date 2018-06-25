﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Destiny.Maple.Life
{
    public sealed class MobSkills : Collection<MobSkill>
    {
        public Mob Parent { get; private set; }

        public MobSkills(Mob parent) : base()
        {
            Parent = parent;
        }

        public MobSkill Random
        {
            get
            {
                return base[Application.Random.Next(Count - 1)];
            }
        }

        public new MobSkill this[int mapleID]
        {
            get
            {
                foreach (MobSkill loopMobSkill in this)
                {
                    if (loopMobSkill.MapleID == mapleID)
                    {
                        return loopMobSkill;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        public bool Contains(int mapleID, byte level)
        {
            foreach (MobSkill loopMobSkill in this)
            {
                if (loopMobSkill.MapleID == mapleID && loopMobSkill.Level == level)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
