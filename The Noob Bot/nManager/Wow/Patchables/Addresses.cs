namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 23857
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // dropped usage, kept only for online script not crashing for older versions.
            public static uint sCurMgr = 0x0; // 0xE0A924 CCommand_ObjUsage
        }

        public enum ObjectManager
        {
            localGuid = 0xF8, // ClntObjMgrGetActivePlayer_0 
            objectGUID = 0x30,
            objectTYPE = 0x10, // ClntObjMgrGetActivePlayer
            firstObject = 0xD8, // CCommand_ObjUsage
            nextObject = 0x44, // ?
            continentId = 0x108,
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xD88F20, //Script_ShowCursor, first offset
            DX_DEVICE_IDX = 0x256C, // DX9_DEVICE_IDX_FOUND
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
            PartyOffset = 0xF9C84C, // Script_SendChatMessage First offset/4th block
            NumOfPlayers = 0xC8, // Script_GetNumGroupMembers
            NumOfPlayersSuBGroup = 0xCC, // NumOFPlayers+4
            PlayerGuid = 0x10,
        }

        /// <summary>
        ///   PetBattle
        /// </summary>
        public enum PetBattle
        {
            IsInBattle = 0xCFE718, // PetInBattle // aC_petbattles ; "C_PetBattles"
            // Findable under string IsInBattle.
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            Unknown = 0x0, // Script_GetItemSpecInfo, gone

            // DBC Offsets are as of 22248 and does not requires update as we now read files.
            Spell = 0x0,
            ItemClass = 0x0, // DB2! Script_GetItemInfo+1C6 // string: ItemClass.db2
            ItemSubClass = 0x0, // string: ItemSubClass.db2
            SpellCategories = 0x0,
            FactionTemplate = 0x0,
            Lock = 0x0,
            QuestPOIPoint = 0x0, // DB2! OneFunction // QuestPOIPointDbTable 
            ResearchSite = 0x0, // DB2! CGWorldMap__SetMap+36D // ResearchSiteDBTable
            Map = 0x0, // DB2! Script_GetRealZoneText
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            GetTime = 0xD75CC0, //FrameTime__GetCurTimeMs
            buildWoWVersionString = 0xDCAAC0, // buildWoWVersionStringFOUND
            gameState = 0xF3DE96, // Script_IsPlayerInWorld
            isLoading = 0xD690F0, //isLoadingFOUND
            AreaId = 0xCF6FE8, // AreaIdFOUND - AreaIdFOUNDCall // to review changes
            SubAreaId = AreaId + 4, // AreaId + 4 bytes
            MapTextureId = 0xCFFFA8, //MapTextureIdFOUND
            zoneMap = 0xF3E0B8, // Script_GetZoneText
            subZoneMap = 0xF3E0C0, // Script_GetSubZoneText 

            // saving
            TextBoxActivated = 0xBBE9AC, // ?// 18414
            LastHardwareAction = 0xD0E090, // ?// Script_ToggleRun
        }

        public enum TargetSystem
        {
            PtrToVMT = 0xCFF3F4, // Script_TargetNearest or any target related script
            Focus = 0x80, // to find manually
            Target = 0x28,
            //TargetTarget = 0x0,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xFB2138, // petGUIDFOUND
            playerName = 0x1020550, // GetPlayerName 
            RetrieveCorpseWindow = 0xF3ED64, // RetrieveCorpseWindowFOUND
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
            LocalPlayerSpellsOnCooldown = 0xDF75A0, // LocalPlayerSpellsOnCooldownFOUND

            // saving
            RuneStartCooldown = 0xF18AA8, // Script_GetRuneCount
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xD75FD0, // EventSystem
            BaseEvents = EventsCount + 0x4,
            EventOffsetName = 0x18,
            EventOffsetCount = 0x48,
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_FIELD_X = 0xAD8, // found with a ugly while
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            UNIT_VELOCITY = 0x8C,
            CastingSpellID = 0x104C, // Script_UnitCastingInfo
            CastingSpellStartTime = 0x1078,
            CastingSpellEndTime = 0x107C,
            ChannelSpellID = 0x1080, // Script_UnitChannelInfo
            ChannelSpellStartTime = 0x1088,
            ChannelSpellEndTime = 0x108C,
            CanInterrupt = 0xFAC, // SpellCanBeInterrupted from Script_UnitCastingInfo/Script_UnitChannelInfo

            // SpellCanBeInterrupted changed too much, not sure if CanInterrupt even work.
            CanInterruptOffset = 0xE02EA0, // SpellCanBeInterrupted = CGSpellBook::m_silenceHarmfulSchoolMask // still no
            CanInterruptOffset2 = CanInterruptOffset + 4, // = CGSpellBook::m_interruptSchoolMask
            CanInterruptOffset3 = CanInterruptOffset2 + 4, // = CGSpellBook::m_silenceSchoolMask

            TransportGUID = 0xAC8, // CGUnit_C__HasVehicleTransport // findable with while inside an elevator
            DBCacheRow = 0xC68, // CGUnit_C__GetUnitName
            CachedSubName = 0x0, // beginning of DBCacheRow pointer = CachedSubName
            CachedName = 0x80, // end of CGUnit_C::GetCreatureRank
            CachedTypeFlag = 0x24, 
            CachedQuestItem1 = 0x3C, 
            CachedModelId1 = 0x6C, // 
            CachedUnitClassification = 0x2C, // Script_UnitClassification + 0x3C (CGUnit_C::GetCreatureRank) ??
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
            GAMEOBJECT_FIELD_R = GAMEOBJECT_FIELD_X + 0x10,
            PackedRotationQuaternion = 0x148, // GetQuaternionOffset => UnpackQuaternion
            TransformationMatrice = 0x270, // GetMatrixOffset  ((this + 0x270), 0x40u) ??
            DBCacheRow = 0x26C, // CGGameObject_C::GetName
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
            StatPvp = 0xD00298, // StatPvpFOUND inside first call in Script_InActiveBattlefield
            PvpExitWindow = 0xFAB4A4, // Script_GetBattlefieldWinner
            MaxBattlegroundId = 0xFABDA0, // Script_GetMaxBattlefieldID
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
            SpellBookNumSpells = 0xF9C5DC, // CGSpellBook__MakeKnownSpellModelsLocal
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xF9C628 + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
            FirstTalentBookPtr = 0xF9C6D4, // FirstTalentBookFOUND
            NextTalentBookPtr = FirstTalentBookPtr - 0x8,
            TalentBookSpellId = 0x14,
            TalentBookOverrideSpellId = 0x1C,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xF3F660,
            NextMessage = 0x17E8,
            msgFormatedChat = 0x65,
            chatBufferPos = 0xFF990C0,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xEC43C8, // CGUnit_C__IsAutoTracking / CGPlayer_C__ClickToMove
            CTM_X = CTM + 0x28, // to Check
            CTM_Y = CTM_X + 0x4,
            CTM_Z = CTM_Y + 0x4,
        }

        /// <summary>
        ///   Virtual Function
        /// </summary>
        public enum VMT
        {
            CGUnit_C__GetFacing = 0x39, // *4 > MovementGetTransportFacing
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            WoWTextCaller = 0x07415E7, // WoWCallingMule, "call edx, rt" address.
            CTMChecker = 0x341FB9,
            CTMChecker2 = 0x550B85,
            RetFromFunctionBelow = 0x1A5DE3,
            // mov [D16250+E48], [D16250+E44]
            // new = D87F40
            SpellChecker = 0xD88F20, // Script_IsStereoVideoAvailable
            SpellCheckerOff1 = 0xE44,
            SpellCheckerOff2 = 0xE48,
            SpellFixer = 0x10E2C3, // ???
            // E38EF0 vs EC3450
            ClntObjMgrGetActivePlayerObj = 0x8DD5A,
            FrameScript_ExecuteBuffer = 0xB2E28,
            CGPlayer_C__ClickToMove = 0x32FE34, // alias CGUnit_C__InitializeTrackingState
            FrameScript__GetLocalizedText = 0x32A5C0,
            WowClientDB2__GetRowPointer = 0x21AA3F,
            CGWorldFrame__Intersect = 0x6434BF,
            Spell_C_HandleTerrainClick = 0x2DBE18,
            CGUnit_C__Interact = 0x5341D,
            strlen = 0x7BA7C0, // ida _strlen
            // saving
            IsOutdoors = 0x0,
            UnitCanAttack = 0x0,
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xF3EDA0 + 0x3C, // RetrieveCorpseWindowFOUND
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            PlayerNameStorePtr = 0xDD5DE0, // CGUnit_C__GetUnitName + 0x62 // Script_ResurrectGetOfferer
            PlayerNameNextOffset = 0x14,
            PlayerNameStringOffset = 0x11,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            // Script_SelectedRealmName
            realmName = 0x101FD3C, // SelectedRealmName_DWORD - ClientServices__Initialize end
            realmNameOffset = 0x394, // SelectedRealmName_Offset
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xF3DEBC, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xF3DEC4, // Cvars+b7
            AutoLoot_Activate_Pointer = 0xF3DEDC, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xF3DEE8, // Cvars+2A8
            Activate_Offset = 0x34,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xEC, //overlooked with cheat engine with baseaddress of 3 NPCs.
            FlightMasterStatus = 0xF0,
            QuestId = 0xFCF6DC, // Script_GetQuestID
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
            AuraTable1 = 0x1190,
            AuraTable2 = 0x880,
            AuraSize = 0x88, // useful size 0x58 § v2 = 136 * a2;

            AuraStructCreatorGuid = 0x48, // read 16 bytes (GUID)
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
            PowerIndexArrays = 0xEC4454, // PowerTypePointer
            Multiplicator = 0x13 - 1,
        }
    }
}