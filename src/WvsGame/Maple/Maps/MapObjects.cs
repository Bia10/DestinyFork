using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Destiny.IO;
using Destiny.Maple.Characters;

namespace Destiny.Maple.Maps
{
    public abstract class MapObjects<T> : KeyedCollection<int, T> where T : MapObject
    {
        protected Map Map { get; }

        protected MapObjects(Map map)
        {
            Map = map;
        }

        public IEnumerable<T> GetInRange(MapObject reference, int range)
        {
            foreach (T loopObject in this)
            {
                if (reference.Position.DistanceFrom(loopObject.Position) <= range)
                {
                    yield return loopObject;
                }
            }
        }

        protected override int GetKeyForItem(T item)
        {
            return item.ObjectID;
        }

        protected override void InsertItem(int index, T item)
        {
            item.Map = Map;

            if (!(item is Character)  && !(item is Portal))
            {
                item.ObjectID = Map.AssignObjectID();
            }

            try
            {
                base.InsertItem(index, item);
            }
            catch(Exception e)
            {
                Log.SkipLine();
                Tracer.TraceErrorMessage(e, "Failed to insert item!");
                Log.SkipLine();
            }     
        }

        protected override void RemoveItem(int index) 
        {
            if (index >= 0 && index < int.MaxValue)
            {
                if (Items.Count < index)
                {
                    Log.SkipLine();
                    Tracer.TraceErrorMessage("Failed to remove item, there is less items in base then index points to!");
                    Log.SkipLine();
                    //Log.Inform("ERROR: MapObjects-RemoveItem() failed to remove item! Index: {0} \n Theres less items then index points to: {1}", index, base.Items.Count);
                }

                else if (Items.Count >= index)
                {
                    T item = Items[index];
                    item.Map = null;

                    if (!(item is Character) && !(item is Portal))
                    {
                        item.ObjectID = -1;
                    }

                    try
                    {
                        base.RemoveItem(index);
                    }
                    catch (Exception e)
                    {
                        Log.SkipLine();
                        Tracer.TraceErrorMessage(e, "Failed to remove item! \n Exception occurred.");
                        Log.SkipLine();
                    }
                }

                else
                {
                    Log.SkipLine();
                    Tracer.TraceErrorMessage("Failed to remove item!.");
                    Log.SkipLine();
                }             
            }

            else
            {
                Log.SkipLine();
                Tracer.TraceErrorMessage("Failed item index out of bounds!!.");
                Log.SkipLine();
            }
        }
    }
}
