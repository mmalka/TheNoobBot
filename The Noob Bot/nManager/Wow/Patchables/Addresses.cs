namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 20726
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // FEFA58 ClntObjMgrInitializeStd
            public static uint sCurMgr = 0xDA6A20; // CCommand_ObjUsage
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
            DX_DEVICE = 0xD1A648, // ClientInitializeGame, first offset
            DX_DEVICE_IDX = 0x28B8, // DX9_DEVICE_IDX_FOUND
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
            PartyOffset = 0xF0CA0C, // Script_SendChatMessage First offset/4th block
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
            ItemClass = 0xD70730, // DB2! Script_GetItemInfo+1C6
            ItemSubClass = 0xD7159C,
            SpellCategories = 0xD754FC,
            FactionTemplate = 0xD6C8D4,
            Lock = 0xD726AC,
            QuestPOIPoint = 0xD75530, // DB2! OneFunction // QuestPOIPointDbTable
            ResearchSite = 0xD76508, // DB2! CGWorldMap__SetMap+36D // ResearchSiteDBTable
            Map = 0xD7CF90,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            GetTime = 0xD0D460, // FrameTime::GetCurTimeMs
            buildWoWVersionString = 0xD65EE8, // buildWoWVersionStringFOUND
            gameState = 0xEAEACA, // Script_IsPlayerInWorld
            isLoadingOrConnecting = 0xD871A0, // isLoadingOrConnectingFOUND
            AreaId = 0xCB8560, // AreaIdFOUND
            SubAreaId = AreaId - 8, // AreaId - 8 bytes
            MapTextureId = 0xCC56EC, // MapTextureIdFOUND
            zoneMap = 0xEAEAC0, // Script_GetZoneText
            subZoneMap = 0xEAEABC, // Script_GetSubZoneText
            // saving
            TextBoxActivated = 0xBBE9AC, // 18414
            LastHardwareAction = 0xD0E090, // Script_ToggleRun
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xF223C0, // petGUIDFOUND
            playerName = 0xFEFA98, // ida: GetPlayerName
            RetrieveCorpseWindow = 0xEAEB2C, // RetrieveCorpseWindowFOUND
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
            LocalPlayerSpellsOnCooldown = 0xD94170, // LocalPlayerSpellsOnCooldownFOUND

            // saving
            RuneStartCooldown = 0xF18AA8, // Script_GetRuneCount
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xD0D8B8, // EventSystem
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
            UNIT_FIELD_X = 0xAC0, // found with a ugly while
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0xF98, // Script_UnitCastingInfo
            CastingSpellStartTime = 0xFB0,
            CastingSpellEndTime = 0xFB4,
            ChannelSpellID = 0xFB8, // Script_UnitChannelInfo
            ChannelSpellStartTime = 0xFBC,
            ChannelSpellEndTime = 0xFC0,
            CanInterrupt = 0xF2C, // SpellCanBeInterrupted from Script_UnitCastingInfo/Script_UnitChannelInfo
            CanInterruptOffset = 0xF0C6B8, // SpellCanBeInterrupted = CGSpellBook::m_silenceHarmfulSchoolMask
            CanInterruptOffset2 = CanInterruptOffset + 4, // = CGSpellBook::m_interruptSchoolMask
            CanInterruptOffset3 = CanInterruptOffset2 + 4, // = CGSpellBook::m_silenceSchoolMask
            TransportGUID = 0xAB0, // CGUnit_C__HasVehicleTransport // ??
            DBCacheRow = 0xC38, // CGUnit_C__GetUnitName ???
            CachedSubName = 0x0, // beginning of DBCacheRow pointer = CachedSubName
            CachedName = 0x7C, // check or update
            CachedTypeFlag = 0x24, // check or update
            CachedQuestItem1 = 0x3C, // check or update
            CachedModelId1 = 0x6C, // check or update
            CachedUnitClassification = 0x2C, // Script_UnitClassification + 0x3E (CGUnit_C::GetCreatureRank)
            CachedIsBoss = 0x5C, // IDA Script_IsBossFOUND
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0x2A8, // check or update
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            PackedRotationQuaternion = 0x150, // check or update
            TransformationMatrice = 0x278, // CGGameObject_C::GetMatrix
            DBCacheRow = 0x274, // CGGameObject_C::GetName
            CachedIconName = 0x08,
            CachedCastBarCaption = 0xC,
            CachedName = 0xB4, // CGGameObject_C__GetName_2
            CachedData0 = 0x14, // CGGameObject_C::GetLockRecord
            CachedSize = CachedData0 + (0x04*33), // just after the 32 data uint32 + 1 unknown value
            CachedQuestItem1 = CachedSize + 0x04, // just after the size float
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            StatPvp = 0xCC530C, // StatPvpFOUND inside first call in Script_InActiveBattlefield
            PvpExitWindow = 0xF0D278, // Script_GetBattlefieldWinner
            MaxBattlegroundId = 0xF0D244, // Script_GetMaxBattlefieldID
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0x104, // ??
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            KnownAllSpells = 0xF0C674, // SpellBookNumSpells - 4 // KnownAllSpells
            SpellDBCMaxIndex = 200000,
            SpellBookNumSpells = 0xF0C6FC, // CGSpellBook__MakeKnownSpellModelsLocal
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xF0C748 + 0x4*0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
            FirstTalentBookPtr = 0xF0C8B4, // FirstTalentBookFOUND
            NextTalentBookPtr = FirstTalentBookPtr - 0x8,
            TalentBookSpellId = 0x14, // check or update
            TalentBookOverrideSpellId = 0x1C, // check or update
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xEB0B10,
            NextMessage = 0x17E8,
            msgFormatedChat = 0x65,
            chatBufferPos = 0xF0AE8C,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xE47AD0, // GetClickToMoveStruct
            CTM_PUSH = 0xE47AEC, // CGUnit_C::IsAutoTracking
            CTM_X = CTM + 0x84, // check or update
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
            ClntObjMgrGetActivePlayerObj = 0x3D58,
            FrameScript_ExecuteBuffer = 0x273EE,
            CGUnit_C__InitializeTrackingState = 0x32B26C, // alias CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x31CAC7,
            CGWorldFrame__Intersect = 0x569F81,
            Spell_C_HandleTerrainClick = 0x28442D,
            CGUnit_C__Interact = 0x99600E,
            strlen = 0x6B1160, // ida _strlen
            // saving
            IsOutdoors = 0x0, // ?
            UnitCanAttack = 0x0, // ?
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xEAEE64,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            PlayerNameStorePtr = 0xD7BF80, // CGUnit_C__GetUnitName + 0x64
            PlayerNameNextOffset = 0x14,
            PlayerNameStringOffset = 0x11,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xFEFC40 + 0x6, // ClientServices__GetSelectedRealm
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xEAEC5C, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xEAEC64, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xEAEC7C, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xEAEC8C, // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x34,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xF4, // check or update
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
            AuraTable1 = 0x1108,
            AuraTable2 = 0x480,
            AuraSize = 0x48,

            AuraStructCreatorGuid = 0x20, // read 16 bytes (GUID)
            AuraStructSpellId = AuraStructCreatorGuid + 16, // read 4 bytes (UINT)
            AuraStructFlag = AuraStructSpellId + 4, // read 1 byte
            AuraStructMask = AuraStructFlag + 1, // read 4 bytes
            AuraStructCount = AuraStructMask + 4, // read 1 byte
            AuraStructCasterLevel = AuraStructCount + 1, // read 1 byte
            AuraStructUnk1 = AuraStructCasterLevel + 1, // read 1 byte, what is this ?
            AuraStructDuration = AuraStructUnk1 + 1, // read 4 bytes
            AuraStructSpellEndTime = AuraStructDuration + 4, // read 4 bytes
            AuraStructUnk2 = AuraStructSpellEndTime + 4, // read 4 byte
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xE4772C, // PowerTypePointer
            Multiplicator = 0x10,
        }
    }
}