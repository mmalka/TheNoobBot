namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 18414
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0xFF6668;
            public static uint sCurMgr = 0xDD9B68; // It's the direct pointer to objectManager clientConn+objectManager
        }

        public enum ObjectManager
        {
            objectManager = 0x62C, // to be used with clientConnection or bypassed if using sCurMgr.
            localGuid = 0x110,
            objectGUID = 0x28,
            objectTYPE = 0xC, // ClntObjMgrGetActivePlayer
            firstObject = 0xD8, // ClntObjMgrEnumVisibleObjects
            nextObject = 0x3C,
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xD41DB8, // ClientInitializeGame, first offset
            DX_DEVICE_IDX = 0x2854, // DX9_DEVICE_IDX_FOUND (AC8A2)
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
            PartyOffset = 0xF1205C, // Script_SendChatMessage First offset/4th block
            NumOfPlayers = 0xCC, // Script_GetNumGroupMembers
            NumOfPlayers_SuBGroup = 0xD0, // NumOFPlayers+4 (toCheck)
            PlayerGuid = 0x10, // toUpdate
        }

        /// <summary>
        ///   PetBattle
        /// </summary>
        public enum PetBattle
        {
            IsInBattle = 0xCBF020, // LUA: Script_C_PetBattles_IsInBattle
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            ItemClass = 0xDAB938, // GetItemClassDBC
            ItemSubClass = 0xC8BB90, // toUpdate
            FactionTemplate = 0xDAAE3C, // missing rows but looks ok, may update struct as well
            Lock = 0xDAC294, // toCheck
            QuestPOIPoint = 0xC8C4D8, // toUpdate
            ResearchSite = 0xC8C690, // toUpdate
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xC5B4E4,
            continentIdOffset = 0xF0, // ?? mia
            buildWowVersion = 0xB94E74, // toUpdate mia yet
            gameState = 0xEB41BE,
            isLoadingOrConnecting = 0xDB91F8,
            AreaId = 0xCB4368, // toChecklel
            MapTextureId = 0xCC408C,
            zoneMap = 0xEB41B4, // toChecklel
            subZoneMap = 0xEB41B0, // toChecklel
            // saving
            TextBoxActivated = 0xBBE9AC, // toUpdate
            LastHardwareAction = 0xD41A48,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDD4A00, // toUpdate
            playerName = 0xFF66A8, // ida: GetPlayerName
            PlayerComboPoint = 0xD65BF9, // toUpdate
            RetrieveCorpseWindow = 0xD65BCC, // toUpdate
            // Some offsets to refine descriptor
            SkillValue = 0x200, // toUpdate
            SkillMaxValue = 0x400, // toUpdate
            // saving
            RuneStartCooldown = 0xDD73EC, // Script_GetRuneCount
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xD35048,
            BaseEvents = EventsCount + 0x4, // toUpdate
            EventOffsetName = 0x14, // toUpdate
            EventOffsetCount = 0x48, // toUpdate
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_FIELD_X = 0xA50, // found with a ugly while
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0xF38, // Script_UnitCastingInfo
            ChannelSpellID = 0xF58, // Script_UnitChannelInfo
            CanInterrupt = 0xC64, // toUpdate // check // Script_UnitCastingInfo/Script_UnitChannelInfo
            TransportGUID = 0x830, // toUpdate // CGUnit_C__HasVehicleTransport
            TaxiStatus = 0xC0, // toUpdate
            DBCacheRow = 0xBC4, // toChecklel // CGUnit_C__GetUnitName
            CachedSubName = 0x0, // toUpdate
            CachedName = 0x7C,
            CachedTypeFlag = 0x4C, // toUpdate
            CachedQuestItem1 = 0x30, // toUpdate
            CachedModelId1 = 0x5C, // toUpdate
            // saving
            UnitClassificationOffset1 = DBCacheRow, // Script_UnitClassification + 0x39 (CGUnit_C::GetCreatureRank)
            UnitClassificationOffset2 = 0x20, // toUpdate // Script_UnitClassification + 0x39 (CGUnit_C::GetCreatureRank)
            IsBossOffset1 = DBCacheRow,
            IsBossOffset2 = 0x5C, // toUpdate // function wow IsBoss() (or function at Script_UnitLevel + 0xB7)
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0x2A0,
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            PackedRotationQuaternion = 0x108, // toUpdate // I have no idea about what fct is behind this
            TransformationMatrice = 0x1C4, // toUpdate // CGGameObject_C::GetMatrix (fct name to be confirmed)
            DBCacheRow = 0x26C, // CGGameObject_C::GetName
            CachedIconName = 0x08, // toUpdate
            CachedCastBarCaption = 0x0C, // toUpdate
            CachedName = 0xB4, // CGGameObject_C__GetName_2
            CachedData0 = 0x14, // toUpdate // CGGameObject_C::GetLockRecord
            CachedSize = CachedData0 + (0x04 * 32), // toUpdate // just after the 32 data uint32
            CachedQuestItem1 = CachedSize + 0x04, // toUpdate // just after the size float
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            StatPvp = 0xCC3C4C, // Script_InActiveBattlefield_0
            PvpExitWindow = 0xF13058, // Script_GetBattlefieldWinner
            MaxBattlegroundId = 0xF13024, // Script_GetMaxBattlefieldID
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0xCC, // toUpdate
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            SpellBookNumSpells = 0xF11D4C, // Script_SetSpellBookItem
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, //toCheck // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xF11D98 + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xEB6180,
            NextMessage = 0x17E8,
            msgFormatedChat = 0x65,
            chatBufferPos = 0xF104FC,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xE57008, // GetClickToMoveStruct
            CTM_PUSH = CTM + 0x1C,
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
            ClntObjMgrGetActivePlayer = 0x0055021B,
            ClntObjMgrGetActivePlayerObj = 0x000042DB,
            FrameScript_ExecuteBuffer = 0x0002A3B7,
            CGUnit_C__InitializeTrackingState = 0x0034CCFB, // ChangedMUCH // alias CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x0033CB53,
            CGWorldFrame__Intersect = 0x0059C5D4,
            Spell_C_HandleTerrainClick = 0x0029819A, // changedMuch (1 big check, need to see if works)
            CGUnit_C__Interact = 0x009D54EB,
            // saving
            IsOutdoors = 0x00414b53, // version?
            UnitCanAttack = 0x0041ad3c, // version? 
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xEB4518,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xDAE3F0 + 0x8,
            nameMaskOffset = 0x24, // function need to be fixed for UInt128 guid + offsets needs to be found
            nameBaseOffset = 0x18, // ??
            nameStringOffset = 0x21, // ??
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xFF6850 + 0x6,
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xEB4344, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xEB434C, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xEB4364, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xEB4374, // CGGame_UI__IsAutoSelfCast
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
            GossipQuestNext = 0x0,*/
        }

        /// <summary>
        ///   Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AURA_COUNT_1 = 0x1520,
            AURA_COUNT_2 = 0x10A0,
            AURA_TABLE_1 = 0x10A0,
            AURA_TABLE_2 = 0x10A4, // toCheck
            AURA_SIZE = 0x40, // toCheck
            AURA_SPELL_ID = 0x30,
            AURA_STACK = 0x1D, // toCheck/Search
            AURA_SPELL_START = 0x24,
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xE56C64, // PowerTypePointer
            Multiplicator = 0x10, // toCheck
        }
    }
}