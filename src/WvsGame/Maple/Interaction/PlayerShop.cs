using System.Collections.Generic;
using System.Linq;

using Destiny.Maple.Maps;
using Destiny.Maple.Characters;
using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Interaction
{
    public sealed class PlayerShop : MapObject, ISpawnable
    {
        public Character Owner { get; private set; }
        public string Description { get; private set; }
        public Character[] Visitors { get; private set; }
        public List<PlayerShopItem> Items { get; private set; }
        public bool Opened { get; private set; }

        public bool IsFull
        {
            get
            { return Visitors.All(curVisitor => curVisitor != null); }
        }
        public PlayerShop(Character owner, string description)
        {
            Owner = owner;
            Description = description;
            Visitors = new Character[3];
            Items = new List<PlayerShopItem>();
            Opened = false;

            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
            {
                oPacket
                    .WriteByte((byte)InteractionConstants.InteractionCode.Room)
                    .WriteByte(4)
                    .WriteByte(4)
                    .WriteByte(0)
                    .WriteByte(0)
                    .WriteBytes(Owner.AppearanceToByteArray())
                    .WriteString(Owner.Name)
                    .WriteByte(byte.MaxValue)
                    .WriteString(Description)
                    .WriteByte(16)
                    .WriteByte(0);

                Owner.Client.Send(oPacket);
            }
        }

        public void Handle(Character character, InteractionConstants.InteractionCode code, Packet iPacket)
        {
            switch (code)
            {
                case InteractionConstants.InteractionCode.OpenStore:
                    {
                        Owner.Map.PlayerShops.Add(this);

                        Opened = true;
                    }
                    break;

                case InteractionConstants.InteractionCode.AddItem:
                    {
                        ItemConstants.ItemType type = (ItemConstants.ItemType)iPacket.ReadByte();
                        short slot = iPacket.ReadShort();
                        short bundles = iPacket.ReadShort();
                        short perBundle = iPacket.ReadShort();
                        int price = iPacket.ReadInt();
                        short quantity = (short)(bundles * perBundle);

                        Item item = character.Items[type, slot];

                        if (item == null)
                        {
                            return;
                        }

                        if (perBundle < 0 || perBundle * bundles > 2000 || bundles < 0 || price < 0)
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
                        }
                        else
                        {
                            character.Items.RemoveItemFromInventory(item, true);
                        }

                        PlayerShopItem shopItem = new PlayerShopItem(item.MapleID, bundles, quantity, price);

                        Items.Add(shopItem);

                        UpdateItems();
                    }
                    break;

                case InteractionConstants.InteractionCode.RemoveItem:
                    {
                        if (character == Owner)
                        {
                            short slot = iPacket.ReadShort();

                            PlayerShopItem shopItem = Items[slot];

                            if (shopItem == null)
                            {
                                return;
                            }

                            if (shopItem.Quantity > 0)
                            {
                                Owner.Items.AddItemToInventory(new Item(shopItem.MapleID, shopItem.Quantity));
                            }

                            Items.Remove(shopItem);

                            UpdateItems();
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.Exit:
                    {
                        if (character == Owner)
                        {
                            Close();
                        }
                        else
                        {
                            RemoveVisitor(character);
                        }
                    }
                    break;

                case InteractionConstants.InteractionCode.Buy:
                    {
                        short slot = iPacket.ReadByte();
                        short quantity = iPacket.ReadShort();

                        PlayerShopItem shopItem = Items[slot];

                        if (shopItem == null)
                        {
                            return;
                        }

                        if (character == Owner)
                        {
                            return;
                        }

                        if (quantity > shopItem.Quantity)
                        {
                            return;
                        }

                        if (character.Stats.Meso < shopItem.MerchantPrice * quantity)
                        {
                            return;
                        }

                        shopItem.Quantity -= quantity;

                        character.Stats.Meso -= shopItem.MerchantPrice * quantity;
                        Owner.Stats.Meso += shopItem.MerchantPrice * quantity;

                        character.Items.AddItemToInventory(new Item(shopItem.MapleID, quantity));

                        UpdateItems(); // TODO: This doesn't mark the item as sold.

                        bool noItemLeft = true;

                        foreach (PlayerShopItem loopShopItem in Items)
                        {
                            if (loopShopItem.Quantity <= 0) continue;

                            noItemLeft = false;
                            break;
                        }

                        if (noItemLeft)
                        {
                            // TODO: Close the owner's shop.
                            // TODO: Notify  the owner the shop has been closed due to items being sold out.

                            Close();
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
                                .WriteByte(8);

                            byte sender = 0;

                            for (int i = 0; i < Visitors.Length; i++)
                            {
                                if (Visitors[i] == character)
                                {
                                    sender = (byte)(i + 1);
                                }
                            }

                            oPacket
                                .WriteByte(sender)
                                .WriteString("{0} : {1}", character.Name, text);

                            Broadcast(oPacket);
                        }
                    }
                    break;
            }
        }

        public void Close()
        {
            foreach (PlayerShopItem loopShopItem in Items)
            {
                if (loopShopItem.Quantity > 0)
                {
                    Owner.Items.AddItemToInventory(new Item(loopShopItem.MapleID, loopShopItem.Quantity));
                }
            }

            if (Opened)
            {
                Map.PlayerShops.Remove(this);

                foreach (var curVisitor in Visitors.Where(curVisitor => curVisitor != null))
                {
                    using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                    {
                        oPacket
                            .WriteByte((byte)InteractionConstants.InteractionCode.Exit)
                            .WriteByte(1)
                            .WriteByte(10);

                        curVisitor.Client.Send(oPacket);
                    }

                    curVisitor.PlayerShop = null;
                }
            }

            Owner.PlayerShop = null;
        }

        public void UpdateItems()
        {
            using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
            {
                oPacket
                    .WriteByte((byte)InteractionConstants.InteractionCode.UpdateItems)
                    .WriteByte((byte)Items.Count);

                foreach (PlayerShopItem loopShopItem in Items)
                {
                    oPacket
                        .WriteShort(loopShopItem.Bundles)
                        .WriteShort(loopShopItem.Quantity)
                        .WriteInt(loopShopItem.MerchantPrice)
                        .WriteBytes(loopShopItem.ToByteArray(true, true));
                }

                Broadcast(oPacket);
            }
        }

        public void Broadcast(Packet oPacket, bool includeOwner = true)
        {
            if (includeOwner)
            {
                Owner.Client.Send(oPacket);
            }

            foreach (var curVisitor in Visitors)
            {
                curVisitor?.Client.Send(oPacket);
            }
        }

        public void AddVisitor(Character visitor)
        {
            for (int i = 0; i < Visitors.Length; i++)
            {
                if (Visitors[i] != null) continue;

                using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                {
                    oPacket
                        .WriteByte((byte)InteractionConstants.InteractionCode.Visit)
                        .WriteByte((byte)(i + 1))
                        .WriteBytes(visitor.AppearanceToByteArray())
                        .WriteString(visitor.Name);

                    Broadcast(oPacket);
                }

                visitor.PlayerShop = this;
                Visitors[i] = visitor;

                using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                {
                    oPacket
                        .WriteByte((byte)InteractionConstants.InteractionCode.Room)
                        .WriteByte(4)
                        .WriteByte(4)
                        .WriteBool(true)
                        .WriteByte(0)
                        .WriteBytes(Owner.AppearanceToByteArray())
                        .WriteString(Owner.Name);

                    for (int slot = 0; slot < 3; slot++)
                    {
                        if (Visitors[slot] != null)
                        {
                            oPacket
                                .WriteByte((byte)(slot + 1))
                                .WriteBytes(Visitors[slot].AppearanceToByteArray())
                                .WriteString(Visitors[slot].Name);
                        }
                    }

                    oPacket
                        .WriteByte(byte.MaxValue)
                        .WriteString(Description)
                        .WriteByte(0x10)
                        .WriteByte((byte)Items.Count);

                    foreach (PlayerShopItem loopShopItem in Items)
                    {
                        oPacket
                            .WriteShort(loopShopItem.Bundles)
                            .WriteShort(loopShopItem.Quantity)
                            .WriteInt(loopShopItem.MerchantPrice)
                            .WriteBytes(loopShopItem.ToByteArray(true, true));
                    }

                    visitor.Client.Send(oPacket);
                }

                break;
            }
        }

        public void RemoveVisitor(Character visitor)
        {
            for (int i = 0; i < Visitors.Length; i++)
            {
                if (Visitors[i] != visitor) continue;

                visitor.PlayerShop = null;
                Visitors[i] = null;

                using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                {
                    oPacket.WriteByte((byte)InteractionConstants.InteractionCode.Exit);

                    if (i > 0)
                    {
                        oPacket.WriteByte((byte)(i + 1));
                    }

                    Broadcast(oPacket, false);
                }

                using (Packet oPacket = new Packet(ServerOperationCode.PlayerInteraction))
                {
                    oPacket
                        .WriteByte((byte)InteractionConstants.InteractionCode.Exit)
                        .WriteByte((byte)(i + 1));

                    Owner.Client.Send(oPacket);
                }

                break;
            }
        }

        public Packet GetCreatePacket()
        {
            return GetSpawnPacket();
        }

        public Packet GetSpawnPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.AnnounceBox);

            oPacket
                .WriteInt(Owner.ID)
                .WriteByte(4)
                .WriteInt(ObjectID)
                .WriteString(Description)
                .WriteByte(0)
                .WriteByte(0)
                .WriteByte(1)
                .WriteByte(4)
                .WriteByte(0);

            return oPacket;
        }

        public Packet GetDestroyPacket()
        {
            Packet oPacket = new Packet(ServerOperationCode.AnnounceBox);

            oPacket
                .WriteInt(Owner.ID)
                .WriteByte(0);

            return oPacket;
        }
    }
}
