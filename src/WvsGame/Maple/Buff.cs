﻿using System;
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
                return this.Parent.Parent;
            }
        }

        public long PrimaryBuffMask
        {
            get
            {
                long mask = 0;

                foreach (KeyValuePair<CharacterConstants.PrimaryBuffStat, short> primaryStatup in this.PrimaryStatups)
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

                foreach (KeyValuePair<CharacterConstants.SecondaryBuffStat, short> secondaryStatus in this.SecondaryStatups)
                {
                    mask |= (long)secondaryStatus.Key;
                }

                return mask;
            }
        }

        public Buff(CharacterBuffs parent, Skill skill, int value)
        {
            this.Parent = parent;
            this.MapleID = skill.MapleID;
            this.SkillLevel = skill.CurrentLevel;
            this.Type = 1;
            this.Value = value;
            this.End = DateTime.Now.AddSeconds(skill.BuffTime);
            this.PrimaryStatups = new Dictionary<CharacterConstants.PrimaryBuffStat, short>();
            this.SecondaryStatups = new Dictionary<CharacterConstants.SecondaryBuffStat, short>();

            this.CalculateStatups(skill);

            Delay.Execute(() =>
            {
                if (this.Parent.Contains(this))
                {
                    this.Parent.RemoveBuff(this);
                }
            }, (int)(this.End - DateTime.Now).TotalMilliseconds);
        }

        public Buff(CharacterBuffs parent, Datum datum)
        {
            this.Parent = parent;
            this.MapleID = (int)datum["MapleID"];
            this.SkillLevel = (byte)datum["SkillLevel"];
            this.Type = (byte)datum["Type"];
            this.Value = (int)datum["Value"];
            this.End = (DateTime)datum["End"];
            this.PrimaryStatups = new Dictionary<CharacterConstants.PrimaryBuffStat, short>();
            this.SecondaryStatups = new Dictionary<CharacterConstants.SecondaryBuffStat, short>();

            if (this.Type == 1)
            {
                this.CalculateStatups(DataProvider.Skills[this.MapleID][this.SkillLevel]);
            }

            Delay.Execute(() =>
            {
                if (this.Parent.Contains(this))
                {
                    this.Parent.RemoveBuff(this);
                }
            }, (int)(this.End - DateTime.Now).TotalMilliseconds);
        }

        public void Save()
        {
            Datum datum = new Datum("buffs");

            datum["CharacterID"] = this.Character.ID;
            datum["MapleID"] = this.MapleID;
            datum["SkillLevel"] = this.SkillLevel;
            datum["Type"] = this.Type;
            datum["Value"] = this.Value;
            datum["End"] = this.End;

            datum.Insert();
        }

        public void Apply()
        {
            switch (this.MapleID)
            {
                default:
                    {
                        using (Packet oPacket = new Packet(ServerOperationCode.TemporaryStatSet))
                        {
                            oPacket
                                .WriteLong(this.PrimaryBuffMask)
                                .WriteLong(this.SecondaryBuffMask);

                            foreach (KeyValuePair<CharacterConstants.PrimaryBuffStat, short> primaryStatup in this.PrimaryStatups)
                            {
                                oPacket
                                    .WriteShort(primaryStatup.Value)
                                    .WriteInt(this.MapleID)
                                    .WriteInt((int)(this.End - DateTime.Now).TotalMilliseconds);
                            }

                            foreach (KeyValuePair<CharacterConstants.SecondaryBuffStat, short> secondaryStatup in this.SecondaryStatups)
                            {
                                oPacket
                                    .WriteShort(secondaryStatup.Value)
                                    .WriteInt(this.MapleID)
                                    .WriteInt((int)(this.End - DateTime.Now).TotalMilliseconds);
                            }

                            oPacket
                                .WriteShort()
                                .WriteShort()
                                .WriteByte()
                                .WriteInt();

                            this.Character.Client.Send(oPacket);
                        }

                        using (Packet oPacket = new Packet(ServerOperationCode.SetTemporaryStat))
                        {
                            oPacket
                                .WriteInt(this.Character.ID)
                                .WriteLong(this.PrimaryBuffMask)
                                .WriteLong(this.SecondaryBuffMask);

                            foreach (KeyValuePair<CharacterConstants.PrimaryBuffStat, short> primaryStatup in this.PrimaryStatups)
                            {
                                oPacket.WriteShort(primaryStatup.Value);
                            }

                            foreach (KeyValuePair<CharacterConstants.SecondaryBuffStat, short> secondaryStatup in this.SecondaryStatups)
                            {
                                oPacket.WriteShort(secondaryStatup.Value);
                            }

                            oPacket
                                .WriteInt()
                                .WriteShort();

                            this.Character.Map.Broadcast(oPacket);
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
                    .WriteLong(this.PrimaryBuffMask)
                    .WriteLong(this.SecondaryBuffMask)
                    .WriteByte(1);

                this.Character.Client.Send(oPacket);
            }

            using (Packet oPacket = new Packet(ServerOperationCode.ResetTemporaryStat))
            {
                oPacket
                    .WriteInt(this.Character.ID)
                    .WriteLong(this.PrimaryBuffMask)
                    .WriteLong(this.SecondaryBuffMask);

                this.Character.Map.Broadcast(oPacket);
            }
        }

        public void CalculateStatups(Skill skill)
        {
            if (skill.WeaponAttack > 0)
            {
                this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.WeaponAttack, skill.WeaponAttack);
            }

            if (skill.WeaponDefense > 0)
            {
                this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.WeaponDefense, skill.WeaponDefense);
            }

            if (skill.MagicAttack > 0)
            {
                this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.MagicAttack, skill.MagicAttack);
            }

            if (skill.MagicDefense > 0)
            {
                this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.MagicDefense, skill.MagicDefense);
            }

            if (skill.Accuracy > 0)
            {
                this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Accuracy, skill.Accuracy);
            }

            if (skill.Avoidability > 0)
            {
                this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Avoid, skill.Avoidability);
            }

            if (skill.Speed > 0)
            {
                this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Speed, skill.Speed);
            }

            if (skill.Jump > 0)
            {
                this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Jump, skill.Jump);
            }

            if (skill.Morph > 0)
            {
                this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.Morph, (short)(skill.Morph + 100 * (int)this.Character.Appearance.Gender));
            }

            switch (this.MapleID)
            {
                case (int)CharacterConstants.SkillNames.SuperGM.HyperBody:
                    this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.HyperBodyHP, skill.ParameterA);
                    this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.HyperBodyMP, skill.ParameterB);
                    break;

                case (int)CharacterConstants.SkillNames.SuperGM.HolySymbol:
                    this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.HolySymbol, skill.ParameterA);
                    break;

                case (int)CharacterConstants.SkillNames.SuperGM.Hide:
                    this.SecondaryStatups.Add(CharacterConstants.SecondaryBuffStat.DarkSight, skill.ParameterA);
                    break;
            }
        }
    }
}
