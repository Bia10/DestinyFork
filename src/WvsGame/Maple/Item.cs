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
                return GetType(MapleID);
            }
        }

        public ItemConstants.WeaponType WeaponType
        {
            get
            {
                switch (MapleID / 10000 % 100)
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
                return DataProvider.Items[MapleID];
            }
        }

        public Character Character
        {
            get
            {
                return Parent.Parent;
            }
        }

        public short MaxPerStack
        {
            get
            {
                if (IsRechargeable && Parent != null)
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
                if (value > MaxPerStack)
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
                return DataProvider.Items.WizetItemIDs.Contains(MapleID);
            }
        }

        public byte Flags
        {
            get
            {
                byte flags = 0;

                if (IsSealed) flags |= (byte)ItemConstants.ItemFlags.Sealed;
                if (PreventsSlipping) flags |= (byte)ItemConstants.ItemFlags.AddPreventSlipping;
                if (PreventsColdness) flags |= (byte)ItemConstants.ItemFlags.AddPreventColdness;
                if (IsScissored) flags |= (byte)ItemConstants.ItemFlags.Scissored;
                if (IsTradeBlocked) flags |= (byte)ItemConstants.ItemFlags.Untradeable;

                return flags;
            }
        }

        public bool IsEquipped
        {
            get
            {
                return Slot < 0;
            }
        }

        public bool IsEquippedCash
        {
            get
            {
                return Slot < -100;
            }
        }

        public bool IsConsumable
        {
            get
            {
                return MapleID / 10000 >= 200 && MapleID / 10000 < 204;
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
                return IsThrowingStar || IsBullet;
            }
        }

        public bool IsThrowingStar
        {
            get
            {
                return MapleID / 10000 == 207;
            }
        }

        public bool IsBullet
        {
            get
            {
                return MapleID / 10000 == 233;
            }
        }

        public bool IsArrow
        {
            get
            {
                return IsArrowForBow || IsArrowForCrossbow;
            }
        }

        public bool IsArrowForBow
        {
            get
            {
                return MapleID >= 2060000 && MapleID < 2061000;
            }
        }

        public bool IsArrowForCrossbow
        {
            get
            {
                return MapleID >= 2061000 && MapleID < 2062000;
            }
        }

        public bool IsOverall
        {
            get
            {
                return MapleID / 10000 == 105;
            }
        }

        public bool IsWeapon
        {
            get
            {
                return WeaponType != ItemConstants.WeaponType.NotAWeapon;
            }
        }

        public bool IsShield
        {
            get
            {
                return MapleID / 10000 % 100 == 9;
            }
        }

        public bool IsPet
        {
            get
            {
                return MapleID >= 5000000 && MapleID <= 5000100;
            }
        }

        public bool IsTownScroll
        {
            get
            {
                return MapleID >= 2030000 && MapleID < 2030020;
            }
        }

        public bool IsTwoHanded
        {
            get
            {
                switch (WeaponType)
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
                return IsCash || IsSealed || (IsTradeBlocked && !IsScissored);
            }
        }

        public byte AbsoluteSlot
        {
            get
            {
                if (IsEquipped)
                {
                    return (byte)(Slot * -1);
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
                if (IsEquippedCash)
                {
                    return ((byte)(AbsoluteSlot - 100));
                }
                else if (IsEquipped)
                {
                    return AbsoluteSlot;
                }
                else
                {
                    return (byte)Slot;
                }
            }
        }

        public bool Assigned { get; set; }

        public Item(int mapleID, short quantity = 1, DateTime? expiration = null, bool equipped = false)
        {
            MapleID = mapleID;
            MaxPerStack = CachedReference.MaxPerStack;
            Quantity = (ItemType == ItemConstants.ItemType.Equipment) ? (short)1 : quantity;
            if (equipped) Slot = (short)GetEquippedSlot();

            if (!expiration.HasValue)
            {
                expiration = new DateTime(2079, 1, 1, 12, 0, 0); // NOTE: Default expiration time (permanent).
            }

            Expiration = (DateTime)expiration;

            IsCash = CachedReference.IsCash;
            OnlyOne = CachedReference.OnlyOne;
            IsTradeBlocked = CachedReference.IsTradeBlocked;
            IsScissored = CachedReference.IsScissored;
            SalePrice = CachedReference.SalePrice;
            RequiredLevel = CachedReference.RequiredLevel;
            Meso = CachedReference.Meso;

            if (ItemType == ItemConstants.ItemType.Equipment)
            {
                PreventsSlipping = CachedReference.PreventsSlipping;
                PreventsColdness = CachedReference.PreventsColdness;

                AttackSpeed = CachedReference.AttackSpeed;
                RecoveryRate = CachedReference.RecoveryRate;
                KnockBackChance = CachedReference.KnockBackChance;

                RequiredStrength = CachedReference.RequiredStrength;
                RequiredDexterity = CachedReference.RequiredDexterity;
                RequiredIntelligence = CachedReference.RequiredIntelligence;
                RequiredLuck = CachedReference.RequiredLuck;
                RequiredFame = CachedReference.RequiredFame;
                RequiredJob = CachedReference.RequiredJob;

                UpgradesAvailable = CachedReference.UpgradesAvailable;
                UpgradesApplied = CachedReference.UpgradesApplied;
                Strength = CachedReference.Strength;
                Dexterity = CachedReference.Dexterity;
                Intelligence = CachedReference.Intelligence;
                Luck = CachedReference.Luck;
                Health = CachedReference.Health;
                Mana = CachedReference.Mana;
                WeaponAttack = CachedReference.WeaponAttack;
                MagicAttack = CachedReference.MagicAttack;
                WeaponDefense = CachedReference.WeaponDefense;
                MagicDefense = CachedReference.MagicDefense;
                Accuracy = CachedReference.Accuracy;
                Avoidability = CachedReference.Avoidability;
                Agility = CachedReference.Agility;
                Speed = CachedReference.Speed;
                Jump = CachedReference.Jump;
            }
            else if (IsConsumable)
            {

                CFlags = CachedReference.CFlags;
                CCureAilments = CachedReference.CCureAilments;
                CEffect = CachedReference.CEffect;
                CHealth = CachedReference.CHealth;
                CMana = CachedReference.CMana;
                CHealthPercentage = CachedReference.CHealthPercentage;
                CManaPercentage = CachedReference.CManaPercentage;
                CMoveTo = CachedReference.CMoveTo;
                CProb = CachedReference.CProb;
                CBuffTime = CachedReference.CBuffTime;
                CWeaponAttack = CachedReference.CWeaponAttack;
                CMagicAttack = CachedReference.CMagicAttack;
                CWeaponDefense = CachedReference.CWeaponDefense;
                CMagicDefense = CachedReference.CMagicDefense;
                CAccuracy = CachedReference.CAccuracy;
                CAvoid = CachedReference.CAvoid;
                CSpeed = CachedReference.CSpeed;
                CJump = CachedReference.CJump;
                CMorph = CachedReference.CMorph;
            }

            Summons = CachedReference.Summons;
        }

        public Item(Datum datum)
        {
            if (DataProvider.IsInitialized)
            {
                ID = (int)datum["ID"];
                Assigned = true;

                AccountID = (int)datum["AccountID"];
                MapleID = (int)datum["MapleID"];
                MaxPerStack = CachedReference.MaxPerStack;
                Quantity = (short)datum["Quantity"];
                Slot = (short)datum["Slot"];
                Creator = (string)datum["Creator"];
                Expiration = (DateTime)datum["Expiration"];
                PetID = (int?)datum["PetID"];

                IsCash = CachedReference.IsCash;
                OnlyOne = CachedReference.OnlyOne;
                IsTradeBlocked = CachedReference.IsTradeBlocked;
                IsScissored = false;
                IsStored = (bool)datum["IsStored"];
                SalePrice = CachedReference.SalePrice;
                RequiredLevel = CachedReference.RequiredLevel;
                Meso = CachedReference.Meso;

                if (ItemType == ItemConstants.ItemType.Equipment)
                {
                    AttackSpeed = CachedReference.AttackSpeed;
                    RecoveryRate = CachedReference.RecoveryRate;
                    KnockBackChance = CachedReference.KnockBackChance;

                    RequiredStrength = CachedReference.RequiredStrength;
                    RequiredDexterity = CachedReference.RequiredDexterity;
                    RequiredIntelligence = CachedReference.RequiredIntelligence;
                    RequiredLuck = CachedReference.RequiredLuck;
                    RequiredFame = CachedReference.RequiredFame;
                    RequiredJob = CachedReference.RequiredJob;

                    UpgradesAvailable = (byte)datum["UpgradesAvailable"];
                    UpgradesApplied = (byte)datum["UpgradesApplied"];
                    Strength = (short)datum["Strength"];
                    Dexterity = (short)datum["Dexterity"];
                    Intelligence = (short)datum["Intelligence"];
                    Luck = (short)datum["Luck"];
                    Health = (short)datum["Health"];
                    Mana = (short)datum["Mana"];
                    WeaponAttack = (short)datum["WeaponAttack"];
                    MagicAttack = (short)datum["MagicAttack"];
                    WeaponDefense = (short)datum["WeaponDefense"];
                    MagicDefense = (short)datum["MagicDefense"];
                    Accuracy = (short)datum["Accuracy"];
                    Avoidability = (short)datum["Avoidability"];
                    Agility = (short)datum["Agility"];
                    Speed = (short)datum["Speed"];
                    Jump = (short)datum["Jump"];
                }
                else if (IsConsumable)
                {
                    CFlags = CachedReference.CFlags;
                    CCureAilments = CachedReference.CCureAilments;
                    CEffect = CachedReference.CEffect;
                    CHealth = CachedReference.CHealth;
                    CMana = CachedReference.CMana;
                    CHealthPercentage = CachedReference.CHealthPercentage;
                    CManaPercentage = CachedReference.CManaPercentage;
                    CMoveTo = CachedReference.CMoveTo;
                    CProb = CachedReference.CProb;
                    CBuffTime = CachedReference.CBuffTime;
                    CWeaponAttack = CachedReference.CWeaponAttack;
                    CMagicAttack = CachedReference.CMagicAttack;
                    CWeaponDefense = CachedReference.CWeaponDefense;
                    CMagicDefense = CachedReference.CMagicDefense;
                    CAccuracy = CachedReference.CAccuracy;
                    CAvoid = CachedReference.CAvoid;
                    CSpeed = CachedReference.CSpeed;
                    CJump = CachedReference.CJump;
                    CMorph = CachedReference.CMorph;
                }

                Summons = CachedReference.Summons;
            }
            else
            {
                MapleID = (int)datum["itemid"];
                MaxPerStack = (short)datum["max_slot_quantity"];

                IsCash = ((string)datum["flags"]).Contains("cash_item");
                OnlyOne = (sbyte)datum["max_possession_count"] > 0;
                IsTradeBlocked = ((string)datum["flags"]).Contains("no_trade");
                IsScissored = false;
                SalePrice = (int)datum["price"];
                RequiredLevel = (byte)datum["min_level"];
                Meso = (int)datum["money"];

                Summons = new List<Tuple<int, short>>();
            }
        }

        public void LoadConsumeData(Datum datum)
        {
            //CFlags = datum["flags"];
            //CCureAilments = datum["cure_ailments"];
            CEffect = (byte)datum["effect"];
            CHealth = (short)datum["hp"];
            CMana = (short)datum["mp"];
            CHealthPercentage = (short)datum["hp_percentage"];
            CManaPercentage = (short)datum["mp_percentage"];
            CMoveTo = (int)datum["move_to"];
            CProb = (byte)datum["prob"];
            CBuffTime = (int)datum["buff_time"];
            CWeaponAttack = (short)datum["weapon_attack"];
            CMagicAttack = (short)datum["magic_attack"];
            CWeaponDefense = (short)datum["weapon_defense"];
            CMagicDefense = (short)datum["magic_defense"];
            CAccuracy = (short)datum["accuracy"];
            CAvoid = (short)datum["avoid"];
            CSpeed = (short)datum["speed"];
            CJump = (short)datum["jump"];
            CMorph = (short)datum["morph"];
        }

        public void LoadEquipmentData(Datum datum)
        {
            RequiredStrength = (short)datum["req_str"];
            RequiredDexterity = (short)datum["req_dex"];
            RequiredIntelligence = (short)datum["req_int"];
            RequiredLuck = (short)datum["req_luk"];
            RequiredFame = (short)datum["req_fame"];

            UpgradesAvailable = (byte)(ushort)datum["scroll_slots"];
            UpgradesApplied = 0;

            Health = (short)datum["hp"];
            Mana = (short)datum["mp"];
            Strength = (short)datum["strength"];
            Dexterity = (short)datum["dexterity"];
            Intelligence = (short)datum["intelligence"];
            Luck = (short)datum["luck"];
            WeaponAttack = (short)datum["weapon_attack"];
            WeaponDefense = (short)datum["weapon_defense"];
            MagicAttack = (short)datum["magic_attack"];
            MagicDefense = (short)datum["magic_defense"];
            Accuracy = (short)datum["accuracy"];
            Avoidability = (short)datum["avoid"];
            Speed = (short)datum["speed"];
            Jump = (short)datum["jump"];
            Agility = (short)datum["hands"];
        }

        public void Save()
        {
            Datum datum = new Datum("items")
            {
                ["AccountID"] = Character.AccountID,
                ["CharacterID"] = Character.ID,
                ["MapleID"] = MapleID,
                ["Quantity"] = Quantity,
                ["Slot"] = Slot,
                ["Creator"] = Creator,
                ["UpgradesAvailable"] = UpgradesAvailable,
                ["UpgradesApplied"] = UpgradesApplied,
                ["Strength"] = Strength,
                ["Dexterity"] = Dexterity,
                ["Intelligence"] = Intelligence,
                ["Luck"] = Luck,
                ["Health"] = Health,
                ["Mana"] = Mana,
                ["WeaponAttack"] = WeaponAttack,
                ["MagicAttack"] = MagicAttack,
                ["WeaponDefense"] = WeaponDefense,
                ["MagicDefense"] = MagicDefense,
                ["Accuracy"] = Accuracy,
                ["Avoidability"] = Avoidability,
                ["Agility"] = Agility,
                ["Speed"] = Speed,
                ["Jump"] = Jump,
                ["IsScissored"] = IsScissored,
                ["IsStored"] = IsStored,
                ["PreventsSlipping"] = PreventsSlipping,
                ["PreventsColdness"] = PreventsColdness
            };


            if (Assigned)
            {
                datum.Update("ID = {0}", ID);
            }
            else
            {
                ID = datum.InsertAndReturnID();
                Assigned = true;
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

            if (EquipmentConstants.HatsMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Hat;
            }
            else if (EquipmentConstants.FaceAccMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.FaceAccessory;
            }
            else if (EquipmentConstants.EyeAccMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.EyeAccessory;
            }
            else if (EquipmentConstants.EarringsMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Earrings;
            }
            else if (EquipmentConstants.TopsMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Top;
            }
            else if (EquipmentConstants.BottomsMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Bottom;
            }
            else if (EquipmentConstants.ShoesMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Shoes;
            }
            else if (EquipmentConstants.GlovesMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Gloves;
            }
            else if (EquipmentConstants.CapesMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Cape;
            }
            else if (EquipmentConstants.ShieldsMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Shield;
            }
            else if (EquipmentConstants.WeaponsMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Weapon;
            }
            else if (EquipmentConstants.RingsMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Ring1;
            }
            else if (EquipmentConstants.NecklacesMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.Necklace;
            }
            else if (EquipmentConstants.MountsEquipMapleIDs.Contains(MapleID))
            {
                slot = ItemConstants.EquipmentSlot.MountEquip;
            }

            if (IsCash)
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
                .WriteInt(MapleID)
                .WriteInt(Quantity)
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
                    byte slot = ComputedSlot;

                    if (slot < 0)
                    {
                        slot = (byte)(slot * -1);
                    }
                    else if (slot > 100)
                    {
                        slot -= 100;
                    }

                    if (ItemType == ItemConstants.ItemType.Equipment)
                    {
                        oPacket.WriteShort(slot);
                    }
                    else
                    {
                        oPacket.WriteByte(slot);
                    }
                }

                oPacket
                    .WriteByte((byte)(PetID != null ? 3 : ItemType == ItemConstants.ItemType.Equipment ? 1 : 2))
                    .WriteInt(MapleID)
                    .WriteBool(IsCash);

                if (IsCash)
                {
                    oPacket.WriteLong(1); // TODO: Unique ID for cash items.
                }

                oPacket.WriteDateTime(Expiration);

                if (PetID != null)
                {

                }
                else if (ItemType == ItemConstants.ItemType.Equipment)
                {
                    oPacket
                        .WriteByte(UpgradesAvailable)
                        .WriteByte(UpgradesApplied)
                        .WriteShort(Strength)
                        .WriteShort(Dexterity)
                        .WriteShort(Intelligence)
                        .WriteShort(Luck)
                        .WriteShort(Health)
                        .WriteShort(Mana)
                        .WriteShort(WeaponAttack)
                        .WriteShort(MagicAttack)
                        .WriteShort(WeaponDefense)
                        .WriteShort(MagicDefense)
                        .WriteShort(Accuracy)
                        .WriteShort(Avoidability)
                        .WriteShort(Agility)
                        .WriteShort(Speed)
                        .WriteShort(Jump)
                        .WriteString(Creator)
                        .WriteByte(Flags)
                        .WriteByte();

                    if (!IsEquippedCash)
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
                        .WriteShort(Quantity)
                        .WriteString(Creator)
                        .WriteByte(Flags)
                        .WriteByte();

                    if (IsRechargeable)
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