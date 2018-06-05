using System;
using System.Collections.Generic;

using Destiny.Maple.Characters;
using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Interaction
{
    public sealed class Trade
    {
        public Character Owner { get; private set; }
        public Character Visitor { get; private set; }
        public int OwnerMeso { get; private set; }
        public int VisitorMeso { get; private set; }
        public List<Item> OwnerItems { get; private set; }
        public List<Item> VisitorItems { get; private set; }
        public bool Started { get; private set; }
        public bool OwnerLocked { get; private set; }
        public bool VisitorLocked { get; private set; }

        public Trade(Character owner)
        {
            Owner = owner;
            Visitor = null;
            OwnerMeso = 0;
            VisitorMeso = 0;
            OwnerItems = new List<Item>();
            VisitorItems = new List<Item>();
            Started = false;
            OwnerLocked = false;
            VisitorLocked = false;

            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
            {
                oPacket
                    .WriteByte((byte)InteractionConstants.InteractionCode.Room)
                    .WriteByte(3)
                    .WriteByte(2)
                    .WriteByte(0) // NOTE: Player index.
                    .WriteByte(0)
                    .WriteBytes(Owner.AppearanceToByteArray())
                    .WriteString(Owner.Name)
                    .WriteByte(byte.MaxValue);

                Owner.Client.Send(oPacket);
            }
        }

        public void Handle(Character character, InteractionConstants.InteractionCode code, Packet iPacket)
        {
            switch (code)
            {
                case InteractionConstants.InteractionCode.Invite:
                    {
                        int characterID = iPacket.ReadInt();

                        if (!Owner.Map.Characters.Contains(characterID))
                        {
                            // TODO: What does happen in case the invitee left?

                            return;
                        }

                        Character invitee = Owner.Map.Characters[characterID];

                        if (invitee.Trade != null)
                        {
                            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                            {
                                oPacket
                                    .WriteByte((byte)InteractionConstants.InteractionCode.Decline)
                                    .WriteByte(2)
                                    .WriteString(invitee.Name);

                                Owner.Client.Send(oPacket);
                            }
                        }
                        else
                        {
                            invitee.Trade = this;
                            Visitor = invitee;

                            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                            {
                                oPacket
                                    .WriteByte((byte)InteractionConstants.InteractionCode.Invite)
                                    .WriteByte(3)
                                    .WriteString(Owner.Name)
                                    .WriteBytes(new byte[4] { 0xB7, 0x50, 0x00, 0x00 });

                                Visitor.Client.Send(oPacket);
                            }
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.Decline:
                    {
                        using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                        {
                            oPacket
                                .WriteByte((byte)InteractionConstants.InteractionCode.Decline)
                                .WriteByte(3)
                                .WriteString(character.Name);

                            Owner.Client.Send(oPacket);
                        }

                        Owner.Trade = null;
                        Visitor.Trade = null;
                        Owner = null;
                        Visitor = null;
                    }
                    break;

                case InteractionConstants.InteractionCode.Visit:
                    {
                        if (Owner == null)
                        {
                            Visitor = null;
                            character.Trade = null;

                           Character.Notify(character, "Trade has been closed.", ServerConstants.NoticeType.Popup);
                        }
                        else
                        {
                            Started = true;

                            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                            {
                                oPacket
                                    .WriteByte((byte)InteractionConstants.InteractionCode.Visit)
                                    .WriteByte(1)
                                    .WriteBytes(Visitor.AppearanceToByteArray())
                                    .WriteString(Visitor.Name);

                                Owner.Client.Send(oPacket);
                            }

                            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                            {
                                oPacket
                                    .WriteByte((byte)InteractionConstants.InteractionCode.Room)
                                    .WriteByte(3)
                                    .WriteByte(2)
                                    .WriteByte(1)
                                    .WriteByte(0)
                                    .WriteBytes(Owner.AppearanceToByteArray())
                                    .WriteString(Owner.Name)
                                    .WriteByte(1)
                                    .WriteBytes(Visitor.AppearanceToByteArray())
                                    .WriteString(Visitor.Name)
                                    .WriteByte(byte.MaxValue);

                                Visitor.Client.Send(oPacket);
                            }
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.SetItems:
                    {
                        ItemConstants.ItemType type = (ItemConstants.ItemType)iPacket.ReadByte();
                        short slot = iPacket.ReadShort();
                        short quantity = iPacket.ReadShort();
                        byte targetSlot = iPacket.ReadByte();

                        Item item = character.Items[type, slot];

                        if (item.IsBlocked)
                        {
                            return;
                        }

                        if (quantity > item.Quantity)
                        {
                            return;
                        }

                        if (quantity < item.Quantity)
                        {
                            item.Quantity -= quantity;
                            Item.UpdateItem(item);

                            item = new Item(item.MapleID, quantity);
                        }
                        else
                        {
                            character.Items.RemoveItemFromInventory(item, true);
                        }

                        item.Slot = 0;

                        using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                        {
                            oPacket
                                .WriteByte((byte)InteractionConstants.InteractionCode.SetItems)
                                .WriteByte(0)
                                .WriteByte(targetSlot)
                                .WriteBytes(item.ToByteArray(true));

                            if (character == Owner)
                            {
                                OwnerItems.Add(item);

                                Owner.Client.Send(oPacket);
                            }
                            else
                            {
                                VisitorItems.Add(item);

                                Visitor.Client.Send(oPacket);
                            }
                        }

                        using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                        {
                            oPacket
                                .WriteByte((byte)InteractionConstants.InteractionCode.SetItems)
                                .WriteByte(1)
                                .WriteByte(targetSlot)
                                .WriteBytes(item.ToByteArray(true));

                            if (character == Owner)
                            {
                                Visitor.Client.Send(oPacket);
                            }
                            else
                            {
                                Owner.Client.Send(oPacket);
                            }
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.SetMeso:
                    {
                        int meso = iPacket.ReadInt();

                        if (meso < 0 || meso > character.Stats.Meso)
                        {
                            return;
                        }

                        // TODO: The meso written in this packet is the added meso amount.
                        // The amount that should be written is the total amount.

                        using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                        {
                            oPacket
                                .WriteByte((byte)InteractionConstants.InteractionCode.SetMeso)
                                .WriteByte(0)
                                .WriteInt(meso);

                            if (character == Owner)
                            {
                                if (OwnerLocked)
                                {
                                    return;
                                }

                                OwnerMeso += meso;
                                Owner.Stats.Meso -= meso;

                                Owner.Client.Send(oPacket);
                            }
                            else
                            {
                                if (VisitorLocked)
                                {
                                    return;
                                }

                                VisitorMeso += meso;
                                Visitor.Stats.Meso -= meso;

                                Visitor.Client.Send(oPacket);
                            }
                        }

                        using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                        {
                            oPacket
                                .WriteByte((byte)InteractionConstants.InteractionCode.SetMeso)
                                .WriteByte(1)
                                .WriteInt(meso);

                            if (Owner == character)
                            {
                                Visitor.Client.Send(oPacket);
                            }
                            else
                            {
                                oPacket.WriteInt(OwnerMeso);

                                Owner.Client.Send(oPacket);
                            }
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.Exit:
                    {
                        if (Started)
                        {
                            Cancel();

                            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                            {
                                oPacket
                                   .WriteByte((byte)InteractionConstants.InteractionCode.Exit)
                                   .WriteByte(0)
                                   .WriteByte(2);

                                Owner.Client.Send(oPacket);
                                Visitor.Client.Send(oPacket);
                            }

                            Owner.Trade = null;
                            Visitor.Trade = null;
                            Owner = null;
                            Visitor = null;
                        }
                        else
                        {
                            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                            {
                                oPacket
                                   .WriteByte((byte)InteractionConstants.InteractionCode.Exit)
                                    .WriteByte(0)
                                    .WriteByte(2);

                                Owner.Client.Send(oPacket);
                            }

                            Owner.Trade = null;
                            Owner = null;
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.Confirm:
                    {
                        using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                        {
                            oPacket.WriteByte((byte)InteractionConstants.InteractionCode.Confirm);

                            if (character == Owner)
                            {
                                OwnerLocked = true;

                                Visitor.Client.Send(oPacket);
                            }
                            else
                            {
                                VisitorLocked = true;

                                Owner.Client.Send(oPacket);
                            }
                        }

                        if (OwnerLocked && VisitorLocked)
                        {
                            Complete();

                            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                            {
                                oPacket
                                    .WriteByte((byte)InteractionConstants.InteractionCode.Exit)
                                    .WriteByte(0)
                                    .WriteByte(6);

                                Owner.Client.Send(oPacket);
                                Visitor.Client.Send(oPacket);
                            }

                            Owner.Trade = null;
                            Visitor.Trade = null;
                            Owner = null;
                            Visitor = null;
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.Chat:
                    {
                        string text = iPacket.ReadString();

                        using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                        {
                            oPacket
                                .WriteByte((byte)InteractionConstants.InteractionCode.Chat)
                                .WriteByte(8)
                                .WriteBool(Owner != character)
                                .WriteString("{0} : {1}", character.Name, text);

                            Owner.Client.Send(oPacket);
                            Visitor.Client.Send(oPacket);
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.Create:
                    break;
                case InteractionConstants.InteractionCode.Room:
                    break;
                case InteractionConstants.InteractionCode.Open:
                    break;
                case InteractionConstants.InteractionCode.TradeBirthday:
                    break;
                case InteractionConstants.InteractionCode.AddItem:
                    break;
                case InteractionConstants.InteractionCode.Buy:
                    break;
                case InteractionConstants.InteractionCode.UpdateItems:
                    break;
                case InteractionConstants.InteractionCode.RemoveItem:
                    break;
                case InteractionConstants.InteractionCode.OpenStore:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }

        public void Complete()
        {
            if (Owner.Items.CouldReceiveItems(VisitorItems) 
                && Visitor.Items.CouldReceiveItems(OwnerItems))
            {
                Owner.Stats.Meso += VisitorMeso;
                Visitor.Stats.Meso += OwnerMeso;

                Owner.Items.AddRangeOfItems(VisitorItems);
                Visitor.Items.AddRangeOfItems(OwnerItems);
            }
            else
            {
                // TODO: Cancel trade.
            }
        }

        public void Cancel()
        {
            Owner.Stats.Meso += OwnerMeso;
            Visitor.Stats.Meso += VisitorMeso;

            Owner.Items.AddRangeOfItems(OwnerItems);
            Visitor.Items.AddRangeOfItems(VisitorItems);
        }
    }
}
