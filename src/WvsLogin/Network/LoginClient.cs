using System;
using System.Collections.Generic;
using System.Net.Sockets;

using Destiny.Data;
using Destiny.Maple;
using Destiny.Security;
using Destiny.Constants;
using Destiny.Network.ClientHandler;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network
{
    public sealed class LoginClient : MapleClientHandler.MapleClientHandler
    {
        public long ID { get; private set; }
        public byte World { get; private set; }
        public byte Channel { get; private set; }
        public Account Account { get; private set; }
        public string LastUsername { get; private set; }
        public string LastPassword { get; private set; }
        public string[] MacAddresses { get; private set; }

        public LoginClient(Socket socket) : base(socket)
        {
            ID = Application.Random.Next();
        }

        protected override bool IsServerAlive
        {
            get
            {
                return WvsLogin.IsAlive;
            }
        }

        protected override void Dispatch(Packet iPacket)
        {
            switch ((ClientOperationCode)iPacket.OperationCode)
            {
                case ClientOperationCode.AccountLogin:
                    Login(iPacket);
                    break;

                case ClientOperationCode.EULA:
                    EULA(iPacket);
                    break;

                case ClientOperationCode.AccountGender:
                    SetGender(iPacket);
                    break;

                case ClientOperationCode.PinCheck:
                    CheckPin(iPacket);
                    break;

                case ClientOperationCode.PinUpdate:
                    UpdatePin(iPacket);
                    break;

                case ClientOperationCode.WorldList:
                case ClientOperationCode.WorldRelist:
                    ListWorlds();
                    break;

                case ClientOperationCode.WorldStatus:
                    InformWorldStatus(iPacket);
                    break;

                case ClientOperationCode.WorldSelect:
                    SelectWorld(iPacket);
                    break;

                case ClientOperationCode.ViewAllChar:
                    ViewAllChar(iPacket);
                    break;

                case ClientOperationCode.VACFlagSet:
                    SetViewAllChar(iPacket);
                    break;

                case ClientOperationCode.CharacterNameCheck:
                    CheckCharacterName(iPacket);
                    break;

                case ClientOperationCode.CharacterCreate:
                    CreateCharacter(iPacket);
                    break;

                case ClientOperationCode.CharacterDelete:
                    DeleteCharacter(iPacket);
                    break;

                case ClientOperationCode.CharacterSelect:
                case ClientOperationCode.SelectCharacterByVAC:
                    SelectCharacter(iPacket);
                    break;

                case ClientOperationCode.CharacterSelectRegisterPic:
                case ClientOperationCode.RegisterPicFromVAC:
                    SelectCharacter(iPacket, registerPic: true);
                    break;

                case ClientOperationCode.CharacterSelectRequestPic:
                case ClientOperationCode.RequestPicFromVAC:
                    SelectCharacter(iPacket, requestPic: true);
                    break;
            }
        }

        private void Login(Packet iPacket)
        {
            string username = iPacket.ReadString();
            string password = iPacket.ReadString();

            if (!username.IsAlphaNumeric())
            {
                SendLoginResult(LoginConstants.LoginResult.InvalidUsername);
            }
            else
            {
                Account = new Account(this);

                try
                {
                    Account.Load(username);

                    if (SHACryptograph.Encrypt(SHAMode.SHA512, password + Account.Salt) != Account.Password)
                    {
                        SendLoginResult(LoginConstants.LoginResult.InvalidPassword);
                    }                  
                    else if (Account.IsBanned)
                    {
                        SendLoginResult(LoginConstants.LoginResult.Banned);
                    }
                    else if (!Account.EULA)
                    {
                        SendLoginResult(LoginConstants.LoginResult.EULA);
                    }
                    else // TODO: Add more scenarios (require master IP, check banned IP, check logged in).
                    {
                        SendLoginResult(LoginConstants.LoginResult.Valid);
                    }
                }
                catch (NoAccountException)
                {
                    if (WvsLogin.AutoRegister && username == LastUsername && password == LastPassword)
                    {
                        Account.Username = username;
                        Account.Salt = HashGenerator.GenerateMD5();
                        Account.Password = SHACryptograph.Encrypt(SHAMode.SHA512, password + Account.Salt);
                        Account.EULA = false;
                        Account.Gender = CharacterConstants.Gender.Unset;
                        Account.Pin = string.Empty;
                        Account.Pic = string.Empty;
                        Account.IsBanned = false;
                        Account.IsMaster = false;
                        Account.Birthday = DateTime.UtcNow;
                        Account.Creation = DateTime.UtcNow;
                        Account.MaxCharacters = WvsLogin.MaxCharacters;

                        Account.Save();

                        SendLoginResult(LoginConstants.LoginResult.Valid);
                    }
                    else
                    {
                        SendLoginResult(LoginConstants.LoginResult.InvalidUsername);

                        LastUsername = username;
                        LastPassword = password;
                    }
                }
            }
        }

        private void SendLoginResult(LoginConstants.LoginResult result)
        {
            using (Packet oPacket = new Packet(ServerOperationCode.CheckPasswordResult))
            {
                oPacket
                    .WriteInt((int)result)
                    .WriteByte()
                    .WriteByte();

                if (result == LoginConstants.LoginResult.Valid)
                {
                    oPacket
                        .WriteInt(Account.ID)
                        .WriteByte((byte)Account.Gender)
                        .WriteByte() // NOTE: Grade code.
                        .WriteByte() // NOTE: Subgrade code.
                        .WriteByte() // NOTE: Country code.
                        .WriteString(Account.Username)
                        .WriteByte() // NOTE: Unknown.
                        .WriteByte() // NOTE: Quiet ban reason. 
                        .WriteLong() // NOTE: Quiet ban lift date.
                        .WriteDateTime(Account.Creation)
                        .WriteInt() // NOTE: Unknown.
                        .WriteByte((byte)(WvsLogin.RequestPin ? 0 : 2)) // NOTE: 1 seems to not do anything.
                        .WriteByte((byte)(WvsLogin.RequestPic ? (string.IsNullOrEmpty(Account.Pic) ? 0 : 1) : 2));
                }

                Send(oPacket);
            }
        }

        private void EULA(Packet iPacket)
        {
            bool accepted = iPacket.ReadBool();

            if (accepted)
            {
                Account.EULA = true;

                Datum datum = new Datum("accounts") {["EULA"] = true};

                datum.Update("ID = {0}", Account.ID);

                SendLoginResult(LoginConstants.LoginResult.Valid);
            }
            else
            {
                Stop(); // NOTE: I'm pretty sure in the real client it disconnects you if you refuse to accept the EULA.
            }
        }

        private void SetGender(Packet iPacket)
        {
            if (Account.Gender != CharacterConstants.Gender.Unset)
            {
                return;
            }

            bool valid = iPacket.ReadBool();

            if (!valid) return;

            CharacterConstants.Gender gender = (CharacterConstants.Gender)iPacket.ReadByte();

            Account.Gender = gender;

            Datum datum = new Datum("accounts") { ["Gender"] = (byte) Account.Gender};

            datum.Update("ID = {0}", Account.ID);

            SendLoginResult(LoginConstants.LoginResult.Valid);
        }

        private void CheckPin(Packet iPacket)
        {
            byte a = iPacket.ReadByte();
            byte b = iPacket.ReadByte();

            LoginConstants.PinResult result;

            if (b == 0)
            {
                string pin = iPacket.ReadString();

                if (SHACryptograph.Encrypt(SHAMode.SHA256, pin) != Account.Pin)
                {
                    result = LoginConstants.PinResult.Invalid;
                }
                else
                {
                    if (a == 1)
                    {
                        result = LoginConstants.PinResult.Valid;
                    }
                    else if (a == 2)
                    {
                        result = LoginConstants.PinResult.Register;
                    }
                    else
                    {
                        result = LoginConstants.PinResult.Error;
                    }
                }
            }
            else if (b == 1)
            {
                if (string.IsNullOrEmpty(Account.Pin))
                {
                    result = LoginConstants.PinResult.Register;
                }
                else
                {
                    result = LoginConstants.PinResult.Request;
                }
            }
            else
            {
                result = LoginConstants.PinResult.Error;
            }

            using (Packet oPacket = new Packet(ServerOperationCode.CheckPinCodeResult))
            {
                oPacket.WriteByte((byte)result);

                Send(oPacket);
            }
        }

        private void UpdatePin(Packet iPacket)
        {
            bool procceed = iPacket.ReadBool();
            string pin = iPacket.ReadString();

            if (procceed)
            {
                Account.Pin = SHACryptograph.Encrypt(SHAMode.SHA256, pin);

                Datum datum = new Datum("accounts");

                datum["Pin"] = Account.Pin;

                datum.Update("ID = {0}", Account.ID);

                using (Packet oPacket = new Packet(ServerOperationCode.UpdatePinCodeResult))
                {
                    oPacket.WriteByte(); // NOTE: All the other result types end up in a "trouble logging into the game" message.

                    Send(oPacket);
                }
            }
        }

        private void ListWorlds()
        {
            foreach (World world in WvsLogin.Worlds)
            {
                using (Packet oPacket = new Packet(ServerOperationCode.WorldInformation))
                {
                    oPacket
                        .WriteByte(world.ID)
                        .WriteString(world.Name)
                        .WriteByte((byte)world.Flag)
                        .WriteString(world.EventMessage)
                        .WriteShort(100) // NOTE: Event EXP rate
                        .WriteShort(100) // NOTE: Event Drop rate
                        .WriteBool(false) // NOTE: Character creation disable.
                        .WriteByte((byte)world.Count);

                    foreach (Channel channel in world)
                    {
                        oPacket
                            .WriteString($"{world.Name}-{channel.ID}")
                            .WriteInt(channel.Population)
                            .WriteByte(1)
                            .WriteByte(channel.ID)
                            .WriteBool(false); // NOTE: Adult channel.
                    }

                    //TODO: Add login balloons. These are chat bubbles shown on the world select screen
                    oPacket.WriteShort(); //balloon count
                                          //foreach (var balloon in balloons)
                                          //{
                                          //    oPacket
                                          //        .WriteShort(balloon.X)
                                          //        .WriteShort(balloon.Y)
                                          //        .WriteString(balloon.Text);
                                          //}

                    Send(oPacket);
                }

                using (Packet oPacket = new Packet(ServerOperationCode.WorldInformation))
                {
                    oPacket.WriteByte(byte.MaxValue);

                    Send(oPacket);
                }

                // TODO: Last connected world. Get this from the database. Set the last connected world once you succesfully load a character.
                using (Packet oPacket = new Packet(ServerOperationCode.LastConnectedWorld))
                {
                    oPacket.WriteInt(); // NOTE: World ID.

                    Send(oPacket);
                }

                // TODO: Recommended worlds. Get this from configuration.
                using (Packet oPacket = new Packet(ServerOperationCode.RecommendedWorldMessage))
                {
                    oPacket
                        .WriteByte(1) // NOTE: Count.
                        .WriteInt() // NOTE: World ID.
                        .WriteString("Check out Scania! The best world to play - and not because it's the only one available... hehe."); // NOTE: Message.

                    Send(oPacket);
                }
            }
        }

        private void InformWorldStatus(Packet iPacket)
        {
            byte worldID = iPacket.ReadByte();

            World world;

            try
            {
                world = WvsLogin.Worlds[worldID];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            using (Packet oPacket = new Packet(ServerOperationCode.CheckUserLimitResult))
            {
                oPacket.WriteShort((short)world.Status);

                Send(oPacket);
            }
        }

        private void SelectWorld(Packet iPacket)
        {
            iPacket.ReadByte(); // NOTE: Connection kind (GameLaunching, WebStart, etc.).
            World = iPacket.ReadByte();
            Channel = iPacket.ReadByte();
            iPacket.ReadBytes(4); // NOTE: IPv4 Address.

            List<byte[]> characters = WvsLogin.CenterConnection.GetCharacters(World, Account.ID);

            using (Packet oPacket = new Packet(ServerOperationCode.SelectWorldResult))
            {
                oPacket
                    .WriteBool(false)
                    .WriteByte((byte)characters.Count);

                foreach (byte[] characterBytes in characters)
                {
                    oPacket.WriteBytes(characterBytes);
                }

                oPacket
                    .WriteByte((byte)(WvsLogin.RequestPic ? (string.IsNullOrEmpty(Account.Pic) ? 0 : 1) : 2))
                    .WriteInt(Account.MaxCharacters);

                Send(oPacket);
            }
        }

        private void ViewAllChar(Packet iPacket)
        {
            /*List<byte[]> characters = WvsLogin.CenterConnection.GetCharacters(World, Account.ID);

            if (IsInViewAllChar)
            {
                using (Packet oPacket = new Packet(ServerOperationCode.ViewAllCharResult))
                {
                    oPacket
                        .WriteByte((byte) VACResult.UnknownError)
                        .WriteByte();

                    Send(oPacket);
                }

                IsInViewAllChar = true;
                List<Character> characters = new List<Character>();

                foreach (Datum datum in new Datums("characters").PopulateWith("ID", "AccountID = {0}", Account.ID))
                {
                    Character character = new Character((int) datum["ID"], this);

                    character.Load();
                    characters.Add(character);
                }

                using (Packet oPacket = new Packet(ServerOperationCode.ViewAllCharResult))
                {
                    if (characters.Count == 0)
                    {
                        oPacket
                            .WriteByte((byte) VACResult.NoCharacters);
                    }

                    else
                    {
                        oPacket
                            .WriteByte((byte) VACResult.SendCount)
                            .WriteInt(characters.Count)
                            .WriteInt(); //unknown

                        Send(oPacket);
                    }
                }
            }
        }

        foreach (WorldServer world in MasterServer.Worlds)
           {
                using (Packet oPacket = new Packet(ServerOperationCode.ViewAllCharResult))
                {
                    IEnumerable<Character> worldChars = characters.Where(x => x.WorldID == world.ID);

                    oPacket
                     .WriteByte((byte)VACResult.CharInfo)
                        .WriteByte(world.ID)
                        .WriteByte((byte)worldChars.Count());

                    foreach (Character character in worldChars)
                    {
                        oPacket.WriteBytes(character.ToByteArray());
                    }

                    Send(oPacket);
                }*/
            }      

        private void SetViewAllChar(Packet iPacket)
        {
            //IsInViewAllChar = iPacket.ReadBool();
        }

        private void CheckCharacterName(Packet iPacket)
        {
            string name = iPacket.ReadString();
            bool unusable = WvsLogin.CenterConnection.IsNameTaken(name);

            using (Packet oPacket = new Packet(ServerOperationCode.CheckDuplicatedIDResult))
            {
                oPacket
                    .WriteString(name)
                    .WriteBool(unusable);

                Send(oPacket);
            }
        }

        private void CreateCharacter(Packet iPacket)
        {
            byte[] characterData = iPacket.ReadBytes();

            using (Packet outPacket = new Packet(ServerOperationCode.CreateNewCharacterResult))
            {
                outPacket.WriteByte(); // NOTE: 1 for failure. Could be implemented as anti-packet editing.
                outPacket.WriteBytes(WvsLogin.CenterConnection.CreateCharacter(World, Account.ID, characterData));

                Send(outPacket);
            }
        }

        // TODO: Proper character deletion with all the necessary checks (cash items, guilds, etcetera). 
        private void DeleteCharacter(Packet iPacket)
        {
            string pic = iPacket.ReadString();
            int characterID = iPacket.ReadInt();

            LoginConstants.CharacterDeletionResult result;

            if (SHACryptograph.Encrypt(SHAMode.SHA256, pic) == Account.Pic || !WvsLogin.RequestPic)
            {
                //NOTE: As long as foreign keys are set to cascade, all child entries related to this CharacterID will also be deleted.
                Database.Delete("characters", "ID = {0}", characterID);

                result = LoginConstants.CharacterDeletionResult.Valid;
            }
            else
            {
                result = LoginConstants.CharacterDeletionResult.InvalidPic;
            }

            using (Packet oPacket = new Packet(ServerOperationCode.DeleteCharacterResult))
            {
                oPacket
                    .WriteInt(characterID)
                    .WriteByte((byte)result);

                Send(oPacket);
            }
        }

        private void SelectCharacter(Packet iPacket, bool fromViewAll = false, bool requestPic = false, bool registerPic = false)
        {
            string pic = string.Empty;

            if (requestPic)
            {
                pic = iPacket.ReadString();
            }
            else if (registerPic)
            {
                iPacket.ReadByte();
            }

            int characterID = iPacket.ReadInt();

            //if (IsInViewAllChar)
            //{
            //    WorldID = (byte)iPacket.ReadInt();
            //    ChannelID = 0; // TODO: Least loaded channel.
            //}

            MacAddresses = iPacket.ReadString().Split(new char[] { ',', ' ' });

            if (registerPic)
            {
                iPacket.ReadString();
                pic = iPacket.ReadString();

                if (string.IsNullOrEmpty(Account.Pic))
                {
                    Account.Pic = SHACryptograph.Encrypt(SHAMode.SHA256, pic);

                    Datum datum = new Datum("accounts") {["Pic"] = Account.Pic};

                    datum.Update("ID = {0}", Account.ID);
                }
            }

            if (!requestPic || SHACryptograph.Encrypt(SHAMode.SHA256, pic) == Account.Pic)
            {
                if (!WvsLogin.CenterConnection.Migrate(RemoteEndPoint.Address.ToString(), Account.ID, characterID))
                {
                    Stop();

                    return;
                }

                using (Packet oPacket = new Packet(ServerOperationCode.SelectCharacterResult))
                {
                    oPacket
                        .WriteByte()
                        .WriteByte()
                        .WriteBytes(127, 0, 0, 1)
                        .WriteUShort(WvsLogin.Worlds[World][Channel].Port)
                        .WriteInt(characterID)
                        .WriteInt()
                        .WriteByte();

                    Send(oPacket);
                }
            }
            else
            {
                using (Packet oPacket = new Packet(ServerOperationCode.CheckSPWResult))
                {
                    oPacket.WriteByte();

                    Send(oPacket);
                }
            }
        }

    }
}
