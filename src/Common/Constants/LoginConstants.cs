namespace Destiny.Constants
{
    public class LoginConstants
    {
        #region Login
        public enum CharacterDeletionResult : byte
        {
            Valid = 0,
            InvalidPic = 20
        }

        public enum LoginResult : int
        {
            Valid = 0,
            Banned = 3,
            InvalidPassword = 4,
            InvalidUsername = 5,
            LoggedIn = 7,
            EULA = 23
        }

        public enum PinResult : byte
        {
            Valid = 0,
            Register = 1,
            Invalid = 2,
            Error = 3,
            Request = 4,
            Cancel = 5
        }

        public enum VACResult : byte
        {
            CharInfo = 0,
            SendCount = 1,
            AlreadyLoggedIn = 2,
            UnknownError = 3,
            NoCharacters = 4
        }
        #endregion
    }
}
