using System;
using System.Collections.Generic;

using Destiny.Maple.Characters;
using Destiny.Data;
using Destiny.Maple.Shops;
using Destiny.Maple.Scripting;
using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Life
{
    public class Npc : LifeObject, ISpawnable, IControllable
    {
        public Npc(Datum datum) : base(datum)
        {
            Scripts = new Dictionary<Character, NpcScript>();
        }

        public Character Controller { get; set; }

        public Shop Shop { get; set; }
        public int StorageCost { get; set; }
        public Dictionary<Character, NpcScript> Scripts { get; }

        public void Move(Packet iPacket)
        {
            byte action1 = iPacket.ReadByte();
            byte action2 = iPacket.ReadByte();

            Movements movements = null;

            if (iPacket.Remaining > 0)
            {
                movements = Movements.Decode(iPacket);
            }

            using (Packet oPacket = new Packet(ServerOperationCode.NpcMove))
            {
                oPacket
                    .WriteInt(ObjectID)
                    .WriteByte(action1)
                    .WriteByte(action2);

                if (movements != null)
                {
                    oPacket.WriteBytes(movements.ToByteArray());
                }

                Map.Broadcast(oPacket);
            }
        }

        public void Converse(Character talker)
        {
            if (Shop != null)
            {
                Shop.Show(talker);
            }
            else if (StorageCost > 0)
            {
                talker.Storage.Show(this);
            }
            else
            {
                var script = new NpcScript(this, talker);

                Scripts[talker] = script;

                try
                {
                    script.Execute();
                }
                catch (Exception ex)
                {
                    //Log.Error("Error in NPC conversation: {0}", ex);
                }
            }
        }

        public void Handle(Character talker, Packet iPacket)
        {
            if (talker.LastNpc == null)
            {
                return;
            }

            NPCsConstants.NpcMessageType lastMessageType = (NPCsConstants.NpcMessageType)iPacket.ReadByte();

            byte action = iPacket.ReadByte();

            // TODO: Validate last message type.

            int selection = -1;

            byte endTalkByte;

            switch (lastMessageType)
            {
                case NPCsConstants.NpcMessageType.RequestText:
                case NPCsConstants.NpcMessageType.RequestNumber:
                case NPCsConstants.NpcMessageType.RequestStyle:
                case NPCsConstants.NpcMessageType.Choice:
                    endTalkByte = 0;
                    break;

                default:
                    endTalkByte = byte.MaxValue;
                    break;
            }

            if (action != endTalkByte)
            {
                if (iPacket.Remaining >= 4)
                {
                    selection = iPacket.ReadInt();
                }
                else if (iPacket.Remaining > 0)
                {
                    selection = iPacket.ReadByte();
                }

                if (lastMessageType == NPCsConstants.NpcMessageType.RequestStyle)
                {
                    //selection = StyleSelectionHelpers[talker][selection];
                }

                if (selection != -1)
                {
                    Scripts[talker].SetResult(selection);
                }
                else
                {
                    Scripts[talker].SetResult(action);
                }
            }
            else
            {
                talker.LastNpc = null;
            }
        }

        public void AssignController()
        {
            if (Controller != null) return;

            int leastControlled = int.MaxValue;
            Character newController = null;

            lock (Map.Characters)
            {
                foreach (Character character in Map.Characters)
                {
                    if (character.ControlledNpcs.Count >= leastControlled) continue;

                    leastControlled = character.ControlledNpcs.Count;
                    newController = character;
                }
            }

            newController?.ControlledNpcs.Add(this);
        }

        public Packet GetCreatePacket()
        {
            return GetSpawnPacket();
        }

        public Packet GetSpawnPacket()
        {
            return GetInternalPacket(false);
        }

        public Packet GetControlRequestPacket()
        {
            return GetInternalPacket(true);
        }

        private Packet GetInternalPacket(bool requestControl)
        {
            Packet oPacket = new Packet(requestControl ? ServerOperationCode.NpcChangeController : ServerOperationCode.NpcEnterField);

            if (requestControl)
            {
                oPacket.WriteBool(true);
            }

            oPacket
                .WriteInt(ObjectID)
                .WriteInt(MapleID)
                .WriteShort(Position.X)
                .WriteShort(Position.Y)
                .WriteBool(!FacesLeft)
                .WriteShort(Foothold)
                .WriteShort(MinimumClickX)
                .WriteShort(MaximumClickX)
                .WriteBool(true); // NOTE: Hide.

            return oPacket;
        }

        public Packet GetControlCancelPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.NpcChangeController);

            oPacket
                .WriteBool(false)
                .WriteInt(ObjectID);

            return oPacket;
        }

        public Packet GetDialogPacket(string text, NPCsConstants.NpcMessageType messageType, params byte[] footer)
        {
            Packet oPacket = new Packet(ServerOperationCode.ScriptMessage);

            oPacket
                .WriteByte(4) // NOTE: Unknown.
                .WriteInt(MapleID)
                .WriteByte((byte)messageType)
                .WriteByte() // NOTE: Speaker.
                .WriteString(text)
                .WriteBytes(footer);

            return oPacket;
        }

        public Packet GetDestroyPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.NpcLeaveField);

            oPacket.WriteInt(ObjectID);

            return oPacket;
        }
    }
}
