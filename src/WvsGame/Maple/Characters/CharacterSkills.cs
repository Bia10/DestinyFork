using System.Collections.Generic;
using System.Collections.ObjectModel;

using Destiny.Data;
using Destiny.Constants;
using Destiny.IO;
using Destiny.Network.Common;
using Destiny.Network.PacketFactory;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterSkills : KeyedCollection<int, Skill>
    {
        public Character Parent { get; private set; }

        public CharacterSkills(Character parent)
             : base()
        {
            this.Parent = parent;
        }

        public void Load()
        {
            foreach (Datum datum in new Datums("skills").Populate("CharacterID = {0}", this.Parent.ID))
            {
                this.Add(new Skill(datum));
            }
        }

        public void Save()
        {
            foreach (Skill skill in this)
            {
                skill.Save();
            }
        }

        public void Delete()
        {
            foreach (Skill skill in this)
            {
                skill.Delete();
            }
        }

        public static Skill GetNewSkillFromInt(int skillMapleID)
        {
            return new Skill(skillMapleID);
        }

    public void CastSkillHandler(Packet inPacket)
        {
            inPacket.ReadInt(); // NOTE: Ticks.
            int mapleID = inPacket.ReadInt();
            byte level = inPacket.ReadByte();

            Skill skill = this[mapleID];

            if (level != skill.CurrentLevel)
            {
                return;
            }

            skill.Recalculate();
            skill.Cast();

            switch (skill.MapleID)
            {
                case (int)CharacterConstants.SkillNames.SuperGM.Resurrection:
                    {
                        byte targets = inPacket.ReadByte();

                        while (targets-- > 0)
                        {
                            int targetID = inPacket.ReadInt();

                            Character target = this.Parent.Map.Characters[targetID];

                            if (!target.IsAlive)
                            {
                                target.Health = target.MaxHealth;
                            }
                        }
                    }
                    break;
            }
        }

        public byte[] ToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket.WriteShort((short)this.Count);

                List<Skill> cooldownSkills = new List<Skill>();

                foreach (Skill loopSkill in this)
                {
                    oPacket.WriteBytes(loopSkill.ToByteArray());

                    if (loopSkill.IsCoolingDown)
                    {
                        cooldownSkills.Add(loopSkill);
                    }
                }

                oPacket.WriteShort((short)cooldownSkills.Count);

                foreach (Skill loopCooldown in cooldownSkills)
                {
                    oPacket
                        .WriteInt(loopCooldown.MapleID)
                        .WriteShort((short)loopCooldown.RemainingCooldownSeconds);
                }

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }

        protected override void InsertItem(int index, Skill item)
        {
            item.Parent = this;

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Skill item = base.Items[index];

            item.Parent = null;

            base.RemoveItem(index);
        }

        protected override int GetKeyForItem(Skill item)
        {
            return item.MapleID;
        }

        public void UpdateSkill(Skill skill)
        {
            skill.Character.Client.Send(CharacterSkillsPackets.UpdateSkill(skill));
        }

        public static bool IsHiddenSkill(int skill)
        {
            switch (skill)
            {
                case (int)CharacterConstants.SkillNames.Aran1.DoubleSwing:
                    return true;

                case (int)CharacterConstants.SkillNames.Aran2.TripleSwing:
                    return true;

                default:
                    return false;
            }
        }

        public static void ModifySkillLevel(Character character, Skill skillToMod, byte newLevel, byte newMaxlevel)
        {
            if (newLevel > 0 && newLevel <= newMaxlevel)
            {
                character.Skills.Add(new Skill(skillToMod.MapleID, newLevel, newMaxlevel));

                if (!IsHiddenSkill(skillToMod.MapleID))
                {
                    character.Skills.UpdateSkill(skillToMod);
                }
            }
            else
            {
                character.Skills.Remove(skillToMod);
                character.Skills.UpdateSkill(skillToMod);
            }
        }

    }
}