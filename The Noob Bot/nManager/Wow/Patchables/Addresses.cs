namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 18996
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xED28E8 ClntObjMgrInitializeStd
            public static uint sCurMgr = 0xCB5CC8; // CCommand_ObjUsage
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
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xC2F990, // ClientInitializeGame, first offset
            DX_DEVICE_IDX = 0x2854, // DX9_DEVICE_IDX_FOUND (0x94587)
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlagsOffsets
        {
            Offset1 = 0x124, // near any movement flag
            Offset2 = 0x40,
        }

        public enum Party
        {
            PartyOffset = 0xDEE21C, // Script_SendChatMessage First offset/4th block
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
            ItemClass = 0xC87AA8,
            ItemSubClass = 0xC87FC0,
            FactionTemplate = 0xC86FAC,
            Lock = 0xC88404,
            QuestPOIPoint = 0xC88AEC,
            ResearchSite = 0xC88D28,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            GetTime = 0xC22788, // FrameTime::GetCurTimeMs
            continentId = 0xB5546C, // ContinentIdFOUND
            continentIdOffset = 0x120,
            buildWoWVersionString = 0xC72488, // buildWoWVersionStringFOUND
            gameState = 0xD9033E, // Script_IsPlayerInWorld
            isLoadingOrConnecting = 0xC95368, // isLoadingOrConnectingFOUND
            AreaId = 0xBA6C10, // AreaIdFOUND
            MapTextureId = 0xBB5030, // MapTextureIdFOUND
            zoneMap = 0xD90334, // Script_GetZoneText
            subZoneMap = 0xD90330, // Script_GetSubZoneText
            // saving
            TextBoxActivated = 0xBBE9AC, // 18414
            LastHardwareAction = 0xC2F61C, // Script_ToggleRun
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xE03788, // petGUIDFOUND
            playerName = 0xED2928, // ida: GetPlayerName
            RetrieveCorpseWindow = 0xD903A0, // RetrieveCorpseWindowFOUND
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
            // saving
            RuneStartCooldown = 0xE06EDC, // Script_GetRuneCount
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xC22C10, // EventSystem
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
            UNIT_FIELD_X = 0xA50, // found with a ugly while
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0xF38, // Script_UnitCastingInfo
            CastingSpellStartTime = 0xF50,
            CastingSpellEndTime = 0xF54,
            ChannelSpellID = 0xF58, // Script_UnitChannelInfo
            ChannelSpellStartTime = 0xF5C,
            ChannelSpellEndTime = 0xF60,
            CanInterrupt = 0xEFC, // SpellCanBeInterrupted from Script_UnitCastingInfo/Script_UnitChannelInfo
            CanInterruptOffset = 0xDEDEC8, // SpellCanBeInterrupted = CGSpellBook::m_silenceHarmfulSchoolMask
            CanInterruptOffset2 = CanInterruptOffset + 4, // = CGSpellBook::m_interruptSchoolMask
            CanInterruptOffset3 = CanInterruptOffset2 + 4, // = CGSpellBook::m_silenceSchoolMask
            TransportGUID = 0xA40, // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0xBC4, // CGUnit_C__GetUnitName
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
            GAMEOBJECT_FIELD_X = 0x2A0,
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            PackedRotationQuaternion = 0x148,
            TransformationMatrice = 0x270, // CGGameObject_C::GetMatrix
            DBCacheRow = 0x26C, // CGGameObject_C::GetName
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
            StatPvp = 0xBB4CF4, // StatPvpFOUND inside first call in Script_InActiveBattlefield
            PvpExitWindow = 0xDEF228, // Script_GetBattlefieldWinner
            MaxBattlegroundId = 0xDEF1F4, // Script_GetMaxBattlefieldID
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
            SpellBookNumSpells = 0xDEDF0C, // CGSpellBook__MakeKnownSpellModelsLocal
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xDEDF58 + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD92300,
            NextMessage = 0x17E8,
            msgFormatedChat = 0x65,
            chatBufferPos = 0xDEC67C,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xD33168, // GetClickToMoveStruct
            CTM_PUSH = 0xD33184, // CGUnit_C::IsAutoTracking
            CTM_X = CTM + 0x8C,
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
            ClntObjMgrGetActivePlayerObj = 0x3A17,
            FrameScript_ExecuteBuffer = 0x23F34,
            CGUnit_C__InitializeTrackingState = 0x2E5A59, // alias CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x2D7313,
            CGWorldFrame__Intersect = 0x4F91F3,
            Spell_C_HandleTerrainClick = 0x245AD7,
            CGUnit_C__Interact = 0x90442A,
            // saving
            IsOutdoors = 0x0, // ?
            UnitCanAttack = 0x0, // ?
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD90698,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            PlayerNameStorePtr = 0xC8A560, // CGUnit_C__GetUnitName + 0x88
            PlayerNameNextOffset = 0x14,
            PlayerNameStringOffset = 0x11,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xED28F1 + 0x6, // ClientServices__GetSelectedRealm
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xD904C4, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xD904CC, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xD904E4, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xD904F4, // CGGame_UI__IsAutoSelfCast
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
            AuraTable1 = 0x10A0,
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
            PowerIndexArrays = 0xD32DC4, // PowerTypePointer
            Multiplicator = 0x10,
        }
    }
}