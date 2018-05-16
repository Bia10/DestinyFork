using System.Collections.ObjectModel;
using Destiny.Constants;
using Destiny.Data;
using Destiny.Network.Common;
using Destiny.Network.ServerHandler;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterKeymap : KeyedCollection<KeyMapConstants.KeymapKey, Shortcut>
    {
        private const int KeyCount = 90;

        public Character Parent { get; private set; }

        public CharacterKeymap(Character parent)
        {
            this.Parent = parent;
        }

        public void Load()
        {
            foreach (Datum datum in new Datums("keymaps").Populate("CharacterID = {0}", this.Parent.ID))
            {
                this.Add(new Shortcut(datum));
            }
        }

        public void Save()
        {
            this.Delete();

            foreach (Shortcut entry in this)
            {
                Datum datum = new Datum("keymaps");

                datum["CharacterID"] = this.Parent.ID;
                datum["Key"] = (int)entry.Key;
                datum["Type"] = (byte)entry.Type;
                datum["Action"] = (int)entry.Action;

                datum.Insert();
            }
        }

        public void Delete()
        {
            Database.Delete("keymaps", "CharacterID = {0}", this.Parent.ID);
        }

        public void Send()
        {
            using (Packet oPacket = new Packet(ServerOperationCode.FuncKeyMappedInit))
            {
                oPacket.WriteBool(false);

                for (int i = 0; i < CharacterKeymap.KeyCount; i++)
                {
                    KeyMapConstants.KeymapKey key = (KeyMapConstants.KeymapKey)i;

                    if (this.Contains(key))
                    {
                        Shortcut shortcut = this[key];

                        oPacket
                            .WriteByte((byte)shortcut.Type)
                            .WriteInt((int)shortcut.Action);
                    }
                    else
                    {
                        oPacket
                            .WriteByte()
                            .WriteInt();
                    }
                }

                this.Parent.Client.Send(oPacket);
            }
        }

        public void Change(Packet iPacket)
        {
            int mode = iPacket.ReadInt();
            int count = iPacket.ReadInt();

            if (mode == 0)
            {
                if (count == 0)
                {
                    return;
                }

                for (int i = 0; i < count; i++)
                {
                    KeyMapConstants.KeymapKey key = (KeyMapConstants.KeymapKey)iPacket.ReadInt();
                    KeyMapConstants.KeymapType type = (KeyMapConstants.KeymapType)iPacket.ReadByte();
                    KeyMapConstants.KeymapAction action = (KeyMapConstants.KeymapAction)iPacket.ReadInt();

                    if (this.Contains(key))
                    {
                        if (type == KeyMapConstants.KeymapType.None)
                        {
                            this.Remove(key);
                        }
                        else
                        {
                            this[key].Type = type;
                            this[key].Action = action;
                        }
                    }
                    else
                    {
                        this.Add(new Shortcut(key, action, type));
                    }
                }
            }
            else if (mode == 1) // NOTE: Pet automatic mana potion.
            {

            }
            else if (mode == 2) // NOTE: Pet automatic mana potion.
            {

            }
        }

        protected override KeyMapConstants.KeymapKey GetKeyForItem(Shortcut item)
        {
            return item.Key;
        }
    }
}
