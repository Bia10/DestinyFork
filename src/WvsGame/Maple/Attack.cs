using System.Collections.Generic;

using Destiny.Constants;
using Destiny.Network.Common;

namespace Destiny.Maple
{
    public sealed class Attack
    {
        public CharacterConstants.AttackType Type { get; private set; }
        public byte Portals { get; private set; }
        public int Targets { get; private set; }
        public int Hits { get; private set; }
        public int SkillID { get; private set; }

        public byte Display { get; private set; }
        public byte Animation { get; private set; }
        public byte WeaponClass { get; private set; }
        public byte WeaponSpeed { get; private set; }
        public int Ticks { get; private set; }

        public uint TotalDamage { get; private set; }
        public Dictionary<int, List<uint>> Damages { get; private set; }

        public Attack(Packet iPacket, CharacterConstants.AttackType type) //TODO: recheck this seems wrong
        {
            Type = type;
            Portals = iPacket.ReadByte();
            byte tByte = iPacket.ReadByte();
            Targets = tByte / 0x10;
            Hits = tByte % 0x10;
            SkillID = iPacket.ReadInt();

            if (SkillID > 0)
            {
            }

            iPacket.Skip(4); // NOTE: Unknown, probably CRC.
            iPacket.Skip(4); // NOTE: Unknown, probably CRC.
            iPacket.Skip(1); // NOTE: Unknown.
            Display = iPacket.ReadByte();
            Animation = iPacket.ReadByte();
            WeaponClass = iPacket.ReadByte();
            WeaponSpeed = iPacket.ReadByte();
            Ticks = iPacket.ReadInt();

            if (Type == CharacterConstants.AttackType.Range)
            {
                short starSlot = iPacket.ReadShort();
                short cashStarSlot = iPacket.ReadShort();
                iPacket.ReadByte(); // NOTE: Unknown.
            }

            Damages = new Dictionary<int, List<uint>>();

            for (int i = 0; i < Targets; i++)
            {
                int objectID = iPacket.ReadInt();
                iPacket.ReadInt(); // NOTE: Unknown.
                iPacket.ReadInt(); // NOTE: Mob position.
                iPacket.ReadInt(); // NOTE: Damage position.
                iPacket.ReadShort(); // NOTE: Distance.

                for (int j = 0; j < Hits; j++)
                {
                    uint damage = iPacket.ReadUInt();

                    if (!Damages.ContainsKey(objectID))
                    {
                        Damages.Add(objectID, new List<uint>());
                    }

                    Damages[objectID].Add(damage);

                    TotalDamage += damage;
                }

                if (Type != CharacterConstants.AttackType.Summon)
                {
                    iPacket.ReadInt(); // NOTE: Unknown, probably CRC.
                }
            }

            if (Type == CharacterConstants.AttackType.Range)
            {
                var rangedAttackPoint = new Point(iPacket.ReadShort(), iPacket.ReadShort());
            }

            var meleeAttackPoint = new Point(iPacket.ReadShort(), iPacket.ReadShort());
        }
    }
}
