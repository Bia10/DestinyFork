using System;
using System.Collections.Generic;

using Destiny.Data;
using Destiny.Maple.Characters;
using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Life
{
    public sealed class MobSkill
    {
        public static Dictionary<short, List<int>> Summons { get; set; }

        public byte MapleID { get; private set; }
        public byte Level { get; private set; }
        public short EffectDelay { get; private set; }

        public int Duration { get; private set; }
        public short MpCost { get; private set; }
        public int ParameterA { get; private set; }
        public int ParameterB { get; private set; }
        public short Chance { get; private set; }
        public short TargetCount { get; private set; }
        public int Cooldown { get; private set; }
        public Point LT { get; private set; }
        public Point RB { get; private set; }
        public short PercentageLimitHP { get; private set; }
        public short SummonLimit { get; private set; }
        public short SummonEffect { get; private set; }

        public MobSkill(Datum datum)
        {
            MapleID = (byte)(int)datum["skillid"];
            Level = (byte)(short)datum["skill_level"];
            EffectDelay = (short)(short)datum["effect_delay"];
        }

        public void Load(Datum datum)
        {
            Duration = (short)datum["buff_time"];
            MpCost = (short)datum["mp_cost"];
            ParameterA = (int)datum["x_property"];
            ParameterB = (int)datum["y_property"];
            Chance = (short)datum["chance"];
            TargetCount = (short)datum["target_count"];
            Cooldown = (int)datum["cooldown"];
            LT = new Point((short)datum["ltx"], (short)datum["lty"]);
            RB = new Point((short)datum["rbx"], (short)datum["rby"]);
            PercentageLimitHP = (short)datum["hp_limit_percentage"];
            SummonLimit = (short)datum["summon_limit"];
            SummonEffect = (short)datum["summon_effect"];
        }

        public void Cast(Mob caster)
        {
            MobConstants.MobStatus status = MobConstants.MobStatus.None;
            CharacterConstants.CharacterDisease disease = CharacterConstants.CharacterDisease.None;
            bool heal = false;
            //bool banish = false;
            bool dispel = false;

            switch ((MobConstants.MobSkillName)MapleID)
            {
                case MobConstants.MobSkillName.WeaponAttackUp:
                case MobConstants.MobSkillName.WeaponAttackUpAreaOfEffect:
                case MobConstants.MobSkillName.WeaponAttackUpMonsterCarnival:
                    status = MobConstants.MobStatus.WeaponAttackUp;
                    break;

                case MobConstants.MobSkillName.MagicAttackUp:
                case MobConstants.MobSkillName.MagicAttackUpAreaOfEffect:
                case MobConstants.MobSkillName.MagicAttackUpMonsterCarnival:
                    status = MobConstants.MobStatus.MagicAttackUp;
                    break;

                case MobConstants.MobSkillName.WeaponDefenseUp:
                case MobConstants.MobSkillName.WeaponDefenseUpAreaOfEffect:
                case MobConstants.MobSkillName.WeaponDefenseUpMonsterCarnival:
                    status = MobConstants.MobStatus.WeaponDefenseUp;
                    break;

                case MobConstants.MobSkillName.MagicDefenseUp:
                case MobConstants.MobSkillName.MagicDefenseUpAreaOfEffect:
                case MobConstants.MobSkillName.MagicDefenseUpMonsterCarnival:
                    status = MobConstants.MobStatus.MagicDefenseUp;
                    break;

                case MobConstants.MobSkillName.HealAreaOfEffect:
                    heal = true;
                    break;

                case MobConstants.MobSkillName.Seal:
                    disease = CharacterConstants.CharacterDisease.Sealed;
                    break;

                case MobConstants.MobSkillName.Darkness:
                    disease = CharacterConstants.CharacterDisease.Darkness;
                    break;

                case MobConstants.MobSkillName.Weakness:
                    disease = CharacterConstants.CharacterDisease.Weaken;
                    break;

                case MobConstants.MobSkillName.Stun:
                    disease = CharacterConstants.CharacterDisease.Stun;
                    break;

                case MobConstants.MobSkillName.Curse:
                    disease = CharacterConstants.CharacterDisease.Curse;
                    break;

                case MobConstants.MobSkillName.Poison:
                    disease = CharacterConstants.CharacterDisease.Poison;
                    break;

                case MobConstants.MobSkillName.Slow:
                    disease = CharacterConstants.CharacterDisease.Slow;
                    break;

                case MobConstants.MobSkillName.Dispel:
                    dispel = true;
                    break;

                case MobConstants.MobSkillName.Seduce:
                    disease = CharacterConstants.CharacterDisease.Seduce;
                    break;

                case MobConstants.MobSkillName.SendToTown:
                    // TODO: Send to town.
                    break;

                case MobConstants.MobSkillName.PoisonMist:                  
                    // TODO: Spawn poison mist.
                    break;

                case MobConstants.MobSkillName.Confuse:
                    disease = CharacterConstants.CharacterDisease.Confuse;
                    break;

                case MobConstants.MobSkillName.Zombify:
                    // TODO: Zombify.
                    break;

                case MobConstants.MobSkillName.WeaponImmunity:
                    status = MobConstants.MobStatus.WeaponImmunity;
                    break;

                case MobConstants.MobSkillName.MagicImmunity:
                    status = MobConstants.MobStatus.MagicImmunity;
                    break;

                case MobConstants.MobSkillName.WeaponDamageReflect:
                case MobConstants.MobSkillName.MagicDamageReflect:
                case MobConstants.MobSkillName.AnyDamageReflect:
                    // TODO: Reflect.
                    break;

                case MobConstants.MobSkillName.AccuracyUpMonsterCarnival:
                case MobConstants.MobSkillName.AvoidabilityUpMonsterCarnival:
                case MobConstants.MobSkillName.SpeedUpMonsterCarnival:
                    // TODO: Monster carnival buffs.
                    break;

                case MobConstants.MobSkillName.Summon:

                    foreach (int mobId in MobSkill.Summons[Level])
                    {
                        Mob summon = new Mob(mobId)
                        {
                            Position = caster.Position,
                            SpawnEffect = SummonEffect
                        };

                        caster.Map.Mobs.Add(summon);
                    }
                    break;

                case MobConstants.MobSkillName.SpeedUpAreaOfEffect:
                    break;
                case MobConstants.MobSkillName.ArmorSkill:
                    break;
                case MobConstants.MobSkillName.SealMonsterCarnival:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (Mob affectedMob in GetAffectedMobs(caster))
            {
                if (heal)
                {
                    affectedMob.Heal((uint)ParameterA, ParameterB);
                }

                if (status != MobConstants.MobStatus.None && !affectedMob.Buffs.Contains(status))
                {
                    affectedMob.Buff(status, (short)ParameterA, this);
                }
            }

            foreach (Character affectedCharacter in GetAffectedCharacters(caster))
            {
                if (dispel)
                {
                    //affectedCharacter.Dispel();
                }

                /*
                if (banish)
                {
                    affectedCharacter.ChangeMap(affectedCharacter.Map.ReturnMapID);
                }
                */

                if (disease != CharacterConstants.CharacterDisease.None)
                {
                    using (Packet oPacket = new Packet(ServerOperationCode.TemporaryStatSet))
                    {
                        oPacket
                            .WriteLong()
                            .WriteLong((long)disease);

                        oPacket
                            .WriteShort((short)ParameterA)
                            .WriteShort(MapleID)
                            .WriteShort(Level)
                            .WriteInt(Duration);

                        oPacket
                            .WriteShort()
                            .WriteShort(900)
                            .WriteByte(1);

                        affectedCharacter.Client.Send(oPacket);
                    }

                    //map packet.
                }
            }

            caster.Mana -= (uint)MpCost;

            if (caster.Cooldowns.ContainsKey(this))
            {
                caster.Cooldowns[this] = DateTime.Now;
            }
            else
            {
                caster.Cooldowns.Add(this, DateTime.Now);
            }
        }

        private IEnumerable<Character> GetAffectedCharacters(Mob caster)
        {
            Rectangle boundingBox = new Rectangle(LT + caster.Position, RB + caster.Position);

            foreach (Character character in caster.Map.Characters)
            {
                if (character.Position.IsInRectangle(boundingBox))
                {
                    yield return character;
                }
            }
        }

        private IEnumerable<Mob> GetAffectedMobs(Mob caster)
        {
            Rectangle boundingBox = new Rectangle(LT + caster.Position, RB + caster.Position);

            foreach (Mob mob in caster.Map.Mobs)
            {
                if (mob.Position.IsInRectangle(boundingBox))
                {
                    yield return mob;
                }
            }
        }
    }
}
