namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 25383
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // dropped usage, kept only for online script not crashing for older versions.
            public static uint sCurMgr = 0x1138BE0; // 1138BE0 CCommand_ObjUsage
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
            DX_DEVICE = 0x10B425C, //Script_ShowCursor, first offset
            DX_DEVICE_IDX = 0x2574, // DX9_DEVICE_IDX_FOUND
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlagsOffsets
        {
            MovementPointer = 0x124, // near any movement flag.
            MovementFlags = 0x40,
            CurrentVelocity = 0x8C,
            GroundVelocity = 0x94,
            SwimmingVelocity = 0x9C,
            FlyingVelocity = 0xA4
        }

        public enum Party
        {
            PartyOffset = 0x12D01B8, // Script_SendChatMessage First offset/4th block
            NumOfPlayers = 0xC8, // Script_GetNumGroupMembers
            NumOfPlayersSuBGroup = 0xCC, // NumOFPlayers+4
            PlayerGuid = 0x10,
        }

        /// <summary>
        ///   PetBattle
        /// </summary>
        public enum PetBattle
        {
            IsInBattle = 0x1022050, // aC_petbattles ; "C_PetBattles"
            // Findable under offset aC_petbattles.
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
            GetTime = 0x10A13B0, //FrameTime__GetCurTimeMs
            buildWoWVersionString = 0x10F6118, // buildWoWVersionStringFOUND
            gameState = 0x12CC9B4, // Script_IsPlayerInWorld
            isLoading = 0x1093E3C, //isLoadingFOUND
            AreaId = 0x1020258, //?CF6FE8, // AreaIdFOUND - AreaIdFOUNDCall // to review changes
            HearthstoneSubAreaId = 0x11F612C, // HearthstoneIDFound called from Script_GetBindLocation 
            SubAreaId = AreaId + 4, // AreaId + 4 bytes
            MapTextureId = 0x1029478, //MapTextureIdFOUND
            zoneMap = 0x12CBB88, // Script_GetZoneText
            subZoneMap = 0x12CBB94, // Script_GetSubZoneText 

            // saving
            TextBoxActivated = 0xBBE9AC, // ?// 18414
            LastHardwareAction = 0xD0E090, // ?// Script_ToggleRun
        }

        public enum TargetSystem
        {
            PtrToVMT = 0x102859C, // Script_TargetNearest or any target related script
            Focus = 0x80, // to find manually
            Target = 0x28,
            //TargetTarget = 0x0,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0x12E2698, // petGUIDFOUND
            playerName = 0x1351CF0, // GetPlayerName 
            RetrieveCorpseWindow = 0x12CC854, // RetrieveCorpseWindowFOUND

            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
            LocalPlayerSpellsOnCooldown = 0x11258F0, // LocalPlayerSpellsOnCooldownFOUND

            // saving
            RuneStartCooldown = 0xF18AA8, // Script_GetRuneCount
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0x10A16B8, // EventSystem2 in EventSystem
            BaseEvents = EventsCount + 0x4,
            EventOffsetName = 0x18,
            EventOffsetCount = 0x48,
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_FIELD_X = 0xAE8, // found with a ugly while
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            UNIT_VELOCITY = 0x8C,
            CastingSpellID = 0x1068, // Script_UnitCastingInfo
            CastingSpellStartTime = 0x1090,
            CastingSpellEndTime = 0x1094,
            ChannelSpellID = 0x1098, // Script_UnitChannelInfo
            ChannelSpellStartTime = 0x10A0,
            ChannelSpellEndTime = 0x10A4,
            CanInterrupt = 0xFC4, // SpellCanBeInterrupted from Script_UnitCastingInfo/Script_UnitChannelInfo

            // SpellCanBeInterrupted changed too much, not sure if CanInterrupt even work.
            CanInterruptOffset = 0xE02EA0, // SpellCanBeInterrupted = CGSpellBook::m_silenceHarmfulSchoolMask // still no
            CanInterruptOffset2 = CanInterruptOffset + 4, // = CGSpellBook::m_interruptSchoolMask
            CanInterruptOffset3 = CanInterruptOffset2 + 4, // = CGSpellBook::m_silenceSchoolMask

            TransportGUID = 0xAD8, // CGUnit_C__HasVehicleTransport // findable with while inside an elevator
            DBCacheRow = 0xC78, // CGUnit_C__GetUnitName
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
            TransportGUID = 0x128, // GO TransportGUID
            DBCacheRow = 0x26C, // CGGameObject_C::GetName
            CachedIconName = 0x08,
            CachedCastBarCaption = 0xC,
            CachedName = 0xB4, // CGGameObject_C__GetName_2
            CachedData0 = 0x14, // CGGameObject_C::GetLockRecord
            CachedSize = CachedData0 + (0x04 * 33), // just after the 32 data uint32 + 1 unknown value
            CachedQuestItem1 = CachedSize + 0x04, // just after the size float
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            StatPvp = 0x1029764, // StatPvpFOUND inside first call in Script_InActiveBattlefield
            PvpExitWindow = 0x12DF5F8, // Script_GetBattlefieldWinner
            MaxBattlegroundId = 0x12DF478, // Script_GetMaxBattlefieldID
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
            SpellBookNumSpells = 0x12CD9B4, // CGSpellBook__MakeKnownSpellModelsLocal
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0x12CDA00 + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
            FirstTalentBookPtr = 0x12CDA9C, // FirstTalentBookFOUND
            NextTalentBookPtr = FirstTalentBookPtr - 0x8,
            TalentBookSpellId = 0x14,
            TalentBookOverrideSpellId = 0x1C,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0x126F630,
            NextMessage = 0x17E8,
            msgFormatedChat = 0x65,
            chatBufferPos = 0x12C9090,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0x11F5294, // CGUnit_C__IsAutoTracking / CGPlayer_C__ClickToMove
            CTM_X = CTM + 0x28,
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
            FrameScript_ExecuteBuffer = 0x2CDB99,
            CGPlayer_C__MoveTo = 0x5B8089,
            FrameScript__GetLocalizedText = 0x5A1504,
            CGWorldFrame__Intersect = 0x9318A3,
            Spell_C_HandleTerrainClick = 0x53C0F5,
            CGUnit_C__Interact = 0x761F3A,
            CGUnit_C__GetUnitName = 0x5A4361,
            strlen = 0xAA87D0, // ida _strlen
            PushESI = 0x5B823C, // CGPlayer_C__ClickToMoveCaller+178

            // saving
            ClntObjMgrGetActivePlayerObj = 0x0,
            WowClientDB2__GetRowPointer = 0x324719,
            CGPlayer_C__ClickToMove = 0x5A7557, // alias CGUnit_C__InitializeTrackingState // to match...
            GetTargetInfo = 0x2C60F5, // our hook address in ida: RenderingMessage
            ReturnFunc = 0x3B5E43, // the function call of our hook: OnHookFunction
            WoWTextCaller = 0x73F487, // WoWCallingMule, "call edx, rt" address.
            CTMChecker = 0x334789,
            CTMChecker2 = 0x61D6E4,
            RetFromFunctionBelow = 0x1A8C84,

            // mov [D16250+E48], [D16250+E44]
            // new = D87F40
            SpellChecker = Hooking.DX_DEVICE, // IsSpellKnown
            SpellCheckerOff1 = 0xE60,
            SpellCheckerOff2 = 0xE64,

            //SpellFixer = 0x10E2C3, // IsSpellKnown
            // E38EF0 vs EC3450
            IsOutdoors = 0x0,
            UnitCanAttack = 0x0,
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap__SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0x12CC9E8 + 0x3C, // RetrieveCorpseWindowFOUND //to check
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            PlayerNameStorePtr = 0x1101968, // CGUnit_C__GetUnitName + 0x62 // Script_ResurrectGetOfferer
            PlayerNameNextOffset = 0x14,
            PlayerNameStringOffset = 0x11,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            // Script_SelectedRealmName
            realmName = 0x135155C, // SelectedRealmName_DWORD - ClientServices__Initialize end
            realmNameOffset = 0x39C, // SelectedRealmName_Offset
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0x12CB9B4, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0x12CB9BC, // CVar+b7
            AutoLoot_Activate_Pointer = 0x12CB9D4, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0x12CB9E0, // CVar+294
            Activate_Offset = 0x34,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xEC, //overlooked with cheat engine with baseaddress of 3 NPCs.
            FlightMasterStatus = 0xF0,
            QuestId = 0x11BAD6C, // Script_GetQuestID
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
            AuraTable1 = 0x11A8,
            AuraTable2 = 0x980,
            AuraSize = 0x98, // useful size 0x68 § v2 = 136 * a2;

            AuraStructCreatorGuid = 0x48, // read 16 bytes (GUID)
            AuraStructSpellId = AuraStructCreatorGuid + 16 + 4 + 4 + 4 + 4, // read 16 bytes (GUID)
            AuraStructFlag = AuraStructSpellId + 4 + 4, // read 1 byte
            AuraStructCount = AuraStructFlag + 1, // read 1 byte
            AuraStructCasterLevel = AuraStructCount + 1, // read 2 bytes
            AuraStructDuration = AuraStructCasterLevel + 2, // read 4 bytes
            AuraStructSpellEndTime = AuraStructDuration + 4, // read 4 bytes
            /*
                public unsafe fixed int struct646_0[8];
                public int int_0;
                private uint uint_0;
                private uint uint_1;
                private uint uint_2;
                private uint uint_3;
                private uint uint_4;
                private uint uint_5;
                private uint uint_6;
                private uint uint_7;
                private uint uint_8;
                public WoWGuid woWGuid_0;
                private uint uint_9;
                private uint uint_10;
                private uint uint_11;
                private uint uint_12;
                public uint SpellId;
                private uint uint_14;
                public byte Flags;
                public byte StackCount;
                public ushort Level;
                public uint Duration;
                public uint EndTime;
                private uint uint_17;
                private uint uint_18;
                private uint uint_19;
                private uint uint_20;
                private uint uint_21;
                private uint uint_22;
                private uint uint_23;
             */
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0x11F52DC, // PowerTypePointer
            Multiplicator = 0x13 - 1,
        }
    }
}