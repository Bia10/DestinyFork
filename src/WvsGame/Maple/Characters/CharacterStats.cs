using System;
using System.Collections.ObjectModel;

using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.PacketFactory;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterStats : KeyedCollection<int, CharacterStats>
    {
        public Character Parent { get; private set; }

        public CharacterStats(Character parent): base()
        {
            this.Parent = parent;
        }
        public CharacterConstants.StatisticType Statistic;

        #region HealthRelated
        private short health;
        private void SetHealthTo(short value)
        {
            // cannot have negative health
            if (value < 0)
            {
                health = 0;
            }
            // cannot have more then MaxHealth
            else if (value > MaxHealth)
            {
                health = MaxHealth;
            }
            else
            {
                health = value;
            }

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.Health);
            }
        }
        public short Health
        {
            get { return health; }
            set { SetHealthTo(value); }
        }

        private short maxHealth;
        private void SetMaxHealthTo(short value)
        {
            maxHealth = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.MaxHealth);
            }
        }
        public short MaxHealth
        {
            get { return maxHealth; }
            set { SetMaxHealthTo(value); }
        }

        public static bool IsAtMaxHP(Character character)
        {
            short currentHealth = character.Stats.Health;
            short characterMaxHealth = character.Stats.MaxHealth;

            return currentHealth == characterMaxHealth;
        }

        public static void AddHP(Character character, int quantity)
        {
            if (character == null) return;
            if (character.Stats.maxHealth == short.MaxValue) return;

            character.Stats.maxHealth += (short)quantity;
            Update(character, CharacterConstants.StatisticType.MaxHealth);
        }

        // TODO: finish this, sub-switch depending on skill and another on skill level.
        public static void AdjustHPOnLevelUP(Character character)
        {
            if (character == null) return;

            CharacterConstants.Job charJob = character.Job;
            Random r = new Random();

            switch (charJob)
            {
                case CharacterConstants.Job.Beginner:
                    short HPBonusBeginner = Convert.ToInt16(r.Next(10, 16));
                    AddHP(character, HPBonusBeginner);
                    break;

                case CharacterConstants.Job.Aran:
                    short HPBonusAran = Convert.ToInt16(r.Next(10, 16));
                    AddHP(character, HPBonusAran);
                    break;

                case CharacterConstants.Job.Noblesse:
                    short HPBonusNoblesse = Convert.ToInt16(r.Next(10, 16));
                    AddHP(character, HPBonusNoblesse);
                    break;

                case CharacterConstants.Job.Warrior:
                    short HPBonusWarrior = Convert.ToInt16(r.Next(24, 28));
                    AddHP(character, HPBonusWarrior);
                    break;

                case CharacterConstants.Job.DawnWarrior1:
                    short HPBonusDawnWarrior1 = Convert.ToInt16(r.Next(24, 28));
                    AddHP(character, HPBonusDawnWarrior1);
                    break;

                case CharacterConstants.Job.Aran1:
                    short HPBonusAran1 = Convert.ToInt16(r.Next(44, 48));
                    AddHP(character, HPBonusAran1);
                    break;

                case CharacterConstants.Job.Magician:
                    short HPBonusCrusader = Convert.ToInt16(r.Next(10, 14));
                    AddHP(character, HPBonusCrusader);
                    break;

                case CharacterConstants.Job.BlazeWizard1:
                    short HPBonusDawnWarrior2 = Convert.ToInt16(r.Next(10, 14));
                    AddHP(character, HPBonusDawnWarrior2);
                    break;

                case CharacterConstants.Job.Bowman:
                    short HPBonusBowman = Convert.ToInt16(r.Next(20, 24));
                    AddHP(character, HPBonusBowman);
                    break;

                case CharacterConstants.Job.WindArcher1:
                    short HPBonusWindArcher1 = Convert.ToInt16(r.Next(20, 24));
                    AddHP(character, HPBonusWindArcher1);
                    break;

                case CharacterConstants.Job.Thief:
                    short HPBonusThief = Convert.ToInt16(r.Next(20, 24));
                    AddHP(character, HPBonusThief);
                    break;

                case CharacterConstants.Job.NightWalker1:
                    short HPBonusNightWalker1 = Convert.ToInt16(r.Next(20, 24));
                    AddHP(character, HPBonusNightWalker1);
                    break;

                case CharacterConstants.Job.Pirate:
                    short HPBonusPirate = Convert.ToInt16(r.Next(22, 28));
                    AddHP(character, HPBonusPirate);
                    break;

                case CharacterConstants.Job.ThunderBreaker1:
                    short HPBonusThunderBreaker1 = Convert.ToInt16(r.Next(22, 28));
                    AddHP(character, HPBonusThunderBreaker1);
                    break;

                case CharacterConstants.Job.GM:
                    short HPBonusGM = 30000;
                    AddHP(character, HPBonusGM);
                    break;

                case CharacterConstants.Job.SuperGM:
                    short HPBonusSuperGM = 30000;
                    AddHP(character, HPBonusSuperGM);
                    break;

                case CharacterConstants.Job.Fighter:
                    short HPBonusFigther = Convert.ToInt16(r.Next(28, 32));
                    AddHP(character, HPBonusFigther);
                    break;
                case CharacterConstants.Job.Crusader:
                    break;
                case CharacterConstants.Job.Hero:
                    break;
                case CharacterConstants.Job.Page:
                    break;
                case CharacterConstants.Job.WhiteKnight:
                    break;
                case CharacterConstants.Job.Paladin:
                    break;
                case CharacterConstants.Job.Spearman:
                    break;
                case CharacterConstants.Job.DragonKnight:
                    break;
                case CharacterConstants.Job.DarkKnight:
                    break;
                case CharacterConstants.Job.FirePoisonWizard:
                    break;
                case CharacterConstants.Job.FirePoisonMage:
                    break;
                case CharacterConstants.Job.FirePoisonArchMage:
                    break;
                case CharacterConstants.Job.IceLightningWizard:
                    break;
                case CharacterConstants.Job.IceLightningMage:
                    break;
                case CharacterConstants.Job.IceLightningArchMage:
                    break;
                case CharacterConstants.Job.Cleric:
                    break;
                case CharacterConstants.Job.Priest:
                    break;
                case CharacterConstants.Job.Bishop:
                    break;
                case CharacterConstants.Job.Hunter:
                    break;
                case CharacterConstants.Job.Ranger:
                    break;
                case CharacterConstants.Job.BowMaster:
                    break;
                case CharacterConstants.Job.CrossbowMan:
                    break;
                case CharacterConstants.Job.Sniper:
                    break;
                case CharacterConstants.Job.CrossbowMaster:
                    break;
                case CharacterConstants.Job.Assassin:
                    break;
                case CharacterConstants.Job.Hermit:
                    break;
                case CharacterConstants.Job.NightLord:
                    break;
                case CharacterConstants.Job.Bandit:
                    break;
                case CharacterConstants.Job.ChiefBandit:
                    break;
                case CharacterConstants.Job.Shadower:
                    break;
                case CharacterConstants.Job.Brawler:
                    break;
                case CharacterConstants.Job.Marauder:
                    break;
                case CharacterConstants.Job.Buccaneer:
                    break;
                case CharacterConstants.Job.Gunslinger:
                    break;
                case CharacterConstants.Job.Outlaw:
                    break;
                case CharacterConstants.Job.Corsair:
                    break;
                case CharacterConstants.Job.MapleleafBrigadier:
                    break;
                case CharacterConstants.Job.DawnWarrior2:
                    break;
                case CharacterConstants.Job.DawnWarrior3:
                    break;
                case CharacterConstants.Job.DawnWarrior4:
                    break;
                case CharacterConstants.Job.BlazeWizard2:
                    break;
                case CharacterConstants.Job.BlazeWizard3:
                    break;
                case CharacterConstants.Job.BlazeWizard4:
                    break;
                case CharacterConstants.Job.WindArcher2:
                    break;
                case CharacterConstants.Job.WindArcher3:
                    break;
                case CharacterConstants.Job.WindArcher4:
                    break;
                case CharacterConstants.Job.NightWalker2:
                    break;
                case CharacterConstants.Job.NightWalker3:
                    break;
                case CharacterConstants.Job.NightWalker4:
                    break;
                case CharacterConstants.Job.ThunderBreaker2:
                    break;
                case CharacterConstants.Job.ThunderBreaker3:
                    break;
                case CharacterConstants.Job.ThunderBreaker4:
                    break;
                case CharacterConstants.Job.Aran2:
                    break;
                case CharacterConstants.Job.Aran3:
                    break;
                case CharacterConstants.Job.Aran4:
                    break;

                default:
                    AddHP(character, 100);
                    break;
            }
        }
        #endregion

        #region ManaRelated
        private short mana;
        private void SetManaTo(short value)
        {
            // cannot have negative mana
            if (value < 0)
            {
                mana = 0;
            }
            // cannot have more then MaxMana
            else if (value > MaxMana)
            {
                mana = MaxMana;
            }
            else
            {
                mana = value;
            }

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.Mana);
            }
        }
        public short Mana
        {
            get { return mana; }
            set { SetManaTo(value); }
        }

        private short maxMana;
        private void SetMaxManaTo(short value)
        {
            maxMana = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.MaxMana);
            }
        }
        public short MaxMana
        {
            get { return maxMana; }
            set { SetMaxManaTo(value); }
        }

        public static bool IsAtMaxMP(Character character)
        {
            short currentMana = character.Stats.Mana;
            short characterMaxMana = character.Stats.MaxMana;

            return currentMana == characterMaxMana;
        }

        public static void AddMP(Character character, int quantity)
        {
            if (character == null) return;
            if (character.Stats.maxMana == short.MaxValue) return;

            character.Stats.maxMana += (short)quantity;
            Update(character, CharacterConstants.StatisticType.MaxMana);
        }

        public static void AdjustMPOnLevelUP(Character character)
        {
            if (character == null) return;

            CharacterConstants.Job charJob = character.Job;
            Random r = new Random();

            switch (charJob)
            {
                case CharacterConstants.Job.Beginner:
                    short MPBonusBeginner = Convert.ToInt16(r.Next(10, 12));
                    AddMP(character, MPBonusBeginner);
                    break;

                case CharacterConstants.Job.Aran:
                    short MPBonusAran = Convert.ToInt16(r.Next(10, 12));
                    AddMP(character, MPBonusAran);
                    break;

                case CharacterConstants.Job.Noblesse:
                    short MPBonusNoblesse = Convert.ToInt16(r.Next(10, 12));
                    AddMP(character, MPBonusNoblesse);
                    break;

                case CharacterConstants.Job.Warrior:
                    short MPBonusWarrior = Convert.ToInt16(r.Next(4, 6));
                    AddMP(character, MPBonusWarrior);
                    break;

                case CharacterConstants.Job.DawnWarrior1:
                    short MPBonusDawnWarrior1 = Convert.ToInt16(r.Next(4, 6));
                    AddMP(character, MPBonusDawnWarrior1);
                    break;

                case CharacterConstants.Job.Aran1:
                    short MPBonusAran1 = Convert.ToInt16(r.Next(4, 8));
                    AddMP(character, MPBonusAran1);
                    break;

                case CharacterConstants.Job.Magician:
                    short MPBonusCrusader = Convert.ToInt16(r.Next(22, 24));
                    AddMP(character, MPBonusCrusader);
                    break;

                case CharacterConstants.Job.BlazeWizard1:
                    short MPBonusDawnWarrior2 = Convert.ToInt16(r.Next(22, 24));
                    AddMP(character, MPBonusDawnWarrior2);
                    break;

                case CharacterConstants.Job.Bowman:
                    short MPBonusBowman = Convert.ToInt16(r.Next(14, 16));
                    AddMP(character, MPBonusBowman);
                    break;

                case CharacterConstants.Job.WindArcher1:
                    short MPBonusWindArcher1 = Convert.ToInt16(r.Next(14, 16));
                    AddMP(character, MPBonusWindArcher1);
                    break;

                case CharacterConstants.Job.Thief:
                    short MPBonusThief = Convert.ToInt16(r.Next(14, 16));
                    AddMP(character, MPBonusThief);
                    break;

                case CharacterConstants.Job.NightWalker1:
                    short MPBonusNightWalker1 = Convert.ToInt16(r.Next(14, 16));
                    AddMP(character, MPBonusNightWalker1);
                    break;

                case CharacterConstants.Job.Pirate:
                    short MPBonusPirate = Convert.ToInt16(r.Next(14, 16));
                    AddMP(character, MPBonusPirate);
                    break;

                case CharacterConstants.Job.ThunderBreaker1:
                    short MPBonusThunderBreaker1 = Convert.ToInt16(r.Next(14, 16));
                    AddMP(character, MPBonusThunderBreaker1);
                    break;

                case CharacterConstants.Job.GM:
                    short MPBonusGM = 30000;
                    AddMP(character, MPBonusGM);
                    break;

                case CharacterConstants.Job.SuperGM:
                    short MPBonusSuperGM = 30000;
                    AddMP(character, MPBonusSuperGM);
                    break;

                case CharacterConstants.Job.Fighter:
                    break;
                case CharacterConstants.Job.Crusader:
                    break;
                case CharacterConstants.Job.Hero:
                    break;
                case CharacterConstants.Job.Page:
                    break;
                case CharacterConstants.Job.WhiteKnight:
                    break;
                case CharacterConstants.Job.Paladin:
                    break;
                case CharacterConstants.Job.Spearman:
                    break;
                case CharacterConstants.Job.DragonKnight:
                    break;
                case CharacterConstants.Job.DarkKnight:
                    break;
                case CharacterConstants.Job.FirePoisonWizard:
                    break;
                case CharacterConstants.Job.FirePoisonMage:
                    break;
                case CharacterConstants.Job.FirePoisonArchMage:
                    break;
                case CharacterConstants.Job.IceLightningWizard:
                    break;
                case CharacterConstants.Job.IceLightningMage:
                    break;
                case CharacterConstants.Job.IceLightningArchMage:
                    break;
                case CharacterConstants.Job.Cleric:
                    break;
                case CharacterConstants.Job.Priest:
                    break;
                case CharacterConstants.Job.Bishop:
                    break;
                case CharacterConstants.Job.Hunter:
                    break;
                case CharacterConstants.Job.Ranger:
                    break;
                case CharacterConstants.Job.BowMaster:
                    break;
                case CharacterConstants.Job.CrossbowMan:
                    break;
                case CharacterConstants.Job.Sniper:
                    break;
                case CharacterConstants.Job.CrossbowMaster:
                    break;
                case CharacterConstants.Job.Assassin:
                    break;
                case CharacterConstants.Job.Hermit:
                    break;
                case CharacterConstants.Job.NightLord:
                    break;
                case CharacterConstants.Job.Bandit:
                    break;
                case CharacterConstants.Job.ChiefBandit:
                    break;
                case CharacterConstants.Job.Shadower:
                    break;
                case CharacterConstants.Job.Brawler:
                    break;
                case CharacterConstants.Job.Marauder:
                    break;
                case CharacterConstants.Job.Buccaneer:
                    break;
                case CharacterConstants.Job.Gunslinger:
                    break;
                case CharacterConstants.Job.Outlaw:
                    break;
                case CharacterConstants.Job.Corsair:
                    break;
                case CharacterConstants.Job.MapleleafBrigadier:
                    break;
                case CharacterConstants.Job.DawnWarrior2:
                    break;
                case CharacterConstants.Job.DawnWarrior3:
                    break;
                case CharacterConstants.Job.DawnWarrior4:
                    break;
                case CharacterConstants.Job.BlazeWizard2:
                    break;
                case CharacterConstants.Job.BlazeWizard3:
                    break;
                case CharacterConstants.Job.BlazeWizard4:
                    break;
                case CharacterConstants.Job.WindArcher2:
                    break;
                case CharacterConstants.Job.WindArcher3:
                    break;
                case CharacterConstants.Job.WindArcher4:
                    break;
                case CharacterConstants.Job.NightWalker2:
                    break;
                case CharacterConstants.Job.NightWalker3:
                    break;
                case CharacterConstants.Job.NightWalker4:
                    break;
                case CharacterConstants.Job.ThunderBreaker2:
                    break;
                case CharacterConstants.Job.ThunderBreaker3:
                    break;
                case CharacterConstants.Job.ThunderBreaker4:
                    break;
                case CharacterConstants.Job.Aran2:
                    break;
                case CharacterConstants.Job.Aran3:
                    break;
                case CharacterConstants.Job.Aran4:
                    break;

                default:
                    AddMP(character, 100);
                    break;
            }
        }
        #endregion

        #region LevelRelated
        private byte level;
        private void LevelIncrease(byte value)
        {
            const int MAX_LEVEL = 200; // TODO: settings?

            // error trying to exceed max level
            if (value > MAX_LEVEL)
            {
                throw new ArgumentException("Level cannot exceed 200.");
            }

            int delta = value - Level;

            if (!Parent.IsInitialized)
            {
                level = value;
            }
            else
            {
                if (delta < 0)
                {
                    level = value;

                    Update(Parent, CharacterConstants.StatisticType.Level);
                }
                else
                {
                    for (int i = 0; i < delta; i++)
                    {
                        LevelUP(Parent, false);
                    }

                    FillToFull(Parent, CharacterConstants.StatisticType.Health);
                    FillToFull(Parent, CharacterConstants.StatisticType.Mana);
                }
            }
        }
        // TODO: Update party's properties.
        public byte Level
        {
            get { return level; }
            set { LevelIncrease(value); }
        }

        public static void IncreaseLevelByOne(Character character, bool PlayEffect)
        {
            if (character == null) return;
            // increase level & update stats
            character.Stats.level += 1;
            Update(character, CharacterConstants.StatisticType.Level);
            // play effect if needed
            if (PlayEffect) CharacterBuffs.ShowRemoteUserEffect(character, CharacterConstants.UserEffect.LevelUp);
        }

        public static void LevelUP(Character character, bool playEffect)
        {
            if (character == null) return;
            // increase level
            IncreaseLevelByOne(character, playEffect);
            // generate randomized HP && MP bonus
            AdjustHPOnLevelUP(character);
            AdjustMPOnLevelUP(character);
            // gain stats
            // TODO: edge cases when overlevling job adv
            GainAPOnLeveLUP(character);
            GainSPOnLeveLUP(character);
            // play effect if needed
            if (playEffect) CharacterBuffs.ShowRemoteUserEffect(character, CharacterConstants.UserEffect.LevelUp);
        }
        #endregion

        #region AbilityPoints
        private short abilityPoints;
        private void SetAbilityPointsTo(short value)
        {
            abilityPoints = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.AbilityPoints);
            }
        }
        public short AbilityPoints
        {
            get { return abilityPoints; }
            set { SetAbilityPointsTo(value); }
        }

        public static void GainAPOnLeveLUP(Character character)
        {
            if (character == null) return;

            if (CharacterJobs.IsCygnus(character) && character.Stats.Level < 70)
            {
                character.Stats.abilityPoints += 6;
            }

            else if (CharacterJobs.IsCygnus(character) && character.Stats.Level > 70)
            {
                character.Stats.abilityPoints += 5;
            }

            else if (CharacterJobs.IsBeginner(character) && character.Stats.Level < 8)
            {
                character.Stats.abilityPoints += 0;

                if (character.Stats.Level < 6)
                {
                    character.Stats.Strength += 5;
                }
                else if (character.Stats.Level >= 6 && character.Stats.Level < 8)
                {
                    character.Stats.Strength += 4;
                    character.Stats.Dexterity += 1;
                }
            }

            else if (CharacterJobs.IsBeginner(character) && character.Stats.Level == 8)
            {
                character.Stats.Strength = 4;
                character.Stats.Dexterity = 4;
                character.Stats.abilityPoints += 35;
            }

            else
            {
                character.Stats.abilityPoints += 5;
            }

            Update(character, CharacterConstants.StatisticType.AbilityPoints);
        }

        public static void DistributeAP(Character character, CharacterConstants.StatisticType type, short amount = 1)
        {
            switch (type)
            {
                case CharacterConstants.StatisticType.Strength:
                    character.Stats.Strength += amount;
                    break;

                case CharacterConstants.StatisticType.Dexterity:
                    character.Stats.Dexterity += amount;
                    break;

                case CharacterConstants.StatisticType.Intelligence:
                    character.Stats.Intelligence += amount;
                    break;

                case CharacterConstants.StatisticType.Luck:
                    character.Stats.Luck += amount;
                    break;

                case CharacterConstants.StatisticType.MaxHealth:
                    // TODO: Get addition based on other factors.
                    break;

                case CharacterConstants.StatisticType.MaxMana:
                    // TODO: Get addition based on other factors.
                    break;
            }
        }

        public void CharDistributeAPHandler(Packet inPacket)
        {
            if (AbilityPoints == 0) return;

            int ticks = inPacket.ReadInt();
            CharacterConstants.StatisticType type = (CharacterConstants.StatisticType) inPacket.ReadInt();

            DistributeAP(Parent, type);
            AbilityPoints--;
        }

        public void AutoDistributeAP(Packet inPacket)
        {
            if (AbilityPoints == 0) return;

            int ticks = inPacket.ReadInt();
            int count = inPacket.ReadInt(); // NOTE: There are always 2 primary stats for each job, but still.

            int total = 0;

            for (int i = 0; i < count; i++)
            {
                CharacterConstants.StatisticType type = (CharacterConstants.StatisticType) inPacket.ReadInt();
                int amount = inPacket.ReadInt();

                if (amount > AbilityPoints || amount < 0) return;

                DistributeAP(Parent, type, (short) amount);
                total += amount;
            }

            AbilityPoints -= (short)total;
        }
        #endregion

        #region SkillPoints
        private short skillPoints;
        private void SetSkillPointsTo(short value)
        {
            skillPoints = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.SkillPoints);
            }
        }

        public short SkillPoints
        {
            get { return skillPoints; }
            set { SetSkillPointsTo(value); }
        }

        public static void GainSPOnLeveLUP(Character character)
        {
            if (character == null) return;

            if (CharacterJobs.IsBeginner(character))
            {
                character.Stats.skillPoints += 1;
            }

            else
            {
                character.Stats.skillPoints += 3;
            }

            Update(character, CharacterConstants.StatisticType.SkillPoints);
        }

        public void DistributeSPHandler(Packet inPacket)
        {
            if (SkillPoints == 0) return;

            int ticks = inPacket.ReadInt();
            int mapleID = inPacket.ReadInt();

            if (!Parent.Skills.Contains(mapleID))
            {
                Parent.Skills.Add(new Skill(mapleID));
            }

            Skill skill = Parent.Skills[mapleID];

            // TODO: Check for skill requirements.

            if (Skill.IsFromBeginner(skill))
            {
                // TODO: Handle beginner skills.
            }

            if (skill.CurrentLevel + 1 <= skill.MaxLevel)
            {
                if (!Skill.IsFromBeginner(skill))
                {
                    SkillPoints--;
                }
                skill.CurrentLevel++;
            }

            Update(Parent, CharacterConstants.StatisticType.SkillPoints);
        }
        #endregion

        #region Strength
        private short strength;
        private void SetStrenghtTo(short value)
        {
            strength = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.Strength);
            }
        }

        public short Strength
        {
            get { return strength; }
            set { SetStrenghtTo(value); }
        }
        #endregion

        #region Dexterity
        private short dexterity;
        private void SetDexterityTo(short value)
        {
            dexterity = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.Dexterity);
            }
        }

        public short Dexterity
        {
            get { return dexterity; }
            set { SetDexterityTo(value); }
        }
        #endregion

        #region Intelligence
        private short intelligence;
        private void SetIntelligenceTo(short value)
        {
            intelligence = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.Intelligence);
            }
        }

        public short Intelligence
        {
            get { return intelligence; }
            set { SetIntelligenceTo(value); }
        }
        #endregion

        #region Luck
        private short luck;
        private void SetLuckTo(short value)
        {
            luck = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.Luck);
            }
        }

        public short Luck
        {
            get { return luck; }
            set { SetLuckTo(value); }
        }
        #endregion

        #region Experience
        private int experience;
        private void SetExperienceTo(int value)
        {
            bool MULTILVL = true; // TODO: add to settings

            int delta = value - experience;
            experience = value;

            if (MULTILVL)
            {
                while (experience >= CharacterConstants.ExperienceTables.CharacterLevel[Parent.Stats.Level])
                {
                    experience -= CharacterConstants.ExperienceTables.CharacterLevel[Parent.Stats.Level];

                    Parent.Stats.Level++;
                }
            }
            else
            {
                if (experience >= CharacterConstants.ExperienceTables.CharacterLevel[Parent.Stats.Level])
                {
                    experience -= CharacterConstants.ExperienceTables.CharacterLevel[Parent.Stats.Level];

                    Parent.Stats.Level++;
                }

                if (experience >= CharacterConstants.ExperienceTables.CharacterLevel[Parent.Stats.Level])
                {
                    experience = CharacterConstants.ExperienceTables.CharacterLevel[Parent.Stats.Level] - 1;
                }
            }

            if (Parent.IsInitialized && delta != 0)
            {
                Update(Parent, CharacterConstants.StatisticType.Experience);
            }
        }

        public int Experience
        {
            get { return experience; }
            set { SetExperienceTo(value); }
        }
        #endregion Experience

        #region Fame
        private short fame;
        private void SetFameTo(short value)
        {
            fame = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.Fame);
            }
        }

        public short Fame
        {
            get { return fame; }
            set { SetFameTo(value); }
        }
        #endregion

        #region Meso
        private int meso;
        private void SetMesoTo(int value)
        {
            meso = value;

            if (Parent.IsInitialized)
            {
                Update(Parent, CharacterConstants.StatisticType.Mesos);
            }
        }

        public int Meso
        {
            get { return meso; }
            set { SetMesoTo(value); }
        }

        public void DropMesoHandler(Packet inPacket) // TODO: validate request by time, no spamming!
        {
            int tRequestTime = inPacket.ReadInt(); // NOTE: tRequestTime (ticks). 
            int amount = inPacket.ReadInt();

            // TODO: add this to settings
            const int MIN_LIMIT = 10;
            const int MAX_LIMIT = 50000;

            // validate ammount to drop
            if (amount > Meso || amount < MIN_LIMIT || amount > MAX_LIMIT) return;

            // take char mesos
            Meso -= amount;

            // create new mesos
            Meso meso = new Meso(amount)
            {
                Dropper = Parent,
                Owner = null
            };

            // add it to current map
            Parent.Map.Drops.Add(meso);
        }
        #endregion

        public static void FillToFull(Character character, CharacterConstants.StatisticType typeToFill)
        {
            if (character == null) return;
            if (IsAtMaxHP(character) || IsAtMaxMP(character)) return;

            switch (typeToFill)
            {
                case CharacterConstants.StatisticType.Mana:
                    character.Stats.Health = character.Stats.MaxHealth;
                    Update(character, CharacterConstants.StatisticType.Health);
                    break;

                case CharacterConstants.StatisticType.Health:
                    character.Stats.Mana = character.Stats.MaxMana;
                    Update(character, CharacterConstants.StatisticType.Mana);
                    break;

                // TODO: fill other stats?
            }
        }
    
        public static void AddAbility(Character character, CharacterConstants.StatisticType statistic, short mod, bool isReset)
        {
            short maxStat = Int16.MaxValue; // TODO: Should this be a setting?
            bool isSubtract = mod < 0;

            lock (character)
            {
                switch (statistic)
                {
                    case CharacterConstants.StatisticType.Strength:
                        if (character.Stats.Strength >= maxStat)
                        {
                            return;
                        }

                        character.Stats.Strength += mod;
                        break;

                    case CharacterConstants.StatisticType.Dexterity:
                        if (character.Stats.Dexterity >= maxStat)
                        {
                            return;
                        }

                        character.Stats.Dexterity += mod;
                        break;

                    case CharacterConstants.StatisticType.Intelligence:
                        if (character.Stats.Intelligence >= maxStat)
                        {
                            return;
                        }

                        character.Stats.Intelligence += mod;
                        break;

                    case CharacterConstants.StatisticType.Luck:
                        if (character.Stats.Luck >= maxStat)
                        {
                            return;
                        }

                        character.Stats.Luck += mod;
                        break;

                    case CharacterConstants.StatisticType.MaxHealth:
                    case CharacterConstants.StatisticType.MaxMana:
                        {
                            // TODO: character is way too complicated for now.
                        }
                        break;
                }

                if (!isReset)
                {
                    character.Stats.abilityPoints -= mod;
                }

                // TODO: Update bonuses.
            }
        }

        //TODO: hp/mp modification bugs out UI bars, add multiple stats, some kind of message to sideBar/chat
        public static void giveStat(Character character, CharacterConstants.StatisticType stat, short quantity)
        {
            switch (stat)
            {
                case CharacterConstants.StatisticType.Strength:
                    int totalStrenght = character.Stats.Strength + quantity;

                    if (totalStrenght < Int16.MaxValue)
                    {
                        character.Stats.Strength += quantity;
                        Update(character, CharacterConstants.StatisticType.Strength);
                        break;
                    }

                    else
                    {
                        character.Stats.Strength = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.Strength);
                        break;
                    }

                case CharacterConstants.StatisticType.Dexterity:
                    int totalDexterity = character.Stats.Dexterity + quantity;

                    if (totalDexterity < Int16.MaxValue)
                    {
                        character.Stats.Dexterity += quantity;
                        Update(character, CharacterConstants.StatisticType.Dexterity);
                        break;
                    }

                    else
                    {
                        character.Stats.Dexterity = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.Dexterity);
                        break;
                    }

                case CharacterConstants.StatisticType.Intelligence:
                    int totalIntelligence = character.Stats.Intelligence + quantity;

                    if (totalIntelligence < Int16.MaxValue)
                    {
                        character.Stats.Intelligence += quantity;
                        Update(character, CharacterConstants.StatisticType.Intelligence);
                        break;
                    }

                    else
                    {
                        character.Stats.Intelligence = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.Intelligence);
                        break;
                    }

                case CharacterConstants.StatisticType.Luck:
                    int totalLuck = character.Stats.Luck + quantity;
                    

                    if (totalLuck < Int16.MaxValue)
                    {
                        character.Stats.Luck += quantity;
                        Update(character, CharacterConstants.StatisticType.Luck);
                        break;
                    }

                    else
                    {
                        character.Stats.Luck = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.Luck);
                        break;
                    }

                case CharacterConstants.StatisticType.Health:
                    int totalHealth = character.Stats.Health + quantity;

                    if (totalHealth < Int16.MaxValue)
                    {
                        character.Stats.Health += quantity;
                        Update(character, CharacterConstants.StatisticType.Health);
                        break;
                    }

                    else
                    {
                        character.Stats.Health = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.Health);
                        break;
                    }

                case CharacterConstants.StatisticType.MaxHealth:
                    int totalMaxHealth = character.Stats.MaxHealth + quantity;

                    if (totalMaxHealth < Int16.MaxValue)
                    {
                        character.Stats.MaxHealth += quantity;
                        Update(character, CharacterConstants.StatisticType.MaxHealth);
                        break;
                    }

                    else
                    {
                        character.Stats.MaxHealth = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.MaxHealth);
                        break;
                    }

                case CharacterConstants.StatisticType.Mana:
                    int totalMana = character.Stats.Mana + quantity;

                    if (totalMana < Int16.MaxValue)
                    {
                        character.Stats.Mana += quantity;
                        Update(character, CharacterConstants.StatisticType.Mana);
                        break;
                    }

                    else
                    {
                        character.Stats.Mana = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.Mana);
                        break;
                    }

                case CharacterConstants.StatisticType.MaxMana:
                    int totalMaxMana = character.Stats.MaxMana + quantity;

                    if (totalMaxMana < Int16.MaxValue)
                    {
                        character.Stats.MaxMana += quantity;
                        Update(character, CharacterConstants.StatisticType.MaxMana);
                        break;
                    }

                    else
                    {
                        character.Stats.MaxMana = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.MaxMana);
                        break;
                    }

                case CharacterConstants.StatisticType.AbilityPoints:
                    int totalAbilityPoints = character.Stats.abilityPoints + quantity;

                    if (totalAbilityPoints < Int16.MaxValue)
                    {
                        character.Stats.abilityPoints += quantity;
                        Update(character, CharacterConstants.StatisticType.AbilityPoints);
                        break;
                    }

                    else
                    {
                        character.Stats.abilityPoints = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.AbilityPoints);
                        break;
                    }

                case CharacterConstants.StatisticType.SkillPoints:
                    int totalSkillPoints = character.Stats.skillPoints + quantity;

                    if (totalSkillPoints < Int16.MaxValue)
                    {
                        character.Stats.skillPoints += quantity;
                        Update(character, CharacterConstants.StatisticType.SkillPoints);
                        break;
                    }
                    else
                    {
                        character.Stats.skillPoints = Int16.MaxValue;
                        Update(character, CharacterConstants.StatisticType.SkillPoints);
                        break;
                    }

                case CharacterConstants.StatisticType.Skin: break;
                case CharacterConstants.StatisticType.Face: break;
                case CharacterConstants.StatisticType.Hair: break;
                case CharacterConstants.StatisticType.Level: break;
                case CharacterConstants.StatisticType.Job: break;
                case CharacterConstants.StatisticType.Experience: break;
                case CharacterConstants.StatisticType.Fame: break;
                case CharacterConstants.StatisticType.Mesos: break;
                case CharacterConstants.StatisticType.Pet: break;
                case CharacterConstants.StatisticType.GachaponExperience: break;

                default: throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
            }
        }

        public static void Update(Character character, params CharacterConstants.StatisticType[] charStats)
        {
            character.Client.Send(CharacterPackets.UpdateStatsPacket(character, charStats));
        }

        protected override int GetKeyForItem(CharacterStats item)
        {
            throw new NotImplementedException();
        }
    }
}