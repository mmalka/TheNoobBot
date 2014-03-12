namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 18019
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xEC4140
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
            DX_DEVICE = 0xBB2AD8, // Tocheck
            DX_DEVICE_IDX = 0x2820, // Tocheck
            ENDSCENE_IDX = 0xA8, // Tocheck
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlagsOffsets
        {
            Offset1 = 0xEC, // Tocheck
            Offset2 = 0x38, // Tocheck
        }

        public enum Party
        {
            PartyOffset = 0xDC23FC, // Tocheck // Script_SendChatMessage First offset/4th block
            NumOfPlayers = 0xC4, // Tocheck
            NumOfPlayers_SuBGroup = 0xC8, // Tocheck
            PlayerGuid = 0x10, // Tocheck
        }

        public enum PetBattle
        {
            IsInBattle = 0xDBF67C, // Tocheck
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            ItemClass = 0xC8AFB8,
            ItemSubClass = 0xC8B430,
            Spell = 0xC8C980,
            SpellCastTimes = 0xC8C3A8,
            SpellRange = 0xC8C8FC,
            SpellMisc = 0xC8C7C8,
            FactionTemplate = 0xC8A670,
            Lock = 0xC8B7F8,
            Map = 0xC8DB14,
            ResearchSite = 0xC8BF30,
            QuestPOIPoint = 0xC8BD78,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xADF5E8, // Tocheck
            buildWowVersion = 0xB94994, // Tocheck
            gameState = 0xD64626, // Tocheck
            isLoadingOrConnecting = 0xC953C0, // Tocheck
            AreaId = 0xB63358, // Tocheck
            lastWowErrorMessage = 0xD64A28, // Tocheck
            zoneMap = 0xD6361C, // Tocheck
            subZoneMap = 0xD63618, // Tocheck
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDD4518, // Tocheck
            playerName = 0xEC4180, // Tocheck
            PlayerComboPoint = 0xD65709, // Tocheck
            RetrieveCorpseWindow = 0xD656DC, // Tocheck
            // Some offsets to refine descriptor
            SkillValue = 0x200, // Tocheck
            SkillMaxValue = 0x400, // Tocheck
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xDD7208, // Tocheck
            nbBar = 0xDD77AC, // Tocheck
            startBar = 0xDD77AC, // Tocheck
            nextSlot = 0x4, // Tocheck
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xBA5CF0, // Tocheck
            BaseEvents = EventsCount + 0x4, // Tocheck
            EventOffsetName = 0x14, // Tocheck
            EventOffsetCount = 0x48, // Tocheck
            LastLootName = 0xD64080, // Tocheck
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
            CastingSpellID = 0xCB8, // Tocheck // Script_UnitCastingInfo
            ChannelSpellID = 0xCD0, // Tocheck // Script_UnitChannelInfo
            CanInterrupt = 0xC64, // Tocheck // check // Script_UnitCastingInfo/Script_UnitChannelInfo
            TransportGUID = 0x830, // Tocheck // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0x9B4, // Tocheck // CGUnit_C__GetUnitName
            CachedName = 0x6C, // Tocheck
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0x1F4, // Tocheck
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4, // Tocheck
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8, // Tocheck
            DBCacheRow = 0x1C0, // Tocheck // CGGameObject_C::GetName
            CachedName = 0xB0, // Tocheck // CGGameObject_C__GetName_2
            CachedData0 = 0x14, // Tocheck
            CachedData1 = 0x18, // Tocheck
            CachedData8 = 0x34, // Tocheck // (Data0 + 8 * 0x04)
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xB63104, // Tocheck // Script_InActiveBattlefield_0
            pvpExitWindow = 0xDC2B60, // Tocheck // Script_GetBattlefieldWinner
            selectedBattlegroundId = 0xDC2AF4, // Tocheck // Script_CanJoinBattlefieldAsGroup
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
            SpellBookNumSpells = 0xDC20F0, // Tocheck // Script_SetSpellBookItem
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // Tocheck // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xDC113C + 0x4 * 0x4, // Tocheck // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4, // Tocheck
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD67428, // Tocheck
            NextMessage = 0x17C8, // Tocheck
            msgFormatedChat = 0x45, // Tocheck
            chatBufferPos = 0xDC0BA0, // Tocheck
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xD0EEA0, // Tocheck // GetClickToMoveStruct
            CTM_PUSH = CTM + 0x1C, // Tocheck
            CTM_X = CTM + 0x8C, // Tocheck
            CTM_Y = CTM_X + 0x4, // Tocheck
            CTM_Z = CTM_Y + 0x4, // Tocheck
        }

        /// <summary>
        ///   Virtual Function
        /// </summary>
        public enum VMT
        {
            CGUnit_C__GetFacing = 0x29, // Tocheck // *4 > MovementGetTransportFacing
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x39B615, // Tocheck
            ClntObjMgrGetActivePlayerObj = 0x4FC6, // Tocheck
            FrameScript_ExecuteBuffer = 0x50236, // Tocheck
            CGUnit_C__InitializeTrackingState = 0x420543, // Tocheck // alias CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x414C74, // Tocheck
            CGWorldFrame__Intersect = 0x5EF3C0, // Tocheck
            Spell_C_HandleTerrainClick = 0x38F92D, // Tocheck
            CGUnit_C__Interact = 0x8D01D0, // Tocheck
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD659E8, // Tocheck
            Y = X + 0x4, // Tocheck
            Z = X + 0x8, // Tocheck
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC86358 + 0x8, // Tocheck
            nameMaskOffset = 0x24, // Tocheck
            nameBaseOffset = 0x18, // Tocheck
            nameStringOffset = 0x21, // Tocheck
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEC4320 + 0x6, // Tocheck
            battlerNetWindow = 0xC953C0, // Tocheck
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xD65848, // Tocheck // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xD65850, // Tocheck // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xD65868, // Tocheck // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xD65874, // Tocheck // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x30, // Tocheck
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
            AURA_COUNT_1 = 0x1218, // Tocheck
            AURA_COUNT_2 = 0xE18, // Tocheck
            AURA_TABLE_1 = 0xE18, // Tocheck
            AURA_TABLE_2 = 0xE1C, // Tocheck
            AURA_SIZE = 0x40, // Tocheck
            AURA_SPELL_ID = 0x28, // Tocheck
            AURA_STACK = 0x1D, // Tocheck
            AURA_SPELL_START = 0x24, // Tocheck
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xD0EB2C, // Tocheck // PowerTypePointer
            Multiplicator = 0x10, // Tocheck
        }
    }
}