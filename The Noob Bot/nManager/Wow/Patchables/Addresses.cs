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
            DX_DEVICE = 0xB9E394,
            DX_DEVICE_IDX = 0x2818, // to change or verify
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlags
        {
            offset1 = 0xE4,
            offset2 = 0x38,
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
            NumOfPlayers = 0xC4,
            NumOfPlayers_SuBGroup = 0xC8,
            PlayerGuid = 0x10,
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
            ItemClass = 0xC763D8,
            ItemSubClass = 0xC76850,
            Spell = 0xC77DA0,
            SpellCastTimes = 0xC777C8,
            SpellRange = 0xC77D1C,
            SpellMisc = 0xC77BE8,
            FactionTemplate = 0xC75A90,
            Lock = 0xC76C18,
            Map = 0xC78ADC,
            ResearchSite = 0xC77350,
            QuestPOIPoint = 0xC77198,
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
            UNIT_SPEED = 0x788, // to change or verify // 0xE4 ?
            UNIT_FIELD_X = 0x830,
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0xCB0, // Script_UnitCastingInfo
            ChannelSpellID = 0xCC8, // Script_UnitChannelInfo
            CanInterrupt = 0xC64, // Script_UnitCastingInfo/Script_UnitChannelInfo
            TransportGUID = 0x828, // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0x9AC, // CGUnit_C__GetUnitName
            CachedName = 0x6C, // to change or verify // CGUnit_C__GetUnitName
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0xF0,
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            DBCacheRow = 0x1B8, // CGGameObject_C__GetName
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
            statPvp = 0xB4D628,
            pvpExitWindow = 0xDAA910,
            selectedBattlegroundId = 0xDAA8A4,
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
            nbSpell = 0xDA9EA0,
            knownSpell = 0xDA9EA4,
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
            ClntObjMgrGetActivePlayer = 0x3930BF,
            FrameScript_ExecuteBuffer = 0x5073E,
            CGUnit_C__InitializeTrackingState = 0x4185F6,
            ClntObjMgrGetActivePlayerObj = 0x4DE0,
            FrameScript__GetLocalizedText = 0x40D186,
            CGWorldFrame__Intersect = 0x5E0DC0,
            Spell_C_HandleTerrainClick = 0x387ACF,
            Interact = 0x8BA907,
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
            nameStorePtr = 0xC71680 + 0x8,
            nameMaskOffset = 0x24,
            nameBaseOffset = 0x18,
            nameStringOffset = 0x21,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEABFE8 + 0x6,
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
            AURA_COUNT_1 = 0x1238,
            AURA_COUNT_2 = 0xE38,
            AURA_TABLE_1 = 0xE38,
            AURA_TABLE_2 = 0xE3C,
            AURA_SIZE = 0x40,
            AURA_SPELL_ID = 0x28,
            AURA_STACK = 0x1D, // to change or verify
            AURA_SPELL_START = 0x24,
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