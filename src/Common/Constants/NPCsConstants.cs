using System;
using System.Collections.Generic;
using System.Linq;

namespace Destiny.Constants
{
    public static class NPCsConstants
    {
        #region NPCs
        public enum NpcMessageType : byte
        {
            Standard,
            YesNo,
            RequestText,
            RequestNumber,
            Choice,
            RequestStyle = 7,
            AcceptDecline = 12
        }

        public enum ShopAction : byte
        {
            Buy,
            Sell,
            Recharge,
            Leave
        }

        public enum AdminShopAction : byte
        {
            Buy = 1,
            Exit = 2,
            Register = 3
        }

        public enum StorageAction : byte
        {
            WithdrawItem = 4,
            DepositItem,
            ArrangeItem,
            ChangeMesos,
            CloseStorage,
            OpenStorage = 22
        }

        public enum StoragePacketType : byte
        {
            TakeItem = 9,
            ErrorPlayerInventoryFull = 10,
            ErrorNotEnoughMesos = 11,
            ErrorOneOfaKind = 12,
            AddItem = 13,
            ErrorStorageInventoryFull = 17,
            UpdateMesos = 19
        }
        #endregion

        #region AdminShopItems
        // We can either make the items constant or load them from SQL.
        // As you can edit them in-game, I think SQL would be better.
        // In order: ID, MapleID, Price, Stock.
        public static readonly List<Tuple<int, int, int, short>> AdminShopItems = new List<Tuple<int, int, int, short>>()
        {
            new Tuple<int, int, int, short>(0, 2000000, 1000, 200),
            new Tuple<int, int, int, short>(1, 2000001, 1000, 200),
            new Tuple<int, int, int, short>(2, 2000002, 1000, 200),
            new Tuple<int, int, int, short>(3, EquipmentConstants.HatsMapleIDs.ElementAt(24), 1, 10),
            new Tuple<int, int, int, short>(4, EquipmentConstants.HatsMapleIDs.ElementAt(37), 1, 10),
            new Tuple<int, int, int, short>(5, EquipmentConstants.HatsMapleIDs.ElementAt(49), 1, 10),
            new Tuple<int, int, int, short>(6, EquipmentConstants.HatsMapleIDs.ElementAt(12), 1, 10),
            new Tuple<int, int, int, short>(7, EquipmentConstants.HatsMapleIDs.ElementAt(2), 1, 10),
            new Tuple<int, int, int, short>(8, EquipmentConstants.HatsMapleIDs.ElementAt(8), 1, 10),
            new Tuple<int, int, int, short>(9, 1002140, 1, 10) 
        };
        #endregion
    }
}
