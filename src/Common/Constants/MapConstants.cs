﻿namespace Destiny.Constants
{
    public class MapConstants
    {
        #region Map
        public enum MovementType : byte
        {
            Normal = 0,
            Jump = 1,
            JumpKnockback = 2,
            Immediate = 3,
            Teleport = 4,
            Normal2 = 5,
            FlashJump = 6,
            Assaulter = 7,
            Assassinate = 8,
            Rush = 9,
            Falling = 10,
            Chair = 11,
            ExcessiveKnockback = 12,
            RecoilShot = 13,
            Unknown = 14,
            JumpDown = 15,
            Wings = 16,
            WingsFalling = 17,
            Unknown2 = 18,
            Unknown3 = 19,
            Aran = 20,
        }

        public enum MapTransferResult : byte
        {
            NoReason = 0,
            PortalClosed = 1,
            CannotGo = 2,
            ForceOfGround = 3,
            CannotTeleport = 4,
            ForceOfGround2 = 5,
            OnlyByParty = 6,
            CashShopNotAvailable = 7
        }

        public enum CommandMaps
        {
            MushroomTown = 10000,
            Amherst = 1000000,
            Southperry = 2000000,
            Henesys = 100000000,
            SomeoneElsesHouse = 100000005,
            HenesysMarket = 100000100,
            HenesysPark = 100000200,
            HenesysGamePark = 100000203,
            Ellinia = 101000000,
            MagicLibrary = 101000003,
            ElliniaStation = 101000300,
            Perion = 102000000,
            KerningCity = 103000000,
            SubwayTicketingBooth = 103000100,
            KerningSquare = 103040000,
            LithHarbor = 104000000,
            ThicketAroundtheBeachIII = 104000400,
            ThePigBeach = 104010001,
            Sleepywood = 105040300,
            RegularSauna = 105040401,
            VIPSauna = 105040402,
            AntTunnel = 105050000,
            AntTunnelPark = 105070001,
            TheGraveofMushmom = 105070002,
            OXQuiz = 109020001,
            OlaOla = 109030001,
            MapleStoryPhysicalFitnessTest = 109040000,
            Snowball = 109060000,
            MinigameChallenge = 109070000,
            CoconutHarvest = 109080000,
            FlorinaBeach = 110000000,
            NautilusHarbor = 120000000,
            Ereve = 130000000,
            Rien = 140000000,
            GM = 180000000,
            Blank = 180000001,
            Orbis = 200000000
        }
        #endregion
    }
}