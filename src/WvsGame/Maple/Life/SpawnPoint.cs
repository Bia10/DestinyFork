﻿using System;
using Destiny.Data;
using Destiny.IO;

namespace Destiny.Maple.Life
{
    public sealed class SpawnPoint : LifeObject
    {
        public bool IsMob { get; private set; }

        public SpawnPoint(Datum datum, bool isMob)
            : base(datum)
        {
            this.IsMob = isMob;
        }

        public void Spawn()
        {
            if (this.IsMob)
            {
                try
                {
                    this.Map.Mobs.Add(new Mob(this));
                }
                catch (Exception e)
                {
                    Log.SkipLine();
                    Log.Inform("SpawnPoint-Spawn() exception occurred: {0}", e);
                    Log.SkipLine();
                }
            }
            else
            {
                this.Map.Reactors.Add(new Reactor(this));
            }
        }
    }
}
