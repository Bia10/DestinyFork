using System;

namespace Destiny.Maple
{
    public sealed class Migration
    {
        public string Host { get; }
        public int AccountID { get; }
        public int CharacterID { get; }
        public DateTime Expiry { get; }

        public Migration(string host, int accountID, int characterID)
        {
            Host = host;
            AccountID = accountID;
            CharacterID = characterID;
            Expiry = DateTime.Now;
        }
    }
}
