using System;
using System.Collections.Generic;

namespace Destiny.Maple.Maps
{
    public sealed class MapPortals : MapObjects<Portal>
    {
        public MapPortals(Map map) : base(map) { }

        public Portal this[string label]
        {
            get
            {
                foreach (Portal portal in this)
                {
                    if (!string.Equals(portal.Label, label, StringComparison.CurrentCultureIgnoreCase)) continue;

                    return portal;
                }

                throw new KeyNotFoundException();
            }
        }

        protected override int GetKeyForItem(Portal item)
        {
            return item.ID;
        }
    }
}