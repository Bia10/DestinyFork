using System.Collections.ObjectModel;

using Destiny.Constants;

namespace Destiny.Maple
{
    public sealed class Worlds : KeyedCollection<byte, World>
    {
        internal Worlds() : base() { }

        internal World Next(ServerConstants.ServerType type)
        {
            lock (this)
            {
                foreach (World loopWorld in this)
                {
                    if (type == ServerConstants.ServerType.Channel && loopWorld.IsFull)
                    {
                        continue;
                    }

                    if (type == ServerConstants.ServerType.Shop && loopWorld.HasShop)
                    {
                        continue;
                    }

                    return loopWorld;
                }

                return null;
            }
        }

        protected override byte GetKeyForItem(World item)
        {
            return item.ID;
        }
    }
}
