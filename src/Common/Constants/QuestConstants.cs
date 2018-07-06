using System;

namespace Destiny.Constants
{
    public static class QuestConstants
    {
        #region Quests
        public enum QuestAction : byte
        {
            RestoreLostItem,
            Start,
            Complete,
            Forfeit,
            ScriptStart,
            ScriptEnd
        }

        [Flags]
        public enum QuestFlags : short
        {
            //TODO: Test this; I'm just guessing
            AutoStart = 0x01,
            SelectedMob = 0x02
        }

        public enum QuestResult : byte
        {
            AddTimeLimit = 0x06,
            RemoveTimeLimit = 0x07,
            Complete = 0x08,
            GenericError = 0x09,
            NoInventorySpace = 0x0A,
            NotEnoughMesos = 0x0B,
            ItemWornByChar = 0x0D,
            OnlyOneOfItemAllowed = 0x0E,
            Expire = 0x0F,
            ResetTimeLimit = 0x10
        }

        public enum QuestStatus : byte
        {
            NotStarted = 0,
            InProgress = 1,
            Complete = 2
        }
        #endregion
    }
}
