using System;
using System.Collections.Generic;

using Destiny.Data;
using Destiny.Maple.Characters;
using Destiny.Maple.Data;
using Destiny.Maple.Maps;
using Destiny.Constants;
using Destiny.IO;
using Destiny.Network.Common;
using Destiny.Network.PacketFactory;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple
{
    // TODO: Try to separate the consumableItems
    // TODO: Try to rework the heuristic hacks
    public class Item : Drop 
    {
        public static ItemConstants.ItemType GetType(int mapleID)
        {
            return (ItemConstants.ItemType)(mapleID / 1000000);
        }

        public CharacterItems Parent { get; set; }

        public int ID { get; private set; }
        public int AccountID { get; private set; }
        public int MapleID { get; private set; }
        public short Slot { get; set; }
        private short maxPerStack;
        private static short quantity;
        public string Creator { get; set; }
        public DateTime Expiration { get; set; }
        public int? PetID { get; set; }

        public bool IsCash { get; private set; }
        public bool OnlyOne { get; private set; }
        public bool PreventsSlipping { get; private set; }
        public bool PreventsColdness { get; private set; }
        public bool IsTradeBlocked { get; private set; }
        public bool IsScissored { get; private set; }
        public bool IsStored { get; set; }
        public int SalePrice { get; private set; }
        public int Meso { get; private set; }

        public byte UpgradesAvailable { get; private set; }
        public byte UpgradesApplied { get; private set; }
        public short Strength { get; private set; }
        public short Dexterity { get; private set; }
        public short Intelligence { get; private set; }
        public short Luck { get; private set; }
        public short Health { get; private set; }
        public short Mana { get; private set; }
        public short WeaponAttack { get; private set; }
        public short MagicAttack { get; private set; }
        public short WeaponDefense { get; private set; }
        public short MagicDefense { get; private set; }
        public short Accuracy { get; private set; }
        public short Avoidability { get; private set; }
        public short Agility { get; private set; }
        public short Speed { get; private set; }
        public short Jump { get; private set; }

        public byte AttackSpeed { get; private set; }
        public short RecoveryRate { get; private set; }
        public short KnockBackChance { get; private set; }

        public short RequiredLevel { get; private set; }
        public short RequiredStrength { get; private set; }
        public short RequiredDexterity { get; private set; }
        public short RequiredIntelligence { get; private set; }
        public short RequiredLuck { get; private set; }
        public short RequiredFame { get; private set; }
        public CharacterConstants.Job RequiredJob { get; private set; }

        // Consume data properties are prefixed with 'C'.
        public int CItemId { get; private set; }
        public string CFlags { get; private set; }
        public string CCureAilments { get; private set; }
        public short CEffect { get; private set; }
        public short CHealth { get; private set; }
        public short CMana { get; private set; }
        public short CHealthPercentage { get; private set; }
        public short CManaPercentage { get; private set; }
        public int CMoveTo { get; private set; }
        public short CProb { get; private set; }
        public int CBuffTime { get; private set; }
        public short CWeaponAttack { get; private set; }
        public short CMagicAttack { get; private set; }
        public short CWeaponDefense { get; private set; }
        public short CMagicDefense { get; private set; }
        public short CAccuracy { get; private set; }
        public short CAvoid { get; private set; }
        public short CSpeed { get; private set; }
        public short CJump { get; private set; }
        public short CMorph { get; private set; }

        public List<Tuple<int, short>> Summons { get; private set; }

        public ItemConstants.ItemType ItemType
        {
            get
            {
                return Item.GetType(this.MapleID);
            }
        }

        public ItemConstants.WeaponType WeaponType
        {
            get
            {
                switch (this.MapleID / 10000 % 100)
                {
                    case 30:
                        return ItemConstants.WeaponType.Sword1H;

                    case 31:
                        return ItemConstants.WeaponType.Axe1H;

                    case 32:
                        return ItemConstants.WeaponType.Blunt1H;

                    case 33:
                        return ItemConstants.WeaponType.Dagger;

                    case 37:
                        return ItemConstants.WeaponType.Wand;

                    case 38:
                        return ItemConstants.WeaponType.Staff;

                    case 40:
                        return ItemConstants.WeaponType.Sword2H;

                    case 41:
                        return ItemConstants.WeaponType.Axe2H;

                    case 42:
                        return ItemConstants.WeaponType.Blunt2H;

                    case 43:
                        return ItemConstants.WeaponType.Spear;

                    case 44:
                        return ItemConstants.WeaponType.PoleArm;

                    case 45:
                        return ItemConstants.WeaponType.Bow;

                    case 46:
                        return ItemConstants.WeaponType.Crossbow;

                    case 47:
                        return ItemConstants.WeaponType.Claw;

                    case 48:
                        return ItemConstants.WeaponType.Knuckle;

                    case 49:
                        return ItemConstants.WeaponType.Gun;

                    default:
                        return ItemConstants.WeaponType.NotAWeapon;
                }
            }
        }

        public Item CachedReference
        {
            get
            {
                return DataProvider.Items[this.MapleID];
            }
        }

        public Character Character
        {
            get
            {
                return this.Parent.Parent;
            }
        }

        public short MaxPerStack
        {
            get
            {
                if (this.IsRechargeable && this.Parent != null)
                {
                    return maxPerStack;
                }
                else
                {
                    return maxPerStack;
                }
            }
            set
            {
                maxPerStack = value;
            }
        }

        public short Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                if (value > this.MaxPerStack)
                {
                    throw new ArgumentException("Quantity too high.");
                }
                else
                {
                    quantity = value;
                }
            }
        }

        public bool IsSealed
        {
            get
            {
                return DataProvider.Items.WizetItemIDs.Contains(this.MapleID);
            }
        }

        public byte Flags
        {
            get
            {
                byte flags = 0;

                if (this.IsSealed) flags |= (byte)ItemConstants.ItemFlags.Sealed;
                if (this.PreventsSlipping) flags |= (byte)ItemConstants.ItemFlags.AddPreventSlipping;
                if (this.PreventsColdness) flags |= (byte)ItemConstants.ItemFlags.AddPreventColdness;
                if (this.IsScissored) flags |= (byte)ItemConstants.ItemFlags.Scissored;
                if (this.IsTradeBlocked) flags |= (byte)ItemConstants.ItemFlags.Untradeable;

                return flags;
            }
        }

        public bool IsEquipped
        {
            get
            {
                return this.Slot < 0;
            }
        }

        public bool IsEquippedCash
        {
            get
            {
                return this.Slot < -100;
            }
        }

        public bool IsConsumable
        {
            get
            {
                return this.MapleID / 10000 >= 200 && this.MapleID / 10000 < 204;
            }
        }

        public static bool IsConsumableItem(Item item)
        {
          return item.MapleID / 10000 >= 200 && item.MapleID / 10000 < 204;
        }

        public static readonly Action consumableAction = () => IsConsumableItemIDaction(2030002); // 2030002 - Return Scroll to Ellinia - Returns you to Ellinia.
    
        public static void IsConsumableItemIDaction(int itemID)
        {
            var b = itemID / 10000 >= 200 && itemID / 10000 < 204;
        }

        public static readonly Action consumableActionHashSet = () => IsConsumableItemIDactionHashSet(2030002); // 2030002 - Return Scroll to Ellinia - Returns you to Ellinia.

        public static void IsConsumableItemIDactionHashSet(int itemID)
        {
            var b = ConsumableConstants.ConsumablesMapleIDs.Contains(itemID);
        }

        public static bool IsConsumableItemID(int itemID)
        {
            return itemID / 10000 >= 200 && itemID / 10000 < 204;
        }

        public bool IsRechargeable
        {
            get
            {
                return this.IsThrowingStar || this.IsBullet;
            }
        }

        public bool IsThrowingStar
        {
            get
            {
                return this.MapleID / 10000 == 207;
            }
        }

        public bool IsBullet
        {
            get
            {
                return this.MapleID / 10000 == 233;
            }
        }

        public bool IsArrow
        {
            get
            {
                return this.IsArrowForBow || this.IsArrowForCrossbow;
            }
        }

        public bool IsArrowForBow
        {
            get
            {
                return this.MapleID >= 2060000 && this.MapleID < 2061000;
            }
        }

        public bool IsArrowForCrossbow
        {
            get
            {
                return this.MapleID >= 2061000 && this.MapleID < 2062000;
            }
        }

        public bool IsOverall
        {
            get
            {
                return this.MapleID / 10000 == 105;
            }
        }

        public bool IsWeapon
        {
            get
            {
                return this.WeaponType != ItemConstants.WeaponType.NotAWeapon;
            }
        }

        public bool IsShield
        {
            get
            {
                return this.MapleID / 10000 % 100 == 9;
            }
        }

        public bool IsPet
        {
            get
            {
                return this.MapleID >= 5000000 && this.MapleID <= 5000100;
            }
        }

        public bool IsTownScroll
        {
            get
            {
                return this.MapleID >= 2030000 && this.MapleID < 2030020;
            }
        }

        public bool IsTwoHanded
        {
            get
            {
                switch (this.WeaponType)
                {
                    case ItemConstants.WeaponType.Sword2H:
                    case ItemConstants.WeaponType.Axe2H:
                    case ItemConstants.WeaponType.Blunt2H:
                    case ItemConstants.WeaponType.Spear:
                    case ItemConstants.WeaponType.PoleArm:
                    case ItemConstants.WeaponType.Bow:
                    case ItemConstants.WeaponType.Crossbow:
                    case ItemConstants.WeaponType.Claw:
                    case ItemConstants.WeaponType.Knuckle:
                    case ItemConstants.WeaponType.Gun:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public bool IsBlocked
        {
            get
            {
                return this.IsCash || this.IsSealed || (this.IsTradeBlocked && !this.IsScissored);
            }
        }

        public byte AbsoluteSlot
        {
            get
            {
                if (this.IsEquipped)
                {
                    return (byte)(this.Slot * -1);
                }
                else
                {
                    throw new InvalidOperationException("Attempting to retrieve absolute slot for non-equipped item.");
                }
            }
        }

        public byte ComputedSlot
        {
            get
            {
                if (this.IsEquippedCash)
                {
                    return ((byte)(this.AbsoluteSlot - 100));
                }
                else if (this.IsEquipped)
                {
                    return this.AbsoluteSlot;
                }
                else
                {
                    return (byte)this.Slot;
                }
            }
        }

        public bool Assigned { get; set; }

        public Item(int mapleID, short quantity = 1, DateTime? expiration = null, bool equipped = false)
        {
            this.MapleID = mapleID;
            this.MaxPerStack = this.CachedReference.MaxPerStack;
            this.Quantity = (this.ItemType == ItemConstants.ItemType.Equipment) ? (short)1 : quantity;
            if (equipped) this.Slot = (short)this.GetEquippedSlot();

            if (!expiration.HasValue)
            {
                expiration = new DateTime(2079, 1, 1, 12, 0, 0); // NOTE: Default expiration time (permanent).
            }

            this.Expiration = (DateTime)expiration;

            this.IsCash = this.CachedReference.IsCash;
            this.OnlyOne = this.CachedReference.OnlyOne;
            this.IsTradeBlocked = this.CachedReference.IsTradeBlocked;
            this.IsScissored = this.CachedReference.IsScissored;
            this.SalePrice = this.CachedReference.SalePrice;
            this.RequiredLevel = this.CachedReference.RequiredLevel;
            this.Meso = this.CachedReference.Meso;

            if (this.ItemType == ItemConstants.ItemType.Equipment)
            {
                this.PreventsSlipping = this.CachedReference.PreventsSlipping;
                this.PreventsColdness = this.CachedReference.PreventsColdness;

                this.AttackSpeed = this.CachedReference.AttackSpeed;
                this.RecoveryRate = this.CachedReference.RecoveryRate;
                this.KnockBackChance = this.CachedReference.KnockBackChance;

                this.RequiredStrength = this.CachedReference.RequiredStrength;
                this.RequiredDexterity = this.CachedReference.RequiredDexterity;
                this.RequiredIntelligence = this.CachedReference.RequiredIntelligence;
                this.RequiredLuck = this.CachedReference.RequiredLuck;
                this.RequiredFame = this.CachedReference.RequiredFame;
                this.RequiredJob = this.CachedReference.RequiredJob;

                this.UpgradesAvailable = this.CachedReference.UpgradesAvailable;
                this.UpgradesApplied = this.CachedReference.UpgradesApplied;
                this.Strength = this.CachedReference.Strength;
                this.Dexterity = this.CachedReference.Dexterity;
                this.Intelligence = this.CachedReference.Intelligence;
                this.Luck = this.CachedReference.Luck;
                this.Health = this.CachedReference.Health;
                this.Mana = this.CachedReference.Mana;
                this.WeaponAttack = this.CachedReference.WeaponAttack;
                this.MagicAttack = this.CachedReference.MagicAttack;
                this.WeaponDefense = this.CachedReference.WeaponDefense;
                this.MagicDefense = this.CachedReference.MagicDefense;
                this.Accuracy = this.CachedReference.Accuracy;
                this.Avoidability = this.CachedReference.Avoidability;
                this.Agility = this.CachedReference.Agility;
                this.Speed = this.CachedReference.Speed;
                this.Jump = this.CachedReference.Jump;
            }
            else if (this.IsConsumable)
            {

                this.CFlags = this.CachedReference.CFlags;
                this.CCureAilments = this.CachedReference.CCureAilments;
                this.CEffect = this.CachedReference.CEffect;
                this.CHealth = this.CachedReference.CHealth;
                this.CMana = this.CachedReference.CMana;
                this.CHealthPercentage = this.CachedReference.CHealthPercentage;
                this.CManaPercentage = this.CachedReference.CManaPercentage;
                this.CMoveTo = this.CachedReference.CMoveTo;
                this.CProb = this.CachedReference.CProb;
                this.CBuffTime = this.CachedReference.CBuffTime;
                this.CWeaponAttack = this.CachedReference.CWeaponAttack;
                this.CMagicAttack = this.CachedReference.CMagicAttack;
                this.CWeaponDefense = this.CachedReference.CWeaponDefense;
                this.CMagicDefense = this.CachedReference.CMagicDefense;
                this.CAccuracy = this.CachedReference.CAccuracy;
                this.CAvoid = this.CachedReference.CAvoid;
                this.CSpeed = this.CachedReference.CSpeed;
                this.CJump = this.CachedReference.CJump;
                this.CMorph = this.CachedReference.CMorph;
            }

            this.Summons = this.CachedReference.Summons;
        }

        public Item(Datum datum)
        {
            if (DataProvider.IsInitialized)
            {
                this.ID = (int)datum["ID"];
                this.Assigned = true;

                this.AccountID = (int)datum["AccountID"];
                this.MapleID = (int)datum["MapleID"];
                this.MaxPerStack = this.CachedReference.MaxPerStack;
                this.Quantity = (short)datum["Quantity"];
                this.Slot = (short)datum["Slot"];
                this.Creator = (string)datum["Creator"];
                this.Expiration = (DateTime)datum["Expiration"];
                this.PetID = (int?)datum["PetID"];

                this.IsCash = this.CachedReference.IsCash;
                this.OnlyOne = this.CachedReference.OnlyOne;
                this.IsTradeBlocked = this.CachedReference.IsTradeBlocked;
                this.IsScissored = false;
                this.IsStored = (bool)datum["IsStored"];
                this.SalePrice = this.CachedReference.SalePrice;
                this.RequiredLevel = this.CachedReference.RequiredLevel;
                this.Meso = this.CachedReference.Meso;

                if (this.ItemType == ItemConstants.ItemType.Equipment)
                {
                    this.AttackSpeed = this.CachedReference.AttackSpeed;
                    this.RecoveryRate = this.CachedReference.RecoveryRate;
                    this.KnockBackChance = this.CachedReference.KnockBackChance;

                    this.RequiredStrength = this.CachedReference.RequiredStrength;
                    this.RequiredDexterity = this.CachedReference.RequiredDexterity;
                    this.RequiredIntelligence = this.CachedReference.RequiredIntelligence;
                    this.RequiredLuck = this.CachedReference.RequiredLuck;
                    this.RequiredFame = this.CachedReference.RequiredFame;
                    this.RequiredJob = this.CachedReference.RequiredJob;

                    this.UpgradesAvailable = (byte)datum["UpgradesAvailable"];
                    this.UpgradesApplied = (byte)datum["UpgradesApplied"];
                    this.Strength = (short)datum["Strength"];
                    this.Dexterity = (short)datum["Dexterity"];
                    this.Intelligence = (short)datum["Intelligence"];
                    this.Luck = (short)datum["Luck"];
                    this.Health = (short)datum["Health"];
                    this.Mana = (short)datum["Mana"];
                    this.WeaponAttack = (short)datum["WeaponAttack"];
                    this.MagicAttack = (short)datum["MagicAttack"];
                    this.WeaponDefense = (short)datum["WeaponDefense"];
                    this.MagicDefense = (short)datum["MagicDefense"];
                    this.Accuracy = (short)datum["Accuracy"];
                    this.Avoidability = (short)datum["Avoidability"];
                    this.Agility = (short)datum["Agility"];
                    this.Speed = (short)datum["Speed"];
                    this.Jump = (short)datum["Jump"];
                }
                else if (this.IsConsumable)
                {
                    this.CFlags = this.CachedReference.CFlags;
                    this.CCureAilments = this.CachedReference.CCureAilments;
                    this.CEffect = this.CachedReference.CEffect;
                    this.CHealth = this.CachedReference.CHealth;
                    this.CMana = this.CachedReference.CMana;
                    this.CHealthPercentage = this.CachedReference.CHealthPercentage;
                    this.CManaPercentage = this.CachedReference.CManaPercentage;
                    this.CMoveTo = this.CachedReference.CMoveTo;
                    this.CProb = this.CachedReference.CProb;
                    this.CBuffTime = this.CachedReference.CBuffTime;
                    this.CWeaponAttack = this.CachedReference.CWeaponAttack;
                    this.CMagicAttack = this.CachedReference.CMagicAttack;
                    this.CWeaponDefense = this.CachedReference.CWeaponDefense;
                    this.CMagicDefense = this.CachedReference.CMagicDefense;
                    this.CAccuracy = this.CachedReference.CAccuracy;
                    this.CAvoid = this.CachedReference.CAvoid;
                    this.CSpeed = this.CachedReference.CSpeed;
                    this.CJump = this.CachedReference.CJump;
                    this.CMorph = this.CachedReference.CMorph;
                }

                this.Summons = this.CachedReference.Summons;
            }
            else
            {
                this.MapleID = (int)datum["itemid"];
                this.MaxPerStack = (short)datum["max_slot_quantity"];

                this.IsCash = ((string)datum["flags"]).Contains("cash_item");
                this.OnlyOne = (sbyte)datum["max_possession_count"] > 0;
                this.IsTradeBlocked = ((string)datum["flags"]).Contains("no_trade");
                this.IsScissored = false;
                this.SalePrice = (int)datum["price"];
                this.RequiredLevel = (byte)datum["min_level"];
                this.Meso = (int)datum["money"];

                this.Summons = new List<Tuple<int, short>>();
            }
        }

        public void LoadConsumeData(Datum datum)
        {
            //this.CFlags = datum["flags"];
            //this.CCureAilments = datum["cure_ailments"];
            this.CEffect = (byte)datum["effect"];
            this.CHealth = (short)datum["hp"];
            this.CMana = (short)datum["mp"];
            this.CHealthPercentage = (short)datum["hp_percentage"];
            this.CManaPercentage = (short)datum["mp_percentage"];
            this.CMoveTo = (int)datum["move_to"];
            this.CProb = (byte)datum["prob"];
            this.CBuffTime = (int)datum["buff_time"];
            this.CWeaponAttack = (short)datum["weapon_attack"];
            this.CMagicAttack = (short)datum["magic_attack"];
            this.CWeaponDefense = (short)datum["weapon_defense"];
            this.CMagicDefense = (short)datum["magic_defense"];
            this.CAccuracy = (short)datum["accuracy"];
            this.CAvoid = (short)datum["avoid"];
            this.CSpeed = (short)datum["speed"];
            this.CJump = (short)datum["jump"];
            this.CMorph = (short)datum["morph"];
        }

        public void LoadEquipmentData(Datum datum)
        {
            this.RequiredStrength = (short)datum["req_str"];
            this.RequiredDexterity = (short)datum["req_dex"];
            this.RequiredIntelligence = (short)datum["req_int"];
            this.RequiredLuck = (short)datum["req_luk"];
            this.RequiredFame = (short)datum["req_fame"];

            this.UpgradesAvailable = (byte)(ushort)datum["scroll_slots"];
            this.UpgradesApplied = 0;

            this.Health = (short)datum["hp"];
            this.Mana = (short)datum["mp"];
            this.Strength = (short)datum["strength"];
            this.Dexterity = (short)datum["dexterity"];
            this.Intelligence = (short)datum["intelligence"];
            this.Luck = (short)datum["luck"];
            this.WeaponAttack = (short)datum["weapon_attack"];
            this.WeaponDefense = (short)datum["weapon_defense"];
            this.MagicAttack = (short)datum["magic_attack"];
            this.MagicDefense = (short)datum["magic_defense"];
            this.Accuracy = (short)datum["accuracy"];
            this.Avoidability = (short)datum["avoid"];
            this.Speed = (short)datum["speed"];
            this.Jump = (short)datum["jump"];
            this.Agility = (short)datum["hands"];
        }

        public void Save()
        {
            Datum datum = new Datum("items")
            {
                ["AccountID"] = this.Character.AccountID,
                ["CharacterID"] = this.Character.ID,
                ["MapleID"] = this.MapleID,
                ["Quantity"] = this.Quantity,
                ["Slot"] = this.Slot,
                ["Creator"] = this.Creator,
                ["UpgradesAvailable"] = this.UpgradesAvailable,
                ["UpgradesApplied"] = this.UpgradesApplied,
                ["Strength"] = this.Strength,
                ["Dexterity"] = this.Dexterity,
                ["Intelligence"] = this.Intelligence,
                ["Luck"] = this.Luck,
                ["Health"] = this.Health,
                ["Mana"] = this.Mana,
                ["WeaponAttack"] = this.WeaponAttack,
                ["MagicAttack"] = this.MagicAttack,
                ["WeaponDefense"] = this.WeaponDefense,
                ["MagicDefense"] = this.MagicDefense,
                ["Accuracy"] = this.Accuracy,
                ["Avoidability"] = this.Avoidability,
                ["Agility"] = this.Agility,
                ["Speed"] = this.Speed,
                ["Jump"] = this.Jump,
                ["IsScissored"] = this.IsScissored,
                ["IsStored"] = this.IsStored,
                ["PreventsSlipping"] = this.PreventsSlipping,
                ["PreventsColdness"] = this.PreventsColdness
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
        }

        public static void DeleteItemFromDB(Item item)
        {
            Database.Delete("items", "ID = {0}", item.ID);

            item.Assigned = false;
        }

        public static void UpdateItem(Item item)
        {
            item.Character.Client.Send(ItemPackets.UpdateItems(item));
        }

        public static void EquipItem(Item item)
        {
            // Check ItemType
            if (item.ItemType != ItemConstants.ItemType.Equipment)
            {
                throw new InvalidOperationException("Can only equip equipment items.");
            }

            // If were not VIP user then check for requirements
            if (!item.Character.IsMaster)
            {
                if (item.Character.Stats.Strength < item.RequiredStrength || item.Character.Stats.Dexterity < item.RequiredDexterity 
                     || item.Character.Stats.Intelligence < item.RequiredIntelligence || item.Character.Stats.Luck < item.RequiredLuck
                     || item.Character.Stats.Fame < item.RequiredFame)
                { return; }
            }
            
            // slot to move item from
            short sourceSlot = item.Slot;
            // slot to move item into
            ItemConstants.EquipmentSlot destinationSlot = item.GetEquippedSlot();

            // get char's items
            Item top = item.Parent[ItemConstants.EquipmentSlot.Top];
            Item bottom = item.Parent[ItemConstants.EquipmentSlot.Bottom];
            Item weapon = item.Parent[ItemConstants.EquipmentSlot.Weapon];
            Item shield = item.Parent[ItemConstants.EquipmentSlot.Shield];

            // get item at char's destination slot
            Item destination = item.Parent[destinationSlot]; 

            // if there is an item
            if (destination != null) 
            {
                // put it into slot of item to be replaced with
                destination.Slot = sourceSlot; 
            }

            // item to replace acquires destination slot
            item.Slot = (short)destinationSlot;

            // unequipped blocking items
            switch (destinationSlot)
            {
                case ItemConstants.EquipmentSlot.Bottom:
                {
                    if (top?.IsOverall == true)
                    {
                        UnequipItem(top);
                    }
                }
                    break;

                case ItemConstants.EquipmentSlot.Top:
                {
                    if (item.IsOverall && bottom != null)
                    {
                        UnequipItem(bottom);
                    }
                }
                    break;

                case ItemConstants.EquipmentSlot.Shield:
                {
                    if (weapon?.IsTwoHanded == true)
                    {
                        UnequipItem(weapon);
                    }
                }
                    break;

                case ItemConstants.EquipmentSlot.Weapon:
                {
                    if (item.IsTwoHanded && shield != null)
                    {
                        UnequipItem(shield);
                    }
                }
                    break;
            }

            // send packet to carry out the equipped action
            item.Character.Client.Send(ItemPackets.EquipOrUnequipItems(item, sourceSlot, (short)destinationSlot));

            // update char appearance
            CharacterAppearance.UpdateApperance(item.Character);
        }

        public static void UnequipItem(Item item, short destinationSlot = 0)
        {
            //check for item type
            if (item.ItemType != ItemConstants.ItemType.Equipment)
            {
                throw new InvalidOperationException("Can only unequip equipment items.");
            }

            // slot to move item from
            short sourceSlot = item.Slot;

            // if no destination given then find first free slot for equip type
            if (destinationSlot == 0)
            {
                destinationSlot = item.Parent.GetNextFreeSlot(ItemConstants.ItemType.Equipment);
            }

            // change slot of item being unequipped to free equip slot
            item.Slot = destinationSlot;

            // send packet to carry out the un-equip item action
            item.Character.Client.Send(ItemPackets.EquipOrUnequipItems(item, sourceSlot, destinationSlot));

            // update char appearance
            CharacterAppearance.UpdateApperance(item.Character);
        }

        public static void DropItem(Item item, short dropQuantity)
        {
            // cannot drop blocked items
            if (item.IsBlocked)
            {
                // TODO: notify character
                return;
            }

            // no multiplication of rechargeable items
            if (item.IsRechargeable)
            {
                dropQuantity = item.Quantity;
            }

            // cannot drop more then i have
            if (dropQuantity > item.Quantity)
            {
                // TODO: notify character
                return;
            }
              
            if (dropQuantity == item.Quantity)
            {
                // send packet to carry out the remove item action
                item.Character.Client.Send(ItemPackets.RemoveItem(item));

                // update item ownership properties
                item.Dropper = item.Character;
                item.Owner = null;

                // create drop on map
                item.Character.Map.Drops.Add(item);

                // remove item from dropper's inventory
                item.Parent.RemoveItemFromInventory(item, false);
            }
            else if (dropQuantity < item.Quantity)
            {
                // subtract the quantity to drop
                item.Quantity -= dropQuantity;

                // send packet to carry out the item quantity update action
                item.Character.Client.Send(ItemPackets.ModifyItemsQuantity(item));

                // create new drop item with droppers properties
                Item dropped = new Item(item.MapleID, quantity)
                {
                    Dropper = item.Character,
                    Owner = null
                };

                // add dropped item to map
                item.Character.Map.Drops.Add(dropped);
            }
        }

        public static bool IsStackableNotRechargeable(Item item)
        {
            return !item.IsRechargeable && item.ItemType != ItemConstants.ItemType.Equipment && item.MaxPerStack > 0;
        }

        public static bool CanStackItems(Item itemToStack, Item itemToBeStackedOnto)
        {
            return itemToBeStackedOnto != null && itemToStack.MapleID == itemToBeStackedOnto.MapleID &&
                   itemToBeStackedOnto.Quantity < itemToBeStackedOnto.MaxPerStack;
        }

        public static void MoveItem(Item item, short destinationSlot)
        {
            // slot to move item from
            short sourceSlot = item.Slot;

            // item in slot to move item to
            Item itemAtDestinationSlot = item.Parent[item.ItemType, destinationSlot];

            // case of moving non-rechargeable non-equip stackable items
            // TODO: proper treatment
            if (IsStackableNotRechargeable(item) && CanStackItems(item, itemAtDestinationSlot))
            {
                int totalQuantity = item.Quantity + itemAtDestinationSlot.Quantity;

                // Sub-case 1: stack at destination will overflow on addition
                if (totalQuantity > itemAtDestinationSlot.MaxPerStack)
                {
                    // subtract whats left at destination slot
                    item.Quantity -= (short)(itemAtDestinationSlot.MaxPerStack-itemAtDestinationSlot.Quantity);

                    // send packet to do stacking action when stack at destination is full
                    item.Character.Client.Send(ItemPackets.StackItemsFullStack(item, itemAtDestinationSlot));
                }
                // Sub-case 2: stack at destination wont overflow on addition
                else
                {
                    // add quantity of itemFirst to itemSecond
                    itemAtDestinationSlot.Quantity += item.Quantity;

                    // send packet to do stacking action
                    item.Character.Client.Send(ItemPackets.StackItems(item, itemAtDestinationSlot));
                }
            }
            // other cases
            else
            {
                // if there is an item at destination slot move it to source slot
                if (itemAtDestinationSlot != null)
                {
                    itemAtDestinationSlot.Slot = sourceSlot;
                }

                // move item to destination slot
                item.Slot = destinationSlot;

                // send packet to do item move action
                item.Character.Client.Send(ItemPackets.MoveItem(item, sourceSlot, destinationSlot));
            }
        }

        private ItemConstants.EquipmentSlot GetEquippedSlot()
        {
            ItemConstants.EquipmentSlot slot = 0;

            if (EquipmentConstants.HatsMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Hat;
            }
            else if (EquipmentConstants.FaceAccMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.FaceAccessory;
            }
            else if (EquipmentConstants.EyeAccMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.EyeAccessory;
            }
            else if (EquipmentConstants.EarringsMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Earrings;
            }
            else if (EquipmentConstants.TopsMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Top;
            }
            else if (EquipmentConstants.BottomsMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Bottom;
            }
            else if (EquipmentConstants.ShoesMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Shoes;
            }
            else if (EquipmentConstants.GlovesMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Gloves;
            }
            else if (EquipmentConstants.CapesMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Cape;
            }
            else if (EquipmentConstants.ShieldsMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Shield;
            }
            else if (EquipmentConstants.WeaponsMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Weapon;
            }
            else if (EquipmentConstants.RingsMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Ring1;
            }
            else if (EquipmentConstants.NecklacesMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Necklace;
            }
            else if (EquipmentConstants.MountsEquipMapleIDs.Contains(this.MapleID))
            {
                slot = ItemConstants.EquipmentSlot.MountEquip;
            }

            if (this.IsCash)
            {
                slot -= 100;
            }

            return slot;
        }

        public override Packet GetShowGainPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.Message);

            oPacket
                .WriteByte((byte)ServerConstants.MessageType.DropPickup)
                .WriteBool(false)
                .WriteInt(this.MapleID)
                .WriteInt(this.Quantity)
                .WriteInt()
                .WriteInt();

            return oPacket;
        }

        /*public static Packet GetShowItemGainPacket(bool white, int itemID, int ammount, bool inChat)
        {
           return Character.GetShowSidebarInfoPacket(MessageType.DropPickup, white, itemID, ammount, inChat, 0, 0);
        }*/

        public byte[] ToByteArray(bool zeroPosition = false, bool leaveOut = false)
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                if (!zeroPosition && !leaveOut)
                {
                    byte slot = this.ComputedSlot;

                    if (slot < 0)
                    {
                        slot = (byte)(slot * -1);
                    }
                    else if (slot > 100)
                    {
                        slot -= 100;
                    }

                    if (this.ItemType == ItemConstants.ItemType.Equipment)
                    {
                        oPacket.WriteShort(slot);
                    }
                    else
                    {
                        oPacket.WriteByte(slot);
                    }
                }

                oPacket
                    .WriteByte((byte)(this.PetID != null ? 3 : this.ItemType == ItemConstants.ItemType.Equipment ? 1 : 2))
                    .WriteInt(this.MapleID)
                    .WriteBool(this.IsCash);

                if (this.IsCash)
                {
                    oPacket.WriteLong(1); // TODO: Unique ID for cash items.
                }

                oPacket.WriteDateTime(this.Expiration);

                if (this.PetID != null)
                {

                }
                else if (this.ItemType == ItemConstants.ItemType.Equipment)
                {
                    oPacket
                        .WriteByte(this.UpgradesAvailable)
                        .WriteByte(this.UpgradesApplied)
                        .WriteShort(this.Strength)
                        .WriteShort(this.Dexterity)
                        .WriteShort(this.Intelligence)
                        .WriteShort(this.Luck)
                        .WriteShort(this.Health)
                        .WriteShort(this.Mana)
                        .WriteShort(this.WeaponAttack)
                        .WriteShort(this.MagicAttack)
                        .WriteShort(this.WeaponDefense)
                        .WriteShort(this.MagicDefense)
                        .WriteShort(this.Accuracy)
                        .WriteShort(this.Avoidability)
                        .WriteShort(this.Agility)
                        .WriteShort(this.Speed)
                        .WriteShort(this.Jump)
                        .WriteString(this.Creator)
                        .WriteByte(this.Flags)
                        .WriteByte();

                    if (!this.IsEquippedCash)
                    {
                        oPacket
                            .WriteByte()
                            .WriteByte()
                            .WriteShort()
                            .WriteShort()
                            .WriteInt()
                            .WriteLong()
                            .WriteLong()
                            .WriteInt(-1);
                    }
                }
                else
                {
                    oPacket
                        .WriteShort(this.Quantity)
                        .WriteString(this.Creator)
                        .WriteByte(this.Flags)
                        .WriteByte();

                    if (this.IsRechargeable)
                    {
                        oPacket.WriteLong(); // TODO: Unique ID.
                    }
                }

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }
    }
}