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
            public static uint clientConnection = 0x0; // 0xEC3140
        }

        public enum ObjectManager
        {
            objectManager = 0x462C, // Tocheck
            localGuid = 0xE8, // Tocheck
            objectGUID = 0x28, // Tocheck
            objectTYPE = 0xC, // Tocheck
            // These are 'hard coded' in the client. I don't remember the last time they changed.
            firstObject = 0xCC, // Tocheck
            nextObject = 0x34, // Tocheck
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xBB1AD8,
            DX_DEVICE_IDX = 0x2820, // Tocheck
            ENDSCENE_IDX = 0xA8, // Tocheck
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
            PartyOffset = 0xDC1400,
            NumOfPlayers = 0xC4,
            NumOfPlayers_SuBGroup = 0xC8,
            PlayerGuid = 0x10,
        }

        public enum PetBattle
        {
            IsInBattle = 0xDBE67C, // Tocheck
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            ItemClass = 0xC89FB8,
            ItemSubClass = 0xC8A430,
            Spell = 0xC8B980,
            SpellCastTimes = 0xC8B3A8,
            SpellRange = 0xBBC088,
            SpellMisc = 0xC8B7C8,
            FactionTemplate = 0xC89670,
            Lock = 0xC8A7F8,
            Map = 0xC8CB14,
            ResearchSite = 0xC8AF30,
            QuestPOIPoint = 0xC8AD78, 
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xADE5E8,
            buildWowVersion = 0xB93994,
            gameState = 0xD64626,
            isLoadingOrConnecting = 0xC943C0,
            AreaId = 0xB62358,
            lastWowErrorMessage = 0xD63A28,
            zoneMap = 0xD6461C,
            subZoneMap = 0xD64618,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDD3518,
            playerName = 0xEC3180,
            PlayerComboPoint = 0xD64709,
            RetrieveCorpseWindow = 0xD646DC,
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xDD6208,
            nbBar = 0xDD67AC,
            startBar = 0xDD67AC,
            nextSlot = 0x4,
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xBA4CF0,
            BaseEvents = EventsCount + 0x4,
            EventOffsetName = 0x14,
            EventOffsetCount = 0x48,
            LastLootName = 0xD63080,
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x80, // Tocheck // check // 0xE4 ?
            UNIT_FIELD_X = 0x838, // Tocheck // check
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // Tocheck // check
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // Tocheck // check
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10, // Tocheck // check
            CastingSpellID = 0xCB8, // Script_UnitCastingInfo
            ChannelSpellID = 0xCD0, // Script_UnitChannelInfo
            CanInterrupt = 0xC64, // Tocheck // check // Script_UnitCastingInfo/Script_UnitChannelInfo
            TransportGUID = 0x830, // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0x9B4, // CGUnit_C__GetUnitName
            CachedName = 0x6C,
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0x1F4, // Tocheck
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            DBCacheRow = 0x1C0, // CGGameObject_C::GetName
            CachedName = 0xB0, // CGGameObject_C__GetName_2
            CachedData0 = 0x14,
            CachedData1 = 0x18,
            CachedData8 = 0x34, // (Data0 + 8 * 0x04)
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xB62104, // Script_InActiveBattlefield_0
            pvpExitWindow = 0xDC1B60, // Script_GetBattlefieldWinner
            selectedBattlegroundId = 0xDC1AF4, // Script_CanJoinBattlefieldAsGroup
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0xCC, // Tocheck
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            SpellBookNumSpells = 0xDC10F0, // Script_SetSpellBookItem
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xDC113C + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD66428,
            NextMessage = 0x17C8,
            msgFormatedChat = 0x45,
            chatBufferPos = 0xDBFBA0,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xD0DEA0, // GetClickToMoveStruct
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
            ClntObjMgrGetActivePlayer = 0x39A658,
            ClntObjMgrGetActivePlayerObj = 0x4F8D,
            FrameScript_ExecuteBuffer = 0x4FD40,
            CGUnit_C__InitializeTrackingState = 0x41F8E5, // alias CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x414011,
            CGWorldFrame__Intersect = 0x5EEE53,
            Spell_C_HandleTerrainClick = 0x38E9CD,
            CGUnit_C__Interact = 0x8D0116,
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD649E8,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC84358 + 0x8,
            nameMaskOffset = 0x24,
            nameBaseOffset = 0x18,
            nameStringOffset = 0x21,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEC3320 + 0x6,
            battlerNetWindow = 0xC943C0,
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xD64848, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xD64850, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xD64868, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xD64874, // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xB4, // Tocheck // check
            // TODO Retrieve unknown Quests offsets
            ActiveQuests = 0x0, // Tocheck //not found
            SelectedQuestId = 0x0, // Tocheck //not found
            TitleText = 0x0, // Tocheck //not found
            GossipQuests = 0x0, // Tocheck //not found
            GossipQuestNext = 0x0, // Tocheck //not found
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
            AURA_SIZE = 0x40, // Tocheck
            AURA_SPELL_ID = 0x28,
            AURA_STACK = 0x1D, // Tocheck
            AURA_SPELL_START = 0x24,
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xD0DB2C, // PowerTypePointer
            Multiplicator = 0x10,
        }
    }
}