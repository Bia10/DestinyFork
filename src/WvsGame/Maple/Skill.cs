using System;
using System.Linq;

using Destiny.IO;
using Destiny.Data;
using Destiny.Maple.Characters;
using Destiny.Maple.Data;
using Destiny.Constants;
using Destiny.Maple.Life;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;
using Destiny.Threading;

namespace Destiny.Maple
{
    public sealed class Skill
    {
        public CharacterSkills Parent { get; set; }

        private byte currentLevel;
        private byte maxLevel;
        private DateTime cooldownEnd = DateTime.MinValue;

        public int ID { get; set; }
        public int MapleID { get; set; }
        public DateTime Expiration { get; set; }

        public sbyte MobCount { get; set; }
        public sbyte HitCount { get; set; }
        public short Range { get; set; }
        public int BuffTime { get; set; }
        public short CostMP { get; set; }
        public short CostHP { get; set; }
        public short Damage { get; set; }
        public int FixedDamage { get; set; }
        public byte CriticalDamage { get; set; }
        public sbyte Mastery { get; set; }
        public int OptionalItemCost { get; set; }
        public int CostItem { get; set; }
        public short ItemCount { get; set; }
        public short CostBullet { get; set; }
        public short CostMeso { get; set; }
        public short ParameterA { get; set; }
        public short ParameterB { get; set; }
        public short Speed { get; set; }
        public short Jump { get; set; }
        public short Strength { get; set; }
        public short WeaponAttack { get; set; }
        public short WeaponDefense { get; set; }
        public short MagicAttack { get; set; }
        public short MagicDefense { get; set; }
        public short Accuracy { get; set; }
        public short Avoidability { get; set; }
        public short HP { get; set; }
        public short MP { get; set; }
        public short Probability { get; set; }
        public short Morph { get; set; }
        public Point LT { get; private set; }
        public Point RB { get; private set; }
        public int Cooldown { get; set; }
        // TODO: skill element?

        public bool HasBuff
        {
            get
            {
                return BuffTime > 0;
            }
        }

        public byte CurrentLevel
        {
            get
            {
                return currentLevel;
            }
            set
            {
                currentLevel = value;

                if (Parent == null) return;

                Recalculate();

                if (Character.IsInitialized)
                {
                    Parent.UpdateSkill(this);
                }
            }
        }

        public byte MaxLevel
        {
            get
            {
                return maxLevel;
            }
            set
            {
                maxLevel = value;

                if (Parent != null && Character.IsInitialized)
                {
                    Parent.UpdateSkill(this);
                }
            }
        }

        public Skill CachedReference
        {
            get
            {
                return DataProvider.Skills[MapleID][CurrentLevel];
            }
        }

        public Character Character
        {
            get
            {
                return Parent.Parent;
            }
        }

        public bool IsFromFourthJob
        {
            get
            {   // TODO: Redo that.
                return MapleID > 1000000 && (MapleID / 10000).ToString()[2] == '2'; 
            }
        }

        public static bool IsFromBeginner(Skill skill)
        {
            switch (skill.MapleID)
            {
                case (int)CharacterConstants.SkillNames.Beginner.NimbleFeet:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.Recovery:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.ThreeSnails:
                    return true;

                default: return false;

                /* case (int)CharacterConstants.SkillNames.Beginner.BambooRain:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.BlessingOfTheFairy:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.ChairMaster:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.EchoOfHero:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.FollowTheLead:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.Invincibility:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.JumpDown:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.LegendarySpirit:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.Maker:
                    return true;
                case (int)CharacterConstants.SkillNames.Beginner.MonsterRider:
                    return true; */
            }
        }

        public bool IsCoolingDown
        {
            get
            {
                return DateTime.Now < CooldownEnd;
            }
        }

        public int RemainingCooldownSeconds
        {
            get
            {
                return Math.Min(0, (int)(CooldownEnd - DateTime.Now).TotalSeconds);
            }
        }

        public DateTime CooldownEnd
        {
            get
            {
                return cooldownEnd;
            }
            set
            {
                cooldownEnd = value;

                if (!IsCoolingDown) return;

                //set cooldown
                using (Packet oPacket = new Packet(ServerOperationCode.Cooldown))
                {
                    oPacket
                        .WriteInt(MapleID)
                        .WriteShort((short)RemainingCooldownSeconds);

                    Character.Client.Send(oPacket);
                }

                //remove cooldown
                Delay.Execute(() => 
                {
                    using (Packet oPacket = new Packet(ServerOperationCode.Cooldown))
                    {
                        oPacket
                            .WriteInt(MapleID)
                            .WriteShort(0);

                        Character.Client.Send(oPacket);
                    }

                }, RemainingCooldownSeconds * 1000);
            }
        }

        private bool Assigned { get; set; }

        public Skill(int mapleID, DateTime? expiration = null)
        {
            MapleID = mapleID;
            CurrentLevel = 0;
            MaxLevel = (byte)DataProvider.Skills[MapleID].Count;

            if (!expiration.HasValue)
            {
                expiration = new DateTime(2079, 1, 1, 12, 0, 0); // NOTE: Default expiration time (permanent).
            }

            Expiration = (DateTime)expiration;
        }

        public Skill(int mapleID, byte currentLevel, byte maxLevel, DateTime? expiration = null)
        {
            MapleID = mapleID;
            CurrentLevel = currentLevel;
            MaxLevel = maxLevel;

            if (!expiration.HasValue)
            {
                expiration = new DateTime(2079, 1, 1, 12, 0, 0); // NOTE: Default expiration time (permanent).
            }

            Expiration = (DateTime)expiration;
        }

        public Skill(Datum datum)
        {
            if (DataProvider.IsInitialized)
            {
                ID = (int)datum["ID"];
                Assigned = true;

                MapleID = (int)datum["MapleID"];
                CurrentLevel = (byte)datum["CurrentLevel"];
                MaxLevel = (byte)datum["MaxLevel"];
                Expiration = (DateTime)datum["Expiration"];
                CooldownEnd = (DateTime)datum["CooldownEnd"];
            }
            else
            {
                MapleID = (int)datum["skillid"];
                CurrentLevel = (byte)(short)datum["skill_level"];
                MobCount = (sbyte)datum["mob_count"];
                HitCount = (sbyte)datum["hit_count"];
                Range = (short)datum["range"];
                BuffTime = (int)datum["buff_time"];
                CostHP = (short)datum["hp_cost"];
                CostMP = (short)datum["mp_cost"];
                Damage = (short)datum["damage"];
                FixedDamage = (int)datum["fixed_damage"];
                CriticalDamage = (byte)datum["critical_damage"];
                Mastery = (sbyte)datum["mastery"];
                OptionalItemCost = (int)datum["optional_item_cost"];
                CostItem = (int)datum["item_cost"];
                ItemCount = (short)datum["item_count"];
                CostBullet = (short)datum["bullet_cost"];
                CostMeso = (short)datum["money_cost"];
                ParameterA = (short)datum["x_property"];
                ParameterB = (short)datum["y_property"];
                Speed = (short)datum["speed"];
                Jump = (short)datum["jump"];
                Strength = (short)datum["str"];
                WeaponAttack = (short)datum["weapon_atk"];
                MagicAttack = (short)datum["magic_atk"];
                WeaponDefense = (short)datum["weapon_def"];
                MagicDefense = (short)datum["magic_def"];
                Accuracy = (short)datum["accuracy"];
                Avoidability = (short)datum["avoid"];
                HP = (short)datum["hp"];
                MP = (short)datum["mp"];
                Probability = (short)datum["prop"];
                Morph = (short)datum["morph"];
                LT = new Point((short)datum["ltx"], (short)datum["lty"]);
                RB = new Point((short)datum["rbx"], (short)datum["rby"]);
                Cooldown = (int)datum["cooldown_time"];
            }
        }

        public void Save()
        {
            Datum datum = new Datum("skills")
            {
                ["CharacterID"] = Character.ID,
                ["MapleID"] = MapleID,
                ["CurrentLevel"] = CurrentLevel,
                ["MaxLevel"] = MaxLevel,
                ["Expiration"] = Expiration,
                ["CooldownEnd"] = CooldownEnd
            };

            if (Assigned)
            {
                datum.Update("ID = {0}", ID);
            }
            else
            {
                ID = datum.InsertAndReturnID();
                Assigned = true;
            }
        }

        public void Delete()
        {
            Database.Delete("skills", "ID = {0}", ID);

            Assigned = false;
        }

        public void Recalculate()
        {
            MobCount = CachedReference.MobCount;
            HitCount = CachedReference.HitCount;
            Range = CachedReference.Range;
            BuffTime = CachedReference.BuffTime;
            CostMP = CachedReference.CostMP;
            CostHP = CachedReference.CostHP;
            Damage = CachedReference.Damage;
            FixedDamage = CachedReference.FixedDamage;
            CriticalDamage = CachedReference.CriticalDamage;
            Mastery = CachedReference.Mastery;
            OptionalItemCost = CachedReference.OptionalItemCost;
            CostItem = CachedReference.CostItem;
            ItemCount = CachedReference.ItemCount;
            CostBullet = CachedReference.CostBullet;
            CostMeso = CachedReference.CostMeso;
            ParameterA = CachedReference.ParameterA;
            ParameterB = CachedReference.ParameterB;
            Speed = CachedReference.Speed;
            Jump = CachedReference.Jump;
            Strength = CachedReference.Strength;
            WeaponAttack = CachedReference.WeaponAttack;
            WeaponDefense = CachedReference.WeaponDefense;
            MagicAttack = CachedReference.MagicAttack;
            MagicDefense = CachedReference.MagicDefense;
            Accuracy = CachedReference.Accuracy;
            Avoidability = CachedReference.Avoidability;
            HP = CachedReference.HP;
            MP = CachedReference.MP;
            Probability = CachedReference.Probability;
            Morph = CachedReference.Morph;
            LT = CachedReference.LT;
            RB = CachedReference.RB;
            Cooldown = CachedReference.Cooldown;
        }

        public void Cast()
        {
            if (!Character.IsAlive) return;       
            if (IsCoolingDown) return;

            Character.Stats.Health -= CostHP;
            Character.Stats.Mana -= CostMP;

            if (Cooldown > 0)
            {
                CooldownEnd = DateTime.Now.AddSeconds(Cooldown);
            }

            if (CostItem > 0)
            {
            }

            if (CostBullet > 0)
            {
            }

            if (CostMeso > 0)
            {
            }

            if (MapleID == (int) CharacterConstants.SkillNames.FirePoisonMage.PoisonMist)
            {
                Point mistMaxLT = new Point(-200, -150);
                Point mistMaxRB = new Point(200, 150);

                Rectangle boundingBox = new Rectangle(mistMaxLT + Character.Position, mistMaxRB + Character.Position);

                Mist poisonMist = new Mist(boundingBox, Character, this);
                //Mist.SpawnMist(Character.Client, poisonMist);            
                //get damage ticks of poisoned mobs within bounds
            }

            if (MapleID == (int) CharacterConstants.SkillNames.FirePoisonWizard.PoisonBreath)
            {
                Mob victim = Character.ControlledMobs.FirstOrDefault();

                victim?.Buff(MobConstants.MobStatus.Poisoned, 1, this);
            }
        }

        //public void Cast(Packet iPacket)
        //{
        //    if (!Character.IsAlive)
        //    {
        //        return;
        //    }

        //    if (IsCoolingDown)
        //    {
        //        return;
        //    }

        //    if (MapleID == (int)SkillNames.Priest.MysticDoor)
        //    {
        //        Point origin = new Point(iPacket.ReadShort(), iPacket.ReadShort());

        //        // TODO: Open mystic door.
        //    }

        //    Character.Health -= CostHP;
        //    Character.Mana -= CostMP;

        //    if (Cooldown > 0)
        //    {
        //        CooldownEnd = DateTime.Now.AddSeconds(Cooldown);
        //    }

        //    // TODO: Money cost.

        //    byte type = 0;
        //    byte direction = 0;
        //    short addedInfo = 0;

        //    switch (MapleID)
        //    {
        //        case (int)SkillNames.Priest.MysticDoor:
        //            // NOTe: Prevents the default case from executing, there's no packet data left for it.
        //            break;

        //        case (int)SkillNames.Brawler.MpRecovery:
        //            {
        //                short healthMod = (short)((Character.MaxHealth * ParameterA) / 100);
        //                short manaMod = (short)((healthMod * ParameterB) / 100);

        //                Character.Health -= healthMod;
        //                Character.Mana += manaMod;
        //            }
        //            break;

        //        case (int)SkillNames.Shadower.Smokescreen:
        //            {
        //                Point origin = new Point(iPacket.ReadShort(), iPacket.ReadShort());

        //                // TODO: Mists.
        //            }
        //            break;

        //        case (int)SkillNames.Corsair.Battleship:
        //            {
        //                // TODO: Reset Battleship health.
        //            }
        //            break;

        //        case (int)SkillNames.Crusader.ArmorCrash:
        //        case (int)SkillNames.WhiteKnight.MagicCrash:
        //        case (int)SkillNames.DragonKnight.PowerCrash:
        //            {
        //                iPacket.ReadInt(); // NOTE: Unknown, probably CRC.
        //                byte mobs = iPacket.ReadByte();

        //                for (byte i = 0; i < mobs; i++)
        //                {
        //                    int objectID = iPacket.ReadInt();

        //                    Mob mob;

        //                    try
        //                    {
        //                        mob = Character.Map.Mobs[objectID];
        //                    }
        //                    catch (KeyNotFoundException)
        //                    {
        //                        return;
        //                    }

        //                    // TODO: Mob crash skill.
        //                }
        //            }
        //            break;

        //        case (int)SkillNames.Hero.MonsterMagnet:
        //        case (int)SkillNames.Paladin.MonsterMagnet:
        //        case (int)SkillNames.DarkKnight.MonsterMagnet:
        //            {
        //                int mobs = iPacket.ReadInt();

        //                for (int i = 0; i < mobs; i++)
        //                {
        //                    int objectID = iPacket.ReadInt();

        //                    Mob mob;

        //                    try
        //                    {
        //                        mob = Character.Map.Mobs[objectID];
        //                    }
        //                    catch (KeyNotFoundException)
        //                    {
        //                        return;
        //                    }

        //                    bool success = iPacket.ReadBool();

        //                    // TODO: Packet.
        //                }

        //                direction = iPacket.ReadByte();
        //            }
        //            break;

        //        case (int)SkillNames.FirePoisonWizard.Slow:
        //        case (int)SkillNames.IceLightningWizard.Slow:
        //        case (int)SkillNames.BlazeWizard.Slow:
        //        case (int)SkillNames.Page.Threaten:
        //            {
        //                iPacket.ReadInt(); // NOTE: Unknown, probably CRC.

        //                byte mobs = iPacket.ReadByte();

        //                for (byte i = 0; i < mobs; i++)
        //                {
        //                    int objectID = iPacket.ReadInt();

        //                    Mob mob;

        //                    try
        //                    {
        //                        mob = Character.Map.Mobs[objectID];
        //                    }
        //                    catch (KeyNotFoundException)
        //                    {
        //                        return;
        //                    }
        //                }

        //                // TODO: Apply mob status.
        //            }
        //            break;

        //        case (int)SkillNames.FirePoisonMage.Seal:
        //        case (int)SkillNames.IceLightningMage.Seal:
        //        case (int)SkillNames.BlazeWizard.Seal:
        //        case (int)SkillNames.Priest.Doom:
        //        case (int)SkillNames.Hermit.ShadowWeb:
        //        case (int)SkillNames.NightWalker.ShadowWeb:
        //        case (int)SkillNames.Shadower.NinjaAmbush:
        //        case (int)SkillNames.NightLord.NinjaAmbush:
        //            {
        //                byte mobs = iPacket.ReadByte();

        //                for (byte i = 0; i < mobs; i++)
        //                {
        //                    int objectID = iPacket.ReadInt();

        //                    Mob mob;

        //                    try
        //                    {
        //                        mob = Character.Map.Mobs[objectID];
        //                    }
        //                    catch (KeyNotFoundException)
        //                    {
        //                        return;
        //                    }
        //                }

        //                // TODO: Apply mob status.
        //            }
        //            break;

        //        case (int)SkillNames.Bishop.HerosWill:
        //        case (int)SkillNames.IceLightningArchMage.HerosWill:
        //        case (int)SkillNames.FirePoisonArchMage.HerosWill:
        //        case (int)SkillNames.DarkKnight.HerosWill:
        //        case (int)SkillNames.Hero.HerosWill:
        //        case (int)SkillNames.Paladin.HerosWill:
        //        case (int)SkillNames.NightLord.HerosWill:
        //        case (int)SkillNames.Shadower.HerosWill:
        //        case (int)SkillNames.Bowmaster.HerosWill:
        //        case (int)SkillNames.Marksman.HerosWill:
        //            {
        //                // TODO: Add Buccaneer & Corsair.

        //                // TODO: Remove Sedcude debuff.
        //            }
        //            break;

        //        case (int)SkillNames.Priest.Dispel:
        //            {

        //            }
        //            break;

        //        case (int)SkillNames.Cleric.Heal:
        //            {
        //                short healthRate = HP;

        //                if (healthRate > 100)
        //                {
        //                    healthRate = 100;
        //                }

        //                int partyPlayers = Character.Party != null ? Character.Party.Count : 1;
        //                short healthMod = (short)(((healthRate * Character.MaxHealth) / 100) / partyPlayers);

        //                if (Character.Party != null)
        //                {
        //                    int experience = 0;

        //                    List<PartyMember> members = new List<PartyMember>();

        //                    foreach (PartyMember member in Character.Party)
        //                    {
        //                        if (member.Character != null && member.Character.Map.MapleID == Character.Map.MapleID)
        //                        {
        //                            members.Add(member);
        //                        }
        //                    }

        //                    foreach (PartyMember member in members)
        //                    {
        //                        short memberHealth = member.Character.Health;

        //                        if (memberHealth > 0 && memberHealth < member.Character.MaxHealth)
        //                        {
        //                            member.Character.Health += healthMod;

        //                            if (member.Character != Character)
        //                            {
        //                                experience += 20 * (member.Character.Health - memberHealth) / (8 * member.Character.Level + 190);
        //                            }
        //                        }
        //                    }

        //                    if (experience > 0)
        //                    {
        //                        Character.Experience += experience;
        //                    }
        //                }
        //                else
        //                {
        //                    Character.Health += healthRate;
        //                }
        //            }
        //            break;

        //        case (int)SkillNames.Fighter.Rage:
        //        case (int)SkillNames.DawnWarrior.Rage:
        //        case (int)SkillNames.Spearman.IronWill:
        //        case (int)SkillNames.Spearman.HyperBody:
        //        case (int)SkillNames.FirePoisonWizard.Meditation:
        //        case (int)SkillNames.IceLightningWizard.Meditation:
        //        case (int)SkillNames.BlazeWizard.Meditation:
        //        case (int)SkillNames.Cleric.Bless:
        //        case (int)SkillNames.Priest.HolySymbol:
        //        case (int)SkillNames.Bishop.Resurrection:
        //        case (int)SkillNames.Bishop.HolyShield:
        //        case (int)SkillNames.Bowmaster.SharpEyes:
        //        case (int)SkillNames.Marksman.SharpEyes:
        //        case (int)SkillNames.Assassin.Haste:
        //        case (int)SkillNames.NightWalker.Haste:
        //        case (int)SkillNames.Hermit.MesoUp:
        //        case (int)SkillNames.Bandit.Haste:
        //        case (int)SkillNames.Buccaneer.SpeedInfusion:
        //        case (int)SkillNames.ThunderBreaker.SpeedInfusion:
        //        case (int)SkillNames.Buccaneer.TimeLeap:
        //        case (int)SkillNames.Hero.MapleWarrior:
        //        case (int)SkillNames.Paladin.MapleWarrior:
        //        case (int)SkillNames.DarkKnight.MapleWarrior:
        //        case (int)SkillNames.FirePoisonArchMage.MapleWarrior:
        //        case (int)SkillNames.IceLightningArchMage.MapleWarrior:
        //        case (int)SkillNames.Bishop.MapleWarrior:
        //        case (int)SkillNames.Bowmaster.MapleWarrior:
        //        case (int)SkillNames.Marksman.MapleWarrior:
        //        case (int)SkillNames.NightLord.MapleWarrior:
        //        case (int)SkillNames.Shadower.MapleWarrior:
        //        case (int)SkillNames.Buccaneer.MapleWarrior:
        //        case (int)SkillNames.Corsair.MapleWarrior:
        //            {
        //                if (MapleID == (int)SkillNames.Buccaneer.TimeLeap)
        //                {
        //                    // TODO: Remove all cooldowns.
        //                }

        //                if (Character.Party != null)
        //                {
        //                    byte targets = iPacket.ReadByte();

        //                    // TODO: Get affected party members.

        //                    List<PartyMember> affected = new List<PartyMember>();

        //                    foreach (PartyMember member in affected)
        //                    {
        //                        using (Packet oPacket = new Packet(ServerOperationCode.Effect))
        //                        {
        //                            oPacket
        //                                .WriteByte((byte)UserEffect.SkillAffected)
        //                                .WriteInt(MapleID)
        //                                .WriteByte(1)
        //                                .WriteByte(1);

        //                            member.Character.Client.Send(oPacket);
        //                        }

        //                        using (Packet oPacket = new Packet(ServerOperationCode.RemoteEffect))
        //                        {
        //                            oPacket
        //                                .WriteInt(member.Character.ID)
        //                                .WriteByte((byte)UserEffect.SkillAffected)
        //                                .WriteInt(MapleID)
        //                                .WriteByte(1)
        //                                .WriteByte(1);

        //                            member.Character.Map.Broadcast(oPacket, member.Character);
        //                        }

        //                        member.Character.Buffs.Add(this, 0);
        //                    }
        //                }
        //            }
        //            break;

        //        case (int)SkillNames.Beginner.EchoOfHero:
        //        case (int)SkillNames.Noblesse.EchoOfHero:
        //        case (int)SkillNames.SuperGM.Haste:
        //        case (int)SkillNames.SuperGM.HolySymbol:
        //        case (int)SkillNames.SuperGM.Bless:
        //        case (int)SkillNames.SuperGM.HyperBody:
        //        case (int)SkillNames.SuperGM.HealPlusDispel:
        //        case (int)SkillNames.SuperGM.Resurrection:
        //            {
        //                byte targets = iPacket.ReadByte();
        //                Func<Character, bool> condition = null;
        //                Action<Character> action = null;

        //                switch (MapleID)
        //                {
        //                    case (int)SkillNames.SuperGM.HealPlusDispel:
        //                        {
        //                            condition = new Func<Character, bool>((target) => target.IsAlive);
        //                            action = new Action<Character>((target) =>
        //                            {
        //                                target.Health = target.MaxHealth;
        //                                target.Mana = target.MaxMana;

        //                                // TODO: Use dispell.
        //                            });
        //                        }
        //                        break;

        //                    case (int)SkillNames.SuperGM.Resurrection:
        //                        {
        //                            condition = new Func<Character, bool>((target) => !target.IsAlive);
        //                            action = new Action<Character>((target) =>
        //                            {
        //                                target.Health = target.MaxHealth;
        //                            });
        //                        }
        //                        break;

        //                    default:
        //                        {
        //                            condition = new Func<Character, bool>((target) => true);
        //                            action = new Action<Character>((target) =>
        //                            {
        //                                target.Buffs.Add(this, 0);
        //                            });
        //                        }
        //                        break;
        //                }

        //                for (byte i = 0; i < targets; i++)
        //                {
        //                    int targetID = iPacket.ReadInt();

        //                    Character target = Character.Map.Characters[targetID];

        //                    if (target != Character && condition(target))
        //                    {
        //                        using (Packet oPacket = new Packet(ServerOperationCode.Effect))
        //                        {
        //                            oPacket
        //                                .WriteByte((byte)UserEffect.SkillAffected)
        //                                .WriteInt(MapleID)
        //                                .WriteByte(1)
        //                                .WriteByte(1);

        //                            target.Client.Send(oPacket);
        //                        }

        //                        using (Packet oPacket = new Packet(ServerOperationCode.RemoteEffect))
        //                        {
        //                            oPacket
        //                                .WriteInt(target.ID)
        //                                .WriteByte((byte)UserEffect.SkillAffected)
        //                                .WriteInt(MapleID)
        //                                .WriteByte(1)
        //                                .WriteByte(1);

        //                            target.Map.Broadcast(oPacket, target);
        //                        }

        //                        action(target);
        //                    }
        //                }
        //            }
        //            break;

        //        default:
        //            {
        //                type = iPacket.ReadByte();

        //                switch (type)
        //                {
        //                    case 0x80:
        //                        addedInfo = iPacket.ReadShort();
        //                        break;
        //                }
        //            }
        //            break;
        //    }

        //    using (Packet oPacket = new Packet(ServerOperationCode.Effect))
        //    {
        //        oPacket
        //            .WriteByte((byte)UserEffect.SkillUse)
        //            .WriteInt(MapleID)
        //            .WriteByte(1)
        //            .WriteByte(1);

        //        Character.Client.Send(oPacket);
        //    }

        //    using (Packet oPacket = new Packet(ServerOperationCode.RemoteEffect))
        //    {
        //        oPacket
        //            .WriteInt(Character.ID)
        //            .WriteByte((byte)UserEffect.SkillUse)
        //            .WriteInt(MapleID)
        //            .WriteByte(1)
        //            .WriteByte(1);

        //        Character.Map.Broadcast(oPacket, Character);
        //    }

        //    if (HasBuff)
        //    {
        //        Character.Buffs.Add(this, 0);
        //    }
        //}

        public byte[] ToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteInt(MapleID)
                    .WriteInt(CurrentLevel)
                    .WriteDateTime(Expiration);

                if (IsFromFourthJob)
                {
                    oPacket.WriteInt(MaxLevel);
                }

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }
    }
}
