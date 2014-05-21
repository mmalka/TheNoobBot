namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 18291
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xEC4628
        }

        public enum ObjectManager
        {
            objectManager = 0x462C,
            localGuid = 0xE8,
            objectGUID = 0x28,
            objectTYPE = 0xC,
            // These are 'hard coded' in the client. I don't remember the last time they changed.
            firstObject = 0xCC,
            nextObject = 0x34,
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xBB2FB8,
            DX_DEVICE_IDX = 0x2820, // DX9_DEVICE_IDX_FOUND  (ida func)
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlagsOffsets
        {
            Offset1 = 0xEC,
            Offset2 = 0x38,
        }

        public enum Party
        {
            PartyOffset = 0xDC28EC, // Script_SendChatMessage First offset/4th block
            NumOfPlayers = 0xC4,
            NumOfPlayers_SuBGroup = 0xC8,
            PlayerGuid = 0x10,
        }

        /// <summary>
        ///   PetBattle
        /// </summary>
        public enum PetBattle
        {
            IsInBattle = 0xB61B20, // LUA: Script_C_PetBattles_IsInBattle
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            ItemClass = 0xC8B718,
            ItemSubClass = 0xC8BB90,
            FactionTemplate = 0xC8ADD0,
            SpellCastTimes = 0xC8CB08,
            Lock = 0xC8BF58,
            QuestPOIPoint = 0xC8C4D8,
            ResearchSite = 0xC8C690,
            Spell = 0xC8D0E0,
            SpellRange = 0xC8D05C,
            SpellMisc = 0xC8CF28,
            Map = 0xC8E004,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xADF5E8,
            continentIdOffset = 0xF0, // ClntObjMgrGetMapID
            buildWowVersion = 0xB94E74,
            gameState = 0xD65B16,
            isLoadingOrConnecting = 0xC958B0,
            AreaId = 0xB635B0, 
            zoneMap = 0xD65B0C,
            subZoneMap = 0xD65B08,
            // saving
            TextBoxActivated = 0xBBE9AC,
            LastHardwareAction = 0xBB2C74,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDD4A00,
            playerName = 0xEC4668,
            PlayerComboPoint = 0xD65BF9,
            RetrieveCorpseWindow = 0xD65BCC,
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
            // saving
            RuneStartCooldown = 0xDD73D0, // Script_GetRuneCount
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xBA61D0,
            BaseEvents = EventsCount + 0x4,
            EventOffsetName = 0x14,
            EventOffsetCount = 0x48,
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x80,
            UNIT_FIELD_X = 0x838,
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0xCB8, // Script_UnitCastingInfo
            ChannelSpellID = 0xCD0, // Script_UnitChannelInfo
            CanInterrupt = 0xC64, // check // Script_UnitCastingInfo/Script_UnitChannelInfo
            TransportGUID = 0x830, // CGUnit_C__HasVehicleTransport
            TaxiStatus = 0xC0,
            DBCacheRow = 0x9B4, // CGUnit_C__GetUnitName
            CachedSubName = 0x0,
            CachedName = 0x6C,
            CachedTypeFlag = 0x4C,
            CachedQuestItem1 = 0x30,
            CachedQuestItem2 = 0x34,
            CachedQuestItem3 = 0x38,
            CachedQuestItem4 = 0x3C,
            CachedModelId1 = 0x5C,
            // saving
            UnitClassificationOffset1 = DBCacheRow, // Script_UnitClassification + 0x39 (CGUnit_C::GetCreatureRank)
            UnitClassificationOffset2 = 0x20, // Script_UnitClassification + 0x39 (CGUnit_C::GetCreatureRank)
            IsBossOffset1 = DBCacheRow,
            IsBossOffset2 = 0x4C, // function wow IsBoss() (or function at Script_UnitLevel + 0xB7)
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0x1F4,
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            DBCacheRow = 0x1C0, // CGGameObject_C::GetName
            CachedName = 0xB0, // CGGameObject_C__GetName_2
            CachedData0 = 0x14, // CGGameObject_C::GetLockRecord
            CachedData1 = 0x18,
            CachedData8 = 0x34, // (Data0 + 8 * 0x04)
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            StatPvp = 0xB6335C, // Script_InActiveBattlefield_0
            PvpExitWindow = 0xDC3050, // Script_GetBattlefieldWinner
            MaxBattlegroundId = 0xDC3014, // Script_GetMaxBattlefieldID
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0xCC,
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            SpellBookNumSpells = 0xDC25E0, // Script_SetSpellBookIte
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xDC262C + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD67918,
            NextMessage = 0x17C8,
            msgFormatedChat = 0x45,
            chatBufferPos = 0xDC1090,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xD0F388, //t  // GetClickToMoveStruct
            CTM_PUSH = CTM + 0x1C, //t 
            CTM_X = CTM + 0x8C, //t 
            CTM_Y = CTM_X + 0x4, //t 
            CTM_Z = CTM_Y + 0x4, //t 
        }

        /// <summary>
        ///   Virtual Function
        /// </summary>
        public enum VMT
        {
            CGUnit_C__GetFacing = 0x29, // *4 > MovementGetTransportFacing
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x0039AB9F,
            ClntObjMgrGetActivePlayerObj = 0x00004F84,
            FrameScript_ExecuteBuffer = 0x0004FD26,
            CGUnit_C__InitializeTrackingState = 0x0041FA94, // alias CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x004141AE,
            CGWorldFrame__Intersect = 0x005EEE4C,
            Spell_C_HandleTerrainClick = 0x0038F0E2,
            CGUnit_C__Interact = 0x008D01EE,
            // saving
            IsOutdoors = 0x00414b53, //t 
            UnitCanAttack = 0x0041ad3c, //t 
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD65ED8,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC86838 + 0x8,
            nameMaskOffset = 0x24,
            nameBaseOffset = 0x18,
            nameStringOffset = 0x21,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEC4808 + 0x6,
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xD65D38, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xD65D40, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xD65D58, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xD65D64, // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xBC, 
            // TODO Retrieve unknown Quests offsets
            ActiveQuests = 0x0, //not found
            SelectedQuestId = 0x0, //not found
            TitleText = 0x0, //not found
            GossipQuests = 0x0, //not found
            GossipQuestNext = 0x0, //not found
        }

        /// <summary>
        ///   Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AURA_COUNT_1 = 0x1218,
            AURA_COUNT_2 = 0xE18,
            AURA_TABLE_1 = 0xE18,
            AURA_TABLE_2 = 0xE1C, 
            AURA_SIZE = 0x40,
            AURA_SPELL_ID = 0x28,
            AURA_STACK = 0x1D,
            AURA_SPELL_START = 0x24,
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xD0F01C, // PowerTypePointer
            Multiplicator = 0x10, //t 
        }
    }
}