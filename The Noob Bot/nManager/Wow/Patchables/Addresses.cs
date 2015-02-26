namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 19678
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xF72CB8 ClntObjMgrInitializeStd
            public static uint sCurMgr = 0xD37198; // CCommand_ObjUsage
            // It's the direct pointer to objectManager clientConn+objectManager
        }

        public enum ObjectManager
        {
            objectManager = 0x62C, // to be used with clientConnection or bypassed if using sCurMgr.
            localGuid = 0xF8, // ClntObjMgrGetActivePlayer_0 // E7B3A0 = localGUID complete?
            objectGUID = 0x28,
            objectTYPE = 0xC, // ClntObjMgrGetActivePlayer
            firstObject = 0xD8, // CCommand_ObjUsage
            nextObject = 0x3C,
            continentId = 0x108,
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xCB25C0, // ClientInitializeGame, first offset
            DX_DEVICE_IDX = 0x2870, // DX9_DEVICE_IDX_FOUND (0x9441C)
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlagsOffsets
        {
            Offset1 = 0x12C, // near any movement flag
            Offset2 = 0x40,
        }

        public enum Party
        {
            PartyOffset = 0xE8F82C, // Script_SendChatMessage First offset/4th block
            NumOfPlayers = 0xCC, // Script_GetNumGroupMembers
            NumOfPlayersSuBGroup = 0xD0, // NumOFPlayers+4
            PlayerGuid = 0x10, // toCheck
        }

        /// <summary>
        ///   PetBattle
        /// </summary>
        public enum PetBattle
        {
            IsInBattle = 0xBA8A10, // 18950  // LUA: Script_C_PetBattles_IsInBattle|Script_GetAuraInfo
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            ItemClass = 0xD09D2C,
            ItemSubClass = 0xD0B87C,
            FactionTemplate = 0xD0597C,
            Lock = 0xD0C554,
            QuestPOIPoint = 0xD0CC3C,
            ResearchSite = 0xD0CE78,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            GetTime = 0xCA53C8, // FrameTime::GetCurTimeMs
            buildWoWVersionString = 0xCF5248, // buildWoWVersionStringFOUND
            gameState = 0xE3191E, // Script_IsPlayerInWorld
            isLoadingOrConnecting = 0xD19718, // isLoadingOrConnectingFOUND
            SubAreaId = 0xC50D40, // AreaId - 8 bytes
            AreaId = 0xC50D48, // AreaIdFOUND
            MapTextureId = 0xC5DDEC, // MapTextureIdFOUND
            zoneMap = 0xE31914, // Script_GetZoneText
            subZoneMap = 0xE31910, // Script_GetSubZoneText
            // saving
            TextBoxActivated = 0xBBE9AC, // 18414
            LastHardwareAction = 0xCB2250, // Script_ToggleRun
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xEA4F40, // petGUIDFOUND
            playerName = 0xF72CF8, // ida: GetPlayerName
            RetrieveCorpseWindow = 0xE31980, // RetrieveCorpseWindowFOUND
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
            // saving
            RuneStartCooldown = 0xEA841C, // Script_GetRuneCount
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xCA5820, // EventSystem
            BaseEvents = EventsCount + 0x4,
            EventOffsetName = 0x18,
            EventOffsetCount = 0x48,
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            // ToDo: Check CachedTypeFlag
            UNIT_FIELD_X = 0xA90, // found with a ugly while
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0xF60, // Script_UnitCastingInfo
            CastingSpellStartTime = 0xF78,
            CastingSpellEndTime = 0xF7C,
            ChannelSpellID = 0xF80, // Script_UnitChannelInfo
            ChannelSpellStartTime = 0xF84,
            ChannelSpellEndTime = 0xF88,
            CanInterrupt = 0xEF4, // SpellCanBeInterrupted from Script_UnitCastingInfo/Script_UnitChannelInfo
            CanInterruptOffset = 0xE8F4D8, // SpellCanBeInterrupted = CGSpellBook::m_silenceHarmfulSchoolMask
            CanInterruptOffset2 = CanInterruptOffset + 4, // = CGSpellBook::m_interruptSchoolMask
            CanInterruptOffset3 = CanInterruptOffset2 + 4, // = CGSpellBook::m_silenceSchoolMask
            TransportGUID = 0xA80, // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0xC04, // CGUnit_C__GetUnitName ??
            CachedSubName = 0x0, // beginning of DBCacheRow pointer = CachedSubName
            CachedName = 0x7C,
            CachedTypeFlag = 0x24,
            CachedQuestItem1 = 0x3C,
            CachedModelId1 = 0x6C,
            CachedUnitClassification = 0x2C, // Script_UnitClassification + 0x3E (CGUnit_C::GetCreatureRank)
            CachedIsBoss = 0x5C, // function wow Script_IsBoss() (or function at Script_UnitLevel + 0xB7)
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0x2A8,
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            PackedRotationQuaternion = 0x150,
            TransformationMatrice = 0x278, // CGGameObject_C::GetMatrix
            DBCacheRow = 0x274, // CGGameObject_C::GetName
            CachedIconName = 0x08,
            CachedCastBarCaption = 0xC,
            CachedName = 0xB4, // CGGameObject_C__GetName_2
            CachedData0 = 0x14, // CGGameObject_C::GetLockRecord
            CachedSize = CachedData0 + (0x04*32), // just after the 32 data uint32
            CachedQuestItem1 = CachedSize + 0x04, // just after the size float
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            StatPvp = 0xC5DA0C, // StatPvpFOUND inside first call in Script_InActiveBattlefield
            PvpExitWindow = 0xE90098, // Script_GetBattlefieldWinner
            MaxBattlegroundId = 0xE90064, // Script_GetMaxBattlefieldID
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0x104,
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            KnownAllSpells = 0xE8F514, // SpellBookNumSpells - 4
            SpellDBCMaxIndex = 200000,
            SpellBookNumSpells = 0xE8F51C, // CGSpellBook__MakeKnownSpellModelsLocal
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xE8F568 + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
            FirstTalentBookPtr = 0xE8F6D4, // FirstTalentBookFOUND
            NextTalentBookPtr = FirstTalentBookPtr - 0x8,
            TalentBookSpellId = 0x14,
            TalentBookOverrideSpellId = 0x1C,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xE33908,
            NextMessage = 0x17E8,
            msgFormatedChat = 0x65,
            chatBufferPos = 0xE8DC84,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xDCCAE8, // GetClickToMoveStruct
            CTM_PUSH = 0xDCCB04, // CGUnit_C::IsAutoTracking
            CTM_X = CTM + 0x84,
            CTM_Y = CTM_X + 0x4,
            CTM_Z = CTM_Y + 0x4,
        }

        /// <summary>
        ///   Virtual Function
        /// </summary>
        public enum VMT
        {
            CGUnit_C__GetFacing = 0x2C, // *4 > MovementGetTransportFacing
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayerObj = 0x3B18,
            FrameScript_ExecuteBuffer = 0x250C6,
            CGUnit_C__InitializeTrackingState = 0x2F8484, // alias CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x2E879E,
            CGWorldFrame__Intersect = 0x51B5D4,
            Spell_C_HandleTerrainClick = 0x2554D2,
            CGUnit_C__Interact = 0x946243,
            // saving
            IsOutdoors = 0x0, // ?
            UnitCanAttack = 0x0, // ?
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xE31CA4,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            PlayerNameStorePtr = 0xD0E5A8, // CGUnit_C__GetUnitName + 0x64
            PlayerNameNextOffset = 0x14,
            PlayerNameStringOffset = 0x11,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xF72EA0 + 0x6, // ClientServices__GetSelectedRealm
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xE31AAC, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xE31AB4, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xE31ACC, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xE31ADC, // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x34,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xF4,
            // TODO Retrieve unknown Quests offsets
            /*ActiveQuests = 0x0,
            SelectedQuestId = 0x0,
            TitleText = 0x0,
            GossipQuests = 0x0,
            GossipQuestNext = 0x0, */
        }

        /// <summary>
        ///   Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AuraTable1 = 0x10C8,
            AuraTable2 = 0x480,
            AuraSize = 0x48,

            AuraStructCreatorGuid = 0x20, // read 16 bytes (GUID)
            AuraStructSpellId = AuraStructCreatorGuid + 16, // read 4 bytes (UINT)
            AuraStructFlags = AuraStructSpellId + 4, // read 1 byte, what is this ?
            AuraStructCount = AuraStructFlags + 1, // read 1 byte
            AuraStructCasterLevel = AuraStructCount + 1, // read 1 byte
            AuraStructUnk2 = AuraStructCasterLevel + 1, // read 1 byte, what is this ?
            AuraStructDuration = AuraStructUnk2 + 1, // read 4 bytes
            AuraStructSpellEndTime = AuraStructDuration + 4, // read 4 bytes
            AuraStructUnk3 = AuraStructSpellEndTime + 4, // read 1 byte, what is this ?
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xDCC744, // PowerTypePointer
            Multiplicator = 0x10,
        }
    }
}