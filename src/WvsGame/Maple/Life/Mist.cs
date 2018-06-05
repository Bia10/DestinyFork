using Destiny.Constants;
using Destiny.Maple.Characters;
using Destiny.Maple.Maps;
using Destiny.Network;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Life
{
    public sealed class Mist : MapObject, ISpawnable
    {
        private Rectangle mistPosition { get; set; }
        public Character mistOwner { get; set; }
        public Skill mistSkill { get; set; }

        public MapleMapObjectType getType()
        {
            return MapleMapObjectType.Mist;
        }

        public enum MistType : int
        {
            mobMist = 0,
            playerPoisonMist = 1,
            playerSmokescreen = 2,
            unknown = 3, 
            recoveryMist = 4
            // flame gear
            // poison bomb
        }

        public MistType mistType { get; set; }

        public MistType getMistType()
        {
            switch (mistSkill.MapleID)
            {
                case (int) CharacterConstants.SkillNames.FirePoisonMage.PoisonMist:
                    return MistType.playerPoisonMist;

                case (int) CharacterConstants.SkillNames.Shadower.Smokescreen:
                    return MistType.playerSmokescreen;
            }

            return MistType.mobMist;
        }

        public Mist(Rectangle boundingBox, Character character, Skill skill)
        {
            ObjectID = ObjectID;
            mistSkill = skill;
            mistType = getMistType();
            mistOwner = character;          
            mistPosition = boundingBox;
        }

        public Mist MistObject(Rectangle boundingBox, Character character, Skill skill)
        {
            ObjectID = ObjectID;
            mistSkill = skill;
            mistType = getMistType();
            mistOwner = character;
            mistPosition = boundingBox;

            return this;
        }

        public static void SpawnMist(GameClient client, Mist mistToSpawn)
        {
            Packet mistPacket = mistToSpawn.GetCreatePacket();

            client.Send(mistPacket);
        }

        public Packet GetCreatePacket()
        {
            return GetSpawnPacket();
        }

        public Packet GetSpawnPacket()
        {
            return GetInternalPacket();
        }

        private Packet GetInternalPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.AffectedAreaCreated);

                    oPacket
                        .WriteInt(ObjectID)
                        .WriteInt((int)mistType) 
                        .WriteInt(mistOwner.ID)
                        .WriteInt(mistSkill.MapleID)
                        .WriteByte(mistSkill.CurrentLevel)
                        .WriteShort((short)mistSkill.Cooldown)
                        .WriteInt(mistPosition.RightBottom.X)
                        .WriteInt(mistPosition.RightBottom.Y)
                        .WriteInt(mistPosition.RightBottom.X + mistPosition.LeftTop.Y)
                        .WriteInt(mistPosition.RightBottom.Y + mistPosition.LeftTop.Y)
                        .WriteInt(0); // ???
                    
            return oPacket;
        }

        public Packet GetDestroyPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.AffectedAreaRemoved);

            oPacket.WriteInt(ObjectID);

            return oPacket;
        }

    }
}