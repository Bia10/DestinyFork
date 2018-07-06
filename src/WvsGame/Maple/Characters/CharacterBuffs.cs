using System;
using System.Collections;
using System.Collections.Generic;

using Destiny.Data;
using Destiny.IO;
using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.PacketFactory;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterBuffs : IEnumerable<Buff>
    {
        public Character Parent { get; }
        private List<Buff> Buffs { get; }
        public Buff this[int mapleId]
        {
            get
            {
                foreach (Buff loopBuff in Buffs)
                {
                    if (loopBuff.MapleID == mapleId)
                    {
                        return loopBuff;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        public CharacterBuffs(Character parent) : base()
        {
            Parent = parent;

            Buffs = new List<Buff>();
        }

        public void Load()
        {
            foreach (Datum datum in new Datums("buffs").Populate("CharacterID = {0}", Parent.ID))
            {
                if ((DateTime)datum["End"] > DateTime.Now)
                {
                    AddBuff(new Buff(this, datum));
                }
            }
        }

        public void Save()
        {
            Delete();

            foreach (Buff loopBuff in Buffs)
            {
                loopBuff.Save();
            }
        }

        public void Delete()
        {
            Database.Delete("buffs", "CharacterID = {0}", Parent.ID);
        }

        public bool Contains(Buff buff)
        {
            return Buffs.Contains(buff);
        }

        public bool Contains(int mapleId)
        {
            foreach (Buff loopBuff in Buffs)
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
            AddBuff(new Buff(this, skill, value));
        }

        public void AddBuff(Buff buff)
        {
            foreach (Buff loopBuff in Buffs)
            {
                if (loopBuff.MapleID != buff.MapleID) continue;

                RemoveBuff(loopBuff);

                break;
            }

            buff.Parent = this;

            Buffs.Add(buff);

            if (!Parent.IsInitialized || buff.Type != 1) return;

            buff.Apply();
        }

        public void RemoveBuffByID(int mapleId)
        {
            RemoveBuff(this[mapleId]);
        }

        public void RemoveBuff(Buff buff)
        {
            Buffs.Remove(buff);

            if (!Parent.IsInitialized) return;

            buff.Cancel();
        }

        public void CancelBuffHandler(Packet inPacket)
        {
            int mapleID = inPacket.ReadInt();

            switch (mapleID)
            {
                // TODO: Handle special skills.
                default:
                    RemoveBuffByID(mapleID);
                    break;
            }
        }

        public IEnumerator<Buff> GetEnumerator()
        {
            return Buffs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Buffs).GetEnumerator();
        }

        public static void ShowOwnBuffEffect(Character character, Skill skill)
        {
            character.Client.Send(CharacterBuffsPackets.ShowMineBuffEffect(character, skill));
        }

        public void ShowRemoteBuffEffect(Character character, CharacterConstants.UserEffect effect, Skill skill, byte direction)
        {
            character.Map.Broadcast(CharacterBuffsPackets.ShowRemoteBuffEffect(character, effect, skill, direction));
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

                if (Contains((int)CharacterConstants.SkillNames.Rogue.DarkSight))
                {
                    mask |= (long)CharacterConstants.SecondaryBuffStat.DarkSight;
                }

                if (Contains((int)CharacterConstants.SkillNames.Crusader.ComboAttack))
                {
                    mask |= (long)CharacterConstants.SecondaryBuffStat.Combo;
                    value = this[(int)CharacterConstants.SkillNames.Crusader.ComboAttack].Value;
                }

                if (Contains((int)CharacterConstants.SkillNames.Hermit.ShadowPartner))
                {
                    mask |= (long)CharacterConstants.SecondaryBuffStat.ShadowPartner;
                }

                if (Contains((int)CharacterConstants.SkillNames.Hunter.SoulArrowBow) 
                    || Contains((int)CharacterConstants.SkillNames.Crossbowman.SoulArrowCrossbow))
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
