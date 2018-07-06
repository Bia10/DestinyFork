using System;
using System.Collections.Generic;

using Destiny.Maple.Characters;
using Destiny.Maple.Life;
using Destiny.IO;
using Destiny.Network.Common;

namespace Destiny.Maple.Maps
{
    public sealed class MapCharacters : MapObjects<Character>
    {
        public MapCharacters(Map map) : base(map) { }

        public Character this[string name]
        {
            get
            {
                foreach (Character character in this)
                {
                    if (!string.Equals(character.Name, name, StringComparison.CurrentCultureIgnoreCase)) continue;

                    return character;
                }

                throw new KeyNotFoundException();
            }
        }

        protected override void InsertItem(int index, Character item)
        {
            lock (this)
            {
                foreach (Character character in this)
                {
                    using (Packet oPacket = character.GetSpawnPacket())
                    {
                        item.Client.Send(oPacket);
                    }
                }
            }

            item.Position = Map.Portals.Count > 0 ? Map.Portals[item.SpawnPoint].Position : new Point(0, 0);

            try
            {
                base.InsertItem(index, item);
            }
            catch (Exception e)
            {
                Log.SkipLine();
                Log.Error("MapCharacters-InsertItem() thrown exception: {0}", e);
                Log.SkipLine();
            }
          
            lock (Map.Drops)
            {
                foreach (Drop drop in Map.Drops)
                {
                    if (drop.Owner == null)
                    {
                        using (Packet oPacket = drop.GetSpawnPacket(item))
                        {
                            item.Client.Send(oPacket);
                        }
                    }
                    else
                    {
                        using (Packet oPacket = drop.GetSpawnPacket())
                        {
                            item.Client.Send(oPacket);
                        }
                    }
                }
            }

            lock (Map.Mobs)
            {
                foreach (Mob mob in Map.Mobs)
                {
                    using (Packet oPacket = mob.GetSpawnPacket())
                    {
                        item.Client.Send(oPacket);
                    }
                }
            }

            lock (Map.Npcs)
            {
                foreach (Npc npc in Map.Npcs)
                {
                    using (Packet oPacket = npc.GetSpawnPacket())
                    {
                        item.Client.Send(oPacket);
                    }
                }
            }

            lock (Map.Reactors)
            {
                foreach (Reactor reactor in Map.Reactors)
                {
                    using (Packet oPacket = reactor.GetSpawnPacket())
                    {
                        item.Client.Send(oPacket);
                    }
                }
            }

            lock (Map.Mobs)
            {
                foreach (Mob mob in Map.Mobs)
                {
                    mob.AssignController();
                }
            }

            lock (Map.Npcs)
            {
                foreach (Npc npc in Map.Npcs)
                {
                    npc.AssignController();
                }
            }

            using (Packet oPacket = item.GetCreatePacket())
            {
                Map.Broadcast(oPacket, item);
            }
        }

        protected override void RemoveItem(int index)
        {
            lock (this)
            {
                Character item = Items[index];

                item.ControlledMobs.Clear();
                item.ControlledNpcs.Clear();

                using (Packet oPacket = item.GetDestroyPacket())
                {
                    Map.Broadcast(oPacket, item);
                }
            }

            base.RemoveItem(index);

            lock (Map.Mobs)
            {
                foreach (Mob mob in Map.Mobs)
                {
                    mob.AssignController();
                }
            }

            lock (Map.Npcs)
            {
                foreach (Npc npc in Map.Npcs)
                {
                    npc.AssignController();
                }
            }
        }

        protected override int GetKeyForItem(Character item)
        {
            return item.ID;
        }
    }
}
