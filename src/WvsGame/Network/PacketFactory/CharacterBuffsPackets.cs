using Destiny.Constants;
using Destiny.Maple;
using Destiny.Maple.Characters;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network.PacketFactory
{
    public class CharacterBuffsPackets : PacketFactoryManager
    {
        #region ShowMineBuffEffect
        public static Packet ShowMineBuffEffect(Character character, Skill skill)
        {
            Packet showMineBuffEffectPacket = new Packet(ServerOperationCode.Effect);

            showMineBuffEffectPacket
                .WriteInt(character.ID)
                .WriteInt(skill.MapleID)
                .WriteByte(0xA9) //??
                .WriteByte(1);   //??

            return showMineBuffEffectPacket;
        }
        #endregion

        #region  ShowRemoteBuffEffect
        public static Packet ShowRemoteBuffEffect(Character character, CharacterConstants.UserEffect effect, Skill skill, byte direction)
        {
            direction = 3; // TODO: fix this

            Packet showRemoteBuffEffectPacket = new Packet(ServerOperationCode.ShowRemoteEffect);

            showRemoteBuffEffectPacket
                .WriteInt(character.ID)
                .WriteByte((byte)effect) //buff level??
                .WriteInt(skill.MapleID)
                .WriteByte(direction)
                .WriteByte((byte)effect)
                .WriteByte(1); //??


            return showRemoteBuffEffectPacket;
        }
        #endregion

        #region ShowLocalBuff
        public static Packet ShowLocalBuffEffect(CharacterConstants.UserEffect effect)
        {
            Packet showLocalBuffEffectPacket = new Packet(ServerOperationCode.ShowRemoteEffect);

            showLocalBuffEffectPacket.WriteByte((byte)effect);

            return showLocalBuffEffectPacket;
        }
        #endregion

        #region ShowLocalBuff
        public static Packet ShowRemoteBuffEffect(CharacterConstants.UserEffect effect)
        {
            Packet showRemoteBuffEffectPacket = new Packet(ServerOperationCode.ShowRemoteEffect);

            showRemoteBuffEffectPacket.WriteByte((byte)effect);

            return showRemoteBuffEffectPacket;
        }
        #endregion
    }
}
