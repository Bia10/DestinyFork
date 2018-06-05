using System;
using System.Collections.Generic;
using System.Net.Sockets;

using Destiny.Constants;
using Destiny.Maple;
using Destiny.IO;
using Destiny.Security;
using Destiny.Network.ClientHandler;
using Destiny.Network.Common;
using Destiny.Network.PacketFactory;

namespace Destiny.Network
{
    public sealed class CenterClient : ClientHandler<InteroperabilityOperationCode, InteroperabilityOperationCode, BlankCryptograph>
    {
        public static string SecurityCode { get; set; }

        public ServerConstants.ServerType Type { get; private set; }

        public World World { get; private set; }
        public byte ID { get; set; }
        public ushort Port { get; set; }
        public int Population { get; private set; }

        public CenterClient(Socket socket) : base(socket) { }

        protected override bool IsServerAlive
        {
            get
            {
                return WvsCenter.IsAlive;
            }
        }

        /*protected override void Prepare(params object[] args)
        {
            base.Prepare(args);
        }*/

        protected override void Register()
        {
            WvsCenter.Clients.Add(this);
        }

        protected override void Terminate()
        {
            switch (Type)
            {
                case ServerConstants.ServerType.Login:
                    {
                        WvsCenter.Login = null;

                        Log.SkipLine();
                        Log.Warn("Unregistered Login Server.");
                        Log.SkipLine();
                    }
                    break;

                case ServerConstants.ServerType.Channel:
                    {
                        World.Remove(this);

                        using (Packet Packet = new Packet(InteroperabilityOperationCode.UpdateChannel))
                        {
                            Packet.WriteByte(World.ID);
                            Packet.WriteBool(false);
                            Packet.WriteByte(ID);

                            WvsCenter.Login?.Send(Packet);
                        }
                        Log.SkipLine();
                        Log.Warn("Unregistered Channel Server ({0}-{1}).", World.Name, (ID + 1));
                        Log.SkipLine();
                    }
                    break;

                case ServerConstants.ServerType.Shop:
                    {
                        World.Shop = null;

                        Log.SkipLine();
                        Log.Warn("Unregistered Shop Server ({0}).", World.Name);
                        Log.SkipLine();

                        break;
                    }

                case ServerConstants.ServerType.None:
                    break;

                case ServerConstants.ServerType.ITC:
                    break;

                default:
                    Log.SkipLine();
                    Log.Warn(" Unhandled Termination request!" +
                             " \n Argument: {0}", Type);
                    Log.SkipLine();
                    break;
            }
        }

        protected override void Unregister()
        {
            WvsCenter.Clients.Remove(this);
        }

        protected override void Dispatch(Packet inPacket)
        {
            switch ((InteroperabilityOperationCode)inPacket.OperationCode)
            {
                case InteroperabilityOperationCode.RegistrationRequest:
                    Register(inPacket);
                    break;

                case InteroperabilityOperationCode.UpdateChannelPopulation:
                    UpdateChannelPopulation(inPacket);
                    break;

                case InteroperabilityOperationCode.CharacterNameCheckRequest:
                    CharacterNameCheckRequest(inPacket);
                    break;

                case InteroperabilityOperationCode.CharacterNameCheckResponse:
                    CharacterNameCheckResponse(inPacket);
                    break;

                case InteroperabilityOperationCode.CharacterEntriesRequest:
                    CharacterEntriesRequest(inPacket);
                    break;

                case InteroperabilityOperationCode.CharacterEntriesResponse:
                    CharacterEntiresResponse(inPacket);
                    break;

                case InteroperabilityOperationCode.CharacterCreationRequest:
                    CharacterCreationRequest(inPacket);
                    break;

                case InteroperabilityOperationCode.CharacterCreationResponse:
                    CharacterCreationResponse(inPacket);
                    break;

                case InteroperabilityOperationCode.MigrationRegisterRequest:
                    Migrate(inPacket);
                    break;

                case InteroperabilityOperationCode.MigrationRequest:
                    MigrateRequest(inPacket);
                    break;

                case InteroperabilityOperationCode.ChannelPortRequest:
                    ChannelPortRequest(inPacket);
                    break;

                case InteroperabilityOperationCode.RegistrationResponse:
                    break;
                case InteroperabilityOperationCode.UpdateChannel:
                    break;
                case InteroperabilityOperationCode.UpdateChannelID:
                    break;
                case InteroperabilityOperationCode.MigrationRegisterResponse:
                    break;
                case InteroperabilityOperationCode.MigrationResponse:
                    break;
                case InteroperabilityOperationCode.ChannelPortResponse:
                    break;

                default:
                    Log.SkipLine();
                    Log.Warn(" Unhandled InteroperabilityOperationCode encountered!" +
                             " \n Argument: {0}", inPacket.OperationCode);
                    Log.SkipLine();
                    break;
            }
        }

        private void Register(Packet inPacket)
        {
            ServerConstants.ServerType type = (ServerConstants.ServerType)inPacket.ReadByte();
            string securityCode = inPacket.ReadString();

            bool valid = true;

            using (Packet Packet = new Packet(InteroperabilityOperationCode.RegistrationResponse))
            {
                if (!Enum.IsDefined(typeof(ServerConstants.ServerType), type))
                {
                    Packet.WriteByte((byte)ServerRegsitrationResponse.InvalidType);

                    valid = false;
                }
                else if (securityCode != CenterClient.SecurityCode)
                {
                    Packet.WriteByte((byte)ServerRegsitrationResponse.InvalidCode);

                    valid = false;
                }
                else
                {
                    switch (type)
                    {
                        case ServerConstants.ServerType.Login:
                            {
                                if (WvsCenter.Login != null)
                                {
                                    Packet.WriteByte((byte)ServerRegsitrationResponse.Full);

                                    valid = false;
                                }
                                else
                                {
                                    Packet.WriteByte((byte)ServerRegsitrationResponse.Valid);

                                    WvsCenter.Login = this;
                                }
                            }
                            break;

                        case ServerConstants.ServerType.Channel:
                        case ServerConstants.ServerType.Shop:
                            {
                                World world = WvsCenter.Worlds.Next(type);

                                if (world == null)
                                {
                                    Packet.WriteByte((byte)ServerRegsitrationResponse.Full);

                                    valid = false;
                                }
                                else
                                {
                                    World = world;

                                    switch (type)
                                    {
                                        case ServerConstants.ServerType.Channel:
                                            World.Add(this);
                                            break;

                                        case ServerConstants.ServerType.Shop:
                                            World.Shop = this;
                                            break;
                                    }

                                    Packet.WriteByte((byte)ServerRegsitrationResponse.Valid);
                                    Packet.WriteByte(World.ID);
                                    Packet.WriteString(World.Name);

                                    if (type == ServerConstants.ServerType.Channel)
                                    {
                                        Packet.WriteString(World.TickerMessage);
                                        Packet.WriteByte(ID);
                                    }

                                    Packet.WriteUShort(Port);

                                    if (type == ServerConstants.ServerType.Channel)
                                    {
                                        Packet.WriteBool(World.AllowMultiLeveling);
                                        Packet.WriteInt(World.ExperienceRate);
                                        Packet.WriteInt(World.QuestExperienceRate);
                                        Packet.WriteInt(World.PartyQuestExperienceRate);
                                        Packet.WriteInt(World.MesoRate);
                                        Packet.WriteInt(World.DropRate);
                                    }
                                }
                            }
                            break;

                        case ServerConstants.ServerType.None:
                            break;

                        case ServerConstants.ServerType.ITC:
                            break;
                        default:
                            Log.SkipLine();
                            Log.Warn(" Unhandled Registration request!" +
                                     " \n Argument: {0}", Type);
                            Log.SkipLine();
                            break;
                    }
                }

                Send(Packet);
            }

            if (valid)
            {
                Type = type;

                switch (type)
                {
                    case ServerConstants.ServerType.Login:
                        {
                            byte count = inPacket.ReadByte();

                            for (byte b = 0; b < count; b++)
                            {
                                if (WvsCenter.Worlds.Contains(b))
                                {
                                    continue;
                                }

                                WvsCenter.Worlds.Add(new World(inPacket));
                            }

                            foreach (World loopWorld in WvsCenter.Worlds)
                            {
                                foreach (CenterClient loopChannel in loopWorld)
                                {
                                    using (Packet Packet = new Packet(InteroperabilityOperationCode.UpdateChannel))
                                    {
                                        Packet.WriteByte(loopChannel.World.ID);
                                        Packet.WriteBool(true);
                                        Packet.WriteByte(loopChannel.ID);
                                        Packet.WriteUShort(loopChannel.Port);
                                        Packet.WriteInt(loopChannel.Population);

                                        WvsCenter.Login.Send(Packet);
                                    }
                                }
                            }
                            Log.SkipLine();
                            Log.Success("Registered Login Server.");
                            Log.SkipLine();
                        }
                        break;

                    case ServerConstants.ServerType.Channel:
                        {
                            using (Packet Packet = new Packet(InteroperabilityOperationCode.UpdateChannel))
                            {
                                Packet.WriteByte(World.ID);
                                Packet.WriteBool(true);
                                Packet.WriteByte(ID);
                                Packet.WriteUShort(Port);
                                Packet.WriteInt(Population);

                                WvsCenter.Login.Send(Packet);
                            }
                            Log.SkipLine();
                            Log.Success("Registered Channel Server ({0}-{1}).", World.Name, (ID + 1));
                            Log.SkipLine();
                        }
                        break;

                    case ServerConstants.ServerType.Shop:
                        {
                            Log.SkipLine();
                            Log.Success("Registered Shop Server ({0}).", World.Name);
                            Log.SkipLine();
                        }
                        break;
                }
            }
        }

        private void UpdateChannelPopulation(Packet inPacket)
        {
            int population = inPacket.ReadInt();

            WvsCenter.Login.Send(CenterClientsPackets.UpdateChannelPopulation(this, population));
        }

        private static void CharacterNameCheckRequest(Packet inPacket)
        {
            string name = inPacket.ReadString();

            WvsCenter.Worlds[0][0].Send(CenterClientsPackets.CharacterNameCheckRequest(name));
        }

        private static void CharacterNameCheckResponse(Packet inPacket)
        {
            string name = inPacket.ReadString();
            bool unusable = inPacket.ReadBool();

            WvsCenter.Login.Send(CenterClientsPackets.CharacterNameCheckResponse(name, unusable));
        }

        private static void CharacterEntriesRequest(Packet inPacket)
        {
            byte worldID = inPacket.ReadByte();
            int accountID = inPacket.ReadInt();
        
            WvsCenter.Worlds[worldID].RandomChannel.Send(CenterClientsPackets.CharacterEntriesRequest(accountID));
        }

        private static void CharacterEntiresResponse(Packet inPacket)
        {
            int accountID = inPacket.ReadInt();
            List<byte[]> entires = new List<byte[]>();

            while (inPacket.Remaining > 0)
            {
                entires.Add(inPacket.ReadBytes(inPacket.ReadByte()));
            }

            WvsCenter.Login.Send(CenterClientsPackets.CharacterEntriesResponse(accountID, entires));
        }

        private static void CharacterCreationRequest(Packet inPacket)
        {
            byte worldID = inPacket.ReadByte();
            int accountID = inPacket.ReadInt();
            byte[] characterData = inPacket.ReadBytes(inPacket.Remaining);

            WvsCenter.Worlds[worldID].RandomChannel.Send(CenterClientsPackets.CharacterCreationRequest(accountID, characterData));
        }

        private static void CharacterCreationResponse(Packet inPacket)
        {
            int accountID = inPacket.ReadInt();
            byte[] characterData = inPacket.ReadBytes(inPacket.Remaining);

            WvsCenter.Login.Send(CenterClientsPackets.CharacterCreationResponse(accountID, characterData));
        }

        private void Migrate(Packet inPacket)
        {
            string host = inPacket.ReadString();
            int accountID = inPacket.ReadInt();
            int characterID = inPacket.ReadInt();

            bool valid;

            if (WvsCenter.Migrations.Contains(host))
            {
                valid = false;
            }

            else
            {
                valid = true;

                WvsCenter.Migrations.Add(new Migration(host, accountID, characterID));
            }

            using (Packet outPacket = new Packet(InteroperabilityOperationCode.MigrationRegisterResponse))
            {
                outPacket
                    .WriteString(host)
                    .WriteBool(valid);

                Send(outPacket);
            }
        }

        private void MigrateRequest(Packet inPacket)
        {
            string host = inPacket.ReadString();
            int characterID = inPacket.ReadInt();

            int accountID =  WvsCenter.Migrations.Validate(host, characterID);

            using (Packet outPacket = new Packet(InteroperabilityOperationCode.MigrationResponse))
            {
                outPacket
                    .WriteString(host)
                    .WriteInt(accountID);

                Send(outPacket);
            }
        }

        private void ChannelPortRequest(Packet inPacket)
        {
            byte id = inPacket.ReadByte();

            using (Packet outPacket = new Packet(InteroperabilityOperationCode.ChannelPortResponse))
            {
                outPacket.WriteByte(id);
                outPacket.WriteUShort(World[id].Port);

                Send(outPacket);
            }
        }
    }
}
