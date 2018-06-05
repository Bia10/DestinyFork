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

        public Character Parent { get; }

        public CharacterKeymap(Character parent)
        {
            Parent = parent;
        }

        public void Load()
        {
            foreach (Datum datum in new Datums("keymaps").Populate("CharacterID = {0}", Parent.ID))
            {
                Add(new Shortcut(datum));
            }
        }

        public void Save()
        {
            // Delete old keymap DB for charID
            Delete();

            foreach (Shortcut entry in this)
            {
                // Create data for new ShortcutKey
                Datum datum = new Datum("keymaps")
                {
                    ["CharacterID"] = Parent.ID,
                    ["Key"] = (int) entry.Key,
                    ["Type"] = (byte) entry.Type,
                    ["Action"] = (int) entry.Action
                };

                // Insert ShortcutKey to DB
                datum.Insert();
            }
        }

        public void Delete()
        {
            Database.Delete("keymaps", "CharacterID = {0}", Parent.ID);
        }

        public void Send()
        {
            using (Packet oPacket = new Packet(ServerOperationCode.FuncKeyMappedInit))
            {
                oPacket.WriteBool(false);

                for (int i = 0; i < KeyCount; i++)
                {
                    KeyMapConstants.KeymapKey key = (KeyMapConstants.KeymapKey)i;

                    if (Contains(key))
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

                Parent.Client.Send(oPacket);
            }
        }

        public void Change(Packet iPacket)
        {
            int mode = iPacket.ReadInt();
            int count = iPacket.ReadInt();

            switch (mode)
            {
                case 0:
                    if (count == 0) return;

                    for (int i = 0; i < count; i++)
                    {
                        KeyMapConstants.KeymapKey key = (KeyMapConstants.KeymapKey) iPacket.ReadInt();
                        KeyMapConstants.KeymapType type = (KeyMapConstants.KeymapType) iPacket.ReadByte();
                        KeyMapConstants.KeymapAction action = (KeyMapConstants.KeymapAction) iPacket.ReadInt();

                        if (Contains(key))
                        {
                            if (type == KeyMapConstants.KeymapType.None)
                            {
                                Remove(key);
                            }
                            else
                            {
                                this[key].Type = type;
                                this[key].Action = action;
                            }
                        }
                        else
                        {
                            Add(new Shortcut(key, action, type));
                        }
                    }
                    break;

                case 1:
                    break;

                case 2:
                    break;
            }
        }

        public void InitiateDefaultKeymap()
        {
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.One, KeyMapConstants.KeymapAction.AllChat));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Two, KeyMapConstants.KeymapAction.PartyChat));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Three, KeyMapConstants.KeymapAction.BuddyChat));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Four, KeyMapConstants.KeymapAction.GuildChat));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Five, KeyMapConstants.KeymapAction.AllianceChat));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Six, KeyMapConstants.KeymapAction.SpouseChat));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Q, KeyMapConstants.KeymapAction.QuestMenu));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.W, KeyMapConstants.KeymapAction.WorldMap));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.E, KeyMapConstants.KeymapAction.EquipmentMenu));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.R, KeyMapConstants.KeymapAction.BuddyList));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.I, KeyMapConstants.KeymapAction.ItemMenu));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.O, KeyMapConstants.KeymapAction.PartySearch));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.P, KeyMapConstants.KeymapAction.PartyList));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.BracketLeft, KeyMapConstants.KeymapAction.Shortcut));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.BracketRight, KeyMapConstants.KeymapAction.QuickSlot));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.LeftCtrl, KeyMapConstants.KeymapAction.Attack));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.S, KeyMapConstants.KeymapAction.AbilityMenu));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.F, KeyMapConstants.KeymapAction.FamilyList));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.G, KeyMapConstants.KeymapAction.GuildList));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.H, KeyMapConstants.KeymapAction.WhisperChat));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.K, KeyMapConstants.KeymapAction.SkillMenu));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.L, KeyMapConstants.KeymapAction.QuestHelper));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Semicolon, KeyMapConstants.KeymapAction.Medal));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Quote, KeyMapConstants.KeymapAction.ExpandChat));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Backtick, KeyMapConstants.KeymapAction.CashShop));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Backslash, KeyMapConstants.KeymapAction.SetKey));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Z, KeyMapConstants.KeymapAction.PickUp));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.X, KeyMapConstants.KeymapAction.Sit));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.C, KeyMapConstants.KeymapAction.Messenger));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.B, KeyMapConstants.KeymapAction.MonsterBook));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.M, KeyMapConstants.KeymapAction.MiniMap));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.LeftAlt, KeyMapConstants.KeymapAction.Jump));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.Space, KeyMapConstants.KeymapAction.NpcChat));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.F1, KeyMapConstants.KeymapAction.Cockeyed));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.F2, KeyMapConstants.KeymapAction.Happy));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.F3, KeyMapConstants.KeymapAction.Sarcastic));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.F4, KeyMapConstants.KeymapAction.Crying));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.F5, KeyMapConstants.KeymapAction.Outraged));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.F6, KeyMapConstants.KeymapAction.Shocked));
            Parent.Keymap.Add(new Shortcut(KeyMapConstants.KeymapKey.F7, KeyMapConstants.KeymapAction.Annoyed));
        }

        protected override KeyMapConstants.KeymapKey GetKeyForItem(Shortcut item)
        {
            return item.Key;
        }
    }
}
