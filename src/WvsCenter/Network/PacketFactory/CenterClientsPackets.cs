using System.Collections.Generic;

using Destiny.Network.Common;

namespace Destiny.Network.PacketFactory
{
    public class CenterClientsPackets : PacketFactoryManager
    {
        public static Packet UpdateChannelPopulation(CenterClient centerClient, int population)
        {
            Packet updateChannelPopulationPacket = new Packet(InteroperabilityOperationCode.UpdateChannelPopulation);

            updateChannelPopulationPacket
                    .WriteByte(centerClient.World.ID)
                    .WriteByte(centerClient.ID)
                    .WriteInt(population);

            return updateChannelPopulationPacket;
        }

        public static Packet CharacterNameCheckRequest(string nameToCheck)
        {
            Packet characterNameCheckRequestPacket = new Packet(InteroperabilityOperationCode.CharacterNameCheckRequest);

            characterNameCheckRequestPacket.WriteString(nameToCheck);

            return characterNameCheckRequestPacket;
        }

        public static Packet CharacterNameCheckResponse(string nameToCheck, bool unusable)
        {
            Packet characterNameCheckResponsePacket = new Packet(InteroperabilityOperationCode.CharacterNameCheckResponse);

            characterNameCheckResponsePacket
                    .WriteString(nameToCheck)
                    .WriteBool(unusable);

            return characterNameCheckResponsePacket;
        }

        public static Packet CharacterEntriesRequest(int accountID)
        {
            Packet characterEntriesRequestPacket = new Packet(InteroperabilityOperationCode.CharacterEntriesRequest);

            characterEntriesRequestPacket.WriteInt(accountID);

            return characterEntriesRequestPacket;
        }

        public static Packet CharacterEntriesResponse(int accountID, List<byte[]> entires)
        {
            Packet characterEntriesResponsePacket = new Packet(InteroperabilityOperationCode.CharacterEntriesResponse);

            characterEntriesResponsePacket.WriteInt(accountID);

            foreach (var entry in entires)
            {
                characterEntriesResponsePacket.WriteByte((byte)entry.Length);
                characterEntriesResponsePacket.WriteBytes(entry);
            }

            return characterEntriesResponsePacket;
        }

        public static Packet CharacterCreationRequest(int accountID, byte[] characterData)
        {
            Packet characterCreationRequestPacket = new Packet(InteroperabilityOperationCode.CharacterCreationRequest);

            characterCreationRequestPacket
                .WriteInt(accountID)
                .WriteBytes(characterData);

            return characterCreationRequestPacket;
        }

        public static Packet CharacterCreationResponse(int accountID, byte[] characterData)
        {
            Packet characterCreationResponsePacket = new Packet(InteroperabilityOperationCode.CharacterCreationRequest);

            characterCreationResponsePacket
                .WriteInt(accountID)
                .WriteBytes(characterData);

            return characterCreationResponsePacket;
        }

    }
}