namespace Destiny.Network.ClientHandler
{
    public enum ClientOperationCode : short
    {
        AccountLogin = 1,
        GuestLogin = 2,
        AccountInfo = 3,
        WorldRelist = 4,
        WorldSelect = 5,
        WorldStatus = 6,
        EULA = 7,
        AccountGender = 8,
        PinCheck = 9,
        PinUpdate = 10,
        WorldList = 11,
        LeaveCharacterSelect = 12,
        ViewAllChar = 13,
        SelectCharacterByVAC = 14,
        VACFlagSet = 15,
        //16
        //17
        //18
        CharacterSelect = 19,
        CharacterLoad = 20,
        CharacterNameCheck = 21,
        CharacterCreate = 22,
        CharacterDelete = 23,
        Pong = 24,
        ClientStartError = 25,
        ClientError = 26,
        StrangeData = 27,
        Relog = 28,
        CharacterSelectRegisterPic = 29,
        CharacterSelectRequestPic = 30,
        RegisterPicFromVAC = 31,
        RequestPicFromVAC = 32,
        //33
        //34
        ClientStart = 35,
        //36
        //37
        MapChange = 38,
        ChannelChange = 39,
        CashShopMigration = 40,
        PlayerMovement = 41,
        Sit = 42,
        UseChair = 43,
        CloseRangeAttack = 44,
        RangedAttack = 45,
        MagicAttack = 46,
        EnergyOrbAttack = 47,
        TakeDamage = 48,
        PlayerChat = 49,
        CloseChalkboard = 50,
        FaceExpression = 51,
        UseItemEffect = 52,
        UseDeathItem = 53,
        //54
        //55
        //56
        MonsterBookCover = 57,
        NpcConverse = 58,
        RemoteStore = 59,
        NpcResult = 60,
        NpcShop = 61,
        Storage = 62,
        HiredMerchant = 63,
        FredrickAction = 64,
        DueyAction = 65,
        //66
        //67
        AdminShopAction = 68, 
        InventorySort = 69,
        InventoryGather = 70,
        InventoryAction = 71,
        UseItem = 72,
        CancelItemEffect = 73,
        //74
        UseSummonBag = 75,
        UsePetFood = 76,
        UseMountFood = 77,
        UseScriptedItem = 78,
        UseCashItem = 79,
        UseCatchItem = 80,
        UseSkillBook = 81,
        //82
        //83
        UseTeleportRock = 84,
        UseReturnScroll = 85,
        UseUpgradeScroll = 86,
        DistributeAP = 87,
        AutoDistributeAP = 88,
        HealOverTime = 89,
        DistributeSP = 90,
        UseSkill = 91,
        CancelBuff = 92,
        SkillEffect = 93,
        MesoDrop = 94,
        GiveFame = 95,
        //96
        PlayerInformation = 97,
        SpawnPet = 98,
        CancelDebuff = 99,
        ChangeMapSpecial = 100,
        UseInnerPortal = 101,
        TrockAction = 102, // trock add map?
        //103
        //104
        //105
        Report = 106,
        QuestAction = 107,
        //108
        SkillMacro = 109,
        SpouseChat = 110,
        UseFishingItem = 111,
        MakerSkill = 112,
        //113
        //114
        UseRemote = 113,
        PartyChat = 114,
        //115
        //116
        //117
        //118
        MultiChat = 119,
        Command = 120,
        //121
        Messenger = 122,
        PlayerInteraction = 123,
        PartyOperation = 124,
        DenyPartyRequest = 125,
        GuildOperation = 126,
        DenyGuildRequest = 127,
        AdminCommand = 128,
        AdminLog = 129,
        BuddyListModify = 130,
        NoteAction = 131,
        UseDoor = 132,
        ChangeKeymap = 135,
        //136 RPS_ACTION(0x88),
        RingAction = 137,
        WeddingAction = 138,
        //139
        //140
        //141
        //142
        //143
        //144
        FamilyPedigree = 145,
        FamilyOpen = 146,
        FamilyAdd = 147,
        //148
        //149
        FamilyAccept = 150,
        FamilyUse = 151,
        AllianceOperation = 152,
        //153
        //154
        BbsOperation = 155,
        MtsMigration = 156,
        UseSolomonItem = 157,
        //158 USE_GACHA_EXP(0x9E),
        //159
        //160
        //161
        ClickGuide = 162,
        AranComboCounter = 163,
        //164
        //165
        //166
        MovePet = 167,
        PetChat = 168,
        PetCommand = 169,
        PetLoot = 170,
        PetAutoLoot = 171,
        PetExcludeItems = 172,
        //173 PetAutoPot??
        //174
        MoveSummon = 175,
        SummonAttack = 176,
        DamageSummon = 177,
        Beholder = 178,
        //179
        //180
        //181
        //182
        //183
        //184
        //185
        //186
        //187
        MoveLife = 188, //MoveMob || MobMovement? 
        MobAutomaticProvoke = 189, //AutoAggro
        //190
        //191
        MobDamageMobFriendly = 192,
        MonsterBomb = 193,
        MobDamageMob = 194,
        //195
        //196
        NpcAction = 197,
        //198
        //199
        //200
        //201
        DropPickup = 202,
        //203
        //204
        HitReactor = 205,
        TouchReactor = 206,
        TemporarySkill = 207,
        //208
        //209
        //210
        Snowball = 211,
        LeftKnockback = 212,
        Coconut = 213,
        MatchTable = 214,
        MonsterCarnival = 218,
        PartySearchRegister = 220,
        //221
        PartySearchStart = 222,
        PlayerUpdate = 223,
        //224
        //225
        //226
        //227
        CheckCash = 228,
        CashShopOperation = 229,
        CouponCode = 230,
        //231
        //232
        //233
        //234
        OpenItemInterface = 235,
        CloseItemInterface = 236,
        UseItemInterface = 237,
        //238
        //239
        //240
        //241
        //242
        //243
        //244
        //245
        //246
        //247
        //248
        //249
        //250
        //251
        //252
        MtsOperation = 253,
        UseMapleLife = 254,
        //255
        //256
        //257
        //258
        //259
        UseHammer = 260,

        #region QUESTIONABLE
        NpcMovement = 197,
        PetTalk = 0x9B,
        PetAutoPot = 0xA5,
        MobMovement = 0xBC,
        DamageReactor = 0xC3,
        ChangedMap = 0xC4,
        BuyCashItem = 0xDB,
        LeaveField = 0xDF,
        MapleTV = 0x222,
        #endregion QUESTIONABLE
    }
}