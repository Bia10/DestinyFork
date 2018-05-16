namespace Destiny.Constants
{
    public class NPCsConstants
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
    }
}
