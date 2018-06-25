using Destiny.Constants;
using Destiny.Data;
using Destiny.Maple.Characters;
using Destiny.Maple.Data;
using Destiny.Maple.Maps;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Life
{
    public sealed class Reactor : MapObject, ISpawnable
    {
        public int MapleID { get; private set; }
        public string Label { get; private set; }
        public byte State { get; set; }
        public SpawnPoint SpawnPoint { get; private set; }
        public ReactorState[] States { get; set; }

        public Reactor CachedReference
        {
            get
            {
                return DataProvider.Reactors[MapleID];
            }
        }

        public Reactor(Datum datum) : base()
        {
            MapleID = (int)datum["reactorid"];
            Label = string.Empty; // TODO: Is this even relevant?
            State = 0;
            States = new ReactorState[(sbyte)datum["max_states"]];
        }

        public Reactor(int mapleID)
        {
            MapleID = mapleID;
            Label = CachedReference.Label;
            State = CachedReference.State;
            States = CachedReference.States;
        }

        public Reactor(SpawnPoint spawnPoint)
            : this(spawnPoint.MapleID)
        {
            SpawnPoint = spawnPoint;
            Position = spawnPoint.Position;
        }

        public void Hit(Character character, short actionDelay, int skillID)
        {
            ReactorState state = States[State];

            switch (state.Type)
            {
                case ReactorConstants.ReactorEventType.PlainAdvanceState:
                    {
                        State = state.NextState;

                        if (State == States.Length - 1) // TODO: Is this the correct way of doing this?
                        {
                            Map.Reactors.Remove(this);
                        }
                        else
                        {
                            using (Packet oPacket = new Packet(ServerOperationCode.ReactorChangeState))
                            {
                                oPacket
                                    .WriteInt(ObjectID)
                                    .WriteByte(State)
                                    .WriteShort(Position.X)
                                    .WriteShort(Position.Y)
                                    .WriteShort(actionDelay)
                                    .WriteByte() // NOTE: Event index.
                                    .WriteByte(4); // NOTE: Delay.

                                Map.Broadcast(oPacket);
                            }
                        }
                    }
                    break;
            }
        }

        public Packet GetCreatePacket()
        {
            return GetSpawnPacket();
        }

        public Packet GetSpawnPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.ReactorEnterField);

            oPacket
                .WriteInt(ObjectID)
                .WriteInt(MapleID)
                .WriteByte(State)
                .WriteShort(Position.X)
                .WriteShort(Position.Y)
                .WriteShort() // NOTE: Flags (not sure).
                .WriteBool(false) // NOTE: Unknown
                .WriteString(Label);

            return oPacket;
        }

        public Packet GetDestroyPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.ReactorLeaveField);

            oPacket
                .WriteInt(ObjectID)
                .WriteByte(State)
                .WriteShort(Position.X)
                .WriteShort(Position.Y);

            return oPacket;
        }
    }
}
