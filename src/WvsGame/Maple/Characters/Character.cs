using System;
using System.Collections.Generic;
using System.Linq;

using Destiny.Maple.Maps;
using Destiny.Constants;
using Destiny.Maple.Commands;
using Destiny.Maple.Life;
using Destiny.Data;
using Destiny.Maple.Data;
using Destiny.Maple.Interaction;
using Destiny.Network;
using Destiny.IO;
using Destiny.Maple.Scripting;
using Destiny.Network.Common;
using Destiny.Network.PacketFactory;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Characters
{
    public sealed class Character : MapObject, IMoveable, ISpawnable
    {
        public GameClient Client { get; private set; }

        public int ID { get; set; }
        public int AccountID { get; set; }
        public byte WorldID { get; set; }
        public string Name { get; set; }
        public bool IsInitialized { get; private set; }

        public byte SpawnPoint { get; set; }
        public byte Stance { get; set; }
        public short Foothold { get; set; }
        public byte Portals { get; set; }
        public int Chair { get; set; }
        public int GuildRank { get; set; }

        public CharacterItems Items { get; private set; }
        public CharacterJobs Jobs { get; private set; }
        public CharacterStats Stats { get; private set; }
        public CharacterAppearance Appearance { get; private set; }
        public CharacterSkills Skills { get; private set; }
        public CharacterQuests Quests { get; private set; }
        public CharacterBuffs Buffs { get; private set; }
        public CharacterKeymap Keymap { get; private set; }
        public CharacterTrocks Trocks { get; private set; }
        public CharacterMemos Memos { get; private set; }
        public CharacterStorage Storage { get; private set; }
        public CharacterVariables Variables { get; private set; }
        public ControlledMobs ControlledMobs { get; private set; }
        public ControlledNpcs ControlledNpcs { get; private set; }
        public Trade Trade { get; set; }
        public PlayerShop PlayerShop { get; set; }
             
        private Npc lastNpc;
        private Quest lastQuest;
        private string chalkboard;

        public bool IsAlive
        {
            get { return this.Stats.Health > 0; }
        }

        public bool IsMaster
        {
            get
            {
                //TODO: Add GM levels and/or character-specific GM rank
                //TODO: Check for data in login DB
                return true;
            }
        }

        public bool FacesLeft
        {
            get { return this.Stance % 2 == 0; }
        }

        public bool IsRanked
        {
            get { return this.Stats.Level >= 30; }
        }

        public Npc LastNpc
        {
            get { return lastNpc; }
            set
            {
                if (value == null)
                {
                    try
                    {
                        if (value.Scripts.ContainsKey(this))
                        {
                            value.Scripts.Remove(this);
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        Log.SkipLine();
                        Log.Error("Character-LastNPC thrown null exception!");
                        Log.SkipLine();
                        throw;
                    }
                    catch (Exception e)
                    {
                        Log.SkipLine();
                        Log.Error("Character-LastNPC thrown exception: {0}!", e);
                        Log.SkipLine();
                    }
                }

                lastNpc = value;
            }
        }

        public Quest LastQuest
        {
            get { return lastQuest; }
            set
            {
                lastQuest = value;
                // TODO: Add checks.
            }
        }

        public string Chalkboard
        {
            get { return chalkboard; }
            set
            {
                chalkboard = value;

                this.Map.Broadcast(CharacterPackets.SetChalkboardPacket(this));
            }
        }

        public Portal ClosestPortal
        {
            get
            {
                Portal closestPortal = null;
                double shortestDistance = double.PositiveInfinity;

                foreach (Portal loopPortal in this.Map.Portals)
                {
                    double distance = loopPortal.Position.DistanceFrom(this.Position);

                    if (distance < shortestDistance)
                    {
                        closestPortal = loopPortal;
                        shortestDistance = distance;
                    }
                }

                return closestPortal;
            }
        }

        public Portal ClosestSpawnPoint
        {
            get
            {
                Portal closestPortal = null;
                double shortestDistance = double.PositiveInfinity;

                foreach (Portal loopPortal in this.Map.Portals)
                {
                    if (loopPortal.IsSpawnPoint)
                    {
                        double distance = loopPortal.Position.DistanceFrom(this.Position);

                        if (distance < shortestDistance)
                        {
                            closestPortal = loopPortal;
                            shortestDistance = distance;
                        }
                    }
                }

                return closestPortal;
            }
        }

        private bool Assigned { get; set; }

        // default character CTOR
        public Character(int id = 0, GameClient client = null)
        {
            // Todo: into settings
            const int START_EQP_SLOTS = 24;
            const int START_USE_SLOTS = 24;
            const int START_SETUP_SLOTS = 24;
            const int START_ETC_SLOTS = 24;
            const int START_CASH_SLOTS = 48;

            this.ID = id;
            this.Client = client;

            this.Items = new CharacterItems(this, START_EQP_SLOTS, START_USE_SLOTS, START_SETUP_SLOTS, START_ETC_SLOTS, START_CASH_SLOTS);
            this.Jobs = new CharacterJobs(this);
            this.Stats = new CharacterStats(this);
            this.Appearance = new CharacterAppearance(this);
            this.Skills = new CharacterSkills(this);
            this.Quests = new CharacterQuests(this);
            this.Buffs = new CharacterBuffs(this);
            this.Keymap = new CharacterKeymap(this);
            this.Trocks = new CharacterTrocks(this);
            this.Memos = new CharacterMemos(this);
            this.Storage = new CharacterStorage(this);
            this.Variables = new CharacterVariables(this);

            this.Position = new Point(0, 0);
            this.ControlledMobs = new ControlledMobs(this);
            this.ControlledNpcs = new ControlledNpcs(this);
        }

        public void Load()
        {
            Datum datum = new Datum("characters");

            datum.Populate("ID = {0}", this.ID);

            this.ID = (int)datum["ID"];
            this.Assigned = true;

            this.AccountID = (int)datum["AccountID"];
            this.WorldID = (byte)datum["WorldID"];
            this.Name = (string)datum["Name"];
            this.Appearance.Gender = (CharacterConstants.Gender)datum["Gender"];
            this.Appearance.Skin = (byte)datum["Skin"];
            this.Appearance.Face = (int)datum["Face"];
            this.Appearance.Hair = (int)datum["Hair"];
            this.Stats.Level = (byte)datum["Level"];
            this.Jobs.Job = (CharacterConstants.Job)datum["Job"];
            this.Stats.Strength = (short)datum["Strength"];
            this.Stats.Dexterity = (short)datum["Dexterity"];
            this.Stats.Intelligence = (short)datum["Intelligence"];
            this.Stats.Luck = (short)datum["Luck"];
            this.Stats.MaxHealth = (short)datum["MaxHealth"];
            this.Stats.MaxMana = (short)datum["MaxMana"];
            this.Stats.Health = (short)datum["Health"];
            this.Stats.Mana = (short)datum["Mana"];
            this.Stats.AbilityPoints = (short)datum["AbilityPoints"];
            this.Stats.SkillPoints = (short)datum["SkillPoints"];
            this.Stats.Experience = (int)datum["Experience"];
            this.Stats.Fame = (short)datum["Fame"];
            this.Map = DataProvider.Maps[(int)datum["MapID"]];
            this.SpawnPoint = (byte)datum["SpawnPoint"];
            this.Stats.Meso = (int)datum["Meso"];

            this.Items.MaxSlots[ItemConstants.ItemType.Equipment] = (byte)datum["EquipmentSlots"];
            this.Items.MaxSlots[ItemConstants.ItemType.Usable] = (byte)datum["UsableSlots"];
            this.Items.MaxSlots[ItemConstants.ItemType.Setup] = (byte)datum["SetupSlots"];
            this.Items.MaxSlots[ItemConstants.ItemType.Etcetera] = (byte)datum["EtceteraSlots"];
            this.Items.MaxSlots[ItemConstants.ItemType.Cash] = (byte)datum["CashSlots"];

            this.Items.LoadInventory();
            this.Skills.Load();
            this.Quests.Load();
            this.Buffs.Load();
            this.Keymap.Load();
            this.Trocks.Load();
            this.Memos.Load();
            this.Variables.Load();
        }

        public void Save()
        {
            if (this.IsInitialized)
            {
                this.SpawnPoint = this.ClosestSpawnPoint.ID;
            }

            Datum datum = new Datum("characters") {
                ["AccountID"] = this.AccountID,
                ["WorldID"] = this.WorldID,
                ["Name"] = this.Name,
                ["Gender"] = (byte) this.Appearance.Gender,
                ["Skin"] = this.Appearance.Skin,
                ["Face"] = this.Appearance.Face,
                ["Hair"] = this.Appearance.Hair,
                ["Level"] = this.Stats.Level,
                ["Job"] = (short) this.Jobs.Job,
                ["Strength"] = this.Stats.Strength,
                ["Dexterity"] = this.Stats.Dexterity,
                ["Intelligence"] = this.Stats.Intelligence,
                ["Luck"] = this.Stats.Luck,
                ["Health"] = this.Stats.Health,
                ["MaxHealth"] = this.Stats.MaxHealth,
                ["Mana"] = this.Stats.Mana,
                ["MaxMana"] = this.Stats.MaxMana,
                ["AbilityPoints"] = this.Stats.AbilityPoints,
                ["SkillPoints"] = this.Stats.SkillPoints,
                ["Experience"] = this.Stats.Experience,
                ["Fame"] = this.Stats.Fame,
                ["MapID"] = this.Map.MapleID,
                ["SpawnPoint"] = this.SpawnPoint,
                ["Meso"] = this.Stats.Meso,
                ["EquipmentSlots"] = this.Items.MaxSlots[ItemConstants.ItemType.Equipment],
                ["UsableSlots"] = this.Items.MaxSlots[ItemConstants.ItemType.Usable],
                ["SetupSlots"] = this.Items.MaxSlots[ItemConstants.ItemType.Setup],
                ["EtceteraSlots"] = this.Items.MaxSlots[ItemConstants.ItemType.Etcetera],
                ["CashSlots"] = this.Items.MaxSlots[ItemConstants.ItemType.Cash]
            };

            if (this.Assigned)
            {
                datum.Update("ID = {0}", this.ID);
            }
            else
            {
                this.ID = datum.InsertAndReturnID();
                this.Assigned = true;
            }

            this.Items.SaveInventory();
            this.Skills.Save();
            this.Quests.Save();
            this.Buffs.Save();
            this.Keymap.Save();
            this.Trocks.Save();
            this.Variables.Save();

            Log.Inform("Saved character '{0}' to database.", this.Name);
        }

        public void InitializeCharacter()
        {
            Client.Send(CharacterPackets.InitializeCharacterSetFieldPacket(this));
            Client.Send(CharacterPackets.InitializeCharacterSrvrStatusChng());

            IsInitialized = true;
            Map.Characters.Add(this);
            Keymap.Send();
            Memos.Send();
        }

        public static void InitializeCharacter(Character character)
        {
            character.Client.Send(CharacterPackets.InitializeCharacterSetFieldPacket(character));
            character.Client.Send(CharacterPackets.InitializeCharacterSrvrStatusChng());

            character.IsInitialized = true;
            character.Map.Characters.Add(character);
            character.Keymap.Send();
            character.Memos.Send();
        }

        public static void Release(Character character)
        {
            CharacterStats.Update(character);
        }

        public static void Notify(Character character, string message, ServerConstants.NoticeType type = ServerConstants.NoticeType.PinkText)
        {
            character.Client.Send(CharacterPackets.BroadcastMessage(type, message));
        }

        public Character GetCharacterByName(string name)
        {
            return Name == name ? this : null;
        }

        public void ChangeMapHandler(Packet inPacket)
        {
            byte portals = inPacket.ReadByte();

            if (portals != this.Portals)
            {
                return;
                //Throw: ChangeMapException occured: wrong portals recieved in packet!
            }

            int mapID = inPacket.ReadInt(); // TODO: needs to be validated
            string portalLabel = inPacket.ReadString();
            inPacket.ReadByte(); // TODO: Unknown, needs to be researched
            bool wheel = inPacket.ReadBool();

            switch (mapID)
            {
                case 0: // NOTE: Death.
                    {
                        if (this.IsAlive)
                        {
                            return;
                        }

                        this.Stats.Health = 50;
                        this.SendChangeMapRequest(this.Map.ReturnMapID);
                    }
                    break;

                case -1: // NOTE: Portal.
                    {
                        Portal portal;

                        try
                        {
                            portal = this.Map.Portals[portalLabel];
                        }
                        catch (KeyNotFoundException)
                        {
                            return;
                        }

                        // TODO: Validate player and portal position.

                        /*if (this.Level < this.Client.Channel.Maps[portal.DestinationMapID].RequiredLevel)
                        {
                            // TODO: Send a force of ground portal message.

                            return;
                        }*/

                        this.SendChangeMapRequest(portal.DestinationMapID, portal.Link.ID);
                    }
                    break;

                default: // NOTE: Admin '/m' command.
                    {
                        if (!this.IsMaster)
                        {
                            return;
                        }

                        // TODO: Validate map ID.

                        this.SendChangeMapRequest(mapID);
                    }
                    break;
            }
        }

        public void ChangeMapFromPortalLbl(int mapID, string portalLabel)
        {
            this.SendChangeMapRequest(mapID, DataProvider.Maps[mapID].Portals[portalLabel].ID);
        }

        public void SendChangeMapRequest(int mapID, byte portalID = 0, bool fromPosition = false, Point position = null)
        {
            this.Map.Characters.Remove(this); // remove character from current map

            this.Client.Send(CharacterPackets.ChangeMap(this, mapID, portalID, fromPosition, position)); // actual packet todo: wraper!

            DataProvider.Maps[mapID].Characters.Add(this); // add character to map of ID witch was sent to client in packet
        }

        public void CharMoveHandler(Packet inPacket)
        {
            byte portals = inPacket.ReadByte();

            if (portals != this.Portals)
            {
                return;
                //Throw: MoveHandlerException occured: wrong portals recieved in packet!
            }

            inPacket.ReadInt(); // NOTE: Unknown.

            Movements movements = Movements.Decode(inPacket);

            this.Position = movements.Position;
            this.Foothold = movements.Foothold;
            this.Stance = movements.Stance;

            this.Map.Broadcast(CharacterPackets.MoveCharacter(this, movements));

            if (this.Foothold == 0) // NOTE: Player is floating in the air.
            {
                if (this.IsMaster)
                {
                    // GMs might be legitimately in this state due to GM fly.
                    // We shouldn't mess with them because they have the tools to get out of falling off the map anyway.
                }
                else
                {
                    // TODO: Attempt to find foothold.
                    // If none found, check the player fall counter.
                    // If it's over 3, reset the player's map.
                }
            }
        }

        public void CharSitHandler(Packet inPacket)
        {
            short seatID = inPacket.ReadShort(); 

            if (seatID == -1) // No chair
            {
                this.Chair = 0;
                this.Map.Broadcast(CharacterPackets.SitOnChair(this), this);
            }
            else
            {
                this.Chair = seatID;
            }

            using (Packet oPacket = new Packet(ServerOperationCode.Sit))
            {
                oPacket.WriteBool(seatID != -1);

                if (seatID != -1)
                {
                    oPacket.WriteShort(seatID);
                }

                this.Client.Send(oPacket);
            }
        }

        public void CharSitOnChairHandler(Packet inPacket)
        {
            int chairMapleID = inPacket.ReadInt();

            if (!this.Items.InventoryContainsItemByID(chairMapleID))
            {
                return;
                //Throw: exception occured no chair with mapleID received in packet found in char inventory
            }

            this.Chair = chairMapleID;

            this.Map.Broadcast(CharacterPackets.ShowChair(this, chairMapleID), this);
        }

        // TODO: Separate incoming packet handler from validation and response
        public void AttackHandler(Packet inPacket, CharacterConstants.AttackType type)
        {
            Attack attack = new Attack(inPacket, type);

            if (attack.Portals != this.Portals)
            {
                return;
            }

            Skill skill = null;

            if (attack.SkillID > 0)
            {
                skill = this.Skills[attack.SkillID];
                skill.Cast();
            }

            // TODO: further adjustments
            switch (type)
            {
                case CharacterConstants.AttackType.Melee:
                    using (Packet oPacket = new Packet(ServerOperationCode.CloseRangeAttack))
                    {
                        oPacket
                            .WriteInt(this.ID)
                            .WriteByte((byte)((attack.Targets * 0x10) + attack.Hits))
                            .WriteByte(0x5B) // NOTE: Unknown.
                            .WriteByte((byte)(attack.SkillID != 0 ? skill.CurrentLevel : 0)); // NOTE: Skill level.

                        if (attack.SkillID > 0)
                        {
                            oPacket.WriteInt(attack.SkillID);
                        }

                        oPacket
                            .WriteByte(attack.Display) // NOTE: display? 
                            .WriteByte() // NOTE: direction? 
                            .WriteByte(attack.Animation) // NOTE: stance? 
                            .WriteByte(attack.WeaponSpeed) // NOTE: speed 
                            .WriteByte() // NOTE: skill mastery?
                            .WriteInt(); // NOTE: projectile? 

                        foreach (var target in attack.Damages)
                        {
                            oPacket
                                .WriteInt(target.Key)
                                .WriteByte(6);

                            foreach (uint hit in target.Value)
                            {
                                oPacket.WriteUInt(hit);
                            }
                        }

                        this.Map.Broadcast(oPacket, this);
                    }

                    break;

                case CharacterConstants.AttackType.Magic:
                    using (Packet oPacket = new Packet(ServerOperationCode.MagicAttack))
                    {
                        oPacket
                            .WriteInt(this.ID)
                            .WriteByte((byte)((attack.Targets * 0x10) + attack.Hits))
                            .WriteByte(0x5B) // NOTE: Unknown.
                            .WriteByte((byte)(attack.SkillID != 0 ? skill.CurrentLevel : 0)); // NOTE: Skill level.

                        if (attack.SkillID > 0)
                        {
                            oPacket.WriteInt(attack.SkillID);
                        }

                        oPacket
                            .WriteByte(attack.Display) // NOTE: display? 
                            .WriteByte() // NOTE: direction? 
                            .WriteByte(attack.Animation) // NOTE: stance? 
                            .WriteByte(attack.WeaponSpeed) // NOTE: speed 
                            .WriteByte() // NOTE: Skill mastery.
                            .WriteInt(); // NOTE: projectile?  

                        foreach (var target in attack.Damages)
                        {
                            oPacket
                                .WriteInt(target.Key)
                                .WriteByte(6);

                            foreach (uint hit in target.Value)
                            {
                                oPacket.WriteUInt(hit);
                            }
                        }

                        this.Map.Broadcast(oPacket, this);
                    }

                    break;

                case CharacterConstants.AttackType.Range:
                    using (Packet oPacket = new Packet(ServerOperationCode.RangedAttack))
                    {
                        oPacket
                            .WriteInt(this.ID)
                            .WriteByte((byte)((attack.Targets * 0x10) + attack.Hits))
                            .WriteByte(0x5B) // NOTE: Unknown.
                            .WriteByte((byte)(attack.SkillID != 0 ? skill.CurrentLevel : 0)); // NOTE: Skill level.

                        if (attack.SkillID > 0)
                        {
                            oPacket.WriteInt(attack.SkillID);
                        }

                        oPacket
                            .WriteByte(attack.Display) // NOTE: display? 
                            .WriteByte() // NOTE: direction? 
                            .WriteByte(attack.Animation) // NOTE: stance? 
                            .WriteByte(attack.WeaponSpeed) // NOTE: speed 
                            .WriteByte() // NOTE: Skill mastery.
                            .WriteInt(); // NOTE: projectile?  

                        foreach (var target in attack.Damages)
                        {
                            oPacket
                                .WriteInt(target.Key)
                                .WriteByte(6);

                            foreach (uint hit in target.Value)
                            {
                                oPacket.WriteUInt(hit);
                            }
                        }

                        this.Map.Broadcast(oPacket, this);
                    }

                    break;

                case CharacterConstants.AttackType.Summon:
                    /*using (Packet oPacket = new Packet(ServerOperationCode.RangedAttack))
                    {
                        oPacket
                            .WriteInt(this.ID)
                            .WriteInt(summonID)
                            .WriteByte(0) //??
                            .Write(damageDirection)
                            .Write(allDamage)

                            foreach (var attackEntry in attack.Damages)
                            {
                                oPacket
                                    .WriteInt(attackEntry.getMonsterOid())
                                    .WriteByte(6)
                                    .WriteInt(attackEntry.getDamage());
                            }
                    }*/
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            foreach (KeyValuePair<int, List<uint>> target in attack.Damages)
            {
                Mob mob;

                try
                {
                    mob = this.Map.Mobs[target.Key];
                }
                catch (KeyNotFoundException)
                {
                    continue;
                }

                mob.IsProvoked = true;
                mob.SwitchController(this);

                foreach (uint hit in target.Value)
                {
                    if (mob.Damage(this, hit))
                    {
                        mob.Die();
                    }
                }
            }
        }

        private const sbyte BumpDamage = -1;
        private const sbyte MapDamage = -2;

        // TODO: Separate incoming packetHanndler from validation and processing
        public void DamageHandler(Packet inPacket)
        {
            inPacket.Skip(4); // NOTE: Ticks.
            sbyte type = (sbyte)inPacket.ReadByte();
            inPacket.ReadByte(); // NOTE: Elemental type.
            int damage = inPacket.ReadInt();
            bool damageApplied = false;
            bool deadlyAttack = false;
            byte hit = 0;
            byte stance = 0;
            int disease = 0;
            byte level = 0;
            short mpBurn = 0;
            int mobID = 0;
            int noDamageSkillID = 0;

            if (type != MapDamage)
            {
                mobID = inPacket.ReadInt();
                int mobObjectID = inPacket.ReadInt();
                Mob mob;

                try
                {
                    mob = this.Map.Mobs[mobObjectID];
                }
                catch (KeyNotFoundException)
                {
                    return;
                }

                if (mobID != mob.MapleID)
                {
                    return;
                }

                if (type != BumpDamage)
                {
                    // TODO: Get mob attack and apply to disease/level/mpBurn/deadlyAttack.
                }
            }

            hit = inPacket.ReadByte();
            byte reduction = inPacket.ReadByte();
            inPacket.ReadByte(); // NOTE: Unknown.

            if (reduction != 0)
            {
                // TODO: Return damage (Power Guard).
            }

            if (type == MapDamage)
            {
                level = inPacket.ReadByte();
                disease = inPacket.ReadInt();
            }
            else
            {
                stance = inPacket.ReadByte();

                if (stance > 0)
                {
                    // TODO: Power Stance.
                }
            }

            if (damage == -1)
            {
                // TODO: Validate no damage skills.
            }

            if (disease > 0 && damage != 0)
            {
                // NOTE: Fake/Guardian don't prevent disease.
                // TODO: Add disease buff.
            }

            if (damage > 0)
            {
                // TODO: Check for Meso Guard.
                // TODO: Check for Magic Guard.
                // TODO: Check for Achilles.

                if (!damageApplied)
                {
                    if (deadlyAttack)
                    {
                        // TODO: Deadly attack function.
                    }
                    else
                    {
                        this.Stats.Health -= (short)damage;
                    }

                    if (mpBurn > 0)
                    {
                        this.Stats.Mana -= (short)mpBurn;
                    }
                }

                // TODO: Apply damage to buffs.
            }

            using (Packet oPacket = new Packet(ServerOperationCode.Hit))
            {
                oPacket
                    .WriteInt(this.ID)
                    .WriteSByte(type);

                switch (type)
                {
                    case MapDamage:
                        {
                            oPacket
                                .WriteInt(damage)
                                .WriteInt(damage);
                        }
                        break;

                    default:
                        {
                            oPacket
                                .WriteInt(damage) // TODO: ... or PGMR damage.
                                .WriteInt(mobID)
                                .WriteByte(hit)
                                .WriteByte(reduction);

                            if (reduction > 0)
                            {
                                // TODO: PGMR stuff.
                            }

                            oPacket
                                .WriteByte(stance)
                                .WriteInt(damage);

                            if (noDamageSkillID > 0)
                            {
                                oPacket.WriteInt(noDamageSkillID);
                            }
                        }
                        break;
                }

                this.Map.Broadcast(oPacket, this);
            }
        }

        public void CharTalkHandler(Packet inPacket)
        {
            string text = inPacket.ReadString();
            bool shout = inPacket.ReadBool(); // NOTE: Used for skill macros.

            // Check if text is a command or not
            if (text.StartsWith(Application.CommandIndiciator.ToString(), StringComparison.Ordinal)) // StringComparison.Ordinal added
            {
                CommandFactory.Execute(this, text);
            }
            //not a command just send it to userChat
            else 
            {
                this.Map.Broadcast(CharacterPackets.TalkToCharacter(this, text, shout));
            }
        }

        public void CharExpressionHandler(Packet inPacket)
        {
            int expressionID = inPacket.ReadInt();

            if (expressionID > 7) // NOTE: Cash facial expression.
            {
                int mapleID = 5159992 + expressionID;

                // TODO: Validate if item exists.
            }

            this.Map.Broadcast(CharacterPackets.ExpressEmotion(this, expressionID));
        }

        public void Converse(int mapleID)
        {
            // TODO.
        }

        public void Converse(Packet inPacket)
        {
            int objectID = inPacket.ReadInt();

            this.Converse(this.Map.Npcs[objectID]);
        }

        public void Converse(Npc npc, Quest quest = null)
        {
            this.LastNpc = npc;
            this.LastQuest = quest;

            this.LastNpc.Converse(this);
        }

        public void InformOnCharacter(Packet iPacket)
        {
            iPacket.Skip(4); // NOTE: Ticks
            int characterID = iPacket.ReadInt();

            Character target;

            try
            {
                target = this.Map.Characters[characterID];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            if (target.IsMaster && !this.IsMaster)
            {
                return;
            }

            using (Packet oPacket = new Packet(ServerOperationCode.CharacterInformation))
            {
                oPacket
                    .WriteInt(target.ID)
                    .WriteByte(target.Stats.Level)
                    .WriteShort((short)target.Jobs.Job)
                    .WriteShort(target.Stats.Fame)
                    .WriteBool(false) // NOTE: Marriage.
                    .WriteString("-") // NOTE: Guild name.
                    .WriteString("-") // NOTE: Alliance name.
                    .WriteByte() // NOTE: Unknown.
                    .WriteByte() // NOTE: Pets.
                    .WriteByte() // NOTE: Mount.
                    .WriteByte() // NOTE: Wishlist.
                    .WriteInt() // NOTE: Monster Book level.
                    .WriteInt() // NOTE: Monster Book normal cards. 
                    .WriteInt() // NOTE: Monster Book special cards.
                    .WriteInt() // NOTE: Monster Book total cards.
                    .WriteInt() // NOTE: Monster Book cover.
                    .WriteInt() // NOTE: Medal ID.
                    .WriteShort(); // NOTE: Medal quests.

                this.Client.Send(oPacket);
            }
        }

        // TODO: Should we refactor it in a way that sends it to the buddy/party/guild objects
        // instead of pooling the world for characters?
        public void MultiTalkHandler(Packet inPacket)
        {
            SocialConstants.MultiChatType type = (SocialConstants.MultiChatType)inPacket.ReadByte();
            byte count = inPacket.ReadByte();

            List<int> recipients = new List<int>();

            while (count-- > 0)
            {
                int recipientID = inPacket.ReadInt();
                recipients.Add(recipientID);
            }

            string text = inPacket.ReadString();

            switch (type)
            {
                case SocialConstants.MultiChatType.Buddy:
                    {
                    }
                    break;

                case SocialConstants.MultiChatType.Party:
                    {
                    }
                    break;

                case SocialConstants.MultiChatType.Guild:
                    {
                    }
                    break;

                case SocialConstants.MultiChatType.Alliance:
                    {
                    }
                    break;
            }

            // NOTE: This is here for convenience. If you accidentally use another text window (like party) and not the main text window,
            // your commands won't be shown but instead executed from there as well.
            if (text.StartsWith(Application.CommandIndiciator.ToString(), StringComparison.Ordinal))
            {
                CommandFactory.Execute(this, text);
            }

            else // NOTE: try to send to each recipient packet with group text
            {
                if (!recipients.Any()) return;

                foreach (int recipient in recipients)
                {
                    //this.Client.World.GetCharacter(recipient).Client.Send(CharacterPackets.TalkToCharacterGroup(type, this, text));                 
                }
            }
        }

        // TODO: Cash Shop/MTS scenarios.
        public void UseCommand(Packet iPacket)
        {
            /*CommandType type = (CommandType)iPacket.ReadByte();
            string targetName = iPacket.ReadString();

            Character target = null;// this.Client.World.GetCharacter(targetName);

            switch (type)
            {
                case CommandType.Find:
                    {
                        if (target == null)
                        {
                            using (Packet oPacket = new Packet(ServerOperationCode.Whisper))
                            {
                                oPacket
                                    .WriteByte(0x0A)
                                    .WriteString(targetName)
                                    .WriteBool(false);

                                this.Client.Send(oPacket);
                            }
                        }
                        else
                        {
                            bool isInSameChannel = this.Client.ChannelID == target.Client.ChannelID;

                            using (Packet oPacket = new Packet(ServerOperationCode.Whisper))
                            {
                                oPacket
                                    .WriteByte(0x09)
                                    .WriteString(targetName)
                                    .WriteByte((byte)(isInSameChannel ? 1 : 3))
                                    .WriteInt(isInSameChannel ? target.Map.MapleID : target.Client.ChannelID)
                                    .WriteInt() // NOTE: Unknown.
                                    .WriteInt(); // NOTE: Unknown.

                                this.Client.Send(oPacket);
                            }
                        }
                    }
                    break;

                case CommandType.Whisper:
                    {
                        string text = iPacket.ReadString();

                        using (Packet oPacket = new Packet(ServerOperationCode.Whisper))
                        {
                            oPacket
                                .WriteByte(10)
                                .WriteString(targetName)
                                .WriteBool(target != null);

                            this.Client.Send(oPacket);
                        }

                        if (target != null)
                        {
                            using (Packet oPacket = new Packet(ServerOperationCode.Whisper))
                            {
                                oPacket
                                    .WriteByte(18)
                                    .WriteString(this.Name)
                                    .WriteByte(this.Client.ChannelID)
                                    .WriteByte() // NOTE: Unknown.
                                    .WriteString(text);

                                target.Client.Send(oPacket);
                            }
                        }
                    }
                    break;
            }*/
        }

        public void Interact(Packet iPacket)
        {
            InteractionConstants.InteractionCode code = (InteractionConstants.InteractionCode)iPacket.ReadByte();

            switch (code)
            {
                case InteractionConstants.InteractionCode.Create:
                    {
                        InteractionConstants.InteractionType type = (InteractionConstants.InteractionType)iPacket.ReadByte();

                        switch (type)
                        {
                            case InteractionConstants.InteractionType.Omok:
                                {

                                }
                                break;

                            case InteractionConstants.InteractionType.Trade:
                                {
                                    if (this.Trade == null)
                                    {
                                        this.Trade = new Trade(this);
                                    }
                                }
                                break;

                            case InteractionConstants.InteractionType.PlayerShop:
                                {
                                    string description = iPacket.ReadString();

                                    if (this.PlayerShop == null)
                                    {
                                        this.PlayerShop = new PlayerShop(this, description);
                                    }
                                }
                                break;

                            case InteractionConstants.InteractionType.HiredMerchant:
                                {

                                }
                                break;
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.Visit:
                    {
                        if (this.PlayerShop == null)
                        {
                            int objectID = iPacket.ReadInt();

                            if (this.Map.PlayerShops.Contains(objectID))
                            {
                                this.Map.PlayerShops[objectID].AddVisitor(this);
                            }
                        }
                    }
                    break;

                default:
                    {
                        if (this.Trade != null)
                        {
                            this.Trade.Handle(this, code, iPacket);
                        }
                        else if (this.PlayerShop != null)
                        {
                            this.PlayerShop.Handle(this, code, iPacket);
                        }
                    }
                    break;
            }
        }

        public void UseAdminCommandHandler(Packet inPacket)
        {
            //do we have privilege to use it?
            if (!this.IsMaster)
            {
                return;
            }

            //handling according to command type
            CharacterConstants.AdminCommandType type = (CharacterConstants.AdminCommandType) inPacket.ReadByte();

            switch (type)
            {
                case CharacterConstants.AdminCommandType.CreateItem:
                    {
                        int itemID = inPacket.ReadInt();

                        this.Items.AddItemToInventory(new Item(itemID));
                    }
                    break;

                case CharacterConstants.AdminCommandType.DestroyFirstItem:
                    {
                        byte itemType = inPacket.ReadByte();
                        // TODO: remove item from inventory by type
                    }
                    break;

                case CharacterConstants.AdminCommandType.GiveExperience:
                    {
                        int amount = inPacket.ReadInt();

                        this.Stats.Experience += amount; // Unsafe
                    }
                    break;

                case CharacterConstants.AdminCommandType.Ban:
                    {
                        string name = inPacket.ReadString();

                        Character target = null; //this.Client.World.GetCharacter(name);

                        if (target != null)
                        {
                            target.Client.Stop();
                        }
                        else
                        {
                            using (Packet oPacket = new Packet(ServerOperationCode.AdminResult))
                            {
                                oPacket
                                    .WriteByte(6)
                                    .WriteByte(1);

                                this.Client.Send(oPacket);
                            }
                        }
                    }
                    break;

                case CharacterConstants.AdminCommandType.Block:
                    {
                        string theBlockedOne = inPacket.ReadString();
                        byte blockType = inPacket.ReadByte();
                        int duration = inPacket.ReadInt();
                        string description = inPacket.ReadString();
                        string reason = inPacket.ReadString();

                        Character target = null; //this.Client.World.GetCharacter(name);

                        if (target != null)
                        {
                            target.Client.Stop();
                        }

                        // TODO: Ban with reason+description.
                        // TODO: Ban by IP, MAC, HWID
                    }
                    break;

                case CharacterConstants.AdminCommandType.Hide:
                    {
                        bool hide = inPacket.ReadBool();

                        if (hide)
                        {
                            Skill hideSkill = new Skill((int)CharacterConstants.SkillNames.SuperGM.Hide);
                            Buff hideBuff = new Buff(this.Buffs, hideSkill, 1);

                            if (!this.Buffs.Contains(hideBuff))
                            {
                                this.Buffs.AddBuff(hideBuff);
                            }
                        }

                        else
                        {
                            Skill hideSkill = new Skill((int)CharacterConstants.SkillNames.SuperGM.Hide);
                            Buff hideBuff = new Buff(this.Buffs, hideSkill, 1);

                            if (this.Buffs.Contains(hideBuff))
                            {
                                this.Buffs.RemoveBuff(hideBuff);
                            }
                        }
                    }
                    break;

                case CharacterConstants.AdminCommandType.Send:
                    {
                        string name = inPacket.ReadString();
                        int destinationID = inPacket.ReadInt();

                        Character target = null; // this.Client.World.GetCharacter(name);

                        if (target != null)
                        {
                            target.SendChangeMapRequest(destinationID);
                        }
                        else
                        {
                            using (Packet oPacket = new Packet(ServerOperationCode.AdminResult))
                            {
                                oPacket
                                    .WriteByte(6)
                                    .WriteByte(1);

                                this.Client.Send(oPacket);
                            }
                        }
                    }
                    break;

                case CharacterConstants.AdminCommandType.Summon:
                    {
                        int mobID = inPacket.ReadInt();
                        int count = inPacket.ReadInt();

                        if (DataProvider.Mobs.Contains(mobID))
                        {
                            for (int i = 0; i < count; i++)
                            {
                                this.Map.Mobs.Add(new Mob(mobID, this.Position));
                            }
                        }

                        else
                        {
                            Notify(this, "invalid mob: " + mobID); // TODO: Actual message.
                        }
                    }
                    break;

                case CharacterConstants.AdminCommandType.ShowMessageMap:
                    {
                        // TODO: What does this do?
                    }
                    break;

                case CharacterConstants.AdminCommandType.Snow:
                    {
                        // TODO: We have yet to implement map weather.
                    }
                    break;

                case CharacterConstants.AdminCommandType.VarSetGet:
                    {
                        // TODO: This seems useless. Should we implement this?
                    }
                    break;

                case CharacterConstants.AdminCommandType.Warn:
                    {
                        string victimName = inPacket.ReadString();
                        string text = inPacket.ReadString();

                        Character target = null; // this.Client.World.GetCharacter(victimName);

                        if (target != null)
                        {
                            Notify(target, text, ServerConstants.NoticeType.Popup);
                        }

                        using (Packet oPacket = new Packet(ServerOperationCode.AdminResult))
                        {
                            oPacket
                                .WriteByte((byte)CharacterConstants.AdminCommandType.Warn)
                                .WriteBool(target != null);

                            this.Client.Send(oPacket);
                        }
                    }

                    break;
                case CharacterConstants.AdminCommandType.Kill:
                    break;
                case CharacterConstants.AdminCommandType.QuestReset:
                    break;
                case CharacterConstants.AdminCommandType.GetMobHP:
                    break;
                case CharacterConstants.AdminCommandType.Log:
                    break;
                case CharacterConstants.AdminCommandType.SetObjectState:
                    break;
                case CharacterConstants.AdminCommandType.ArtifactRanking:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
                    // TODO: register encounter of unhanded GM packet, get debug info
            }
        }

        public void EnterPortal(Packet iPacket)
        {
            byte portals = iPacket.ReadByte();

            if (portals != this.Portals)
            {
                return;
            }

            string label = iPacket.ReadString();
            Portal portal;

            try
            {
                portal = this.Map.Portals[label];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            /*if (false) // TODO: Check if portal is onlyOnce and player already used it.
            {
                // TODO: Send a "closed for now" portal message.
                return;
            }*/

            try
            {
                new PortalScript(portal, this).Execute();
            }
            catch (Exception ex)
            {
                Log.Error("Script error: {0}", ex.ToString());
            }
        }

        public void Report(Packet iPacket)
        {
            CharacterConstants.ReportType type = (CharacterConstants.ReportType)iPacket.ReadByte();
            string victimName = iPacket.ReadString();
            iPacket.ReadByte(); // NOTE: Unknown.
            string description = iPacket.ReadString();

            CharacterConstants.ReportResult result;

            switch (type)
            {
                case CharacterConstants.ReportType.IllegalProgramUsage:
                    {
                    }
                    break;

                case CharacterConstants.ReportType.ConversationClaim:
                    {
                        string chatLog = iPacket.ReadString();
                    }
                    break;
            }

            if (true) // TODO: Check for available report claims.
            {
                /*if (this.Client.World.IsCharacterOnline(victimName)) // TODO: Should we check for map existance instead? The hacker can teleport away before the reported is executed.
                {
                    if (this.Meso >= 300)
                    {
                        this.Meso -= 300;

                        // TODO: Update GMs of reported player.
                        // TODO: Update available report claims.

                        result = ReportResult.Success;
                    }
                    else
                    {
                        result = ReportResult.UnknownError;
                    }
                }
                else
                {
                    result = ReportResult.UnableToLocate;
                }*/
                result = CharacterConstants.ReportResult.Success;
            }
            else
            /*{
                result = CharacterConstants.ReportResult.Max10TimesADay;
            }*/

            using (Packet oPacket = new Packet(ServerOperationCode.SueCharacterResult))
            {
                oPacket.WriteByte((byte)result);

                this.Client.Send(oPacket);
            }
        }

        public byte[] ToByteArray(bool viewAllCharacters = false)
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteBytes(this.StatisticsToByteArray())
                    .WriteBytes(this.AppearanceToByteArray());

                if (!viewAllCharacters)
                {
                    oPacket.WriteByte(); // NOTE: Family
                }

                oPacket.WriteBool(this.IsRanked);

                if (this.IsRanked)
                {
                    oPacket
                        .WriteInt()
                        .WriteInt()
                        .WriteInt()
                        .WriteInt();
                }

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }

        public byte[] StatisticsToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteInt(this.ID)
                    .WriteStringFixed(this.Name, 13)
                    .WriteByte((byte)this.Appearance.Gender)
                    .WriteByte(this.Appearance.Skin)
                    .WriteInt(this.Appearance.Face)
                    .WriteInt(this.Appearance.Hair)
                    .WriteLong()
                    .WriteLong()
                    .WriteLong()
                    .WriteByte(this.Stats.Level)
                    .WriteShort((short)this.Jobs.Job)
                    .WriteShort(this.Stats.Strength)
                    .WriteShort(this.Stats.Dexterity)
                    .WriteShort(this.Stats.Intelligence)
                    .WriteShort(this.Stats.Luck)
                    .WriteShort(this.Stats.Health)
                    .WriteShort(this.Stats.MaxHealth)
                    .WriteShort(this.Stats.Mana)
                    .WriteShort(this.Stats.MaxMana)
                    .WriteShort(this.Stats.AbilityPoints)
                    .WriteShort(this.Stats.SkillPoints)
                    .WriteInt(this.Stats.Experience)
                    .WriteShort(this.Stats.Fame)
                    .WriteInt()
                    .WriteInt(this.Map.MapleID)
                    .WriteByte(this.SpawnPoint)
                    .WriteInt();

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }

        public byte[] AppearanceToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteByte((byte)this.Appearance.Gender)
                    .WriteByte(this.Appearance.Skin)
                    .WriteInt(this.Appearance.Face)
                    .WriteBool(true)
                    .WriteInt(this.Appearance.Hair);

                Dictionary<byte, int> visibleLayer = new Dictionary<byte, int>();
                Dictionary<byte, int> hiddenLayer = new Dictionary<byte, int>();

                foreach (Item item in this.Items.GetEquipped())
                {
                    byte slot = item.AbsoluteSlot;

                    if (slot < 100 && !visibleLayer.ContainsKey(slot))
                    {
                        visibleLayer[slot] = item.MapleID;
                    }
                    else if (slot > 100 && slot != 111)
                    {
                        slot -= 100;

                        if (visibleLayer.ContainsKey(slot))
                        {
                            hiddenLayer[slot] = visibleLayer[slot];
                        }

                        visibleLayer[slot] = item.MapleID;
                    }
                    else if (visibleLayer.ContainsKey(slot))
                    {
                        hiddenLayer[slot] = item.MapleID;
                    }
                }

                foreach (KeyValuePair<byte, int> entry in visibleLayer)
                {
                    oPacket
                        .WriteByte(entry.Key)
                        .WriteInt(entry.Value);
                }

                oPacket.WriteByte(byte.MaxValue);

                foreach (KeyValuePair<byte, int> entry in hiddenLayer)
                {
                    oPacket
                        .WriteByte(entry.Key)
                        .WriteInt(entry.Value);
                }

                oPacket.WriteByte(byte.MaxValue);

                Item cashWeapon = this.Items[ItemConstants.EquipmentSlot.CashWeapon];

                oPacket.WriteInt(cashWeapon != null ? cashWeapon.MapleID : 0);

                oPacket
                    .WriteInt()
                    .WriteInt()
                    .WriteInt();

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }

        public byte[] DataToByteArray(long flag = long.MaxValue)
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteLong(flag)
                    .WriteByte() // NOTE: Unknown.
                    .WriteBytes(this.StatisticsToByteArray())
                    .WriteByte(20) // NOTE: Max buddylist size.
                    .WriteBool(false) // NOTE: Blessing of Fairy.
                    .WriteInt(this.Stats.Meso)
                    .WriteBytes(this.Items.ItemToByteArray())
                    .WriteBytes(this.Skills.ToByteArray())
                    .WriteBytes(this.Quests.ToByteArray())
                    .WriteShort() // NOTE: Mini games record.
                    .WriteShort() // NOTE: Rings (1).
                    .WriteShort() // NOTE: Rings (2). 
                    .WriteShort() // NOTE: Rings (3).
                    .WriteBytes(CharacterTrocks.RegularTrockToByteArray())
                    .WriteBytes(CharacterTrocks.VIPTrockToByteArray())
                    .WriteInt() // NOTE: Monster Book cover ID.
                    .WriteByte() // NOTE: Monster Book cards.
                    .WriteShort() // NOTE: New Year Cards.
                    .WriteShort() // NOTE: QuestRecordEX.
                    .WriteShort() // NOTE: AdminShop.
                    .WriteShort(); // NOTE: Unknown.

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }

        //TODO: theoretically this could handle all kinds of messages to player like drops, mesos, guild points etc....
        public static Packet GetShowSidebarInfoPacket(ServerConstants.MessageType type, bool white, int itemID, int ammount,
            bool inChat, int partyBonus, int equipBonus)
        {
            Packet oPacket = new Packet(ServerOperationCode.Message);

            //the mesos work, drops dont idk why
            switch (type)
            {
                case ServerConstants.MessageType.DropPickup: //when itemID == 0:
                    oPacket
                        .WriteByte((byte)type)
                        .WriteBool(white)
                        .WriteByte(0) // NOTE: Unknown.
                        .WriteInt(ammount)
                        .WriteShort(0);
                    break;

                /* case (MessageType.DropPickup when itemID > 0):
                     oPacket
                         .WriteByte((byte) type) 
                         .WriteBool(false)
                         .WriteInt(itemID)
                         .WriteInt(ammount)
                         .WriteInt(0)
                         .WriteInt(0);
                     break; */

                case ServerConstants.MessageType.IncreaseEXP:
                    oPacket
                        .WriteByte((byte)type) // NOTE: enum MessageType 
                        .WriteBool(white) // NOTE: white is default as 1, 0 = yellow
                        .WriteInt(ammount)
                        .WriteBool(inChat) // NOTE: display message in chat box
                        .WriteInt(0) // NOTE: monster book bonus (Bonus Event Exp)
                        .WriteShort(0) // NOTE: unknown
                        .WriteInt(0) // NOTE: wedding bonus
                        .WriteByte(0) // NOTE: 0 = party bonus, 1 = Bonus Event party Exp () x0
                        .WriteInt(partyBonus)
                        .WriteInt(equipBonus)
                        .WriteInt(0) // NOTE: Internet Cafe Bonus
                        .WriteInt(0); // NOTE: Rainbow Week Bonus          

                    if (inChat) //is this necessary?
                    {
                        oPacket
                            .WriteByte(0);
                    }

                    break;

                case ServerConstants.MessageType.QuestRecord:
                    break;
                case ServerConstants.MessageType.CashItemExpire:
                    break;
                case ServerConstants.MessageType.IncreaseFame:
                    break;
                case ServerConstants.MessageType.IncreaseMeso:
                    break;
                case ServerConstants.MessageType.IncreaseGP:
                    break;
                case ServerConstants.MessageType.GiveBuff:
                    break;
                case ServerConstants.MessageType.GeneralItemExpire:
                    break;
                case ServerConstants.MessageType.System:
                    break;
                case ServerConstants.MessageType.QuestRecordEx:
                    break;
                case ServerConstants.MessageType.ItemProtectExpire:
                    break;
                case ServerConstants.MessageType.ItemExpireReplace:
                    break;
                case ServerConstants.MessageType.SkillExpire:
                    break;
                case ServerConstants.MessageType.TutorialMessage:
                    break;
            }

            return oPacket;
        }

        public Packet GetCreatePacket()
        {
            return this.GetSpawnPacket();
        }

        public Packet GetSpawnPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.UserEnterField);

            oPacket
                .WriteInt(this.ID)
                .WriteByte(this.Stats.Level)
                .WriteString(this.Name);

            if (false) // ??
            {
                oPacket
                    .WriteString("")
                    .WriteShort()
                    .WriteByte()
                    .WriteShort()
                    .WriteByte();
            }
            else
            {
                oPacket.Skip(8);
            }

            oPacket
                .WriteBytes(this.Buffs.ToByteArray())
                .WriteShort((short)this.Jobs.Job)
                .WriteBytes(this.AppearanceToByteArray())
                .WriteInt(this.Items.InventoryAvailableItemByID(5110000))
                .WriteInt() // NOTE: Item effect.
                .WriteInt((int)(Item.GetType(this.Chair) == ItemConstants.ItemType.Setup ? this.Chair : 0))
                .WriteShort(this.Position.X)
                .WriteShort(this.Position.Y)
                .WriteByte(this.Stance)
                .WriteShort(this.Foothold)
                .WriteByte()
                .WriteByte()
                .WriteInt(1)
                .WriteLong();

            if (this.PlayerShop != null && this.PlayerShop.Owner == this)
            {
                oPacket
                    .WriteByte(4)
                    .WriteInt(this.PlayerShop.ObjectID)
                    .WriteString(this.PlayerShop.Description)
                    .WriteByte()
                    .WriteByte()
                    .WriteByte(1)
                    .WriteByte((byte)(this.PlayerShop.IsFull ? 1 : 2)) // NOTE: Visitor availability.
                    .WriteByte();
            }
            else
            {
                oPacket.WriteByte();
            }

            bool hasChalkboard = !string.IsNullOrEmpty(this.Chalkboard);

            oPacket.WriteBool(hasChalkboard);

            if (hasChalkboard)
            {
                oPacket.WriteString(this.Chalkboard);
            }

            oPacket
                .WriteByte() // NOTE: Couple ring.
                .WriteByte() // NOTE: Friendship ring.
                .WriteByte() // NOTE: Marriage ring.
                .Skip(3) // NOTE: Unknown.
                .WriteByte(byte.MaxValue); // NOTE: Team.

            return oPacket;
        }

        public Packet GetDestroyPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.UserLeaveField);

            oPacket.WriteInt(this.ID);

            return oPacket;
        }
    }
}