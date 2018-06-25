using System;

using Destiny.Data;
using Destiny.Maple.Data;
using Destiny.IO;

namespace Destiny.Maple.Shops
{
    public sealed class ShopItem
    {
        public Shop Parent { get; private set; }

        public int MapleID { get; private set; }
        public short Quantity { get; private set; }
        public int PurchasePrice { get; private set; }
        public int Sort { get; private set; }

        public short MaxPerStack
        {
            get
            {
                return DataProvider.Items[MapleID].MaxPerStack;
            }
        }

        public int SalePrice
        {
            get
            {
                return DataProvider.Items[MapleID].SalePrice;
            }
        }

        public double UnitPrice
        {
            get
            {
                return Parent.UnitPrices[MapleID];
            }
        }

        public bool IsRecharageable
        {
            get
            {
                return DataProvider.Items[MapleID].IsRechargeable;
            }
        }

        public ShopItem(Shop parent, Datum datum)
        {
            Parent = parent;

            MapleID = (int)datum["itemid"];
            Quantity = (short)datum["quantity"];
            PurchasePrice = (int)datum["price"];
            Sort = (int)datum["sort"];
        }

        public ShopItem(Shop parent, int mapleID)
        {
            Parent = parent;

            MapleID = mapleID;
            Quantity = 1;
            PurchasePrice = 0;
        }

        public byte[] ToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteInt(MapleID)
                    .WriteInt(PurchasePrice)
                    .WriteInt() // NOTE: Perfect Pitch.
                    .WriteInt() // NOTE: Time limit.
                    .WriteInt(); // NOTE: Unknown.

                if (IsRecharageable)
                {
                    oPacket
                        .WriteShort()
                        .WriteInt()
                        .WriteShort((short)(BitConverter.DoubleToInt64Bits(UnitPrice) >> 48))
                        .WriteShort(MaxPerStack);
                }
                else
                {
                    oPacket
                        .WriteShort(Quantity)
                        .WriteShort(MaxPerStack);
                }

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }
    }
}
