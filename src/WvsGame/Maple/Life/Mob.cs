using System;
using System.Collections.Generic;

using Destiny.Data;
using Destiny.Maple.Characters;
using Destiny.Maple.Data;
using Destiny.Maple.Maps;
using Destiny.Constants;
using Destiny.IO;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;
using Destiny.Threading;

namespace Destiny.Maple.Life
{
    public sealed class Mob : MapObject, IMoveable, ISpawnable, IControllable
    {
        public int MapleID { get; private set; }
        public Character Controller { get; set; }
        public Dictionary<Character, uint> Attackers { get; private set; }
        public SpawnPoint SpawnPoint { get; private set; }
        public byte Stance { get; set; }
        public bool IsProvoked { get; set; }
        public bool CanDrop { get; set; }
        public List<Loot> Loots { get; private set; }
        public short Foothold { get; set; }
        public MobSkills Skills { get; private set; }
        public Dictionary<MobSkill, DateTime> Cooldowns { get; private set; }
        public List<MobConstants.MobStatus> Buffs { get; private set; }
        public List<int> DeathSummons { get; private set; }

        public short Level { get; private set; }
        public uint Health { get; set; }
        public uint Mana { get; set; }
        public uint MaxHealth { get; private set; }
        public uint MaxMana { get; private set; }
        public uint HealthRecovery { get; private set; }
        public uint ManaRecovery { get; private set; }
        public int ExplodeHealth { get; private set; }
        public uint Experience { get; private set; }
        public int Link { get; private set; }
        public short SummonType { get; private set; }
        public int KnockBack { get; private set; }
        public int FixedDamage { get; private set; }
        public int DeathBuff { get; private set; }
        public int DeathAfter { get; private set; }
        public double Traction { get; private set; }
        public int DamagedBySkillOnly { get; private set; }
        public int DamagedByMobOnly { get; private set; }
        public int DropItemPeriod { get; private set; }
        public byte HpBarForeColor { get; private set; }
        public byte HpBarBackColor { get; private set; }
        public byte CarnivalPoints { get; private set; }
        public int WeaponAttack { get; private set; }
        public int WeaponDefense { get; private set; }
        public int MagicAttack { get; private set; }
        public int MagicDefense { get; private set; }
        public short Accuracy { get; private set; }
        public short Avoidability { get; private set; }
        public short Speed { get; private set; }
        public short ChaseSpeed { get; private set; }

        public bool IsFacingLeft
        {
            get
            {
                return Stance % 2 == 0;
            }
        }

        public bool CanRespawn
        {
            get
            {
                return true; // TODO.
            }
        }

        public int SpawnEffect { get; set; }
        public int DeathEffect { get; set; }

        public Mob CachedReference
        {
            get
            {
                return DataProvider.Mobs[MapleID];
            }
        }

        public Mob(Datum datum) : base()
        {
            MapleID = (int)datum["mobid"];

            Level = (short)datum["mob_level"];
            Health = MaxHealth = (uint)datum["hp"];
            Mana = MaxMana = (uint)datum["mp"];
            HealthRecovery = (uint)datum["hp_recovery"];
            ManaRecovery = (uint)datum["mp_recovery"];
            ExplodeHealth = (int)datum["explode_hp"];
            Experience = (uint)datum["experience"];
            Link = (int)datum["link"];
            SummonType = (short)datum["summon_type"];
            KnockBack = (int)datum["knockback"];
            FixedDamage = (int)datum["fixed_damage"];
            DeathBuff = (int)datum["death_buff"];
            DeathAfter = (int)datum["death_after"];
            Traction = (double)datum["traction"];
            DamagedBySkillOnly = (int)datum["damaged_by_skill_only"];
            DamagedByMobOnly = (int)datum["damaged_by_mob_only"];
            DropItemPeriod = (int)datum["drop_item_period"];
            HpBarForeColor = (byte)(sbyte)datum["hp_bar_color"];
            HpBarBackColor = (byte)(sbyte)datum["hp_bar_bg_color"];
            CarnivalPoints = (byte)(sbyte)datum["carnival_points"];
            WeaponAttack = (int)datum["physical_attack"];
            WeaponDefense = (int)datum["physical_defense"];
            MagicAttack = (int)datum["magical_attack"];
            MagicDefense = (int)datum["magical_defense"];
            Accuracy = (short)datum["accuracy"];
            Avoidability = (short)datum["avoidability"];
            Speed = (short)datum["speed"];
            ChaseSpeed = (short)datum["chase_speed"];

            Loots = new List<Loot>();
            Skills = new MobSkills(this);
            DeathSummons = new List<int>();
        }

        public Mob(int mapleID)
        {
            MapleID = mapleID;

            Level = CachedReference.Level;
            Health = CachedReference.Health;
            Mana = CachedReference.Mana;
            MaxHealth = CachedReference.MaxHealth;
            MaxMana = CachedReference.MaxMana;
            HealthRecovery = CachedReference.HealthRecovery;
            ManaRecovery = CachedReference.ManaRecovery;
            ExplodeHealth = CachedReference.ExplodeHealth;
            Experience = CachedReference.Experience;
            Link = CachedReference.Link;
            SummonType = CachedReference.SummonType;
            KnockBack = CachedReference.KnockBack;
            FixedDamage = CachedReference.FixedDamage;
            DeathBuff = CachedReference.DeathBuff;
            DeathAfter = CachedReference.DeathAfter;
            Traction = CachedReference.Traction;
            DamagedBySkillOnly = CachedReference.DamagedBySkillOnly;
            DamagedByMobOnly = CachedReference.DamagedByMobOnly;
            DropItemPeriod = CachedReference.DropItemPeriod;
            HpBarForeColor = CachedReference.HpBarForeColor;
            HpBarBackColor = CachedReference.HpBarBackColor;
            CarnivalPoints = CachedReference.CarnivalPoints;
            WeaponAttack = CachedReference.WeaponAttack;
            WeaponDefense = CachedReference.WeaponDefense;
            MagicAttack = CachedReference.MagicAttack;
            MagicDefense = CachedReference.MagicDefense;
            Accuracy = CachedReference.Accuracy;
            Avoidability = CachedReference.Avoidability;
            Speed = CachedReference.Speed;
            ChaseSpeed = CachedReference.ChaseSpeed;

            Loots = CachedReference.Loots;
            Skills = CachedReference.Skills;
            DeathSummons = CachedReference.DeathSummons;

            Attackers = new Dictionary<Character, uint>();
            Cooldowns = new Dictionary<MobSkill, DateTime>();
            Buffs = new List<MobConstants.MobStatus>();
            Stance = 5;
            CanDrop = true;
        }

        public Mob(SpawnPoint spawnPoint) : this(spawnPoint.MapleID)
        {
            SpawnPoint = spawnPoint;
            Foothold = SpawnPoint.Foothold;
            Position = SpawnPoint.Position;
            Position.Y -= 1; // TODO: Is this needed?
        }

        public Mob(int mapleID, Point position) : this(mapleID)
        {
            Foothold = 0; // TODO.
            Position = position;
            Position.Y -= 5; // TODO: Is this needed?
        }

        public void AssignController()
        {
            if (Controller == null)
            {
                int leastControlled = int.MaxValue;
                Character newMobController = null;

                lock (Map.Characters)
                {
                    // check for all characters in current map
                    foreach (Character character in Map.Characters)
                    {
                        if (character.ControlledMobs.Count < leastControlled)
                        {
                            leastControlled = character.ControlledMobs.Count;
                            newMobController = character;
                        }
                    }
                }

                if (newMobController != null)
                {
                    // destroy agro on mob
                    IsProvoked = false;

                    try
                    {
                        if (!newMobController.ControlledMobs.Contains(ObjectID))
                        {
                            newMobController.ControlledMobs.Add(this);
                        }
                    }

                    catch (Exception e)
                    {
                        Log.SkipLine();
                        Log.Warn("AssignController() failed to add mobObject: {0} to newController.ControlledMobs! \n Exception occurred: {1}", ObjectID, e);
                        Log.SkipLine();
                    }
                    
                }
            }
        }

        public void SwitchController(Character newController)
        {
            lock (this)
            {
                if (Controller != newController)
                {
                    if (this != null)
                    {
                        if (Controller.ControlledMobs.Contains(this))
                        {
                            try
                            {
                                Controller.ControlledMobs.Remove(this);
                            }
                            catch (Exception e)
                            {
                                Log.SkipLine();
                                Log.Inform("ERROR: SwitchController() failed to remove mobObject: {0} from Controller.ControlledMobs! \n Exception occurred: {1}", ObjectID, e);
                                Log.SkipLine();
                            }
                        }

                        if (!newController.ControlledMobs.Contains(this))
                        {
                            try
                            {
                                newController.ControlledMobs.Add(this);
                            }
                            catch (Exception e)
                            {
                                Log.SkipLine();
                                Log.Inform("ERROR: SwitchController() failed to add mobObject: {0} to newController.ControlledMobs! \n Exception occurred: {1}", ObjectID, e);
                                Log.SkipLine();
                            }
                        }
                    }
                }
            }
        }

        public void Move(Packet inPacket)
        {
            short moveAction = inPacket.ReadShort();
            bool cheatResult = (inPacket.ReadByte() & 0xF) != 0;
            byte centerSplit = inPacket.ReadByte();
            int illegalVelocity = inPacket.ReadInt();
            inPacket.Skip(8);
            inPacket.ReadByte();
            inPacket.ReadInt();

            Movements movements = Movements.Decode(inPacket);

            Position = movements.Position;
            Foothold = movements.Foothold;
            Stance = movements.Stance;

            byte skillID = 0;
            byte skillLevel = 0;
            MobSkill skill = null;
            
            if (skill != null)
            {
                if (Health * 100 / MaxHealth > skill.PercentageLimitHP ||
                    (Cooldowns.ContainsKey(skill) && Cooldowns[skill].AddSeconds(skill.Cooldown) >= DateTime.Now) ||
                    ((MobConstants.MobSkillName)skill.MapleID) == MobConstants.MobSkillName.Summon && Map.Mobs.Count >= 100)
                {
                    skill = null;
                }
            }

            if (skill != null)
            {
                skill.Cast(this);
            }

            using (Packet oPacket = new Packet(ServerOperationCode.MobCtrlAck))
            {
                oPacket
                    .WriteInt(ObjectID)
                    .WriteShort(moveAction) //moveActionID
                    .WriteBool(cheatResult) //UseSkills??
                    .WriteShort((short)Mana)
                    .WriteByte(skillID)
                    .WriteByte(skillLevel);

                Controller.Client.Send(oPacket);
            }

            using (Packet oPacket = new Packet(ServerOperationCode.MobMove))
            {
                oPacket
                    .WriteInt(ObjectID)
                    .WriteBool(false)
                    .WriteBool(cheatResult) //UseSkills??
                    .WriteByte(centerSplit)
                    .WriteInt(illegalVelocity)
                    .WriteBytes(movements.ToByteArray());

                Map.Broadcast(oPacket, Controller);
            }
        }

        public void Buff(MobConstants.MobStatus buff, short value, MobSkill skill)
        {
            using (Packet oPacket = new Packet(ServerOperationCode.MobStatSet))
            {
                oPacket
                    .WriteInt(ObjectID)
                    .WriteLong()
                    .WriteInt()
                    .WriteInt((int)buff)
                    .WriteShort(value)
                    .WriteShort(skill.MapleID)
                    .WriteShort(skill.Level)
                    .WriteShort(-1)
                    .WriteShort(0) // Delay
                    .WriteInt();

                Map.Broadcast(oPacket);
            }

            Delay.Execute(() =>
            {
                using (Packet Packet = new Packet(ServerOperationCode.MobStatReset))
                {
                    Packet
                        .WriteInt(ObjectID)
                        .WriteLong()
                        .WriteInt()
                        .WriteInt((int)buff)
                        .WriteInt();

                    Map.Broadcast(Packet);
                }

                Buffs.Remove(buff);
            }, skill.Duration * 1000);
        }

        public void Buff(MobConstants.MobStatus buff, short value, Skill skill)
        {
            using (Packet oPacket = new Packet(ServerOperationCode.MobStatSet))
            {
                oPacket
                    .WriteInt(ObjectID)
                    .WriteLong()
                    .WriteInt()
                    .WriteInt((int)buff)
                    .WriteShort(value)
                    .WriteShort((short)skill.MapleID)
                    .WriteShort(skill.CurrentLevel)
                    .WriteShort(-1)
                    .WriteShort(0) // Delay
                    .WriteInt();

                Map.Broadcast(oPacket);
            }

            Delay.Execute(() =>
                {
                    using (Packet Packet = new Packet(ServerOperationCode.MobStatReset))
                    {
                        Packet
                            .WriteInt(ObjectID)
                            .WriteLong()
                            .WriteInt()
                            .WriteInt((int)buff)
                            .WriteInt();

                        Map.Broadcast(Packet);
                    }

                    Buffs.Remove(buff);
                }, skill.BuffTime * 1000);
        }

        public void Heal(uint hp, int range)
        {
            Health = Math.Min(MaxHealth, (uint)(Health + hp + Application.Random.Next(-range / 2, range / 2)));

            using (Packet Packet = new Packet(ServerOperationCode.MobDamaged))
            {
                Packet
                    .WriteInt(ObjectID)
                    .WriteByte()
                    .WriteInt((int)-hp)
                    .WriteByte()
                    .WriteByte()
                    .WriteByte();

                Map.Broadcast(Packet);
            }
        }

        public void Die()
        {
            try
            {
                Map.Mobs.Remove(this);
            }
            catch (Exception ex)
            {
                Log.SkipLine();
                Tracer.TraceErrorMessage(ex, "Failed to remove Mob on death from Map.Mobs");
                Log.SkipLine();
            }
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public void ShowHpTo(Character player)
        {
            int hpRemaining = (int)(Math.Max(1, (Health * 100) / MaxHealth));

            using (Packet oPacket = new Packet(ServerOperationCode.MobHPIndicator))
            {
                oPacket
                    .WriteInt(ObjectID)
                    .WriteByte((byte)(hpRemaining));

                player.Client.Send(oPacket);
            }
        }

        public bool Damage(Character attacker, uint amount)
        {
            lock (this)
            {
                amount = Math.Min(amount, Health);

                //does the victim knows its attacker?
                if (Attackers.ContainsKey(attacker))
                {
                    //if so then add to his established dmg bill
                    Attackers[attacker] += amount;
                }
                else
                {
                    //if not so then add him as new attacker
                    Attackers.Add(attacker, amount);
                }
                
                Health -= amount; //decrease health by amount dealt
                ShowHpTo(attacker); //show monster's remaining hp bar to attacker

                if (Health <= 0) //???
                {
                    return true;
                }

                return false;
            }
        }

        public Packet GetCreatePacket()
        {
            return GetInternalPacket(false, true);
        }

        public Packet GetSpawnPacket()
        {
            return GetInternalPacket(false, false);
        }

        public Packet GetControlRequestPacket()
        {
            return GetInternalPacket(true, false);
        }

        private Packet GetInternalPacket(bool requestControl, bool newSpawn)
        {
            Packet oPacket = new Packet(requestControl ? ServerOperationCode.MobChangeController : ServerOperationCode.MobEnterField);

            if (requestControl)
            {
                oPacket.WriteByte((byte)(IsProvoked ? 2 : 1));
            }

            oPacket
                .WriteInt(ObjectID)
                .WriteByte((byte)(Controller == null ? 5 : 1))
                .WriteInt(MapleID)
                .Skip(15) // NOTE: Unknown.
                .WriteByte(0x88) // NOTE: Unknown.
                .Skip(6) // NOTE: Unknown.
                .WriteShort(Position.X)
                .WriteShort(Position.Y)
                .WriteByte((byte)(0x02 | (IsFacingLeft ? 0x01 : 0x00))) // implement: getStance()
                .WriteShort(Foothold) //foothold of origin MapObject.GetStartFH()
                .WriteShort(Foothold);

            if (SpawnEffect > 0)
            {
                oPacket
                    .WriteByte((byte)SpawnEffect)
                    .WriteByte()
                    .WriteShort();

                if (SpawnEffect == 15)
                {
                    oPacket.WriteByte();
                }
            }

            oPacket
                .WriteByte((byte)(newSpawn ? -1 : -2)) //-2 : -1, seems wrong 
                .WriteByte() //??
                .WriteByte(byte.MaxValue) // NOTE: Carnival team.
                .WriteInt(); // NOTE: Unknown.

            return oPacket;
        }

        public Packet GetControlCancelPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.MobChangeController);

            oPacket
                .WriteBool(false)
                .WriteInt(ObjectID);

            return oPacket;
        }

        public enum DeathEffects
        {
            Dissapear,
            FadeOut,
            Special1,
            Special2,
            Special3
        }

        public Packet GetDestroyPacket(DeathEffects deathEffect)
        {
            Packet oPacket = new Packet(ServerOperationCode.MobLeaveField);

            oPacket
                .WriteInt(ObjectID)
                .WriteByte(1)
                .WriteByte((byte)DeathEffect);

            return oPacket;
        }

        public Packet GetDestroyPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.MobLeaveField);

            oPacket
                .WriteInt(ObjectID)
                .WriteByte(1)
                .WriteByte(1); // TODO: Death effects.

            return oPacket;
        }
    }
}