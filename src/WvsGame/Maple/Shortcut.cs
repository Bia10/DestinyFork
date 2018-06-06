using Destiny.Constants;
using Destiny.Data;

namespace Destiny.Maple
{
    public sealed class Shortcut
    {
        public KeyMapConstants.KeymapKey Key { get; }
        public KeyMapConstants.KeymapType Type { get; set; }
        public KeyMapConstants.KeymapAction Action { get; set; }

        public Shortcut(Datum datum)
        {
            Key = (KeyMapConstants.KeymapKey)datum["Key"];
            Type = (KeyMapConstants.KeymapType)datum["Type"];
            Action = (KeyMapConstants.KeymapAction)datum["Action"];
        }

        public Shortcut(KeyMapConstants.KeymapKey key, KeyMapConstants.KeymapAction action, KeyMapConstants.KeymapType type = KeyMapConstants.KeymapType.None)
        {
            Key = key;

            Type = type == KeyMapConstants.KeymapType.None ? GetTypeFromAction(action) : type;

            Action = action;
        }

        private static KeyMapConstants.KeymapType GetTypeFromAction(KeyMapConstants.KeymapAction action)
        {
            switch (action)
            {
                case KeyMapConstants.KeymapAction.Cockeyed:
                case KeyMapConstants.KeymapAction.Happy:
                case KeyMapConstants.KeymapAction.Sarcastic:
                case KeyMapConstants.KeymapAction.Crying:
                case KeyMapConstants.KeymapAction.Outraged:
                case KeyMapConstants.KeymapAction.Shocked:
                case KeyMapConstants.KeymapAction.Annoyed:
                    return KeyMapConstants.KeymapType.BasicFace;

                case KeyMapConstants.KeymapAction.PickUp:
                case KeyMapConstants.KeymapAction.Sit:
                case KeyMapConstants.KeymapAction.Attack:
                case KeyMapConstants.KeymapAction.Jump:
                case KeyMapConstants.KeymapAction.NpcChat:
                    return KeyMapConstants.KeymapType.BasicAction;

                case KeyMapConstants.KeymapAction.EquipmentMenu:
                case KeyMapConstants.KeymapAction.ItemMenu:
                case KeyMapConstants.KeymapAction.AbilityMenu:
                case KeyMapConstants.KeymapAction.SkillMenu:
                case KeyMapConstants.KeymapAction.BuddyList:
                case KeyMapConstants.KeymapAction.WorldMap:
                case KeyMapConstants.KeymapAction.Messenger:
                case KeyMapConstants.KeymapAction.MiniMap:
                case KeyMapConstants.KeymapAction.QuestMenu:
                case KeyMapConstants.KeymapAction.SetKey:
                case KeyMapConstants.KeymapAction.AllChat:
                case KeyMapConstants.KeymapAction.WhisperChat:
                case KeyMapConstants.KeymapAction.PartyChat:
                case KeyMapConstants.KeymapAction.BuddyChat:
                case KeyMapConstants.KeymapAction.Shortcut:
                case KeyMapConstants.KeymapAction.QuickSlot:
                case KeyMapConstants.KeymapAction.ExpandChat:
                case KeyMapConstants.KeymapAction.GuildList:
                case KeyMapConstants.KeymapAction.GuildChat:
                case KeyMapConstants.KeymapAction.PartyList:
                case KeyMapConstants.KeymapAction.QuestHelper:
                case KeyMapConstants.KeymapAction.SpouseChat:
                case KeyMapConstants.KeymapAction.MonsterBook:
                case KeyMapConstants.KeymapAction.CashShop:
                case KeyMapConstants.KeymapAction.AllianceChat:
                case KeyMapConstants.KeymapAction.PartySearch:
                case KeyMapConstants.KeymapAction.FamilyList:
                case KeyMapConstants.KeymapAction.Medal:
                    return KeyMapConstants.KeymapType.Menu;

                default:
                    return KeyMapConstants.KeymapType.None;
            }      
        }
    }
}
