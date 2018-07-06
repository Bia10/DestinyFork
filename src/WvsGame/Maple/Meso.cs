using Destiny.Constants;
using Destiny.Maple.Characters;
using Destiny.Maple.Maps;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple
{
    public sealed class Meso : Drop
    {
        public int Amount { get; }

        public Meso(int amount) : base()
        {
            Amount = amount;
        }

        public const int mesoLimit = int.MaxValue;

        public static void giveMesos(Character character, int mesosGiven)
        {
            long myPlusGiven = (long)character.Stats.Meso + mesosGiven;  

            if (myPlusGiven > mesoLimit)
            {
                character.Stats.Meso = mesoLimit;
            }

            else
            {
                character.Stats.Meso += mesosGiven;
                Packet showMesosGain = GetShowMesoGainPacket(true, mesosGiven, false);
                character.Client.Send(showMesosGain);
            }                   
        }

        public override Packet GetShowGainPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.Message);

            oPacket
                .WriteByte((byte)ServerConstants.MessageType.DropPickup)
                .WriteBool(true)
                .WriteByte() // NOTE: Unknown.
                .WriteInt(Amount)
                .WriteShort();

            return oPacket;
        }

        public static Packet GetShowMesoGainPacket(bool white, int ammount, bool inChat)
        {
            return Character.GetShowSidebarInfoPacket(ServerConstants.MessageType.DropPickup, white, 0, ammount, inChat, 0, 0);
        }

    }
}