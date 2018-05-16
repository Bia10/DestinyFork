﻿namespace Destiny.Constants
{
    public class InteractionConstants
    {
        #region Interaction
        public enum InteractionCode : byte
        {
            Create = 0,
            Invite = 2,
            Decline = 3,
            Visit = 4,
            Room = 5,
            Chat = 6,
            Exit = 10,
            Open = 11,
            TradeBirthday = 14,
            SetItems = 15,
            SetMeso = 16,
            Confirm = 17,
            AddItem = 22,
            Buy = 23,
            UpdateItems = 25,
            RemoveItem = 27,
            OpenStore = 30,
        }

        public enum InteractionType : byte
        {
            Omok = 1,
            Trade = 3,
            PlayerShop = 4,
            HiredMerchant = 5
        }
        #endregion
    }
}
