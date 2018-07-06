namespace Destiny.Constants
{
    public static class ServerRegistrationResponseResolver
    {
        public static string Explain(ServerRegsitrationResponse Packet)
        {
            switch (Packet)
            {
                case ServerRegsitrationResponse.InvalidType:
                    return "Unknown server type.";

                case ServerRegsitrationResponse.InvalidCode:
                    return "The provided security code is not corresponding.";

                case ServerRegsitrationResponse.Full:
                    return "Cannot register as all the spots are occupied.";

                case ServerRegsitrationResponse.Valid:
                    return "ALL OK!";

                default:
                    return null;
            }
        }
    }

    public enum ServerRegsitrationResponse : byte
    {
        Valid,
        InvalidType,
        InvalidCode,
        Full
    }

    public static class ServerConstants
    {
        #region Server
        public enum AccountLevel : byte
        {
            Normal,
            Intern,
            Gm,
            SuperGm,
            Administrator
        }

        public enum MessageType : byte
        {
            DropPickup,
            QuestRecord,
            CashItemExpire,
            IncreaseEXP,
            IncreaseFame,
            IncreaseMeso,
            IncreaseGP,
            GiveBuff,
            GeneralItemExpire,
            System,
            QuestRecordEx,
            ItemProtectExpire,
            ItemExpireReplace,
            SkillExpire,
            TutorialMessage = 23
        }

        public enum ServerType
        {
            None,
            Login,
            Channel,
            Shop,
            ITC
        }

        public enum ScriptType
        {
            Npc,
            Portal
        }

        public enum NoticeType : byte
        {
            Notice,
            Popup,
            Megaphone,
            SuperMegaphone,
            ScrollingText,
            PinkText,
            LightBlueText,
            // 7
            ItemMegaphone = 8,
            // 9
            // 10
        }
        #endregion
    }
}
