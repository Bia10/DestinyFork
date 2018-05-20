﻿using System;
using System.Data;

using Destiny.Data;
using Destiny.IO;
using Destiny.Network;
using Destiny.Constants;

namespace Destiny.Maple
{
    public sealed class Account
    {
        public LoginClient Client { get; private set; }

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
            this.Client = client;
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

            this.ID = (int)datum["ID"];
            this.Assigned = true;

            this.Username = (string)datum["Username"];
            this.Password = (string)datum["Password"];
            this.Salt = (string)datum["Salt"];
            this.EULA = (bool)datum["EULA"];
            this.Gender = (CharacterConstants.Gender)datum["Gender"];
            this.Pin = (string)datum["Pin"];
            this.Pic = (string)datum["Pic"];
            this.IsBanned = (bool)datum["IsBanned"];
            this.IsMaster = (bool)datum["IsMaster"];
            this.Birthday = (DateTime)datum["Birthday"];
            this.Creation = (DateTime)datum["Creation"];
            this.MaxCharacters = (int)datum["MaxCharacters"];
        }
        
        public void Save()
        {
            Datum datum = new Datum("accounts") {
                ["Username"] = this.Username,
                ["Password"] = this.Password,
                ["Salt"] = this.Salt,
                ["EULA"] = this.EULA,
                ["Gender"] = (byte) this.Gender,
                ["Pin"] = this.Pin,
                ["Pic"] = this.Pic,
                ["IsBanned"] = this.IsBanned,
                ["IsMaster"] = this.IsMaster,
                ["Birthday"] = this.Birthday,
                ["Creation"] = this.Creation,
                ["MaxCharacters"] = this.MaxCharacters
            };


            if (this.Assigned)
            {
                datum.Update("ID = {0}", this.ID);
            }
            else
            {
                this.ID = datum.InsertAndReturnID();
                this.Assigned = true;
            }

            Log.Inform("Saved account '{0}' to database.", this.Username);
        }
    }
}
