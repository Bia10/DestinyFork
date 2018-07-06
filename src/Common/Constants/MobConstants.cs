using System;

namespace Destiny.Constants
{
    public static class MobConstants
    {
        #region Mobs
        [Flags]
        public enum MobStatus
        {
            None,

            WeaponAttackIcon = 0x01,
            WeaponDefenceIcon = 0x02,
            MagicAttackIcon = 0x04,
            MagicDefenceIcon = 0x08,
            AccuracyIcon = 0x10,
            AvoidabilityIcon = 0x20,
            SpeedIcon = 0x40,

            Stunned = 0x80,
            Frozen = 0x100,
            Poisoned = 0x200,
            Sealed = 0x400,

            ShowDown = 0x800,

            WeaponAttackUp = 0x1000,
            WeaponDefenseUp = 0x2000,
            MagicAttackUp = 0x4000,
            MagicDefenseUp = 0x8000,

            Doom = 0x10000,
            ShadowWeb = 0x20000,

            WeaponImmunity = 0x40000,
            MagicImmunity = 0x80000,

            Unknown2 = 0x100000,
            HardSkin = 0x200000,
            NinjaAmbush = 0x400000,
            ElementalAttribute = 0x800000,
            VenomousWeapon = 0x1000000,
            Blind = 0x2000000,
            SealSkill = 0x4000000,
            Empty = 0x8000000,
            Hypnotized = 0x10000000,
            WeaponDamageReflect = 0x20000000,
            MagicDamageReflect = 0x40000000
        }

        public enum MobSkillName
        {
            WeaponAttackUp = 100,
            MagicAttackUp = 101,
            WeaponDefenseUp = 102,
            MagicDefenseUp = 103,

            WeaponAttackUpAreaOfEffect = 110,
            MagicAttackUpAreaOfEffect = 111,
            WeaponDefenseUpAreaOfEffect = 112,
            MagicDefenseUpAreaOfEffect = 113,
            HealAreaOfEffect = 114,
            SpeedUpAreaOfEffect = 115,

            Seal = 120,
            Darkness = 121,
            Weakness = 122,
            Stun = 123,
            Curse = 124,
            Poison = 125,
            Slow = 126,
            Dispel = 127,
            Seduce = 128,
            SendToTown = 129,
            PoisonMist = 131,
            Confuse = 132,
            Zombify = 133,

            WeaponImmunity = 140,
            MagicImmunity = 141,
            ArmorSkill = 142,

            WeaponDamageReflect = 143,
            MagicDamageReflect = 144,
            AnyDamageReflect = 145,

            WeaponAttackUpMonsterCarnival = 150,
            MagicAttackUpMonsterCarnival = 151,
            WeaponDefenseUpMonsterCarnival = 152,
            MagicDefenseUpMonsterCarnival = 153,
            AccuracyUpMonsterCarnival = 154,
            AvoidabilityUpMonsterCarnival = 155,
            SpeedUpMonsterCarnival = 156,
            SealMonsterCarnival = 157,

            Summon = 200
        }
        #endregion
    }
}
