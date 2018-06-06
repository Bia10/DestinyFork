using System;
using System.Data;

using Destiny.Data;
using Destiny.Network;
using Destiny.Constants;

namespace Destiny.Maple
{
    public sealed class GameAccount
    {
        public GameClient Client { get; }

        public int ID { get; private set; }
        public string Username { get; set; }
        public CharacterConstants.Gender Gender { get; set; }
        public bool IsMaster { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime Creation { get; set; }

        private bool Assigned { get; set; }

        public GameAccount(GameClient client)
        {
            Client = client;
        }

        public void Load(int accountID)
        {
            Datum datum = new Datum("accounts");

            try
            {
                datum.Populate("ID = {0}", accountID);
            }

            catch (RowNotInTableException)
            {
                throw new NoGameAccountException();
            }

            ID = (int)datum["ID"];
            Assigned = true;

            Username = (string)datum["Username"];
            Gender = (CharacterConstants.Gender)datum["Gender"];
            IsMaster = (bool)datum["IsMaster"];
            Birthday = (DateTime)datum["Birthday"];
            Creation = (DateTime)datum["Creation"];
        }
    }
}