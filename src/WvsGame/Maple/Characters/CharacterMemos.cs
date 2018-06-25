using System.Collections.ObjectModel;

using Destiny.Data;
using Destiny.Constants;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterMemos : KeyedCollection<int, Memo>
    {
        public Character Parent { get; private set; }

        public CharacterMemos(Character parent)
        {
            Parent = parent;
        }

        public void Load()
        {
            foreach (Datum datum in new Datums("memos").Populate("CharacterID = {0}", Parent.ID))
            {
                Add(new Memo(datum));
            }
        }

        // NOTE: Memos are inserted straight into the database.
        // Therefore, there is no need for a save method.

        public void Handle(Packet iPacket)
        {
            ItemConstants.MemoAction action = (ItemConstants.MemoAction)iPacket.ReadByte();

            switch (action)
            {
                case ItemConstants.MemoAction.Send:
                    {
                        // TODO: This is occured when you send a note from the Cash Shop.
                        // As we don't have Cash Shop implemented yet, this remains unhandled.
                    }
                    break;

                case ItemConstants.MemoAction.Delete:
                    {
                        byte count = iPacket.ReadByte();
                        byte a = iPacket.ReadByte();
                        byte b = iPacket.ReadByte();

                        for (byte i = 0; i < count; i++)
                        {
                            int id = iPacket.ReadInt();

                            if (!Contains(id))
                            {
                                continue;
                            }

                            this[id].Delete();
                        }

                    }
                    break;
            }
        }

        public void Send()
        {
            using (Packet oPacket = new Packet(ServerOperationCode.MemoResult))
            {
                oPacket
                    .WriteByte((byte)ItemConstants.MemoResult.Send)
                    .WriteByte((byte)Count);

                foreach (Memo memo in this)
                {
                    oPacket.WriteBytes(memo.ToByteArray());
                }

                Parent.Client.Send(oPacket);
            }
        }

        protected override int GetKeyForItem(Memo item)
        {
            return item.ID;
        }
    }
}
