using System;
using System.Collections;
using System.Collections.Generic;

using Destiny.Data;
using Destiny.IO;
using Destiny.Network;
using Destiny.Constants;
using Destiny.Network.PacketFactory;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterBuffs : IEnumerable<Buff>
    {
        public Character Parent { get; private set; }
        private List<Buff> Buffs { get; set; }
        public Buff this[int mapleId]
        {
            get
            {
                foreach (Buff loopBuff in this.Buffs)
                {
                    if (loopBuff.MapleID == mapleId)
                    {
                        return loopBuff;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        public CharacterBuffs(Character parent)
            : base()
        {
            this.Parent = parent;

            this.Buffs = new List<Buff>();
        }

        public void Load()
        {
            foreach (Datum datum in new Datums("buffs").Populate("CharacterID = {0}", this.Parent.ID))
            {
                if ((DateTime)datum["End"] > DateTime.Now)
                {
                    this.AddBuff(new Buff(this, datum));
                }
            }
        }

        public void Save()
        {
            this.Delete();

            foreach (Buff loopBuff in this.Buffs)
            {
                loopBuff.Save();
            }
        }

        public void Delete()
        {
            Database.Delete("buffs", "CharacterID = {0}", this.Parent.ID);
        }

        public bool Contains(Buff buff)
        {
            return this.Buffs.Contains(buff);
        }

        public bool Contains(int mapleId)
        {
            foreach (Buff loopBuff in this.Buffs)
            {
                if (loopBuff.MapleID == mapleId)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddBuffBySkill(Skill skill, int value)
        {
            this.AddBuff(new Buff(this, skill, value));
        }

        public void AddBuff(Buff buff)
        {
            foreach (Buff loopBuff in this.Buffs)
            {
                if (loopBuff.MapleID == buff.MapleID)
                {
                    this.RemoveBuff(loopBuff);

                    break;
                }
            }

            buff.Parent = this;

            this.Buffs.Add(buff);

            if (this.Parent.IsInitialized && buff.Type == 1)
            {
                buff.Apply();
            }
        }

        public void RemoveBuffByID(int mapleId)
        {
            this.RemoveBuff(this[mapleId]);
        }

        public void RemoveBuff(Buff buff)
        {
            this.Buffs.Remove(buff);

            if (this.Parent.IsInitialized)
            {
                buff.Cancel();
            }
        }

        public void CancelBuffHandler(Packet inPacket)
        {
            int mapleID = inPacket.ReadInt();

            switch (mapleID)
            {
                // TODO: Handle special skills.
                default:
                    this.RemoveBuffByID(mapleID);
                    break;
            }
        }

        public IEnumerator<Buff> GetEnumerator()
        {
            return this.Buffs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Buffs).GetEnumerator();
        }

        public static void ShowOwnBuffEffect(Character character, Skill skill)
        {
            character.Client.Send(CharacterBuffsPackets.ShowMineBuffEffect(character, skill));
        }

        public void ShowRemoteBuffEffect(Character character, CharacterConstants.UserEffect effect, Skill skill, byte direction)
        {
            character.Map.Broadcast(CharacterBuffsPackets.ShowRemoteBuffEffect(character, effect, skill, direction), null);
        }

        public static void ShowLocalUserEffect(Character character, CharacterConstants.UserEffect effect)
        {
            character.Client.Send(CharacterBuffsPackets.ShowLocalBuffEffect(effect));
        }

        public static void ShowRemoteUserEffect(Character character, CharacterConstants.UserEffect effect, bool skipSelf = false)
        {
            character.Map.Broadcast(CharacterBuffsPackets.ShowRemoteBuffEffect(effect), skipSelf ? character : null);
        }

        // TODO: Refactor this to use actual TwoStateTemporaryStat and not some random values.
        // For now, we use the default mask until we learn more about how buffs work.
        public byte[] ToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteInt()
                    .WriteShort()
                    .WriteByte(0xFC)
                    .WriteByte(1)
                    .WriteInt();

                long mask = 0;
                int value = 0;

                if (this.Contains((int)CharacterConstants.SkillNames.Rogue.DarkSight))
                {
                    mask |= (long)CharacterConstants.SecondaryBuffStat.DarkSight;
                }

                if (this.Contains((int)CharacterConstants.SkillNames.Crusader.ComboAttack))
                {
                    mask |= (long)CharacterConstants.SecondaryBuffStat.Combo;
                    value = this[(int)CharacterConstants.SkillNames.Crusader.ComboAttack].Value;
                }

                if (this.Contains((int)CharacterConstants.SkillNames.Hermit.ShadowPartner))
                {
                    mask |= (long)CharacterConstants.SecondaryBuffStat.ShadowPartner;
                }

                if (this.Contains((int)CharacterConstants.SkillNames.Hunter.SoulArrowBow) 
                    || this.Contains((int)CharacterConstants.SkillNames.Crossbowman.SoulArrowCrossbow))
                {
                    mask |= (long)CharacterConstants.SecondaryBuffStat.SoulArrow;
                }

                oPacket.WriteInt((int)((mask >> 32) & 0xFFFFFFFFL));

                if (value != 0)
                {
                    oPacket.WriteByte((byte)value);
                }

                oPacket.WriteInt((int)(mask & 0xFFFFFFFFL));

                int magic = Application.Random.Next();

                oPacket
                    .Skip(6)
                    .WriteInt(magic)
                    .Skip(11)
                    .WriteInt(magic)
                    .Skip(11)
                    .WriteInt(magic)
                    .WriteShort()
                    .WriteByte()
                    .WriteLong()
                    .WriteInt(magic)
                    .Skip(9)
                    .WriteInt(magic)
                    .WriteShort()
                    .WriteInt()
                    .Skip(10)
                    .WriteInt(magic)
                    .Skip(13)
                    .WriteInt(magic)
                    .WriteShort()
                    .WriteByte();

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }
    }
}
