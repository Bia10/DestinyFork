﻿using System;
using System.Collections.Generic;
using System.Net;

using Destiny.Collections;
using Destiny.Constants;
using Destiny.IO;
using Destiny.Maple;
using Destiny.Security;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Interoperability
{
    public class LoginToCenterServer : ServerHandler<InteroperabilityOperationCode, InteroperabilityOperationCode, BlankCryptograph>
    {
        public LoginToCenterServer(IPEndPoint remoteEP, string code) : base(remoteEP, "Center server", new object[] { code }) { }

        protected override bool IsServerAlive
        {
            get
            {
                return WvsLogin.IsAlive;
            }
        }

        protected override void StopServer()
        {
            WvsLogin.Stop();
        }

        public static void Main()
        {
            try
            {
                WvsLogin.CenterConnection = new LoginToCenterServer(new IPEndPoint(
                        Settings.GetIPAddress("Center/IP"),
                        Settings.GetInt("Center/Port")),
                        Settings.GetString("Center/SecurityCode"));

                WvsLogin.CenterConnection.Loop();
            }
            catch (Exception e)
            {
                Log.Error("Server connection failed: \n{0}", e.Message);

                WvsLogin.Stop();
            }
            finally
            {
                WvsLogin.CenterConnectionDone.Set();
            }
        }

        protected override void Initialize(params object[] args)
        {
            using (Packet Packet = new Packet(InteroperabilityOperationCode.RegistrationRequest))
            {
                Packet.WriteByte((byte)ServerConstants.ServerType.Login);
                Packet.WriteString((string)args[0]);
                Packet.WriteByte((byte)WvsLogin.Worlds.Count);

                foreach (World loopWorld in WvsLogin.Worlds)
                {
                    Packet.WriteByte(loopWorld.ID);
                    Packet.WriteString(loopWorld.Name);
                    Packet.WriteUShort(loopWorld.Port);
                    Packet.WriteUShort(loopWorld.ShopPort);
                    Packet.WriteByte(loopWorld.Channels);
                    Packet.WriteString(loopWorld.TickerMessage);
                    Packet.WriteBool(loopWorld.AllowMultiLeveling);
                    Packet.WriteInt(loopWorld.ExperienceRate);
                    Packet.WriteInt(loopWorld.QuestExperienceRate);
                    Packet.WriteInt(loopWorld.PartyQuestExperienceRate);
                    Packet.WriteInt(loopWorld.MesoRate);
                    Packet.WriteInt(loopWorld.DropRate);
                }

                this.Send(Packet);
            }
        }

        protected override void Terminate()
        {
            WvsLogin.Stop();
        }

        protected override void Dispatch(Packet inPacket)
        {
            switch ((InteroperabilityOperationCode)inPacket.OperationCode)
            {
                case InteroperabilityOperationCode.RegistrationResponse:
                    this.Register(inPacket);
                    break;

                case InteroperabilityOperationCode.UpdateChannel:
                    this.UpdateChannel(inPacket);
                    break;

                case InteroperabilityOperationCode.UpdateChannelPopulation:
                    this.UpdateChannelPopulation(inPacket);
                    break;

                case InteroperabilityOperationCode.CharacterNameCheckResponse:
                    this.CheckCharacterName(inPacket);
                    break;

                case InteroperabilityOperationCode.CharacterEntriesResponse:
                    this.GetCharacters(inPacket);
                    break;

                case InteroperabilityOperationCode.CharacterCreationResponse:
                    this.CreateCharacter(inPacket);
                    break;

                case InteroperabilityOperationCode.MigrationRegisterResponse:
                    this.Migrate(inPacket);
                    break;
            }
        }

        private void Register(Packet inPacket)
        {
            ServerRegsitrationResponse response = (ServerRegsitrationResponse)inPacket.ReadByte();

            switch (response)
            {
                case ServerRegsitrationResponse.Valid:
                    {
                        WvsLogin.Listen();
                        WvsLogin.CenterConnectionDone.Set();

                        Log.SkipLine();
                        Log.Success("Registered Login Server.");
                        Log.SkipLine();
                    }
                    break;

                default:
                    {
                        Log.Error(ServerRegistrationResponseResolver.Explain(response));

                        WvsLogin.Stop();
                    }
                    break;
            }
        }

        private void UpdateChannel(Packet inPacket)
        {
            byte worldID = inPacket.ReadByte();
            bool add = inPacket.ReadBool();

            World world = WvsLogin.Worlds[worldID];

            if (add)
            {
                world.Add(new Channel(inPacket));
            }
            else
            {
                byte channelID = inPacket.ReadByte();

                world.Remove(channelID);
            }
        }

        private void UpdateChannelPopulation(Packet inPacket)
        {
            byte worldID = inPacket.ReadByte();
            byte channelID = inPacket.ReadByte();
            int population = inPacket.ReadInt();

            WvsLogin.Worlds[worldID][channelID].Population = population;
        }

        private void CheckCharacterName(Packet inPacket)
        {
            string name = inPacket.ReadString();
            bool unusable = inPacket.ReadBool();

            this.NameCheckPool.Enqueue(name, unusable);
        }

        private PendingKeyedQueue<int, List<byte[]>> CharacterEntriesPool = new PendingKeyedQueue<int, List<byte[]>>();

        private void GetCharacters(Packet inPacket)
        {
            int accountID = inPacket.ReadInt();

            List<byte[]> entires = new List<byte[]>();

            while (inPacket.Remaining > 0)
            {
                entires.Add(inPacket.ReadBytes(inPacket.ReadByte()));
            }

            this.CharacterEntriesPool.Enqueue(accountID, entires);
        }

        public List<byte[]> GetCharacters(byte worldID, int accountID)
        {
            using (Packet outPacket = new Packet(InteroperabilityOperationCode.CharacterEntriesRequest))
            {
                outPacket.WriteByte(worldID);
                outPacket.WriteInt(accountID);

                this.Send(outPacket);
            }

            return this.CharacterEntriesPool.Dequeue(accountID);
        }

        private PendingKeyedQueue<string, bool> NameCheckPool = new PendingKeyedQueue<string, bool>();

        public bool IsNameTaken(string name)
        {
            using (Packet outPacket = new Packet(InteroperabilityOperationCode.CharacterNameCheckRequest))
            {
                outPacket.WriteString(name);

                this.Send(outPacket);
            }

            return this.NameCheckPool.Dequeue(name);
        }

        private PendingKeyedQueue<int, byte[]> CharacterCreationPool = new PendingKeyedQueue<int, byte[]>();

        private void CreateCharacter(Packet inPacket)
        {
            int accountID = inPacket.ReadInt();
            byte[] characterData = inPacket.ReadBytes(inPacket.Remaining);

            this.CharacterCreationPool.Enqueue(accountID, characterData);
        }

        public byte[] CreateCharacter(byte worldID, int accountID, byte[] characterData)
        {
            using (Packet outPacket = new Packet(InteroperabilityOperationCode.CharacterCreationRequest))
            {
                outPacket.WriteByte(worldID);
                outPacket.WriteInt(accountID);
                outPacket.WriteBytes(characterData);

                this.Send(outPacket);
            }

            return this.CharacterCreationPool.Dequeue(accountID);
        }

        private PendingKeyedQueue<string, bool> MigrationPool = new PendingKeyedQueue<string, bool>();

        public bool Migrate(string host, int accountID, int characterID)
        {
            using (Packet outPacket = new Packet(InteroperabilityOperationCode.MigrationRegisterRequest))
            {
                outPacket
                    .WriteString(host)
                    .WriteInt(accountID)
                    .WriteInt(characterID);

                this.Send(outPacket);
            }

            return this.MigrationPool.Dequeue(host);
        }

        private void Migrate(Packet inPacket)
        {
            string host = inPacket.ReadString();
            bool valid = inPacket.ReadBool();

            this.MigrationPool.Enqueue(host, valid);
        }
    }
}
