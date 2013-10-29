namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 17399
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xEA2388
        }

        public enum ObjectManager
        {
            objectManager = 0x462C,
            localGuid = 0xE0,
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
            DX_DEVICE = 0xB94948,
            DX_DEVICE_IDX = 0x281C,
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlagsOffsets
        {
            Offset1 = 0xE4,
            Offset2 = 0x38,
        }

        public enum Party
        {
            PartyOffset = 0xDA0880,
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
            ItemClass = 0xBBA8D4, 
            ItemSubClass = 0xBBACF0, 
            Spell = 0xBBC10C, 
            SpellCastTimes = 0xBBBB8C, 
            SpellRange = 0xBBC088, 
            SpellMisc = 0xBBBFAC, 
            FactionTemplate = 0xBBA010, 
            Lock = 0xBBB060, 
            Map = 0xBBCE04, 
            ResearchSite = 0xBBB740, 
            QuestPOIPoint = 0xBBB588, 
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xAC15E8,
            buildWowVersion = 0xB76810,
            gameState = 0xD43ACE,
            isLoadingOrConnecting = 0xC76888,
            AreaId = 0xB44448,
            lastWowErrorMessage = 0xD42ED0,
            zoneMap = 0xD43AC4,
            subZoneMap = 0xD43AC0,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDB27D8,
            playerName = 0xEA1E08,
            PlayerComboPoint = 0xD43BB1,
            RetrieveCorpseWindow = 0xD43B84,
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xDB54C8,
            startBar = 0xDB5A6C,
            nbBar = 0xDB5A68,
            nextSlot = 0x4,
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xB87B64,
            BaseEvents = EventsCount + 0x4,
            EventOffsetName = 0x14,
            EventOffsetCount = 0x48,
            LastLootName = 0xD42528,
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x788, // 0xE4 ?
            UNIT_FIELD_X = 0x830,
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0xCB0, // Script_UnitCastingInfo
            ChannelSpellID = 0xCC8, // Script_UnitChannelInfo
            CanInterrupt = 0xC64, // Script_UnitCastingInfo/Script_UnitChannelInfo
            TransportGUID = 0x828, // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0x9AC,  // CGUnit_C__GetUnitName
            CachedName = 0x6C, // CGUnit_C__GetUnitName
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0xF0,
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            DBCacheRow = 0x1B8, // CGGameObject_C::GetName
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
            statPvp = 0xB441F0, // Script_InActiveBattlefield
            pvpExitWindow = 0xDA0FE0, // Script_GetBattlefieldWinner
            selectedBattlegroundId = 0xDA0F74, // Script_CanJoinBattlefieldAsGroup
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0xC4,
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            SpellBookNumSpells = 0xDA0570, // Script_SetSpellBookItem
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xDA05C0 + 0x4 * 0x4, 
            MountBookMountsPtr = MountBookNumMounts + 0x4, 
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD458D8,
            NextMessage = 0x17C8,
            msgFormatedChat = 0x45,
            chatBufferPos = 0xD9F050,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xCEFD38, // GetClickToMoveStruct
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
            CGUnit_C__GetFacing = 0x29, // MovementGetTransportFacing
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x38FE23,
            ClntObjMgrGetActivePlayerObj = 0x4E5A,
            FrameScript_ExecuteBuffer = 0x4F9EC, 
            CGUnit_C__InitializeTrackingState = 0x4153AA,  // CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x409F01, 
            CGWorldFrame__Intersect = 0x5DE2F9, 
            Spell_C_HandleTerrainClick = 0x38476E, 
            CGUnit_C__Interact = 0x8B7A83,
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD43E90,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC67BD0 + 0x8,
            nameMaskOffset = 0x24,
            nameBaseOffset = 0x18,
            nameStringOffset = 0x21,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEA2558 + 0x6,
            battlerNetWindow = 0xC76888,
            //battleNetAccountId = 0xEA1E08, // like in the /WTF/Account directory
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xD43CF0, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xD43CF8, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xD43D10, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xD43D1C, // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x30, // check
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xB4,
            // TODO Retrieve unknown Quests offsets
            ActiveQuests = 0x0, // check //not found
            SelectedQuestId = 0x0, // check //not found
            TitleText = 0x0, // check //not found
            GossipQuests = 0x0, // check //not found
            GossipQuestNext = 0x0, // check //not found
        }

        /// <summary>
        ///   Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AURA_COUNT_1 = 0x1238,
            AURA_COUNT_2 = 0xE38,
            AURA_TABLE_1 = 0xE38,
            AURA_TABLE_2 = 0xE3C,
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
            PowerIndexArrays = 0xCEF9C4, // PowerTypePointer
            Multiplicator = 0x10,
        }
    }
}