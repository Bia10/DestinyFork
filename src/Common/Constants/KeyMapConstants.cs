﻿namespace Destiny.Constants
{
    public static class KeyMapConstants
    {
        #region Keymap
        public enum KeymapType : byte
        {
            None = 0,
            Skill = 1,
            Item = 2,
            Face = 3,
            Menu = 4,
            BasicAction = 5,
            BasicFace = 6,
            Effect = 7
        }

        public enum KeymapAction
        {
            EquipmentMenu = 0,
            ItemMenu = 1,
            AbilityMenu = 2,
            SkillMenu = 3,
            BuddyList = 4,
            WorldMap = 5,
            Messenger = 6,
            MiniMap = 7,
            QuestMenu = 8,
            SetKey = 9,
            AllChat = 10,
            WhisperChat = 11,
            PartyChat = 12,
            BuddyChat = 13,
            Shortcut = 14,
            QuickSlot = 15,
            ExpandChat = 16,
            GuildList = 17,
            GuildChat = 18,
            PartyList = 19,
            QuestHelper = 20,
            SpouseChat = 21,
            MonsterBook = 22,
            CashShop = 23,
            AllianceChat = 24,
            PartySearch = 25,
            FamilyList = 26,
            Medal = 27,

            PickUp = 50,
            Sit = 51,
            Attack = 52,
            Jump = 53,
            NpcChat = 54,

            Cockeyed = 100,
            Happy = 101,
            Sarcastic = 102,
            Crying = 103,
            Outraged = 104,
            Shocked = 105,
            Annoyed = 106
        }

        public enum KeymapKey
        {
            None = 0,
            Escape = 1,
            One = 2,
            Two = 3,
            Three = 4,
            Four = 5,
            Five = 6,
            Six = 7,
            Seven = 8,
            Eight = 9,
            Nine = 10,
            Zero = 11,
            Minus = 12,
            Equals = 13,
            Backspace = 14,
            Tab = 15,
            Q = 16,
            W = 17,
            E = 18,
            R = 19,
            T = 20,
            Y = 21,
            U = 22,
            I = 23,
            O = 24,
            P = 25,
            BracketLeft = 26,
            BracketRight = 27,
            Enter = 28,
            LeftCtrl = 29,
            A = 30,
            S = 31,
            D = 32,
            F = 33,
            G = 34,
            H = 35,
            J = 36,
            K = 37,
            L = 38,
            Semicolon = 39,
            Quote = 40,
            Backtick = 41,
            LeftShift = 42,
            Backslash = 43,
            Z = 44,
            X = 45,
            C = 46,
            V = 47,
            B = 48,
            N = 49,
            M = 50,
            Comma = 51,
            Dot = 52,
            Slash = 53,
            RightShift = 54, // NOTE: Maps to LeftShift automatically
            Multiply = 55,
            LeftAlt = 56,
            Space = 57,
            CapsLock = 58,
            F1 = 59,
            F2 = 60,
            F3 = 61,
            F4 = 62,
            F5 = 63,
            F6 = 64,
            F7 = 65,
            F8 = 66,
            F9 = 67,
            F10 = 68,
            NumLock = 69,
            ScrollLock = 70,
            Numpad7 = 71,
            Numpad8 = 72,
            Numpad9 = 73,
            Subtract = 74,
            Numpad4 = 75,
            Numpad5 = 76,
            Numpad6 = 77,
            Add = 78,
            Numpad1 = 79,
            Numpad2 = 80,
            Numpad3 = 81,
            Numpad0 = 82,
            NumpadDecimal = 83,
            F11 = 87,
            F12 = 88,
            F13 = 100,
            F14 = 101,
            F15 = 102,
            JapaneseKana = 112,
            JapaneseConvert = 121,
            JapaneseNoConvert = 122,
            JapaneseYen = 125,
            NumpadEquals = 141,
            JapaneseCircumflex = 144,
            NecpcAt = 145,
            NecpcColon = 146,
            NecpcUnderline = 147,
            JapaneseKanji = 148,
            NecpcStop = 149,
            JapanAX = 150,
            J3100Unlabeled = 151,
            NumpadEnter = 156,
            RightCtrl = 157,
            Divide = 181,
            Sysrq = 183,
            RightAlt = 184,
            Pause = 197,
            Home = 199,
            ArrowUp = 200,
            PageUp = 201,
            ArrowLeft = 203,
            ArrowRight = 205,
            End = 207,
            ArrowDown = 208,
            PageDown = 209,
            Insert = 210,
            DeleteKey = 211,
            LeftWindows = 219,
            RightWindows = 220,
            Menu = 221,
            Power = 222,
            Sleep = 223
        }
        #endregion
    }
}
