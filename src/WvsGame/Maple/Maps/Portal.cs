using Destiny.Constants;
using Destiny.Data;
using Destiny.Maple.Characters;
using Destiny.Maple.Data;
using Destiny.IO;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Maps
{
    public sealed class Portal : MapObject
    {
        public byte ID { get; private set; }
        public string Label { get; private set; }
        public int DestinationMapID { get; private set; }
        public string DestinationLabel { get; private set; }
        public string Script { get; private set; }

        public bool IsSpawnPoint
        {
            get
            {
                return Label == "sp";
            }
        }

        public Map DestinationMap
        {
            get
            {
                return DataProvider.Maps[DestinationMapID];
            }
        }

        public Portal Link
        {
            get
            {
                return DataProvider.Maps[DestinationMapID].Portals[DestinationLabel];
            }
        }

        public Portal(Datum datum)
        {
            ID = (byte)(int)datum["id"];
            Label = (string)datum["label"];
            Position = new Point((short)datum["x_pos"], (short)datum["y_pos"]);
            DestinationMapID = (int)datum["destination"];
            DestinationLabel = (string)datum["destination_label"];
            Script = (string)datum["script"];
        }

        public void Enter(Character character)
        {
            Log.Warn("'{0}' attempted to enter an unimplemented portal '{1}'.", character.Name, Script);

            using (Packet oPacket = new Packet(ServerOperationCode.TransferFieldReqInogred))
            {
                oPacket.WriteByte((byte)MapConstants.MapTransferResult.NoReason);

                character.Client.Send(oPacket);
            }
        }

        public void PlaySoundEffect(Character character)
        {
            CharacterBuffs.ShowLocalUserEffect(character, CharacterConstants.UserEffect.PlayPortalSE);
        }

        public void ShowBalloonMessage(Character character, string text, short width, short height)
        {
            using (Packet oPacket = new Packet(ServerOperationCode.BalloonMsg))
            {
                oPacket
                    .WriteString(text)
                    .WriteShort(width)
                    .WriteShort(height)
                    .WriteByte(1);

                character.Client.Send(oPacket);
            }
        }

        public void ShowTutorialMessage(Character character, string dataPath)
        {
            using (Packet oPacket = new Packet(ServerOperationCode.Effect))
            {
                oPacket
                    .WriteByte((byte)CharacterConstants.UserEffect.AvatarOriented)
                    .WriteString(dataPath)
                    .WriteInt(1);

                character.Client.Send(oPacket);
            }
        }
    }
}
