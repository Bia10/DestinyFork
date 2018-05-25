using System;
using System.Collections.Generic;

using Destiny.Constants;
using Destiny.Maple;
using Destiny.Maple.Characters;
using Destiny.Maple.Life;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Network.PacketFactory
{
    public class CharacterPackets : PacketFactoryManager
    {
        #region GenderPackets
        public static Packet SetGenderPacket(CharacterConstants.Gender gender)
        {
            Packet setGenderPacket = new Packet(ServerOperationCode.SetGender);
            setGenderPacket.WriteByte((byte)gender);

            return setGenderPacket;
        }

        public static Packet SetGenderPacket(byte gender)
        {
            Packet setGenderPacket = new Packet(ServerOperationCode.SetGender);
            setGenderPacket.WriteByte(gender);

            return setGenderPacket;
        }
        #endregion

        #region ChalkboardPackets
        public static Packet SetChalkboardPacket(Character character)
        {
            Packet setChalkboardPacket = new Packet(ServerOperationCode.Chalkboard);

            setChalkboardPacket
                .WriteInt(character.ID)
                .WriteBool(!string.IsNullOrEmpty(character.Chalkboard))
                .WriteString(character.Chalkboard);

            return setChalkboardPacket;
        }
        #endregion

        #region  ApperancePackets
        public static Packet UpdateApperancePacket(Character character)
        {
            Packet setApperancePacket = new Packet(ServerOperationCode.AvatarModified);

            setApperancePacket
                    .WriteInt(character.ID)
                    .WriteBool(true)
                    .WriteBytes(character.AppearanceToByteArray())
                    .WriteByte()
                    .WriteShort();

            return setApperancePacket;
        }
        #endregion

        #region  UpdateStatsPackets
        public static Packet UpdateStatsPacket(Character character, params CharacterConstants.StatisticType[] charStats)
        {
            Packet setStatsPacket = new Packet(ServerOperationCode.StatChanged);

            setStatsPacket.WriteBool(true); // TODO: bOnExclRequest.

            int flag = 0;

            foreach (CharacterConstants.StatisticType statistic in charStats)
            {
                flag |= (int)statistic;
            }

            setStatsPacket.WriteInt(flag);

            Array.Sort(charStats);

            foreach (CharacterConstants.StatisticType statistic in charStats)
            {
                switch (statistic)
                {
                    case CharacterConstants.StatisticType.Skin:
                        setStatsPacket.WriteByte(character.Skin);
                        break;

                    case CharacterConstants.StatisticType.Face:
                        setStatsPacket.WriteInt(character.Face);
                        break;

                    case CharacterConstants.StatisticType.Hair:
                        setStatsPacket.WriteInt(character.Hair);
                        break;

                    case CharacterConstants.StatisticType.Level:
                        setStatsPacket.WriteByte(character.Stats.Level);
                        break;

                    case CharacterConstants.StatisticType.Job:
                        setStatsPacket.WriteShort((short)character.Job);
                        break;

                    case CharacterConstants.StatisticType.Strength:
                        setStatsPacket.WriteShort(character.Stats.Strength);
                        break;

                    case CharacterConstants.StatisticType.Dexterity:
                        setStatsPacket.WriteShort(character.Stats.Dexterity);
                        break;

                    case CharacterConstants.StatisticType.Intelligence:
                        setStatsPacket.WriteShort(character.Stats.Intelligence);
                        break;

                    case CharacterConstants.StatisticType.Luck:
                        setStatsPacket.WriteShort(character.Stats.Luck);
                        break;

                    case CharacterConstants.StatisticType.Health:
                        setStatsPacket.WriteShort(character.Stats.Health);
                        break;

                    case CharacterConstants.StatisticType.MaxHealth:
                        setStatsPacket.WriteShort(character.Stats.MaxHealth);
                        break;

                    case CharacterConstants.StatisticType.Mana:
                        setStatsPacket.WriteShort(character.Stats.Mana);
                        break;

                    case CharacterConstants.StatisticType.MaxMana:
                        setStatsPacket.WriteShort(character.Stats.MaxMana);
                        break;

                    case CharacterConstants.StatisticType.AbilityPoints:
                        setStatsPacket.WriteShort(character.Stats.AbilityPoints);
                        break;

                    case CharacterConstants.StatisticType.SkillPoints:
                        setStatsPacket.WriteShort(character.Stats.SkillPoints);
                        break;

                    case CharacterConstants.StatisticType.Experience:
                        setStatsPacket.WriteInt(character.Stats.Experience);
                        break;

                    case CharacterConstants.StatisticType.Fame:
                        setStatsPacket.WriteShort(character.Stats.Fame);
                        break;

                    case CharacterConstants.StatisticType.Mesos:
                        setStatsPacket.WriteInt(character.Stats.Meso);
                        break;
                }
            }

            return setStatsPacket;
        }
        #endregion

        #region InitializePackets
        public static Packet InitializeCharacterSetFieldPacket(Character character)
        {
            Packet setFieldPacket = new Packet(ServerOperationCode.SetField);

            setFieldPacket
                .WriteInt(WvsGame.ChannelID)
                .WriteByte(++character.Portals)
                .WriteBool(true)
                .WriteShort(); // NOTE: Floating messages at top corner.

            for (int i = 0; i < 3; i++)
            {
                setFieldPacket.WriteInt(Application.Random.Next());
            }

            setFieldPacket
                .WriteBytes(character.DataToByteArray())
                .WriteDateTime(DateTime.UtcNow);

            return setFieldPacket;
        }

        public static Packet InitializeCharacterSrvrStatusChng()
        {
            Packet srvrStatusChng = new Packet(ServerOperationCode.ClaimSvrStatusChanged);

            srvrStatusChng.WriteBool(true);

            return srvrStatusChng;
        }
        #endregion

        #region MessagePackets
        public static Packet BroadcastMessage(ServerConstants.NoticeType messageType, string message)
        {
            Packet broadcastMessage = new Packet(ServerOperationCode.BroadcastMsg);

            broadcastMessage
                .WriteByte((byte)messageType);

            if (messageType == ServerConstants.NoticeType.ScrollingText)
            {
                broadcastMessage.WriteBool(!string.IsNullOrEmpty(message));
            }

            broadcastMessage.WriteString(message);

            return broadcastMessage;
        }
        #endregion

        #region StoragePackets
        public static Packet StorageErrorPacket(Character character, NPCsConstants.StoragePacketType type)
        {
            Packet storageErrorPacket = new Packet(ServerOperationCode.Storage);

            storageErrorPacket.WriteByte((byte)type);

            return storageErrorPacket;
        }

        public static Packet StorageRemoveItem(Character character, Item itemToWithdraw, List<Item> itemsByType)
        {
            Packet storageRemovePacket = new Packet(ServerOperationCode.Storage);

            storageRemovePacket
                .WriteByte((byte)NPCsConstants.StorageAction.WithdrawItem)
                .WriteByte(character.Storage.Slots)
                .WriteShort((short)(2 << (byte)itemToWithdraw.Type)) // ??
                .WriteShort() // ??
                .WriteInt()   // ??
                .WriteByte((byte)itemsByType.Count);

            foreach (Item loopItem in itemsByType)
            {
                storageRemovePacket.WriteBytes(loopItem.ToByteArray(true, true));
            }

            return storageRemovePacket;
        }

        public static Packet StorageAddItem(Character character, Item itemToDeposit, List<Item> itemsByType)
        {
            Packet storageAddItem = new Packet(ServerOperationCode.Storage);

            storageAddItem
                .WriteByte((byte)NPCsConstants.StorageAction.DepositItem)
                .WriteByte(character.Storage.Slots)
                .WriteShort((short)(2 << (byte)itemToDeposit.Type)) //  ??
                .WriteShort() // ??
                .WriteInt()   // ??
                .WriteByte((byte)itemsByType.Count);

            foreach (Item loopItem in itemsByType)
            {
                storageAddItem.WriteBytes(loopItem.ToByteArray(true, true));
            }

            return storageAddItem;
        }

        public static Packet StorageArrangeItems(Character character)
        {
            Packet storageArrangePacket = new Packet(ServerOperationCode.Storage);

            storageArrangePacket
                .WriteByte((byte)NPCsConstants.StorageAction.ChangeMesos)
                .WriteByte(character.Storage.Slots)
                .WriteShort(2) // ??
                .WriteShort() // ??
                .WriteInt()   // ??
                .WriteInt(character.Storage.Meso);

            return storageArrangePacket;
        }

        public static Packet StorageOpen(Npc storageNpc, CharacterStorage charStorage)
        {
            Packet storageOpenPacket = new Packet(ServerOperationCode.Storage);

            storageOpenPacket
                .WriteByte((byte)NPCsConstants.StorageAction.OpenStorage)
                .WriteInt(storageNpc.MapleID)
                .WriteByte(charStorage.Slots)
                .WriteShort(126)
                .WriteShort()
                .WriteInt()
                .WriteInt(charStorage.Meso)
                .WriteShort()
                .WriteByte((byte)charStorage.Items.Count);

            foreach (Item item in charStorage.Items)
            {
                storageOpenPacket.WriteBytes(item.ToByteArray(true, true));
            }

            storageOpenPacket
                .WriteShort()
                .WriteByte();

            return storageOpenPacket;
        }

        public static Packet StorageChangeMesos(Character character)
        {
            Packet storageChangeMesosPacket = new Packet(ServerOperationCode.Storage);

            storageChangeMesosPacket
                .WriteByte((byte)NPCsConstants.StoragePacketType.UpdateMesos)
                .WriteByte(character.Storage.Slots)
                .WriteShort(2)
                .WriteShort()
                .WriteInt()
                .WriteInt(character.Storage.Meso);

            return storageChangeMesosPacket;
        }
        #endregion

        #region ChangeMapPackets
        public static Packet ChangeMap(Character character, int mapID, byte portalID = 0, bool fromPosition = false, Point position = null)
        {
            Packet characterChangeMapPacket = new Packet(ServerOperationCode.SetField);

            characterChangeMapPacket
                .WriteInt(WvsGame.ChannelID)
                .WriteByte(++character.Portals)
                .WriteBool(false)
                .WriteShort()
                .WriteByte()
                .WriteInt(mapID)
                .WriteByte(character.SpawnPoint)
                .WriteShort(character.Stats.Health)
                .WriteBool(fromPosition);

            if (fromPosition)
            {
                characterChangeMapPacket
                    .WriteShort(position.X)
                    .WriteShort(position.Y);
            }

            characterChangeMapPacket.WriteDateTime(DateTime.Now);

            return characterChangeMapPacket;
        }
        #endregion

        #region MoveCharacter
        public static Packet MoveCharacter(Character character, Movements movements)
        {
            Packet moveCharacterPacket = new Packet(ServerOperationCode.Move);

            moveCharacterPacket
                .WriteInt(character.ID)
                .WriteBytes(movements.ToByteArray());

            return moveCharacterPacket;
        }
        #endregion

        #region CharacterSit
        public static Packet SitOnChair(Character character)
        {
            Packet characterSitOnChairPacket = new Packet(ServerOperationCode.Sit);

            characterSitOnChairPacket
                .WriteInt(character.ID)
                .WriteInt();

            return characterSitOnChairPacket;
        }

        public static Packet ShowChair(Character character, int chairID)
        {
            Packet characterShowChairPacket = new Packet(ServerOperationCode.SetActiveRemoteChair);

            characterShowChairPacket
                .WriteInt(character.ID) //CharID
                .WriteInt(chairID);     //ChairID

            return characterShowChairPacket;
        }
        #endregion

        #region CharacterEmotion
        public static Packet ExpressEmotion(Character character, int emotionID)
        {
            Packet characterExpressEmotion = new Packet(ServerOperationCode.Emotion);

            characterExpressEmotion
                .WriteInt(character.ID)
                .WriteInt(emotionID);

            return characterExpressEmotion;
        }
        #endregion

        #region CharacterTalkTo
        public static Packet TalkToCharacter(Character character, string text, bool isShout)
        {
            Packet characterTalkTo = new Packet(ServerOperationCode.UserChat);

            characterTalkTo
                .WriteInt(character.ID)
                .WriteBool(character.IsMaster)
                .WriteString(text)
                .WriteBool(isShout);

            return characterTalkTo;
        }
        #endregion

        #region CharacterTalkToGroup
        public static Packet TalkToCharacterGroup(SocialConstants.MultiChatType type, Character character, string text)
        {
            Packet characterTalkToGroup = new Packet(ServerOperationCode.GroupMessage);

            characterTalkToGroup
                .WriteByte((byte)type)
                .WriteString(character.Name)
                .WriteString(text);

            return characterTalkToGroup;
        }
        #endregion

        #region Attack
        #endregion

        #region Damage
        #endregion

    }
}