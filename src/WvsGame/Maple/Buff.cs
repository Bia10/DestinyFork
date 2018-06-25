using System;
using System.Collections.Generic;

using Destiny.Data;
using Destiny.Maple.Characters;
using Destiny.Maple.Data;
using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;
using Destiny.Threading;

namespace Destiny.Maple
{
    public sealed class Buff
    {
        public CharacterBuffs Parent { get; set; }

        public int MapleID { get; set; }
        public byte SkillLevel { get; set; }
        public byte Type { get; set; }
        public Dictionary<CharacterConstants.PrimaryBuffStat, short> PrimaryStatups { get; set; }
        public Dictionary<CharacterConstants.SecondaryBuffStat, short> SecondaryStatups { get; set; }
        public DateTime End { get; set; }
        public int Value { get; set; }

        public Character Character
        {
            get
            {
                return Parent.Parent;
            }
        }

        public long PrimaryBuffMask
        {
            get
            {
                long mask = 0;

                foreach (KeyValuePair<CharacterConstants.PrimaryBuffStat, short> primaryStatup in PrimaryStatups)
                {
                    mask |= (long)primaryStatup.Key;
                }

                return mask;
            }
        }

        public long SecondaryBuffMask
        {
            get
            {
                long mask = 0;

                foreach (KeyValuePair<CharacterConstants.SecondaryBuffStat, short> secondaryStatus in SecondaryStatups)
                {
                    mask |= (long)secondaryStatus.Key;
                }

                return mask;
            }
        }

        public Buff(CharacterBuffs parent, Skill skill, int value)
        {
            Parent = parent;
            MapleID = skill.MapleID;
            SkillLevel = skill.CurrentLevel;
            Type = 1;
            Value = value;
            End = DateTime.Now.AddSeconds(skill.BuffTime);
            PrimaryStatups = new Dictionary<CharacterConstants.PrimaryBuffStat, short>();
            SecondaryStatups = new Dictionary<CharacterConstants.SecondaryBuffStat, short>();

            CalculateStatups(skill);

            Delay.Execute(() =>
            {
                if (Parent.Contains(this))
                {
                    Parent.RemoveBuff(this);
                }
            }, (int)(End - DateTime.Now).TotalMilliseconds);
        }

        public Buff(CharacterBuffs parent, Datum datum)
        {
            Parent = parent;
            MapleID = (int)datum["MapleID"];
            SkillLevel = (byte)datum["SkillLevel"];
            Type = (byte)datum["Type"];
            Value = (int)datum["Value"];
            End = (DateTime)datum["End"];
            PrimaryStatups = new Dictionary<CharacterConstants.PrimaryBuffStat, short>();
            SecondaryStatups = new Dictionary<CharacterConstants.SecondaryBuffStat, short>();

            if (Type == 1)
            {
                CalculateStatups(DataProvider.Skills[MapleID][SkillLevel]);
            }

            Delay.Execute(() =>
            {
                if (Parent.Contains(this))
                {
                    Parent.RemoveBuff(this);
                }
            }, (int)(End - DateTime.Now).TotalMilliseconds);
        }

        public void Save()
        {
            Datum datum = new Datum("buffs")
            {
                ["CharacterID"] = Character.ID,
                ["MapleID"] = MapleID,
                ["SkillLevel"] = SkillLevel,
                ["Type"] = Type,
                ["Value"] = Value,
                ["End"] = End
            };


            datum.Insert();
        }

        public void Apply()
        {
            switch (MapleID)
            {
                default:
                    {
                        using (Packet oPacket = new Packet(ServerOperationCode.TemporaryStatSet))
                        {
                            oPacket
                                .WriteLong(PrimaryBuffMask)
                                .WriteLong(SecondaryBuffMask);

                            foreach (KeyValuePair<CharacterConstants.PrimaryBuffStat, short> primaryStatup in PrimaryStatups)
                            {
                                oPacket
                                    .WriteShort(primaryStatup.Value)
                                    .WriteInt(MapleID)
                                    .WriteInt((int)(End - DateTime.Now).TotalMilliseconds);
                            }

                            foreach (KeyValuePair<CharacterConstants.SecondaryBuffStat, short> secondaryStatup in SecondaryStatups)
                            {
                                oPacket
                                    .WriteShort(secondaryStatup.Value)
                                    .WriteInt(MapleID)
                                    .WriteInt((int)(End - DateTime.Now).TotalMilliseconds);
                            }

                            oPacket
                                .WriteShort()
                                .WriteShort()
                                .WriteByte()
                                .WriteInt();

                            Character.Client.Send(oPacket);
                        }

                        using (Packet oPacket = new Packet(ServerOperationCode.SetTemporaryStat))
                        {
                            oPacket
                                .WriteInt(Character.ID)
                                .WriteLong(PrimaryBuffMask)
                                .WriteLong(SecondaryBuffMask);

                            foreach (KeyValuePair<CharacterConstants.PrimaryBuffStat, short> primaryStatup in PrimaryStatups)
                            {
                                oPacket.WriteShort(primaryStatup.Value);
                            }

                            foreach (KeyValuePair<CharacterConstants.SecondaryBuffStat, short> secondaryStatup in SecondaryStatups)
                            {
                                oPacket.WriteShort(secondaryStatup.Value);
                            }

                            oPacket
                                .WriteInt()
                                .WriteShort();

                            Character.Map.Broadcast(oPacket);
                        }
                    }
                    break;
            }
        }

        public void Cancel()
        {
            using (Packet oPacket = new Packet(ServerOperationCode.TemporaryStatReset))
            {
                oPacket
                    .WriteLong(PrimaryBuffMask)
                    .WriteLong(SecondaryBuffMask)
                    .WriteByte(1);

                Character.Client.Send(oPacket);
            }

            using (Packet oPacket = new Packet(ServerOperationCode.ResetTemporaryStat))
            {
                oPacket
                    .WriteInt(Character.ID)
                    .WriteLong(PrimaryBuffMask)
                    .WriteLong(SecondaryBuffMask);

                Character.Map.Broadcast(oPacket);
            }
        }

        public void CalculateStatups(Skill skill)
        {
            if (skill.WeaponAttack > 0)
            {
                SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.WeaponAttack, skill.WeaponAttack);
            }

            if (skill.WeaponDefense > 0)
            {
                SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.WeaponDefense, skill.WeaponDefense);
            }

            if (skill.MagicAttack > 0)
            {
                SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.MagicAttack, skill.MagicAttack);
            }

            if (skill.MagicDefense > 0)
            {
                SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.MagicDefense, skill.MagicDefense);
            }

            if (skill.Accuracy > 0)
            {
                SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Accuracy, skill.Accuracy);
            }

            if (skill.Avoidability > 0)
            {
                SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Avoid, skill.Avoidability);
            }

            if (skill.Speed > 0)
            {
                SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Speed, skill.Speed);
            }

            if (skill.Jump > 0)
            {
                SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Jump, skill.Jump);
            }

            if (skill.Morph > 0)
            {
                SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Morph, (short)(skill.Morph + 100 * (int)Character.Appearance.Gender));
            }

            switch (MapleID)
            {
                case (int)CharacterConstants.SkillNames.SuperGM.HyperBody:
                    SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.HyperBodyHP, skill.ParameterA);
                    SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.HyperBodyMP, skill.ParameterB);
                    break;

                case (int)CharacterConstants.SkillNames.SuperGM.HolySymbol:
                    SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.HolySymbol, skill.ParameterA);
                    break;

                case (int)CharacterConstants.SkillNames.SuperGM.Hide:
                    SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.DarkSight, skill.ParameterA);
                    break;
            }
        }
    }
}
