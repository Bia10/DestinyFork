using System.Collections.Generic;

using Destiny.Data;
using Destiny.Maple.Data;
using Destiny.Constants;

namespace Destiny.Maple
{
    public class Quest
    {
        public ushort MapleID { get; private set; }
        public ushort NextQuestID { get; private set; }
        public sbyte Area { get; private set; }
        public byte MinimumLevel { get; private set; }
        public byte MaximumLevel { get; private set; }
        public short PetCloseness { get; private set; }
        public sbyte TamingMobLevel { get; private set; }
        public int RepeatWait { get; private set; }
        public short Fame { get; private set; }
        public int TimeLimit { get; private set; }
        public bool AutoStart { get; private set; }
        public bool SelectedMob { get; private set; }

        public List<ushort> PreRequiredQuests { get; private set; }
        public List<ushort> PostRequiredQuests { get; private set; }
        public Dictionary<int, short> PreRequiredItems { get; private set; }
        public Dictionary<int, short> PostRequiredItems { get; private set; }
        public Dictionary<int, short> PostRequiredKills { get; private set; }
        public List<CharacterConstants.Job> ValidJobs { get; private set; }

        // Rewards (Start, End)
        public int[] ExperienceReward { get; set; }
        public int[] MesoReward { get; set; }
        public int[] PetClosenessReward { get; set; }
        public bool[] PetSpeedReward { get; set; }
        public int[] FameReward { get; set; }
        public int[] PetSkillReward { get; set; }
        public Dictionary<int, short> PreItemRewards { get; private set; }
        public Dictionary<int, short> PostItemRewards { get; private set; }
        public Dictionary<Skill, CharacterConstants.Job> PreSkillRewards { get; set; }
        public Dictionary<Skill, CharacterConstants.Job> PostSkillRewards { get; set; }

        public Quest NextQuest
        {
            get
            {
                return NextQuestID > 0 ? DataProvider.Quests[NextQuestID] : null;
            }
        }

        public byte Flags
        {
            get
            {
                byte flags = 0;

                if (AutoStart) flags |= (byte)QuestConstants.QuestFlags.AutoStart;
                if (SelectedMob) flags |= (byte)QuestConstants.QuestFlags.SelectedMob;

                return flags;
            }
        }

        public Quest(Datum datum)
        {
            MapleID = (ushort)datum["questid"];
            NextQuestID = (ushort)datum["next_quest"];
            Area = (sbyte)datum["quest_area"];
            MinimumLevel = (byte)datum["min_level"];
            MaximumLevel = (byte)datum["max_level"];
            PetCloseness = (short)datum["pet_closeness"];
            TamingMobLevel = (sbyte)datum["taming_mob_level"];
            RepeatWait = (int)datum["repeat_wait"];
            Fame = (short)datum["fame"];
            TimeLimit = (int)datum["time_limit"];
            AutoStart = datum["flags"].ToString().Contains("auto_start");
            SelectedMob = datum["flags"].ToString().Contains("selected_mob");

            PreRequiredQuests = new List<ushort>();
            PostRequiredQuests = new List<ushort>();
            PreRequiredItems = new Dictionary<int, short>();
            PostRequiredItems = new Dictionary<int, short>();
            PostRequiredKills = new Dictionary<int, short>();

            ExperienceReward = new int[2];
            MesoReward = new int[2];
            PetClosenessReward = new int[2];
            PetSpeedReward = new bool[2];
            FameReward = new int[2];
            PetSkillReward = new int[2];

            PreItemRewards = new Dictionary<int, short>();
            PostItemRewards = new Dictionary<int, short>();
            PreSkillRewards = new Dictionary<Skill, CharacterConstants.Job>();
            PostSkillRewards = new Dictionary<Skill, CharacterConstants.Job>();

            ValidJobs = new List<CharacterConstants.Job>();
        }
    }
}
