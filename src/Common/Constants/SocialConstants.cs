namespace Destiny.Constants
{
    public static class SocialConstants
    {
        #region Social
        public enum MessengerAction : byte
        {
            Open = 0,
            Join = 1,
            Leave = 2,
            Invite = 3,
            Note = 4,
            Decline = 5,
            Chat = 6
        }

        public enum MessengerResult : byte
        {
            Open = 0,
            Join = 1,
            Leave = 2,
            Invite = 3,
            Note = 4,
            Decline = 5,
            Chat = 6
        }

        public enum MultiChatType : byte
        {
            Buddy = 0,
            Party = 1,
            Guild = 2,
            Alliance = 3
        }

        public enum PartyAction : byte
        {
            Create = 1,
            Leave = 2,
            Join = 3,
            Invite = 4,
            Expel = 5,
            ChangeLeader = 6
        }

        public enum PartyResult : byte
        {
            Invite = 4,
            Update = 7,
            Create = 8,
            RemoveOrLeave = 12,
            Join = 15,
            ChangeLeader = 26
        }

        public enum GuildAction : byte
        {
            Update = 0,
            Create = 2,
            Invite = 5,
            Join = 6,
            Leave = 7,
            Expel = 8,
            ModifyTitles = 13,
            ModifyRank = 14,
            ModifyEmblem = 15,
            ModifyNotice = 16
        }

        public enum GuildResult : byte
        {
            Create = 1,
            Invite = 5,
            ChangeEmblem = 17,
            Info = 26,
            AddMember = 39,
            InviteeNotInChannel = 40,
            InviteeAlreadyInGuild = 42,
            LeaveMember = 44,
            MemberExpel = 47,
            Disband = 50,
            MemberOnline = 61,
            UpdateRanks = 62,
            ChangeRank = 64,
            ShowEmblem = 66,
            UpdateNotice = 68
        }

        public enum BbsAction : byte
        {
            AddOrEdit,
            Delete,
            List,
            View,
            Reply,
            DeleteReply
        }
        #endregion
    }
}
