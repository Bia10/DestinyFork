using System;
using System.Collections.Generic;

using Destiny.Data;
using Destiny.Maple.Data;
using Destiny.Constants;
using Destiny.Maple.Life;
using Destiny.IO;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterQuests
    {
        public Character Parent { get; private set; }

        public Dictionary<ushort, Dictionary<int, short>> Started { get; private set; }
        public Dictionary<ushort, DateTime> Completed { get; private set; }

        public CharacterQuests(Character parent)
        {
            Parent = parent;

            Started = new Dictionary<ushort, Dictionary<int, short>>();
            Completed = new Dictionary<ushort, DateTime>();
        }

        public void Load()
        {
            foreach (Datum datum in new Datums("quests_started").Populate("CharacterID = {0}", Parent.ID))
            {
                if (!Started.ContainsKey((ushort)datum["QuestID"]))
                {
                    Started.Add((ushort)datum["QuestID"], new Dictionary<int, short>());
                }

                if (datum["MobID"] != null && datum["Killed"] != null)
                {
                    Started[(ushort)datum["QuestID"]].Add((int)datum["MobID"], ((short)datum["Killed"]));
                }
            }
        }

        public void Save()
        {
            foreach (KeyValuePair<ushort, Dictionary<int, short>> loopStarted in Started)
            {
                if (loopStarted.Value == null || loopStarted.Value.Count == 0)
                {
                    Datum datum = new Datum("quests_started")
                    {
                        ["CharacterID"] = Parent.ID,
                        ["QuestID"] = loopStarted.Key
                    };

                    if (!Database.Exists("quests_started", "CharacterID = {0} && QuestID = {1}", Parent.ID, loopStarted.Key))
                    {
                        datum.Insert();
                    }
                }

                else
                {
                    foreach (KeyValuePair<int, short> mobKills in loopStarted.Value)
                    {
                        Datum datum = new Datum("quests_started")
                        {
                            ["CharacterID"] = Parent.ID,
                            ["QuestID"] = loopStarted.Key,
                            ["MobID"] = mobKills.Key,
                            ["Killed"] = mobKills.Value
                        };

                        if (Database.Exists("quests_started", "CharacterID = {0} && QuestID = {1} && MobID = {2}", Parent.ID, loopStarted.Key, mobKills.Key))
                        {
                            datum.Update("CharacterID = {0} && QuestID = {1} && MobID = {2}", Parent.ID, loopStarted.Key, mobKills.Key);
                        }

                        else
                        {
                            datum.Insert();
                        }
                    }
                }
            }

            foreach (KeyValuePair<ushort, DateTime> loopCompleted in Completed)
            {
                Datum datum = new Datum("quests_completed")
                {
                    ["CharacterID"] = Parent.ID,
                    ["QuestID"] = loopCompleted.Key,
                    ["CompletionTime"] = loopCompleted.Value
                };

                if (Database.Exists("quests_completed", "CharacterID = {0} && QuestID = {1}", Parent.ID, loopCompleted.Key))
                {
                    datum.Update("CharacterID = {0} && QuestID = {1}", Parent.ID, loopCompleted.Key);
                }

                else
                {
                    datum.Insert();
                }
            }
        }

        public void Delete(ushort questID)
        {
            if (Started.ContainsKey(questID))
            {
                Started.Remove(questID);
            }

            if (Database.Exists("quests_started", "QuestID = {0}", questID))
            {
                Database.Delete("quests_started", "QuestID = {0}", questID);
            }
        }

        public void Delete()
        {

        }

        public void Handle(Packet iPacket)
        {
            QuestConstants.QuestAction action = (QuestConstants.QuestAction)iPacket.ReadByte();
            ushort questID = iPacket.ReadUShort();

            if (!DataProvider.Quests.Contains(questID))
            {
                return;
            }

            Quest quest = DataProvider.Quests[questID];

            int npcId;

            switch (action)
            {
                case QuestConstants.QuestAction.RestoreLostItem: // TODO: Validate.
                    {
                        int quantity = iPacket.ReadInt();
                        int itemID = iPacket.ReadInt();

                        quantity -= Parent.Items.InventoryAvailableItemByID(itemID);

                        Item item = new Item(itemID, (short)quantity);

                        Parent.Items.AddItemToInventory(item);
                    }
                    break;

                case QuestConstants.QuestAction.Start:
                    {
                        npcId = iPacket.ReadInt();

                        Start(quest, npcId);
                    }
                    break;

                case QuestConstants.QuestAction.Complete:
                    {
                        npcId = iPacket.ReadInt();
                        iPacket.ReadInt(); // NOTE: Unknown
                        int selection = iPacket.Remaining >= 4 ? iPacket.ReadInt() : 0;

                        Complete(quest, selection);
                    }
                    break;

                case QuestConstants.QuestAction.Forfeit:
                    {
                        Forfeit(quest.MapleID);
                    }
                    break;

                case QuestConstants.QuestAction.ScriptStart:
                case QuestConstants.QuestAction.ScriptEnd:
                    {
                        npcId = iPacket.ReadInt();

                        Npc npc = null;

                        foreach (Npc loopNpc in Parent.Map.Npcs)
                        {
                            if (loopNpc.MapleID == npcId)
                            {
                                npc = loopNpc;

                                break;
                            }
                        }

                        if (npc == null)
                        {
                            return;
                        }

                        Parent.Converse(npc, quest);
                    }
                    break;
            }
        }

        public void Start(Quest quest, int npcID)
        {
            Started.Add(quest.MapleID, new Dictionary<int, short>());

            foreach (KeyValuePair<int, short> requiredKills in quest.PostRequiredKills)
            {
                Started[quest.MapleID].Add(requiredKills.Key, 0);
            }

            Parent.Stats.Experience += quest.ExperienceReward[0];
            Parent.Stats.Fame += (short)quest.FameReward[0];
            Parent.Stats.Meso += quest.MesoReward[0] * WvsGame.MesoRate;

            // TODO: Skill and pet rewards.

            foreach (KeyValuePair<int, short> item in quest.PreItemRewards)
            {
                if (item.Value > 0)
                {
                    Parent.Items.AddItemToInventory(new Item(item.Key, item.Value)); // TODO: Quest items rewards are displayed in chat.
                }
                else if (item.Value < 0)
                {
                    Parent.Items.RemoveItemFromInventoryByID(item.Key, Math.Abs(item.Value));
                }
            }

            Update(quest.MapleID, QuestConstants.QuestStatus.InProgress);

            using (Packet oPacket = new Packet(ServerOperationCode.QuestResult))
            {
                oPacket
                    .WriteByte((byte)QuestConstants.QuestResult.Complete)
                    .WriteUShort(quest.MapleID)
                    .WriteInt(npcID)
                    .WriteInt();

                Parent.Client.Send(oPacket);
            }
        }

        public void Complete(Quest quest, int selection)
        {
            foreach (KeyValuePair<int, short> item in quest.PostRequiredItems)
            {
                Parent.Items.RemoveItemFromInventoryByID(item.Key, item.Value);
            }

            Parent.Stats.Experience += quest.ExperienceReward[1];

            using (Packet oPacket = new Packet(ServerOperationCode.Message))
            {
                oPacket
                    .WriteByte((byte)ServerConstants.MessageType.IncreaseEXP)
                    .WriteBool(true)
                    .WriteInt(quest.ExperienceReward[1])
                    .WriteBool(true)
                    .WriteInt() // NOTE: Monster Book bonus.
                    .WriteShort() // NOTE: Unknown.
                    .WriteInt() // NOTE: Wedding bonus.
                    .WriteByte() // NOTE: Party bonus.
                    .WriteInt() // NOTE: Party bonus.
                    .WriteInt() // NOTE: Equip bonus.
                    .WriteInt() // NOTE: Internet Cafe bonus.
                    .WriteInt() // NOTE: Rainbow Week bonus.
                    .WriteByte(); // NOTE: Unknown.

                Parent.Client.Send(oPacket);
            }

            Parent.Stats.Fame += (short)quest.FameReward[1];

            // TODO: Fame gain packet in chat.

            Parent.Stats.Meso += quest.MesoReward[1] * WvsGame.MesoRate;

            // TODO: Meso gain packet in chat.

            foreach (KeyValuePair<Skill, CharacterConstants.Job> skill in quest.PostSkillRewards)
            {
                if (Parent.Jobs.Job == skill.Value)
                {
                    Parent.Skills.Add(skill.Key);

                    // TODO: Skill update packet.
                }
            }

            // TODO: Pet rewards.

            if (selection != -1) // NOTE: Selective reward.
            {
                //if (selection == -1) // NOTE: Randomized reward.
                //{
                //    KeyValuePair<int, short> item = quest.PostItemRewards.ElementAt(Constants.Random.Next(0, quest.PostItemRewards.Count));

                //    Parent.Items.Add(new Item(item.Key, item.Value));

                //    using (Packet oPacket = new Packet(ServerOperationCode.Effect))
                //    {
                //        oPacket
                //            .WriteByte((byte)UserEffect.Quest)
                //            .WriteByte(1)
                //            .WriteInt(item.Key)
                //            .WriteInt(item.Value);

                //        Parent.Client.Send(oPacket);
                //    }
                //}
                //else
                //{
                //    // TODO: Selective reward based on selection.
                //}
            }
            else
            {
                foreach (KeyValuePair<int, short> item in quest.PostItemRewards)
                {
                    if (item.Value > 0)
                    {
                        Parent.Items.AddItemToInventory(new Item(item.Key, item.Value));
                    }
                    else if (item.Value < 0)
                    {
                        Parent.Items.RemoveItemFromInventoryByID(item.Key, Math.Abs(item.Value));
                    }

                    using (Packet oPacket = new Packet(ServerOperationCode.Effect))
                    {
                        oPacket
                            .WriteByte((byte)CharacterConstants.UserEffect.Quest)
                            .WriteByte(1)
                            .WriteInt(item.Key)
                            .WriteInt(item.Value);

                        Parent.Client.Send(oPacket);
                    }
                }
            }

            Update(quest.MapleID, QuestConstants.QuestStatus.Complete);

            Delete(quest.MapleID);

            Completed.Add(quest.MapleID, DateTime.UtcNow);

            CharacterBuffs.ShowLocalUserEffect(Parent, CharacterConstants.UserEffect.QuestComplete);
            CharacterBuffs.ShowRemoteUserEffect(Parent, CharacterConstants.UserEffect.QuestComplete, true);
        }

        public void Forfeit(ushort questID)
        {
            Delete(questID);

            Update(questID, QuestConstants.QuestStatus.NotStarted);
        }

        private void Update(ushort questID, QuestConstants.QuestStatus status, string progress = "")
        {
            using (Packet oPacket = new Packet(ServerOperationCode.Message))
            {
                oPacket
                    .WriteByte((byte)ServerConstants.MessageType.QuestRecord)
                    .WriteUShort(questID)
                    .WriteByte((byte)status);

                if (status == QuestConstants.QuestStatus.InProgress)
                {
                    oPacket.WriteString(progress);
                }
                else if (status == QuestConstants.QuestStatus.Complete)
                {
                    oPacket.WriteDateTime(DateTime.Now);
                }

                Parent.Client.Send(oPacket);
            }
        }

        public bool CanComplete(ushort questID, bool onlyOnFinalKill = false)
        {
            Quest quest = DataProvider.Quests[questID];

            foreach (KeyValuePair<int, short> requiredItem in quest.PostRequiredItems)
            {
                if (!Parent.Items.InventoryContainsItemByID(requiredItem.Key, requiredItem.Value))
                {
                    return false;
                }
            }

            foreach (ushort requiredQuest in quest.PostRequiredQuests)
            {
                if (!Completed.ContainsKey(requiredQuest))
                {
                    return false;
                }
            }

            foreach (KeyValuePair<int, short> requiredKill in quest.PostRequiredKills)
            {
                if (onlyOnFinalKill)
                {
                    if (Started[questID][requiredKill.Key] != requiredKill.Value)
                    {
                        return false;
                    }
                }
                else
                {
                    if (Started[questID][requiredKill.Key] < requiredKill.Value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void NotifyComplete(ushort questID)
        {
            using (Packet oPacket = new Packet(ServerOperationCode.QuestClear))
            {
                oPacket.WriteUShort(questID);

                Parent.Client.Send(oPacket);
            }
        }

        public byte[] ToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket.WriteShort((short)Started.Count);

                foreach (KeyValuePair<ushort, Dictionary<int, short>> quest in Started)
                {
                    oPacket.WriteUShort(quest.Key);

                    string kills = string.Empty;

                    foreach (int kill in quest.Value.Values)
                    {
                        kills += kill.ToString().PadLeft(3, '\u0030');
                    }

                    oPacket.WriteString(kills);
                }

                oPacket.WriteShort((short)Completed.Count);

                foreach (KeyValuePair<ushort, DateTime> quest in Completed)
                {
                    oPacket
                        .WriteUShort(quest.Key)
                        .WriteDateTime(quest.Value);
                }

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }
    }
}

