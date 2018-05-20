using System;
using System.Collections.Generic;

using Destiny.Data;
using Destiny.Maple.Maps;
using Destiny.Constants;
using Destiny.IO;
using Destiny.Maple.Data;
using Destiny.Network.Common;
using Destiny.Network.PacketFactory;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterTrocks
    {
        public Character TrockParent { get; private set; }

        public static List<int> RegularTrocks { get; private set; }
        public static List<int> VIPTrocks { get; private set; }

        public CharacterTrocks(Character parent)
        {
            TrockParent = parent;

            RegularTrocks = new List<int>();
            VIPTrocks = new List<int>();
        }

        public void Load()
        {
            foreach (Datum datum in new Datums("trocks").Populate("CharacterID = {0}", TrockParent.ID))
            {
                byte index = (byte)datum["Index"];
                int map = (int)datum["Map"];

                if (index >= 5)
                {
                    VIPTrocks.Add(map);
                }
                else
                {
                    RegularTrocks.Add(map);
                }
            }
        }

        public void Save()
        {
            Database.Delete("trocks", "CharacterID = {0}", TrockParent.ID);

            byte index = 0;

            foreach (int map in RegularTrocks)
            {
                Datum datum = new Datum("trocks")
                {
                    ["CharacterID"] = TrockParent.ID,
                    ["Index"] = index++,
                    ["Map"] = map
                };

                datum.Insert();
            }

            index = 5;

            foreach (int map in VIPTrocks)
            {
                Datum datum = new Datum("trocks")
                {
                    ["CharacterID"] = TrockParent.ID,
                    ["Index"] = index++,
                    ["Map"] = map
                };

                datum.Insert();
            }
        }

        public bool Contains(int mapID)
        {
            foreach (int map in VIPTrocks)
            {
                if (map == mapID)
                {
                    return true;
                }
            }

            foreach (int map in VIPTrocks)
            {
                if (map == mapID)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateTrockHandler(Packet inPacket)
        {
            ItemConstants.TrockInventoryAction action = (ItemConstants.TrockInventoryAction)inPacket.ReadByte();
            ItemConstants.TrockType type = (ItemConstants.TrockType)inPacket.ReadByte();

            switch (action)
            {
                case ItemConstants.TrockInventoryAction.RemoveTrock:
                    {
                        int mapID = inPacket.ReadInt();

                        switch (type)
                        {
                            case ItemConstants.TrockType.Regular:
                                if (!RegularTrocks.Contains(mapID)) return;

                                VIPTrocks.Remove(mapID);
                                break;

                            case ItemConstants.TrockType.VIP:
                                if (!VIPTrocks.Contains(mapID)) return;

                                VIPTrocks.Remove(mapID);
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                        break;

                case ItemConstants.TrockInventoryAction.AddTrock:
                    {
                        int mapID = TrockParent.Map.MapleID;

                        // TODO: Check if the map field limits allow trocks (e.g. Maple Island is forbidden).

                        if (true)
                        {
                            switch (type)
                            {
                                case ItemConstants.TrockType.Regular:
                                    RegularTrocks.Add(mapID);
                                    break;

                                case ItemConstants.TrockType.VIP:
                                    VIPTrocks.Add(mapID);
                                    break;
                            }
                        }

                        /*else
                        {
                            return;
                        }*/
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            TrockParent.Client.Send(CharacterTrocksPackets.TrockInventoryUpdate(action, type));
        }

        public bool UseTrockHandler(int trockID, Packet inPacket)
        {
            bool used = false;
            byte action = inPacket.ReadByte();
            ItemConstants.TrockType type = ItemConstants.TrockType.Regular;

            switch (trockID)
            {
                case (int)ItemConstants.UsableCashItems.TeleportRock:
                    type = ItemConstants.TrockType.Regular;
                    break;

                case (int)ItemConstants.UsableCashItems.VIPTeleportRock:
                    type = ItemConstants.TrockType.VIP;
                    break;
            }

            int destinationMapID = -1;
            ItemConstants.TrockResult result = ItemConstants.TrockResult.Success;

            if ( action == (int)ItemConstants.TrockUseAction.TeleportToPresetMap)
            {
                int mapID = inPacket.ReadInt();

                if (!TrockParent.Trocks.Contains(mapID))
                {
                    result = ItemConstants.TrockResult.CannotGo;
                }

                destinationMapID = mapID;
            }
            else if (action == (int)ItemConstants.TrockUseAction.TeleportToIGN)
            {
                string targetName = inPacket.ReadString();

                Character target = null; // TrockParent.Client.Channel.Characters.GetCharacter(targetName);

                if (target == null)
                {
                    result = ItemConstants.TrockResult.DifficultToLocate;
                }
                else
                {
                    destinationMapID = target.Map.MapleID;
                }
            }

            int ticks = inPacket.ReadInt();

            if (destinationMapID != -1)
            {
                Map originMap = TrockParent.Map;
                Map destinationMap = DataProvider.Maps[destinationMapID];

                if (false) // TODO: Field limit check.
                {
                    result = ItemConstants.TrockResult.CannotGo;
                }
                else if (false) // TODO: Origin map field limit check.
                {
                    result = ItemConstants.TrockResult.CannotGo;
                }
                else if (originMap.MapleID == destinationMap.MapleID)
                {
                    result = ItemConstants.TrockResult.AlreadyThere;
                }
                else if (false) // TODO: Continent check.
                {
                    result = ItemConstants.TrockResult.CannotGo;
                }
            }
            
            if (result == ItemConstants.TrockResult.Success)
            {
                TrockParent.SendChangeMapRequest(destinationMapID);

                used = true;
            }
            else
            {
                TrockParent.Client.Send(CharacterTrocksPackets.TrockTransferResult(result, type));
            }

            return used;
        }

        public static byte[] RegularTrockToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                int remaining = 1;

                while (remaining <= RegularTrocks.Count)
                {
                    oPacket.WriteInt(RegularTrocks[remaining - 1]);

                    remaining++;
                }

                while (remaining <= 5)
                {
                    oPacket.WriteInt(999999999);

                    remaining++;
                }

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }

        public static byte[] VIPTrockToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                int remaining = 1;

                while (remaining <= VIPTrocks.Count)
                {
                    oPacket.WriteInt(VIPTrocks[remaining - 1]);

                    remaining++;
                }

                while (remaining <= 10)
                {
                    oPacket.WriteInt(999999999);

                    remaining++;
                }

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }
    }
}
