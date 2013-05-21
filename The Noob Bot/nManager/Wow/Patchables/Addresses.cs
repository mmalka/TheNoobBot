namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 16965
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xE416A0
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
            DX_DEVICE = 0xB35714,
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
            PartyOffset = 0xD3E264,
            NumOfPlayers = 0xC4,
            NumOfPlayers_SuBGroup = 0xC8,
            PlayerGuid = 0x10,
        }

        public enum PetBattle
        {
            IsInBattle = 0xAE1FF8,
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            Spell = 0xC0E550,
            SpellCastTimes = 0xC0E028,
            SpellRange = 0xC0E4F8,
            SpellMisc = 0xC0E41C,
            FactionTemplate = 0xC0C428,
            Lock = 0xC0D478,
            Map = 0xC0F2E0,
            ResearchSite = 0xC0DBB0,
            QuestPOIPoint = 0xC0D9F8,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xA62814,
            buildWowVersion = 0xB176E4,
            gameState = 0xCE13EE,
            isLoadingOrConnecting = 0xC16B80,
            AreaId = 0xAE3A68,
            lastWowErrorMessage = 0xCE07F0,
            zoneMap = 0xCE13E4,
            subZoneMap = 0xCE13E0,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xD50040,
            playerName = 0xE416E0,
            PlayerComboPoint = 0xCE14D1,
            RetrieveCorpseWindow = 0xCE14A4,
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xD52230,
            startBar = 0xD527D4,
            nbBar = 0xD527D0,
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
            statPvp = 0xAE3810,
            pvpExitWindow = 0xD3E9C8,
            selectedBattlegroundId = 0xD3E95C,
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            knownSpell = 0xD3DF5C,
            nbSpell = 0xD3DF58,
        }

        /// <summary>
        ///   Chat
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xCE32C8,
            NextMessage = 0x17C8,
            msgFormatedChat = 0x45,
            chatBufferPos = 0xD3CA40,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xC8F2F8,
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
            ClntObjMgrGetActivePlayer = 0X364F37,
            FrameScript_ExecuteBuffer = 0x54DCB,
            CGPlayer_C__ClickToMove = 0x3E2366, // CGUnit_C__InitializeTrackingState
            ClntObjMgrGetActivePlayerObj = 0x2D15,
            FrameScript__GetLocalizedText = 0x3DD5C0,
            CGWorldFrame__Intersect = 0x5A341F,
            Spell_C__HandleTerrainClick = 0x357714,
            Interact = 0x850D08,
        }

        /// <summary>
        ///   Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xCE17A0,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC088F8 + 0x8,
            nameMaskOffset = 0x024,
            nameBaseOffset = 0x18,
            nameStringOffset = 0x21,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xE41870 + 0x6,
            battlerNetWindow = 0xC16B80,
            //battleNetAccountId = 0xE41120, // like in the /WTF/Account directory
        }

        /// <summary>
        ///   Active AutoLoot
        /// </summary>
        public enum AutoLoot
        {
            AutoLoot_Activate_Pointer = 0xCE1628,
            AutoLoot_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Active Auto Cast
        /// </summary>
        public enum AutoSelfCast
        {
            AutoSelfCast_Activate_Pointer = 0xCE1634,
            AutoSelfCast_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Active Auto Interact CTM
        /// </summary>
        public enum AutoInteract
        {
            AutoInteract_Activate_Pointer = 0xCE1608, // CGUnit_C__CanAutoInteract
            AutoInteract_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Active Dismount when casting spell even when flying
        /// </summary>
        public enum AutoDismount
        {
            AutoInteract_Activate_Pointer = 0xCE1614, // CGUnit_C__CanAutoDismount
            AutoInteract_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
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
            PowerIndexArrays = 0xC8EF84,
            Multiplicator = 0x10,
        }
    }
}