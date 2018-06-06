using System;
using System.Data;

using Destiny.Data;
using Destiny.IO;
using Destiny.Network;
using Destiny.Constants;

namespace Destiny.Maple
{
    public sealed class Account
    {
        public LoginClient Client { get; }

        public int ID { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool EULA { get; set; }
        public CharacterConstants.Gender Gender { get; set; }
        public string Pin { get; set; }
        public string Pic { get; set; }
        public bool IsBanned { get; set; }
        public bool IsMaster { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime Creation { get; set; }
        public int MaxCharacters { get; set; }

        private bool Assigned { get; set; }

        public Account(LoginClient client)
        {
            Client = client;
        }

        public void Load(string username)
        {
            Datum datum = new Datum("accounts");

            try
            {
                datum.Populate("Username = {0}", username);
            }

            catch (RowNotInTableException)
            {
                throw new NoAccountException();
            }

            ID = (int)datum["ID"];
            Assigned = true;

            Username = (string)datum["Username"];
            Password = (string)datum["Password"];
            Salt = (string)datum["Salt"];
            EULA = (bool)datum["EULA"];
            Gender = (CharacterConstants.Gender)datum["Gender"];
            Pin = (string)datum["Pin"];
            Pic = (string)datum["Pic"];
            IsBanned = (bool)datum["IsBanned"];
            IsMaster = (bool)datum["IsMaster"];
            Birthday = (DateTime)datum["Birthday"];
            Creation = (DateTime)datum["Creation"];
            MaxCharacters = (int)datum["MaxCharacters"];
        }
        
        public void Save()
        {
            Datum datum = new Datum("accounts") {
                ["Username"] = Username,
                ["Password"] = Password,
                ["Salt"] = Salt,
                ["EULA"] = EULA,
                ["Gender"] = (byte) Gender,
                ["Pin"] = Pin,
                ["Pic"] = Pic,
                ["IsBanned"] = IsBanned,
                ["IsMaster"] = IsMaster,
                ["Birthday"] = Birthday,
                ["Creation"] = Creation,
                ["MaxCharacters"] = MaxCharacters
            };

            if (Assigned)
            {
                datum.Update("ID = {0}", ID);
            }
            else
            {
                ID = datum.InsertAndReturnID();
                Assigned = true;
            }

            Log.Inform("Saved account '{0}' to database.", Username);
        }
    }
}
