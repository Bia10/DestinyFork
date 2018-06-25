using System;
using System.Collections.Generic;

using Destiny.Data;
using Destiny.Maple.Life;
using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.PacketFactory;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterStorage
    {
        public Character Parent { get; private set; }

        public Npc Npc { get; private set; }
        public byte Slots { get; private set; }
        public int Meso { get; private set; }

        public List<Item> Items { get; set; }

        public bool IsFull
        {
            get
            {
                return Items.Count == Slots;
            }
        }

        public CharacterStorage(Character parent)
        {
            Parent = parent;
        }

        public void Load()
        {
            Datum datum = new Datum("storages");

            try
            {
                datum.Populate("AccountID = {0}", Parent.AccountID);
            }
            catch
            {
                datum["AccountID"] = Parent.AccountID;
                datum["Slots"] = (byte)4;
                datum["Meso"] = 0;

                datum.Insert();
            }

            Slots = (byte)datum["Slots"];
            Meso = (int)datum["Meso"];

            Items = new List<Item>();

            foreach (Datum itemDatum in new Datums("items").Populate("AccountID = {0} AND IsStored = True", Parent.AccountID))
            {
                Items.Add(new Item(itemDatum));
            }
        }

        public void Save()
        {
            Datum datum = new Datum("storages")
            {
                ["Slots"] = Slots,
                ["Meso"] = Meso
            };

            datum.Update("AccountID = {0}", Parent.AccountID);

            foreach (Item item in Items)
            {
                item.Save();
            }
        }

        public void Show(Npc npc)
        {
            Npc = npc;

            Load();

            Parent.Client.Send(CharacterPackets.StorageOpen(npc, this));
        }

        public void CharStorageHandler(Packet inPacket)
        {
            NPCsConstants.StorageAction actionType = (NPCsConstants.StorageAction)inPacket.ReadByte();

            switch (actionType)
            {
                case NPCsConstants.StorageAction.WithdrawItem:
                    {
                        // Read packet data
                        ItemConstants.ItemType itemType = (ItemConstants.ItemType)inPacket.ReadByte();
                        byte itemSlot = inPacket.ReadByte();

                        // First seek slot to find the item to withdraw in
                        if (itemSlot < 0 || itemSlot > Slots)
                        {
                            return;
                            // TODO: storage exceptions on wrongly received/read data from packet, CharacterStorageExceptions.WrongSlotRecieved 
                        }

                        // Slot is valid thus seek an item on its position
                        Item item = Items[itemSlot];
                        if (item == null)
                        {
                            return;
                            // TODO: CharacterStorageExceptions.ItemCouldNotBeFound 
                        }

                        // Do i actually have free inventory slot to place withdrawn item in?
                        if (Parent.Items.IsInventoryFull(item.ItemType))
                        {
                            Parent.Client.Send(CharacterPackets.StorageErrorPacket(Parent, NPCsConstants.StoragePacketType.ErrorPlayerInventoryFull));
                            return;
                        }

                        // Even if i do, still i may not have enough mesos to demand this payed service :(
                        int costToWithdraw = 1000; // TODO: how is the cost actually calculated?
                        if (Parent.Stats.Meso < costToWithdraw)
                        {
                            Parent.Client.Send(CharacterPackets.StorageErrorPacket(Parent, NPCsConstants.StoragePacketType.ErrorNotEnoughMesos));
                            return;
                        }

                        if (Parent.Stats.Meso >= costToWithdraw)
                        {
                            // Devour character mesos   
                            Maple.Meso.giveMesos(Parent, costToWithdraw);     

                            // Remove item from storage
                            Items.Remove(item); 

                            // Delete item from Database
                            Item.DeleteItemFromDB(item); 

                            // Set stored flag to false
                            item.IsStored = false; 

                            // Add item to char. items
                            Parent.Items.AddItemToInventory(item, forceGetSlot: true); 

                            // routine to get count of items of same type
                            List<Item> itemsByType = new List<Item>();
                            foreach (Item loopItem in Items)
                            {
                                if (loopItem.Type == item.Type)
                                {
                                    itemsByType.Add(loopItem);
                                }
                            }

                            Parent.Client.Send(CharacterPackets.StorageRemoveItem(Parent, item, itemsByType));
                        }
                    }
                    break;

                case NPCsConstants.StorageAction.DepositItem:
                    {
                        // Read packet data
                        short slot = inPacket.ReadShort();
                        int itemID = inPacket.ReadInt();
                        short quantity = inPacket.ReadShort();

                        // TODO: slot validation, quantity validation
                        // try to get item to deposit from slot in char. inventory
                        Item item = Parent.Items[itemID, slot];

                        // storage full cant deposit
                        if (IsFull)
                        {
                            Parent.Client.Send(CharacterPackets.StorageErrorPacket(Parent, NPCsConstants.StoragePacketType.ErrorStorageInventoryFull));
                            return;
                        }

                        // not enough mesos to pay for deposit
                        if (Parent.Stats.Meso <= Npc.StorageCost)
                        {
                            Parent.Client.Send(CharacterPackets.StorageErrorPacket(Parent, NPCsConstants.StoragePacketType.ErrorNotEnoughMesos));
                            return;
                        }

                        if (Parent.Stats.Meso >= Npc.StorageCost)
                        {
                            //devour character mesos
                            Parent.Stats.Meso -= Npc.StorageCost; 

                            // remove item form inventory
                            Parent.Items.RemoveItemFromInventory(item, true);

                            // NOTE: This is needed because when we remove the item is sets parent to none.
                            item.Parent = Parent.Items;

                            // slot in storage is maxCount of items in it
                            item.Slot = (short)Items.Count;

                            // set flag on item
                            item.IsStored = true;

                            // add item to storage
                            Items.Add(item); 

                            // routine to get count of items of same type
                            List<Item> itemsByType = new List<Item>();
                            foreach (Item loopItem in Items)
                            {
                                if (loopItem.Type == item.Type)
                                {
                                    itemsByType.Add(loopItem);
                                }
                            }

                            Parent.Client.Send(CharacterPackets.StorageAddItem(Parent, item, itemsByType));
                        }
                    }
                    break;

                case NPCsConstants.StorageAction.ArrangeItem:
                    {
                        Parent.Client.Send(CharacterPackets.StorageArrangeItems(Parent));
                    }
                    break;

                case NPCsConstants.StorageAction.ChangeMesos:
                    {
                        int meso = inPacket.ReadInt();

                        if (meso > 0) // NOTE: Withdraw meso.
                        {
                            // TODO: Meso checks.
                        }
                        else // NOTE: Deposit meso.
                        {
                            // TODO: Meso checks.
                        }

                        Meso -= meso;
                        Parent.Stats.Meso += meso;

                        Parent.Client.Send(CharacterPackets.StorageChangeMesos(Parent));
                    }
                    break;

                case NPCsConstants.StorageAction.CloseStorage:
                    {
                        Save();
                    }
                    break;

                case NPCsConstants.StorageAction.OpenStorage:
                    break;

                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}