using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Destiny.Maple.Maps;
using Destiny.Data;
using Destiny.Constants;
using Destiny.Maple.Data;
using Destiny.Maple.Life;
using Destiny.IO;
using Destiny.Network.Common;
using Destiny.Network.PacketFactory;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterItems : IEnumerable<Item>
    {
        public Character Parent { get; }
        public Dictionary<ItemConstants.ItemType, byte> MaxSlots { get; }
        private List<Item> Items { get; }

        public CharacterItems(Character parent, byte equipmentSlots, byte usableSlots, byte setupSlots, byte etceteraSlots, byte cashSlots) : base()
        {
            Parent = parent;

            MaxSlots = new Dictionary<ItemConstants.ItemType, byte>(Enum.GetValues(typeof(ItemConstants.ItemType)).Length)
                {
                    {ItemConstants.ItemType.Equipment, equipmentSlots},
                    {ItemConstants.ItemType.Usable, usableSlots},
                    {ItemConstants.ItemType.Setup, setupSlots},
                    {ItemConstants.ItemType.Etcetera, etceteraSlots},
                    {ItemConstants.ItemType.Cash, cashSlots}
                };

            Items = new List<Item>();
        }

        public void LoadInventory()
        {
            // TODO: Use JOIN with the pets table.
            foreach (Datum datum in new Datums("items").Populate("CharacterID = {0} AND IsStored = 0", Parent.ID))
            {
                Item item = new Item(datum);

                AddItemToInventory(item);

                if (item.PetID != null)
                {
                    //Parent.Pets.Add(new Pet(item));
                }
            }
        }

        public void SaveInventory()
        {
            foreach (Item item in this)
            {
                item.Save();
            }
        }

        public void AddItemToInventory(Item item, bool fromDrop = false, bool autoMerge = true, bool forceGetSlot = false)
        {
            if (InventoryAvailableItemByID(item.MapleID) % item.MaxPerStack != 0 && autoMerge)
            {
                foreach (Item loopItem in this.Where(x => x.MapleID == item.MapleID && x.Quantity < x.MaxPerStack))
                {
                    if (loopItem.Quantity + item.Quantity <= loopItem.MaxPerStack)
                    {
                        loopItem.Quantity += item.Quantity;
                        Item.UpdateItem(loopItem);

                        item.Quantity = 0;

                        break;
                    }

                    item.Quantity -= (short)(loopItem.MaxPerStack - loopItem.Quantity);
                    item.Slot = GetNextFreeSlot(item.ItemType);

                    loopItem.Quantity = loopItem.MaxPerStack;

                    if (Parent.IsInitialized)
                    {
                        Item.UpdateItem(loopItem);
                    }

                    break;
                }
            }

            if (item.Quantity <= 0) return;

            item.Parent = this;

            if (Parent.IsInitialized && item.Slot == 0 || forceGetSlot)
            {
                item.Slot = GetNextFreeSlot(item.ItemType);
            }

            Items.Add(item);

            if (Parent.IsInitialized)
            {
                Parent.Client.Send(CharacterItemsPackets.AddItemToInventoryPacket(item));
            }
        }

        public void AddRangeOfItems(IEnumerable<Item> items, bool fromDrop = false, bool autoMerge = true)
        {
            foreach (Item loopItem in items)
            {
                AddItemToInventory(loopItem, fromDrop, autoMerge);
            }
        }

        public void RemoveItemFromInventoryByID(int mapleId, short quantity)
        {
            short leftToRemove = quantity;

            List<Item> toRemove = new List<Item>();

            foreach (Item loopItem in this)
            {
                if (loopItem.MapleID != mapleId) continue;

                if (loopItem.Quantity > leftToRemove)
                {
                    loopItem.Quantity -= leftToRemove;
                    Item.UpdateItem(loopItem);

                    break;
                }

                leftToRemove -= loopItem.Quantity;
                toRemove.Add(loopItem);             
            }

            foreach (Item loopItem in toRemove)
            {
                RemoveItemFromInventory(loopItem, true);
            }
        }

        public void RemoveItemFromInventory(Item item, bool removeFromSlot, bool fromDrop = false)
        {
            if (removeFromSlot && item.IsEquipped)
            {
                throw new InvalidOperationException("Cannot remove equipped items from slot.");
            }

            if (removeFromSlot)
            {
                Parent.Client.Send(CharacterItemsPackets.RemoveItemFromInventory(item));
            }

            if (item.Assigned)
            {
                Item.DeleteItemFromDB(item);
            }

            item.Parent = null;

            bool wasEquipped = item.IsEquipped;

            Items.Remove(item);

            if (wasEquipped)
            {
               CharacterAppearance.UpdateApperance(Parent);
            }
        }

        public void ClearInventory(bool removeFromSlot)
        {
            List<Item> toRemove = new List<Item>();

            foreach (Item loopItem in this)
            {
                toRemove.Add(loopItem);
            }

            foreach (Item loopItem in toRemove)
            {
                if (!(loopItem.IsEquipped && removeFromSlot))
                {
                    RemoveItemFromInventory(loopItem, removeFromSlot);
                }
            }
        }

        public bool InventoryContainsItemByID(int mapleId)
        {
            foreach (Item loopItem in this)
            {
                if (loopItem.MapleID == mapleId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool InventoryContainsItemByID(int mapleId, short quantity)
        {
            int count = 0;

            foreach (Item loopItem in this)
            {
                if (loopItem.MapleID == mapleId)
                {
                    count += loopItem.Quantity;
                }
            }

            return count >= quantity;
        }

        public int InventoryAvailableItemByID(int mapleId)
        {
            int count = 0;

            foreach (Item loopItem in this)
            {
                if (loopItem.MapleID == mapleId)
                {
                    count += loopItem.Quantity;
                }
            }

            return count;
        }

        public sbyte GetNextFreeSlot(ItemConstants.ItemType type)
        {
            for (sbyte i = 1; i <= MaxSlots[type]; i++)
            {
                if (this[type, i] == null)
                {
                    return i;
                }
            }

            NotifyInventoryIsFull(Parent, type);
            throw new InventoryFullException();
        }

        public void NotifyInventoryIsFull(Character character, ItemConstants.ItemType inventoryType)
        {
            switch (inventoryType)
            {
                case ItemConstants.ItemType.Equipment:
                    Character.Notify(character, "Your equipment(EQP) inventory is full!.", ServerConstants.NoticeType.Popup);
                    break;

                case ItemConstants.ItemType.Etcetera:
                    Character.Notify(character, "Your etcetera(ETC) inventory is full!.", ServerConstants.NoticeType.Popup);
                    break;

                case ItemConstants.ItemType.Usable:
                    Character.Notify(character, "Your usable(USE) inventory is full!.", ServerConstants.NoticeType.Popup);
                    break;

                case ItemConstants.ItemType.Setup:
                    Character.Notify(character, "Your setup inventory is full!.", ServerConstants.NoticeType.Popup);
                    break;

                case ItemConstants.ItemType.Cash:
                    Character.Notify(character, "Your cash inventory is full!.", ServerConstants.NoticeType.Popup);
                    break;

                case ItemConstants.ItemType.Count:
                    Character.Notify(character, "Your count inventory is full!.", ServerConstants.NoticeType.Popup);
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(inventoryType), inventoryType, null);
            }
        }

        public bool IsInventoryFull(ItemConstants.ItemType type)
        {
            short count = 0;

            foreach (Item item in this)
            {
                if (item.ItemType == type)
                {
                    count++;
                }
            }

            return (count == MaxSlots[type]);
        }

        public int GetRemainingSlots(ItemConstants.ItemType type)
        {
            short remaining = MaxSlots[type];

            foreach (Item item in this)
            {
                if (item.ItemType == type)
                {
                    remaining--;
                }
            }

            return remaining;
        }

        public void SortItemsHandler(Packet inPacket)
        {
            int ticks = inPacket.ReadInt();
            ItemConstants.ItemType type = (ItemConstants.ItemType)inPacket.ReadByte();
        }

        public void GatherItemsHandler(Packet inPacket)
        {
            int ticks = inPacket.ReadInt();
            ItemConstants.ItemType type = (ItemConstants.ItemType)inPacket.ReadByte();
        }

        public void CharItemsHandler(Packet inPacket)
        {
            int ticks = inPacket.ReadInt();
            ItemConstants.ItemType type = (ItemConstants.ItemType)inPacket.ReadByte();
            short sourceSlot = inPacket.ReadShort();
            short destinationSlot = inPacket.ReadShort();
            short itemQuantity = inPacket.ReadShort();

            try
            {
                Item item = this[type, sourceSlot];

                // destinationSlot < 0 means "into equipable slot"
                if (destinationSlot < 0)
                {
                    Item.EquipItem(item);
                }
                // sourceSlot < 0 means "from equipable slot" destinationSlot > 0 means "into inventory slot"
                else if (sourceSlot < 0 && destinationSlot > 0)
                {
                    Item.UnequipItem(item, destinationSlot);
                }
                // destinationSlot == 0 means "neither to equipable slot nor to inventory slot"
                else if (destinationSlot == 0)
                {
                    Item.DropItem(item, itemQuantity);
                }
                else
                {
                    Item.MoveItem(item, destinationSlot);
                }
            }

            catch (InventoryFullException)
            {
                NotifyInventoryIsFull(Parent, type);
            }
        }

        public void UseItemHandler(Packet inPacket)
        {
            int ticks = inPacket.ReadInt();
            short slot = inPacket.ReadShort();
            int itemID = inPacket.ReadInt();

            Item item = this[ItemConstants.ItemType.Usable, slot];

            if (item == null || item.Quantity < 1 || itemID != item.MapleID)
            {
                return;
            }

            RemoveItemFromInventoryByID(itemID, 1);

            if (item.CHealth > 0)
            {
                Parent.Stats.Health += item.CHealth;
            }

            if (item.CMana > 0)
            {
                Parent.Stats.Mana += item.CMana;
            }

            if (item.CHealthPercentage != 0)
            {
                Parent.Stats.Health += (short)((item.CHealthPercentage * Parent.Stats.MaxHealth) / 100);
            }

            if (item.CManaPercentage != 0)
            {
                Parent.Stats.Mana += (short)((item.CManaPercentage * Parent.Stats.MaxMana) / 100);
            }

            if (item.CBuffTime > 0 && item.CProb == 0)
            {
                // TODO: Add buff.
            }

            if (false)
            {
                // TODO: Add Monster Book card.
            }
        }

        public void UseSummonBagHandler(Packet inPacket)
        {
            int ticks = inPacket.ReadInt();
            short slot = inPacket.ReadShort();
            int itemID = inPacket.ReadInt();

            Item item = this[ItemConstants.ItemType.Usable, slot];

            if (item == null || itemID != item.MapleID)
            {
                return;
            }

            RemoveItemFromInventoryByID(itemID, 1);

            foreach (Tuple<int, short> summon in item.Summons)
            {
                if (Application.Random.Next(0, 100) < summon.Item2)
                {
                    if (DataProvider.Mobs.Contains(summon.Item1))
                    {
                        Parent.Map.Mobs.Add(new Mob(summon.Item1, Parent.Position));
                    }
                }
            }
        }

        public void UseCashItemHandler(Packet inPacket)
        {
            short slot = inPacket.ReadShort();
            int itemID = inPacket.ReadInt();
            bool itemUsed = false;

            Item item = this[ItemConstants.ItemType.Cash, slot];
            if (item == null || item.Quantity < 1 || itemID != item.MapleID) return;

            switch (item.MapleID)
            {
                #region TeleportRocks
                case (int) ItemConstants.UsableCashItems.TeleportRock:
                {
                }
                    break;

                case (int) ItemConstants.UsableCashItems.CokeTeleportRock:
                {
                }
                    break;

                case (int) ItemConstants.UsableCashItems.VIPTeleportRock:
                {
                    itemUsed = Parent.Trocks.UseTrockHandler(itemID, inPacket);
                }
                    break;
                #endregion

                #region AP/SP Reset
                case (int) ItemConstants.UsableCashItems.APReset:
                {
                    CharacterConstants.StatisticType statDestination = (CharacterConstants.StatisticType)inPacket.ReadInt();
                    CharacterConstants.StatisticType statSource = (CharacterConstants.StatisticType)inPacket.ReadInt();

                    CharacterStats.AddAbility(Parent, statDestination, 1, true);
                    CharacterStats.AddAbility(Parent, statSource, -1, true);

                    itemUsed = true;
                }
                    break;

                case (int) ItemConstants.UsableCashItems.SPReset1stJob:
                     {
                        if (!CharacterJobs.IsFirstJob(Parent)) return;

                         int SPTo = inPacket.ReadInt();
                         int SPFrom = inPacket.ReadInt();

                         Skill skillSPTo = CharacterSkills.GetNewSkillFromInt(SPTo); 
                         Skill skillSPFrom = CharacterSkills.GetNewSkillFromInt(SPFrom);

                         byte curLevelSPTo = skillSPTo.CurrentLevel;
                         byte curLevelSPFrom = skillSPFrom.CurrentLevel;

                         if (curLevelSPTo < skillSPTo.MaxLevel && curLevelSPFrom > 0)
                         {
                             CharacterSkills.ModifySkillLevel(Parent, skillSPFrom, (byte)(curLevelSPFrom - 1), skillSPFrom.MaxLevel);
                             CharacterSkills.ModifySkillLevel(Parent, skillSPTo, (byte)(curLevelSPTo + 1), skillSPTo.MaxLevel);
                        }

                        itemUsed = true;
                     }
                    break;

                case (int) ItemConstants.UsableCashItems.SPReset2stJob:
                    {
                        if (!CharacterJobs.IsSecondJob(Parent)) return;
                        //TODO: skill change
                        itemUsed = true;
                    }
                    break;

                case (int) ItemConstants.UsableCashItems.SPReset3stJob:
                    {
                        if (!CharacterJobs.IsThirdJob(Parent)) return;
                        //TODO: skill change
                        itemUsed = true;
                    }
                    break;

                case (int) ItemConstants.UsableCashItems.SPReset4stJob:
                    {
                        if (!CharacterJobs.IsFourthJob(Parent)) return;
                        //TODO: skill change
                        itemUsed = true;
                    }
                    break;
                #endregion

                #region ItemTags/ItemGuards
                case (int)ItemConstants.UsableCashItems.ItemTag:
                {
                    short targetSlot = inPacket.ReadShort();
                    if (targetSlot == 0) return;

                    Item targetItem = this[ItemConstants.ItemType.Equipment, targetSlot];
                    if (targetItem == null) return;

                    targetItem.Creator = Parent.Name;
                    Item.UpdateItem(targetItem);// TODO: This does not seem to update the item's creator.

                    itemUsed = true;
                }
                    break;

                case (int)ItemConstants.UsableCashItems.ItemGuard:
                {
                }
                    break;

                case (int)ItemConstants.UsableCashItems.Incubator: //doest belong here by name only by ordering of usableCashItemsID
                {
                }
                    break;

                case (int)ItemConstants.UsableCashItems.ItemGuard7Days:
                {
                }
                    break;

                case (int)ItemConstants.UsableCashItems.ItemGuard30Days:
                {
                }
                    break;
                case (int)ItemConstants.UsableCashItems.ItemGuard90Days:
                {
                }
                    break;
                #endregion

                #region Megaphones/Messengers                   
                case (int)ItemConstants.UsableCashItems.CheapMegaphone:
                    {
                        // NOTE: You can't use a megaphone unless you're over level 10.
                        if (Parent.Stats.Level < 11) return;

                        string text = inPacket.ReadString();
                        string message = string.Format($"{Parent.Name} : {text}"); // TODO: Include medal name.

                        // NOTE: In GMS, this sends to everyone on the current channel, not the map (despite the item's description).
                        using (Packet oPacket = new Packet(ServerOperationCode.BroadcastMsg))
                        {
                            oPacket
                                .WriteByte((byte)ServerConstants.NoticeType.Megaphone)
                                .WriteString(message);

                            //Parent.Client.Channel.Broadcast(oPacket);
                        }

                        itemUsed = true;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.Megaphone:
                    {
                        if (Parent.Stats.Level < 11) return;

                        string text = inPacket.ReadString();
                        string message = string.Format($"{Parent.Name} : {text}"); // TODO: Include medal name.

                        // NOTE: In GMS, this sends to everyone on the current channel, not the map (despite the item's description).
                        using (Packet oPacket = new Packet(ServerOperationCode.BroadcastMsg))
                        {
                            oPacket
                                .WriteByte((byte) ServerConstants.NoticeType.Megaphone)
                                .WriteString(message);

                            //Parent.Client.Channel.Broadcast(oPacket);
                        }

                        itemUsed = true;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.SuperMegaphone:
                    {
                        if (Parent.Stats.Level < 11) return;

                        string text = inPacket.ReadString();
                        bool whisper = inPacket.ReadBool();

                        string message = string.Format($"{Parent.Name} : {text}"); // TODO: Include medal name.

                        using (Packet oPacket = new Packet(ServerOperationCode.BroadcastMsg))
                        {
                            oPacket
                                .WriteByte((byte) ServerConstants.NoticeType.SuperMegaphone)
                                .WriteString(message)
                                .WriteByte(WvsGame.ChannelID)
                                .WriteBool(whisper);

                            //Parent.Client.World.Broadcast(oPacket);
                        }

                        itemUsed = true;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.HeartMegaphone:
                    {
                        if (Parent.Stats.Level < 11) return;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.SkullMegaphone:
                    {
                        if (Parent.Stats.Level < 11) return;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.MapleTVMessenger:
                    {
                        if (Parent.Stats.Level < 11) return;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.MapleTVStarMessenger:
                    {
                        if (Parent.Stats.Level < 11) return;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.MapleTVHeartMessenger:
                    {
                        if (Parent.Stats.Level < 11) return;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.Megassenger:
                    {
                        if (Parent.Stats.Level < 11) return;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.StarMegassenger:
                    {
                        if (Parent.Stats.Level < 11) return;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.HeartMegassenger:
                    {
                        if (Parent.Stats.Level < 11) return;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.ItemMegaphone: // NOTE: Item Megaphone.
                    {
                        if (Parent.Stats.Level < 11) return;

                        string text = inPacket.ReadString();
                        bool whisper = inPacket.ReadBool();
                        bool includeItem = inPacket.ReadBool();

                        Item targetItem = null;

                        if (includeItem)
                        {
                            ItemConstants.ItemType type = (ItemConstants.ItemType)inPacket.ReadInt();
                            short targetSlot = inPacket.ReadShort();

                            targetItem = this[type, targetSlot];

                            if (targetItem == null)
                            {
                                return;
                            }
                        }

                        string message = string.Format($"{Parent.Name} : {text}"); // TODO: Include medal name.

                        using (Packet oPacket = new Packet(ServerOperationCode.BroadcastMsg))
                        {
                            oPacket
                                .WriteByte((byte) ServerConstants.NoticeType.ItemMegaphone)
                                .WriteString(message)
                                .WriteByte(WvsGame.ChannelID)
                                .WriteBool(whisper)
                                .WriteByte((byte) (targetItem != null ? targetItem.Slot : 0));

                            if (targetItem != null)
                            {
                                oPacket.WriteBytes(targetItem.ToByteArray(true));
                            }

                            //Parent.Client.World.Broadcast(oPacket);
                        }

                        itemUsed = true;
                    }
                    break;
                #endregion

                #region FloatingMessage
                case (int)ItemConstants.UsableCashItems.KoreanKite:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.HeartBalloon:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.GraduationBanner:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.AdmissionBanner:
                    {
                    }
                    break;
                #endregion

                #region otherStuff
                case (int)ItemConstants.UsableCashItems.Note: // NOTE: Memo.
                    {
                        //string targetName = iPacket.ReadString();
                        //string message = iPacket.ReadString();

                        //if (Parent.Client.World.IsCharacterOnline(targetName))
                        //{
                        //    using (Packet oPacket = new Packet(ServerOperationCode.MemoResult))
                        //    {
                        //        oPacket
                        //            .WriteByte((byte)MemoResult.Error)
                        //            .WriteByte((byte)MemoError.ReceiverOnline);

                        //        Parent.Client.Send(oPacket);
                        //    }
                        //}
                        //else if (!Database.Exists("characters", "Name = {0}", targetName))
                        //{
                        //    using (Packet oPacket = new Packet(ServerOperationCode.MemoResult))
                        //    {
                        //        oPacket
                        //            .WriteByte((byte)MemoResult.Error)
                        //            .WriteByte((byte)MemoError.ReceiverInvalidName);

                        //        Parent.Client.Send(oPacket);
                        //    }
                        //}
                        //else if (false) // TODO: Receiver's inbox is full. I believe the maximum amount is 5, but need to verify.
                        //{
                        //    using (Packet oPacket = new Packet(ServerOperationCode.MemoResult))
                        //    {
                        //        oPacket
                        //            .WriteByte((byte)MemoResult.Error)
                        //            .WriteByte((byte)MemoError.ReceiverInboxFull);

                        //        Parent.Client.Send(oPacket);
                        //    }
                        //}
                        //else
                        //{
                        //    Datum datum = new Datum("memos");

                        //    datum["CharacterID"] = Database.Fetch("characters", "ID", "Name = {0}", targetName);
                        //    datum["Sender"] = Parent.Name;
                        //    datum["Message"] = message;
                        //    datum["Received"] = DateTime.Now;

                        //    datum.Insert();

                        //    using (Packet oPacket = new Packet(ServerOperationCode.MemoResult))
                        //    {
                        //        oPacket.WriteByte((byte)MemoResult.Sent);

                        //        Parent.Client.Send(oPacket);
                        //    }

                        //    used = true;
                        //}
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.CongratulatorySong:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.PetNameTag:
                        {
                            //// TODO: Get the summoned pet.

                            //string name = iPacket.ReadString();

                            //using (Packet oPacket = new Packet(ServerOperationCode.PetNameChanged))
                            //{
                            //    oPacket
                            //        .WriteInt(Parent.ID)
                            //        .WriteByte() // NOTE: Index.
                            //        .WriteString(name)
                            //        .WriteByte();

                            //    Parent.Map.Broadcast(oPacket);
                            //}
                        }
                    break;

                case (int) ItemConstants.UsableCashItems.BronzeSackofMesos:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.SilverSackofMesos:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.GoldSackofMesos:
                    {
                        Parent.Stats.Meso += item.Meso;

                        // TODO: We definitely need a GainMeso method with inChat parameter.
                        using (Packet oPacket = new Packet(ServerOperationCode.Message))
                        {
                            oPacket
                                .WriteByte((byte)ServerConstants.MessageType.IncreaseMeso)
                                .WriteInt(item.Meso)
                                .WriteShort();

                            Parent.Client.Send(oPacket);
                        }

                        itemUsed = true;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.FungusScroll:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.OinkerDelight:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.ZetaNightmare:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.ChalkBoard:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.ChalkBoard2:
                    {
                        string text = inPacket.ReadString();
                        Parent.Chalkboard = text;
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.ScissorsofKarma:
                    {
                    }
                    break;

                case (int)ItemConstants.UsableCashItems.ViciousHammer:
                    {
                    }
                    break;
                #endregion
            }

            if (itemUsed) RemoveItemFromInventoryByID(itemID, 1);
            else Character.Release(Parent); // TODO: Blank inventory update.
        }

        public void UseReturnScrollHandler(Packet inPacket)
        {
            inPacket.ReadInt(); // NOTE: Ticks.
            short slot = inPacket.ReadShort();
            int itemID = inPacket.ReadInt();

            Item item = this[itemID, slot];
            if (item == null) return;

            RemoveItemFromInventoryByID(itemID, 1);

            Parent.SendChangeMapRequest(item.CMoveTo);
        }

        public void Pickup(Drop drop)
        {
            if (drop.Picker == null)
            {
                try
                {
                    drop.Picker = Parent;

                    if (drop is Meso)
                    {
                        Meso mesoDrop = (Meso) drop;

                        // get total mesos
                        long myPlusDropMesos = (long)(Parent.Stats.Meso + mesoDrop.Amount);

                        // if int32 overflow reset to limit
                        if (myPlusDropMesos > Meso.mesoLimit)
                        {
                            Parent.Stats.Meso = Meso.mesoLimit;
                        }

                        // add mesos to chars mesos                       
                        else
                        {
                            Parent.Stats.Meso += mesoDrop.Amount;
                        }
                    }

                    else if (drop is Item)
                    {
                        Item itemDrop = (Item) drop;

                        // check if item is unique
                        if (itemDrop.OnlyOne)
                        {
                            // TODO: Check if i have such item in inventory, if so
                            // TODO: Inform looter about ownership of such unique item
                            return;
                        }

                        // try obtaining free slot for itemType of itemDrop
                        itemDrop.Slot = GetNextFreeSlot(itemDrop.ItemType);

                        if (itemDrop.Slot > 0)
                        {
                            // slot found add it into inventory
                            AddItemToInventory(itemDrop, true);
                        }
                    }

                    // remove item from map
                    Parent.Map.Drops.Remove(drop);  
                                  
                    // show player his item gain
                    using (Packet oPacket = drop.GetShowGainPacket())
                    {
                        drop.Picker.Client.Send(oPacket);
                    }                      
                }

                // getNextFreeSlot failed to obtain slot to place item in
                catch (InventoryFullException)
                {
                    NotifyInventoryIsFull(Parent, ((Item)drop).ItemType);
                }
            }
        }

        public void PickupItemHandler(Packet inPacket)
        {
            inPacket.Skip(1); // ??
            inPacket.Skip(4); // ??

            Point itemObjectPosition = new Point(inPacket.ReadShort(), inPacket.ReadShort());
            Point itemPickerPosition = new Point(Parent.Position.X, Parent.Position.Y);
          
            // try to check distance from char before loot, no vacuuming!
            // TODO: needs proper check
            if (itemPickerPosition.DistanceFrom(itemObjectPosition) < 5)
            {
                int itemObjectID = inPacket.ReadInt();

                lock (Parent.Map.Drops)
                {
                    // check for drop itemObject on current char Map
                    if (Parent.Map.Drops.Contains(itemObjectID))
                    {
                        // found so pick it up!
                        Pickup(Parent.Map.Drops[itemObjectID]);
                    }
                }
            }

            else
            {
                // TODO: error your too far to loot!
            }
        }

        public Item this[ItemConstants.ItemType type, short slot]
        {
            get
            {
                foreach (Item item in this)
                {
                    if (item.ItemType == type && item.Slot == slot)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        public Item this[ItemConstants.EquipmentSlot slot]
        {
            get
            {
                foreach (Item item in this)
                {
                    if (item.Slot == (sbyte)slot)
                    {
                        return item;
                    }
                }
                return null; // TODO: Should be keynotfoundexception, but I'm lazy.
            }
        }

        public Item this[int mapleId, short slot]
        {
            get
            {
                foreach (Item item in this)
                {
                    if (item.Slot == slot && item.ItemType == Item.GetType(mapleId))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        public IEnumerable<Item> this[ItemConstants.ItemType type]
        {
            get
            {
                foreach (Item loopItem in Items)
                {
                    if (loopItem.ItemType == type && !loopItem.IsEquipped)
                    {
                        yield return loopItem;
                    }
                }
            }
        }

        public IEnumerable<Item> GetStored()
        {
            foreach (Item loopItem in Items)
            {
                if (loopItem.IsStored)
                {
                    yield return loopItem;
                }
            }
        }

        public IEnumerable<Item> GetEquipped(ItemConstants.EquippedQueryMode mode = ItemConstants.EquippedQueryMode.Any)
        {
            foreach (Item loopItem in Items)
            {
                if (!loopItem.IsEquipped) continue;

                switch (mode)
                {
                    case ItemConstants.EquippedQueryMode.Any:
                        yield return loopItem;

                        break;

                    case ItemConstants.EquippedQueryMode.Normal:
                        if (loopItem.Slot > -100)
                        {
                            yield return loopItem;
                        }
                        break;

                    case ItemConstants.EquippedQueryMode.Cash:
                        if (loopItem.Slot < -100)
                        {
                            yield return loopItem;
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
            }
        }

        public int SpaceTakenByItem(Item item, bool autoMerge = true)
        {
            if (item.Quantity < 0) return 0;

            if (InventoryAvailableItemByID(item.MapleID) % item.MaxPerStack == 0 || !autoMerge) return 1;

            foreach (Item loopItem in this.Where(x => x.MapleID == item.MapleID && x.Quantity < x.MaxPerStack))
            {
                return loopItem.Quantity + item.Quantity <= loopItem.MaxPerStack ? 0 : 1;
            }

            return 1;

        }

        public bool CouldReceiveItems(IEnumerable<Item> items, bool autoMerge = true)
        {
            Dictionary<ItemConstants.ItemType, int> spaceCount = new Dictionary<ItemConstants.ItemType, int>(5);
            {
                spaceCount.Add(ItemConstants.ItemType.Equipment, 0);
                spaceCount.Add(ItemConstants.ItemType.Usable, 0);
                spaceCount.Add(ItemConstants.ItemType.Setup, 0);
                spaceCount.Add(ItemConstants.ItemType.Etcetera, 0);
                spaceCount.Add(ItemConstants.ItemType.Cash, 0);
            }

            foreach (Item loopItem in items)
            {
                spaceCount[loopItem.ItemType] += SpaceTakenByItem(loopItem, autoMerge);
            }

            foreach (KeyValuePair<ItemConstants.ItemType, int> loopSpaceCount in spaceCount)
            {
                if (GetRemainingSlots(loopSpaceCount.Key) < loopSpaceCount.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public byte[] ItemToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteByte(MaxSlots[ItemConstants.ItemType.Equipment])
                    .WriteByte(MaxSlots[ItemConstants.ItemType.Usable])
                    .WriteByte(MaxSlots[ItemConstants.ItemType.Setup])
                    .WriteByte(MaxSlots[ItemConstants.ItemType.Etcetera])
                    .WriteByte(MaxSlots[ItemConstants.ItemType.Cash])
                    .WriteLong(); // NOTE: Unknown.

                foreach (Item item in GetEquipped(ItemConstants.EquippedQueryMode.Normal))
                {
                    oPacket.WriteBytes(item.ToByteArray());
                }

                oPacket.WriteShort();

                foreach (Item item in GetEquipped(ItemConstants.EquippedQueryMode.Cash))
                {
                    oPacket.WriteBytes(item.ToByteArray());
                }

                oPacket.WriteShort();

                foreach (Item item in this[ItemConstants.ItemType.Equipment])
                {
                    oPacket.WriteBytes(item.ToByteArray());
                }

                oPacket.WriteShort();
                oPacket.WriteShort(); // TODO: Evan inventory.

                foreach (Item item in this[ItemConstants.ItemType.Usable])
                {
                    oPacket.WriteBytes(item.ToByteArray());
                }

                oPacket.WriteByte();

                foreach (Item item in this[ItemConstants.ItemType.Setup])
                {
                    oPacket.WriteBytes(item.ToByteArray());
                }

                oPacket.WriteByte();

                foreach (Item item in this[ItemConstants.ItemType.Etcetera])
                {
                    oPacket.WriteBytes(item.ToByteArray());
                }

                oPacket.WriteByte();

                foreach (Item item in this[ItemConstants.ItemType.Cash])
                {
                    oPacket.WriteBytes(item.ToByteArray());
                }

                oPacket.WriteByte();

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Items).GetEnumerator();
        }
    }
}
