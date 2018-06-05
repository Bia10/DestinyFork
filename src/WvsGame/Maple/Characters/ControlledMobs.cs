using System.Collections.Generic;
using System.Collections.ObjectModel;

using Destiny.IO;
using Destiny.Maple.Life;
using Destiny.Network.Common;

namespace Destiny.Maple.Characters
{
    public class ControlledMobs : KeyedCollection<int, Mob>
    {
        public Character Parent { get; }

        public ControlledMobs(Character parent)
        {
            Parent = parent;
        }

        public void MoveHandler(Packet inPacket)
        {
            int objectID = inPacket.ReadInt();
            if (objectID <= 0) return;

            Mob mob;

            try
            {
                mob = this[objectID];
            }

            catch (KeyNotFoundException)
            {
                Log.SkipLine();
                Log.Warn(string.Format("ControlledMobs-MoveHandler: KeyNotFound! Argument: {0} ", this[objectID]));
                Log.SkipLine();
                return;
            }

            mob.Move(inPacket);
        }

        protected override void InsertItem(int index, Mob newlyControlledMob)
        {
            lock (this)
            {
                if (Parent.Client.IsAlive)
                {
                    newlyControlledMob.Controller = Parent;

                    base.InsertItem(index, newlyControlledMob);

                    using (Packet oPacket = newlyControlledMob.GetControlRequestPacket())
                    {
                        Parent.Client.Send(oPacket);
                    }
                }
                else
                {
                    newlyControlledMob.AssignController();
                }
            }
        }

        protected override void RemoveItem(int index)
        {
            lock (this)
            {
                Mob controlledMobAtIndex = Items[index];

                if (Parent.Client.IsAlive)
                {
                    using (Packet oPacket = controlledMobAtIndex.GetControlCancelPacket())
                    {
                        Parent.Client.Send(oPacket);
                    }
                }
                controlledMobAtIndex.Controller = null;

                base.RemoveItem(index);
            }
        }

        protected override void ClearItems()
        {
            lock (this)
            {
                // create empty holder
                List<Mob> controlledMobsToRemove = new List<Mob>();

                // populate with each mob in controlledMobs class
                foreach (Mob mob in this)
                {
                    controlledMobsToRemove.Add(mob);
                }

                // remove each mob from holder
                foreach (Mob mob in controlledMobsToRemove)
                {
                    Remove(mob);
                }
            }
        }

        protected override int GetKeyForItem(Mob item)
        {
            return item.ObjectID;
        }
    }
}
