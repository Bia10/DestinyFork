using System.Collections.Generic;

using Destiny.Data;
using Destiny.Maple.Characters;
using Destiny.Maple.Life;
using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Shops
{
    public sealed class Shop
    {
        public static void LoadRechargeTiers()
        {
            RechargeTiers = new Dictionary<byte, Dictionary<int, double>>();

            foreach (Datum datum in new Datums("shop_recharge_data").Populate())
            {
                if (!RechargeTiers.ContainsKey((byte)(int)datum["tierid"]))
                {
                    RechargeTiers.Add((byte)(int)datum["tierid"], new Dictionary<int, double>());
                }
                RechargeTiers[(byte)(int)datum["tierid"]].Add((int)datum["itemid"], (double)datum["price"]);
            }
        }

        private byte RechargeTierID { get; set; }

        public int ID { get; private set; }
        public Npc Parent { get; private set; }
        public List<ShopItem> Items { get; private set; }
        public static Dictionary<byte, Dictionary<int, double>> RechargeTiers { get; set; }

        public Dictionary<int, double> UnitPrices
        {
            get
            {
                return RechargeTiers[RechargeTierID];
            }
        }

        public Shop(Npc parent, Datum datum)
        {
            Parent = parent;

            ID = (int)datum["shopid"];
            RechargeTierID = (byte)(int)datum["recharge_tier"];

            Items = new List<ShopItem>();

            foreach (Datum itemDatum in new Datums("shop_items").Populate("shopid = {0} ORDER BY sort DESC", ID))
            {
                Items.Add(new ShopItem(this, itemDatum));
            }

            if (RechargeTierID <= 0) return;

            foreach (KeyValuePair<int, double> rechargeable in UnitPrices)
            {
                Items.Add(new ShopItem(this, rechargeable.Key));
            }
        }

        public void Show(Character customer)
        {
            using (Packet oPacket = new Packet(ServerOperationCode.OpenNpcShop))
            {
                oPacket
                    .WriteInt(ID)
                    .WriteShort((short)Items.Count);

                foreach (ShopItem loopShopItem in Items)
                {
                    oPacket.WriteBytes(loopShopItem.ToByteArray());
                }

                customer.Client.Send(oPacket);
            }
        }

        public void Handle(Character customer, Packet iPacket)
        {
            NPCsConstants.ShopAction action = (NPCsConstants.ShopAction)iPacket.ReadByte();

            switch (action)
            {
                case NPCsConstants.ShopAction.Buy:
                    {
                        short index = iPacket.ReadShort();
                        int mapleID = iPacket.ReadInt();
                        short quantity = iPacket.ReadShort();

                        ShopItem item = Items[index];

                        if (customer.Stats.Meso < item.PurchasePrice * quantity)
                        {
                            return;
                        }

                        Item purchase;
                        int price;

                        if (item.IsRecharageable)
                        {
                            purchase = new Item(item.MapleID, item.MaxPerStack);
                            price = item.PurchasePrice;
                        }
                        else if (item.Quantity > 1)
                        {
                            purchase = new Item(item.MapleID, item.Quantity);
                            price = item.PurchasePrice;
                        }
                        else
                        {
                            purchase = new Item(item.MapleID, quantity);
                            price = item.PurchasePrice * quantity;
                        }

                        if (customer.Items.SpaceTakenByItem(purchase) > customer.Items.GetRemainingSlots(purchase.ItemType))
                        {
                            Character.Notify(customer, "Your inventory is full.", ServerConstants.NoticeType.Popup);
                        }
                        else
                        {
                            customer.Stats.Meso -= price;
                            customer.Items.AddItemToInventory(purchase);
                        }

                        using (Packet oPacket = new Packet(ServerOperationCode.ConfirmShopTransaction))
                        {
                            oPacket.WriteByte();

                            customer.Client.Send(oPacket);
                        }
                    }
                    break;

                case NPCsConstants.ShopAction.Sell:
                    {
                        short slot = iPacket.ReadShort();
                        int mapleID = iPacket.ReadInt();
                        short quantity = iPacket.ReadShort();

                        Item item = customer.Items[mapleID, slot];

                        if (item.IsRechargeable)
                        {
                            quantity = item.Quantity;
                        }

                        if (quantity > item.Quantity)
                        {
                            return;
                        }
                        else if (quantity == item.Quantity)
                        {
                            customer.Items.RemoveItemFromInventory(item, true);
                        }
                        else if (quantity < item.Quantity)
                        {
                            item.Quantity -= quantity;
                            Item.UpdateItem(item);
                        }

                        if (item.IsRechargeable)
                        {
                            customer.Stats.Meso += item.SalePrice + (int)(UnitPrices[item.MapleID] * item.Quantity);
                        }
                        else
                        {
                            customer.Stats.Meso += item.SalePrice * quantity;
                        }

                        using (Packet oPacket = new Packet(ServerOperationCode.ConfirmShopTransaction))
                        {
                            oPacket.WriteByte(8);

                            customer.Client.Send(oPacket);
                        }
                    }
                    break;

                case NPCsConstants.ShopAction.Recharge:
                    {
                        short slot = iPacket.ReadShort();

                        Item item = customer.Items[ItemConstants.ItemType.Usable, slot];

                        int price = (int)(UnitPrices[item.MapleID] * (item.MaxPerStack - item.Quantity));

                        if (customer.Stats.Meso < price)
                        {
                            Character.Notify(customer, "You do not have enough mesos.", ServerConstants.NoticeType.Popup);
                        }
                        else
                        {
                            customer.Stats.Meso -= price;

                            item.Quantity = item.MaxPerStack;

                            Item.UpdateItem(item);
                        }

                        using (Packet oPacket = new Packet(ServerOperationCode.ConfirmShopTransaction))
                        {
                            oPacket.WriteByte(8);

                            customer.Client.Send(oPacket);
                        }
                    }
                    break;

                case NPCsConstants.ShopAction.Leave:
                    {
                        customer.LastNpc = null;
                    }
                    break;
            }
        }
    }
}
