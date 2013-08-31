namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 17337
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xEABE18
        }

        public enum ObjectManager
        {
            objectManager = 0x462C,
            firstObject = 0xCC, // to change or verify
            nextObject = 0x34, // to change or verify
            localGuid = 0xE0, // to change or verify
            objectGUID = 0x28, // to change or verify
            objectTYPE = 0xC, // to change or verify
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xB9E394,
            DX_DEVICE_IDX = 0x2818, // to change or verify
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlags
        {
            offset1 = 0xE4, // to change or verify
            offset2 = 0x38, // to change or verify
            MovingForward = 0x1,
            MovingBackwards = 0x2,
            StrafingLeft = 0x4,
            StrafingRight = 0x8,
            TurningLeft = 0x10,
            TurningRight = 0x20,
            Stunned = 0x400,
            Falling = 0x800, // Script_IsFalling
            FallingFar = 0x1000, // What is this exactly ? "Going to die from the fall" or "far from the floor at this moment"
            Swimming = 0x100000, // Script_IsSwimming
            Flying = 0x1000000, // Script_IsFlying
        }

        public enum Party
        {
            PartyOffset = 0xDAA1AC,
            NumOfPlayers = 0xC4, // to change or verify
            NumOfPlayers_SuBGroup = 0xC8, // to change or verify
            PlayerGuid = 0x10, // to change or verify
        }

        public enum PetBattle
        {
            IsInBattle = 0xDBE67C,
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            ItemClass = 0xC08178, // to change or verify
            ItemSubClass = 0xC08594, // to change or verify
            Spell = 0xC099B0, // to change or verify
            SpellCastTimes = 0xC09488, // to change or verify
            SpellRange = 0xC09958, // to change or verify
            SpellMisc = 0xC0987C, // to change or verify
            FactionTemplate = 0xC07888, // to change or verify
            Lock = 0xC088D8, // to change or verify
            Map = 0xC0A740, // to change or verify
            ResearchSite = 0xC09010, // to change or verify
            QuestPOIPoint = 0xC08E58, // to change or verify
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xAC95E4,
            buildWowVersion = 0xB8020C,
            gameState = 0xD4D3FE,
            isLoadingOrConnecting = 0xC803E0,
            AreaId = 0xB4D880,
            lastWowErrorMessage = 0xD4C800,
            zoneMap = 0xD4D3F4,
            subZoneMap = 0xD4D3F0,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDBC110,
            playerName = 0xEABE58,
            PlayerComboPoint = 0xD4D4E1,
            RetrieveCorpseWindow = 0xD4D4B4,
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xDBEE00,
            startBar = 0xDBF3A4,
            nbBar = 0xDBF3A0,
            nextSlot = 0x4,
        }

        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x788, // to change or verify
            UNIT_FIELD_X = 0x7F8, // to change or verify
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // to change or verify
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // to change or verify
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10, // to change or verify
            CastingSpellID = 0xC60, // to change or verify // Script_UnitCastingInfo
            ChannelSpellID = 0xC78, // to change or verify // Script_UnitChannelInfo
            TransportGUID = 0x7F0, // to change or verify // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0x974, // to change or verify // CGUnit_C__GetUnitName
            CachedName = 0x6C, // to change or verify // CGUnit_C__GetUnitName
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0xF0, // to change or verify
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4, // to change or verify
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8, // to change or verify
            DBCacheRow = 0x1B8, // to change or verify
            CachedName = 0xB0, // to change or verify
            CachedData0 = 0x14, // to change or verify
            CachedData1 = 0x18, // to change or verify
            CachedData8 = 0x34, // to change or verify // (Data0 + 8 * 0x04)
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xB4D628,
            pvpExitWindow = 0xDAA910,
            selectedBattlegroundId = 0xDAA8A4,
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0xC4, // to change or verify
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            knownSpell = 0xDA9EA0,
            nbSpell = 0xDA9EA0,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD4F208,
            NextMessage = 0x17C8,
            msgFormatedChat = 0x45,
            chatBufferPos = 0xDA8980,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xCF99D0,
            CTM_PUSH = CTM + 0x1C, // to verify
            CTM_X = CTM + 0x8C, // to verify
            CTM_Y = CTM_X + 0x4, // to verify
            CTM_Z = CTM_Y + 0x4, // to verify
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
            ClntObjMgrGetActivePlayer = 0x3932B8,
            FrameScript_ExecuteBuffer = 0x506C4,
            CGUnit_C__InitializeTrackingState = 0x41850B,
            ClntObjMgrGetActivePlayerObj = 0x4ED1,
            FrameScript__GetLocalizedText = 0x40D03E,
            CGWorldFrame__Intersect = 0x5E0D4A,
            Spell_C_HandleTerrainClick = 0x387BDE,
            Interact = 0x8BA9E7,
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD4D7C0,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC71680 + 0x8, // to verify the + 0x8
            nameMaskOffset = 0x024, // to verify
            nameBaseOffset = 0x18, // to verify
            nameStringOffset = 0x21, // to verify
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEABFE8 + 0x6, // to verify + 0x6
            battlerNetWindow = 0xC803E0,
            //battleNetAccountId = 0xEAB898, // like in the /WTF/Account directory
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xD4D620, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xD4D628, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xD4D640, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xD4D64C, // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            // TODO Retrieve unknown Quests offsets
            QuestGiverStatus = 0xB4, // to change or verify
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
            AURA_COUNT_1 = 0x10F8, // to change or verify
            AURA_COUNT_2 = 0xDF8, // to change or verify
            AURA_TABLE_1 = 0xDF8, // to change or verify
            AURA_TABLE_2 = 0xDFC, // to change or verify
            AURA_SIZE = 0x30, // to change or verify
            AURA_SPELL_ID = 0x18, // to change or verify
            AURA_STACK = 0x1D, // to change or verify
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xCF965C,
            Multiplicator = 0x10, // to change or verify
        }
    }
}