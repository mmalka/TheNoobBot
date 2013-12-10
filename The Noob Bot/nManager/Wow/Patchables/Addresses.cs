namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 17658
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xEBF608
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
            DX_DEVICE = 0xBADFF0,
            DX_DEVICE_IDX = 0x2820,
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
            PartyOffset = 0xDBD8D8,
            NumOfPlayers = 0xC4,
            NumOfPlayers_SuBGroup = 0xC8,
            PlayerGuid = 0x10,
        }

        public enum PetBattle
        {
            IsInBattle = 0xDBE67C, // check
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            ItemClass = 0xC864D8,
            ItemSubClass = 0xC86950,
            Spell = 0xC87EA0,
            SpellCastTimes = 0xC878C8,
            SpellRange = 0xC87E1C,
            SpellMisc = 0xC87CE8,
            FactionTemplate = 0xC85B90,
            Lock = 0xC86D18,
            Map = 0xC89034,
            ResearchSite = 0xC87450,
            QuestPOIPoint = 0xC87298, 
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xADA5E8,
            buildWowVersion = 0xB8FEAC,
            gameState = 0xD60B0E,
            isLoadingOrConnecting = 0xC908E0,
            AreaId = 0xB5E410,
            lastWowErrorMessage = 0xD5FF10,
            zoneMap = 0xD60B04,
            subZoneMap = 0xD60B00,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDCF848,
            playerName = 0xEBF648,
            PlayerComboPoint = 0xD60BF1,
            RetrieveCorpseWindow = 0xD60BC4,
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xDD2538,
            nbBar = 0xDD2AD8,
            startBar = 0xDD2ADC,
            nextSlot = 0x4,
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xBA1208,
            BaseEvents = EventsCount + 0x4,
            EventOffsetName = 0x14,
            EventOffsetCount = 0x48,
            LastLootName = 0xD5F560,
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x788, // check // 0xE4 ?
            UNIT_FIELD_X = 0x838, // check
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // check
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // check
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10, // check
            CastingSpellID = 0xCB8, // Script_UnitCastingInfo
            ChannelSpellID = 0xCD0, // Script_UnitChannelInfo
            CanInterrupt = 0xC64, // check // Script_UnitCastingInfo/Script_UnitChannelInfo
            TransportGUID = 0x830, // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0x9B4, // CGUnit_C__GetUnitName
            CachedName = 0x6C, // check // CGUnit_C__GetUnitName
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
            CachedData0 = 0x14, // check
            CachedData1 = 0x18, // check
            CachedData8 = 0x34, // check // (Data0 + 8 * 0x04)
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xB5E1BC, // Script_InActiveBattlefield_0
            pvpExitWindow = 0xDBE038, // Script_GetBattlefieldWinner
            selectedBattlegroundId = 0xDBDFCC, // Script_CanJoinBattlefieldAsGroup
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0xC4, // check
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            SpellBookNumSpells = 0xDBD5C8, // Script_SetSpellBookItem
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xDBD614 + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD62910,
            NextMessage = 0x17C8,
            msgFormatedChat = 0x45,
            chatBufferPos = 0xDBC088,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xD0A398, // GetClickToMoveStruct
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
            CGUnit_C__GetFacing = 0x29, // *4 > MovementGetTransportFacing
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x3995FB,
            ClntObjMgrGetActivePlayerObj = 0x4E3B,
            FrameScript_ExecuteBuffer = 0x5039C,
            CGUnit_C__InitializeTrackingState = 0x41E945, // alias CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x41345F,
            CGWorldFrame__Intersect = 0x5EDC2E,
            Spell_C_HandleTerrainClick = 0x38D7AA,
            CGUnit_C__Interact = 0x8CDB78,
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD60ED0,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC81878 + 0x8,
            nameMaskOffset = 0x24, // check
            nameBaseOffset = 0x18, // check
            nameStringOffset = 0x21, // check
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEBF7D8 + 0x6,
            battlerNetWindow = 0xC908E0,
            //battleNetAccountId = 0xEA1E08, // check // like in the /WTF/Account directory
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xD60D30, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xD60D38, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xD60D50, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xD60D5C, // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xB4, // check
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
            AURA_SIZE = 0x40, // check
            AURA_SPELL_ID = 0x28,
            AURA_STACK = 0x1D, // check
            AURA_SPELL_START = 0x24,
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xD0A024, // PowerTypePointer
            Multiplicator = 0x10,
        }
    }
}