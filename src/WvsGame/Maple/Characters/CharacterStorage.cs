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
                return this.Items.Count == this.Slots;
            }
        }

        public CharacterStorage(Character parent)
        {
            this.Parent = parent;
        }

        public void Load()
        {
            Datum datum = new Datum("storages");

            try
            {
                datum.Populate("AccountID = {0}", this.Parent.AccountID);
            }
            catch
            {
                datum["AccountID"] = this.Parent.AccountID;
                datum["Slots"] = (byte)4;
                datum["Meso"] = 0;

                datum.Insert();
            }

            this.Slots = (byte)datum["Slots"];
            this.Meso = (int)datum["Meso"];

            this.Items = new List<Item>();

            foreach (Datum itemDatum in new Datums("items").Populate("AccountID = {0} AND IsStored = True", this.Parent.AccountID))
            {
                this.Items.Add(new Item(itemDatum));
            }
        }

        public void Save()
        {
            Datum datum = new Datum("storages");

            datum["Slots"] = this.Slots;
            datum["Meso"] = this.Meso;

            datum.Update("AccountID = {0}", this.Parent.AccountID);

            foreach (Item item in this.Items)
            {
                item.Save();
            }
        }

        public void Show(Npc npc)
        {
            this.Npc = npc;

            this.Load();

            this.Parent.Client.Send(CharacterPackets.StorageOpen(npc, this));
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
                        if (itemSlot < 0 || itemSlot > this.Slots)
                        {
                            return;
                            // TODO: storage exceptions on wrongly received/read data from packet, CharacterStorageExceptions.WrongSlotRecieved 
                        }

                        // Slot is valid thus seek an item on its position
                        Item item = this.Items[itemSlot];
                        if (item == null)
                        {
                            return;
                            // TODO: CharacterStorageExceptions.ItemCouldNotBeFound 
                        }

                        // Do i actually have free inventory slot to place withdrawn item in?
                        if (this.Parent.Items.IsInventoryFull(item.ItemType))
                        {
                            this.Parent.Client.Send(CharacterPackets.StorageErrorPacket(this.Parent, NPCsConstants.StoragePacketType.ErrorPlayerInventoryFull));
                            return;
                        }

                        // Even if i do, still i may not have enough mesos to demand this payed service :(
                        int costToWithdraw = 1000; // TODO: how is the cost actually calculated?
                        if (this.Parent.Meso < costToWithdraw)
                        {
                            this.Parent.Client.Send(CharacterPackets.StorageErrorPacket(this.Parent, NPCsConstants.StoragePacketType.ErrorNotEnoughMesos));
                            return;
                        }

                        if (this.Parent.Meso >= costToWithdraw)
                        {
                            Maple.Meso.giveMesos(this.Parent, costToWithdraw); // Devour mesos       

                            this.Items.Remove(item); // Remove item from storage
                            item.Delete(); // Delete item from Database
                            item.IsStored = false; // Set stored flag to false
                            this.Parent.Items.AddItemToInventory(item, forceGetSlot: true); // Add item to char. items

                            // routine to get count of items of same type
                            List<Item> itemsByType = new List<Item>();
                            foreach (Item loopItem in this.Items)
                            {
                                if (loopItem.Type == item.Type)
                                {
                                    itemsByType.Add(loopItem);
                                }
                            }

                            this.Parent.Client.Send(CharacterPackets.StorageRemoveItem(this.Parent, item, itemsByType));
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
                        Item item = this.Parent.Items[itemID, slot];

                        // storage full cant deposit
                        if (this.IsFull)
                        {
                            this.Parent.Client.Send(CharacterPackets.StorageErrorPacket(this.Parent, NPCsConstants.StoragePacketType.ErrorStorageInventoryFull));
                            return;
                        }

                        // not enough mesos to pay for deposit
                        if (this.Parent.Meso <= this.Npc.StorageCost)
                        {
                            this.Parent.Client.Send(CharacterPackets.StorageErrorPacket(this.Parent, NPCsConstants.StoragePacketType.ErrorNotEnoughMesos));
                            return;
                        }

                        if (this.Parent.Meso >= this.Npc.StorageCost)
                        {
                            this.Parent.Meso -= this.Npc.StorageCost; //devour mesos
                            this.Parent.Items.RemoveItemFromInventory(item, true); // remove item form inventory

                            item.Parent = this.Parent.Items; // NOTE: This is needed because when we remove the item is sets parent to none.
                            item.Slot = (short)this.Items.Count; // slot in storage is maxCount of items in it
                            item.IsStored = true; // set flag on item

                            this.Items.Add(item); // add item to storage

                            // routine to get count of items of same type
                            List<Item> itemsByType = new List<Item>();
                            foreach (Item loopItem in this.Items)
                            {
                                if (loopItem.Type == item.Type)
                                {
                                    itemsByType.Add(loopItem);
                                }
                            }

                            this.Parent.Client.Send(CharacterPackets.StorageAddItem(this.Parent, item, itemsByType));
                        }
                    }
                    break;

                case NPCsConstants.StorageAction.ArrangeItem:
                    {
                        this.Parent.Client.Send(CharacterPackets.StorageArrangeItems(this.Parent));
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

                        this.Meso -= meso;
                        this.Parent.Meso += meso;

                        this.Parent.Client.Send(CharacterPackets.StorageChangeMesos(this.Parent));
                    }
                    break;

                case NPCsConstants.StorageAction.CloseStorage:
                    {
                        this.Save();
                    }
                    break;

                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}