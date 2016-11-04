namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 22950
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // dropped usage, kept only for online script not crashing for older versions.
            public static uint sCurMgr = 0x0; // 0xD99190 CCommand_ObjUsage
        }

        public enum ObjectManager
        {
            localGuid = 0xF8, // lntObjMgrGetActivePlayer_0 
            objectGUID = 0x30,
            objectTYPE = 0x10, // ClntObjMgrGetActivePlayer
            firstObject = 0xD8, // CCommand_ObjUsage
            nextObject = 0x44,
            continentId = 0x108,
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xD120E0, //Script_ShowCursor, first offset
            DX_DEVICE_IDX = 0x2590, // DX9_DEVICE_IDX_FOUND
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlagsOffsets
        {
            Offset1 = 0x124, // near any movement flag.
            Offset2 = 0x40,
        }

        public enum Party
        {
            PartyOffset = 0xF0B180, // Script_SendChatMessage First offset/4th block
            NumOfPlayers = 0xC8, // Script_GetNumGroupMembers
            NumOfPlayersSuBGroup = 0xCC, // NumOFPlayers+4
            PlayerGuid = 0x10,
        }

        /// <summary>
        ///   PetBattle
        /// </summary>
        public enum PetBattle
        {
            IsInBattle = 0xC864E4, // PetInBattle
            // Findable under string IsInBattle.
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            Unknown = 0xF8B578, // Script_GetItemSpecInfo

            // DBC Offsets are as of 22248 and does not requires update as we now read files.
            Spell = 0xD1FEF0,
            ItemClass = 0xD173C0, // DB2! Script_GetItemInfo+1C6 // string: ItemClass.db2
            ItemSubClass = 0x0, // string: ItemSubClass.db2
            SpellCategories = 0x0,
            FactionTemplate = 0x0,
            Lock = 0x0,
            QuestPOIPoint = 0xD1E950, // DB2! OneFunction // QuestPOIPointDbTable 
            ResearchSite = 0xD1D2D0, // DB2! CGWorldMap__SetMap+36D // ResearchSiteDBTable
            Map = 0xD291A0, // DB2! Script_GetRealZoneText
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            GetTime = 0xCFEF20, //FrameTime::GetCurTimeMs
            buildWoWVersionString = 0xD4F0B8, // buildWoWVersionStringFOUND
            gameState = 0xEAC715, // Script_IsPlayerInWorld
            isLoading = 0xCF2760, //isLoadingFOUND
            AreaId = 0xC7EBD4, // AreaIdFOUND - AreaIdFOUNDCall
            SubAreaId = AreaId + 4, // AreaId + 4 bytes
            MapTextureId = 0xC87D68, //MapTextureIdFOUND
            zoneMap = 0xEAC744, // Script_GetZoneText
            subZoneMap = 0xEAC748, // Script_GetSubZoneText 
            // saving
            TextBoxActivated = 0xBBE9AC, // ?// 18414
            LastHardwareAction = 0xD0E090, // ?// Script_ToggleRun
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xF1CF10, // petGUIDFOUND
            playerName = 0xF8BF70, // GetPlayerName 
            RetrieveCorpseWindow = 0xEAD3E8, // RetrieveCorpseWindowFOUND
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
            LocalPlayerSpellsOnCooldown = 0xD87238, // LocalPlayerSpellsOnCooldownFOUND

            // saving
            RuneStartCooldown = 0xF18AA8, // ?// Script_GetRuneCount
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xCFF240, // EventSystem
            BaseEvents = EventsCount + 0x4,
            EventOffsetName = 0x18,
            EventOffsetCount = 0x48,
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_FIELD_X = 0xAF8, // found with a ugly while
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0x106C, // Script_UnitCastingInfo
            CastingSpellStartTime = 0x1090,
            CastingSpellEndTime = 0x1094,
            ChannelSpellID = 0x1098, // Script_UnitChannelInfo
            ChannelSpellStartTime = 0x109C,
            ChannelSpellEndTime = 0x10A0,
            CanInterrupt = 0xFCC, // SpellCanBeInterrupted from Script_UnitCastingInfo/Script_UnitChannelInfo

            // SpellCanBeInterrupted changed too much, not sure if CanInterrupt even work.
            CanInterruptOffset = 0xE02EA0, // SpellCanBeInterrupted = CGSpellBook::m_silenceHarmfulSchoolMask
            CanInterruptOffset2 = CanInterruptOffset + 4, // = CGSpellBook::m_interruptSchoolMask
            CanInterruptOffset3 = CanInterruptOffset2 + 4, // = CGSpellBook::m_silenceSchoolMask

            TransportGUID = 2792, // ?// CGUnit_C__HasVehicleTransport // ??
            DBCacheRow = 3200, // ?// CGUnit_C__GetUnitName ???
            CachedSubName = 0x0, // ?// beginning of DBCacheRow pointer = CachedSubName
            CachedName = 0x80, // end of CGUnit_C::GetCreatureRank
            CachedTypeFlag = 0x24, // ?// check or update
            CachedQuestItem1 = 0x3C, // ?// check or update
            CachedModelId1 = 0x6C, // ?// check or update
            CachedUnitClassification = 0x2C, // Script_UnitClassification + 0x3C (CGUnit_C::GetCreatureRank)
            CachedIsBoss = 0x60,
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0x138, // ?
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            PackedRotationQuaternion = 0x148, // GetQuaternionOffset => UnpackQuaternion
            TransformationMatrice = 0x270, // GetMatrixOffset  ((this + 0x270), 0x40u)
            DBCacheRow = 0x26C, // CGGameObject_C::GetName
            CachedIconName = 0x08,
            CachedCastBarCaption = 0xC,
            CachedName = 0xB4, // CGGameObject_C__GetName_2
            CachedData0 = 0x14, // CGGameObject_C::GetLockRecord
            CachedSize = CachedData0 + (0x04*33), // ?// just after the 32 data uint32 + 1 unknown value
            CachedQuestItem1 = CachedSize + 0x04, // ?// just after the size float
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            StatPvp = 0xC88064, // StatPvpFOUND inside first call in Script_InActiveBattlefield
            PvpExitWindow = 0xF19E80, // Script_GetBattlefieldWinner
            MaxBattlegroundId = 0xF19D00, // Script_GetMaxBattlefieldID
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0xF8,
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            SpellDBCMaxIndex = 200000,

            KnownAllSpells = SpellBookNumSpells - 0x4, // found via SpellBookNumSpells - 4
            SpellBookNumSpells = 0xF0AE60, // CGSpellBook__MakeKnownSpellModelsLocal
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xF0AEAC + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
            FirstTalentBookPtr = 0xF0B014, // FirstTalentBookFOUND
            NextTalentBookPtr = FirstTalentBookPtr - 0x8,
            TalentBookSpellId = 0x14,
            TalentBookOverrideSpellId = 0x1C,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xEADB90,
            NextMessage = 0x17E8,
            msgFormatedChat = 0x65,
            chatBufferPos = 0xEADB8C,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xE34E44, // GetClickToMoveStruct
            CTM_PUSH = 0xE34E18, // CGPlayer_C__ClickToMove
            CTM_X = CTM + 0x28, // to Check
            CTM_Y = CTM_X + 0x4,
            CTM_Z = CTM_Y + 0x4,
        }

        /// <summary>
        ///   Virtual Function
        /// </summary>
        public enum VMT
        {
            CGUnit_C__GetFacing = 0x35, // *4 > MovementGetTransportFacing
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayerObj = 0x81C7A,
            FrameScript_ExecuteBuffer = 0xA6B55,
            CGPlayer_C__ClickToMove = 0x2FFA05, // alias CGUnit_C__InitializeTrackingState
            FrameScript__GetLocalizedText = 0x2FA33B,
            WowClientDB2__GetRowPointer = 0x201DD2,
            CGWorldFrame__Intersect = 0x5EA43E,
            Spell_C_HandleTerrainClick = 0x2B0850,
            CGUnit_C__Interact = 0x52FB9,
            strlen = 0x75A9C0, // ida _strlen
            // saving
            IsOutdoors = 0x0,
            UnitCanAttack = 0x0,
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xEAD550 + 0x3C, // RetrieveCorpseWindowFOUND
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            PlayerNameStorePtr = 0xD5A2F8, // CGUnit_C__GetUnitName + 0x62 // Script_ResurrectGetOfferer
            PlayerNameNextOffset = 0x14,
            PlayerNameStringOffset = 0x11,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            // Script_SelectedRealmName
            realmName = 0xF8BC64, // SelectedRealmName_DWORD
            realmNameOffset = 0x38C, // SelectedRealmName_Offset
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xEAC53C, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xEAC544, // Cvars+b7
            AutoLoot_Activate_Pointer = 0xEAC55C, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xEAC568, // offset aAutoselfcast ; "autoSelfCast"
            Activate_Offset = 0x34,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xEC, //overlooked with cheat engine with baseaddress of 3 NPCs.
            // TODO Retrieve unknown Quests offsets
            /*ActiveQuests = 0x0,
            SelectedQuestId = 0x0,
            TitleText = 0x0,
            GossipQuests = 0x0,
            GossipQuestNext = 0x0, // ?*/
        }

        /// <summary>
        ///   Get Buff GetAuraInfo / GetAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AuraTable1 = 0x1158,
            AuraTable2 = 0x580,
            AuraSize = 0x58,

            AuraStructCreatorGuid = 0x20, // ?// read 16 bytes (GUID)
            AuraStructSpellId = AuraStructCreatorGuid + 16, // ?// read 4 bytes (UINT)
            AuraStructFlag = AuraStructSpellId + 4, // ?// read 1 byte
            AuraStructMask = AuraStructFlag + 1, // ?// read 4 bytes
            AuraStructCount = AuraStructMask + 4, // ?// read 1 byte
            AuraStructCasterLevel = AuraStructCount + 1, // ?// read 1 byte
            AuraStructUnk1 = AuraStructCasterLevel + 1, // ?// read 1 byte, // ?what is this ?
            AuraStructDuration = AuraStructUnk1 + 1, // ?// read 4 bytes
            AuraStructSpellEndTime = AuraStructDuration + 4, // ?// read 4 bytes
            AuraStructUnk2 = AuraStructSpellEndTime + 4, // ?// read 4 byte
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xE34E8C, // PowerTypePointer
            Multiplicator = 0x13 - 1,
        }
    }
}