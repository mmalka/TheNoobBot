namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 17055
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xE3CB00
        }

        public enum ObjectManager
        {
            objectManager = 0x462C,
            firstObject = 0xCC,
            nextObject = 0x34,
            localGuid = 0xE0,
            objectGUID = 0x28,
            objectTYPE = 0xC,
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xB30B74,
            DX_DEVICE_IDX = 0x2818,
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary>
        ///   Is Falling (Script_IsFalling)
        /// </summary>
        public enum IsFalling
        {
            flag = 0x800,
            offset1 = 0xE4,
            offset2 = 0x38,
        }

        /// <summary>
        ///   Is Swimming (Script_IsSwimming) [[base+offset1]+offset2]
        /// </summary>
        public enum IsSwimming
        {
            flag = 0x100000,
            offset1 = 0xE4,
            offset2 = 0x38,
        }

        /// <summary>
        ///   Is Flying (Script_IsFlying) [[base+offset1]+offset2]
        /// </summary>
        public enum IsFlying
        {
            flag = 0x1000000,
            offset1 = 0xE4,
            offset2 = 0x38,
        }

        public enum Party
        {
            PartyOffset = 0xD396C4,
            NumOfPlayers = 0xC4,
            NumOfPlayers_SuBGroup = 0xC8,
            PlayerGuid = 0x10,
        }

        public enum PetBattle
        {
            IsInBattle = 0xADDFF8,
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            Spell = 0xC099B0,
            SpellCastTimes = 0xC09488,
            SpellRange = 0xC09958,
            SpellMisc = 0xC0987C,
            FactionTemplate = 0xC07888,
            Lock = 0xC088D8,
            Map = 0xC0A740,
            ResearchSite = 0xC09010,
            QuestPOIPoint = 0xC0D9F8, // not found
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xA5E814,
            buildWowVersion = 0xB12B44,
            gameState = 0xCDC84E,
            isLoadingOrConnecting = 0xC11FE0,
            AreaId = 0xADFA68,
            lastWowErrorMessage = 0xCDBC50,
            zoneMap = 0xCDC844,
            subZoneMap = 0xCDC840,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xD4B4A0,
            playerName = 0xE3CB40,
            PlayerComboPoint = 0xCDC931,
            RetrieveCorpseWindow = 0xCDC904,
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xD4D690,
            startBar = 0xD4DC34,
            nbBar = 0xD4DC30,
            nextSlot = 0x4,
        }

        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x788,
            UNIT_FIELD_X = 0x7F8,
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0xC60, // Script_UnitCastingInfo
            ChannelSpellID = 0xC78, // Script_UnitChannelInfo
            TransportGUID = 0x7F0, // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0x974, // CGUnit_C__GetUnitName
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
            DBCacheRow = 0x1B8,
            CachedName = 0xB0,
            CachedData0 = 0x14,
            CachedData1 = 0x18,
            CachedData8 = 0x34, // (Data0 + 8 * 0x04)
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xADF810,
            pvpExitWindow = 0xD39E28,
            selectedBattlegroundId = 0xD39DBC,
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
            knownSpell = 0xD393BC,
            nbSpell = 0xD393B8,
        }

        /// <summary>
        ///   Chat
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xCDE728,
            NextMessage = 0x17C8,
            msgFormatedChat = 0x45,
            chatBufferPos = 0xD37EA0,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xC8A758,
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
            CGUnit_C__GetFacing = 0x27, // MovementGetTransportFacing
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x3656D6,
            FrameScript_ExecuteBuffer = 0x55347,
            CGUnit_C__InitializeTrackingState = 0x3E269F,
            ClntObjMgrGetActivePlayerObj = 0x2CB4,
            FrameScript__GetLocalizedText = 0x3DD8F9,
            CGWorldFrame__Intersect = 0x5A3E53,
            Spell_C_HandleTerrainClick = 0x357DB3,
            Interact = 0x84DE67,
        }

        /// <summary>
        ///   Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xCDCC00,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC03D58 + 0x8,
            nameMaskOffset = 0x024,
            nameBaseOffset = 0x18,
            nameStringOffset = 0x21,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xE3CCD0 + 0x6,
            battlerNetWindow = 0xC11FE0,
            //battleNetAccountId = 0xE3C580, // like in the /WTF/Account directory
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xCDCA68, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xCDCA70, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xCDCA88, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xCDCA94, // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            // TODO Retrieve unknown Quests offsets
            QuestGiverStatus = 0xB4,
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
            AURA_COUNT_1 = 0x10F8,
            AURA_COUNT_2 = 0xDF8,
            AURA_TABLE_1 = 0xDF8,
            AURA_TABLE_2 = 0xDFC,
            AURA_SIZE = 0x30,
            AURA_SPELL_ID = 0x18,
            AURA_STACK = 0x1D,
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xC8A3E4,
            Multiplicator = 0x10,
        }
    }
}