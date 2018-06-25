using System;

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
            IsMob = isMob;
        }

        public void Spawn()
        {
            if (IsMob)
            {
                try
                {
                    Map.Mobs.Add(new Mob(this));
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
                Map.Reactors.Add(new Reactor(this));
            }
        }
    }
}
