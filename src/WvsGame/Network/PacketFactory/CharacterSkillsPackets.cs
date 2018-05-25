using Destiny.Maple;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network.PacketFactory
{
    public class CharacterSkillsPackets : PacketFactoryManager
    {
        public static Packet UpdateSkill(Skill skill)
        {
            Packet updateSkillPacket = new Packet(ServerOperationCode.ChangeSkillRecordResult);

            updateSkillPacket
                .WriteByte(1)
                .WriteShort(1)
                .WriteInt(skill.MapleID)
                .WriteInt(skill.CurrentLevel)
                .WriteInt(skill.MaxLevel)
                .WriteDateTime(skill.Expiration)
                .WriteByte(4);

            return updateSkillPacket;
        }
    }
}