namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 16309
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // E28420
        }

        public enum ObjectManager
        {
            objectManager = 0x462C,
            firstObject = 0xCC,
            nextObject = 0x3C,
            localGuid = 0xD0,
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xB18ADC,
            DX_DEVICE_IDX = 0x2808,
            ENDSCENE_IDX = 0xA8,
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

        public enum PetBattle
        {
            IsInBattle = 0xACB2A8, // LUA: C_PetBattles.IsInBattle
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            Spell = 0xBFDBC8,
            SpellCastTimes = 0xBFD648,
            SpellRange = 0xBFDB44,
            SpellMisc = 0xBFDA68,
            FactionTemplate = 0xBFBACC,
            Lock = 0xBFCAF0,
            Map = 0xBFE8C4,
            ResearchSite = 0xBFD1D0,
            QuestPOIPoint = 0xBFD018,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            buildWowVersion = 0xAFACDC,
            gameState = 0xCC9EFA,
            isLoadingOrConnecting = 0xC05F80,
            continentId = 0xA55824,
            AreaId = 0xC6BC74,
            lastWowErrorMessage = 0xCC92F8,
            zoneMap = 0xCC9EF0,
            subZoneMap = 0xCC9EEC,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xD39518,
            playerName = 0xE28460,
            PlayerComboPoint = 0xCC9FDD,
            RetrieveCorpseWindow = 0xCC9FB0,
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xD3AAB8,
            startBar = 0xD3B05C, // STRUCTURE CHANGED ON 5.1.0.16309!
            nbBar = 0xD3AAB8,
            nextSlot = 0x4,
        }

        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x858,
            UNIT_FIELD_X = 0x7E8,
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
            CastingSpellID = 0xC38, // Script_UnitCastingInfo
            ChannelSpellID = 0xC50, // Script_UnitChannelInfo
            TransportGUID = 0x7E0, // CGUnit_C__GetTransportGUID
            DBCacheRow = 0x970, // CGUnit_C__GetUnitName
            CachedName = 0x64, // CGUnit_C__GetUnitName
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
            CachedName = 0xB4,
            CachedData0 = 0x14,
            CachedData1 = 0x18,
            CachedData8 = 0x34, // (Data0 + 8 * 0x04)
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xACCAE8,
            pvpExitWindow = 0xD26F30,
            selectedBattleGroundID = 0xD26ED4,
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            knownSpell = 0xD266D8,
            nbSpell = 0xD266D4,
        }

        /// <summary>
        ///   Chat
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xCCBD68,
            NextMessage = 0x17C0,
            msgFormatedChat = 0x3C,
            chatBufferPos = 0xD25300,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xC7CC90,
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
            CGUnit_C__GetFacing = 36,
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x40E510,
            FrameScript_ExecuteBuffer = 0x75AD0,
            CGPlayer_C__ClickToMove = 0x4B2750,
            ClntObjMgrGetActivePlayerObj = 0x33E0,
            FrameScript__GetLocalizedText = 0x4AB730,
            CGWorldFrame__Intersect = 0x721A80,
            Spell_C__HandleTerrainClick = 0x3FCCE0,
            Interact = 0x5A5E10,
        }

        /// <summary>
        ///   Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xCCA288,
            Y = X + 0x4,
            Z = Y + 0x4,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xBF8508 + 0x8,
            nameMaskOffset = 0x024,
            nameBaseOffset = 0x01c,
            nameStringOffset = 0x021,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xE285E8 + 0x6,
            battlerNetWindow = 0xC05F80,
        }

        /// <summary>
        ///   Active AutoLoot
        /// </summary>
        public enum AutoLoot
        {
            AutoLoot_Activate_Pointer = 0xCCA134,
            AutoLoot_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Active Auto Cast
        /// </summary>
        public enum AutoSelfCast
        {
            AutoSelfCast_Activate_Pointer = 0xCCA140,
            AutoSelfCast_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Active Auto Interact CTM
        /// </summary>
        public enum AutoInteract
        {
            AutoInteract_Activate_Pointer = 0xCCA114,
            AutoInteract_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            // TODO Retrieve unknown Quests offsets
            ActiveQuests = 0x0, // Not Update
            SelectedQuestId = 0x0, // Not Update
            TitleText = 0x0, // Not Update
            GossipQuests = 0x0, // Not Update
            GossipQuestNext = 0x0, // Not Update
        }

        /// <summary>
        ///   Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AURA_COUNT_1 = 0x1088,
            AURA_COUNT_2 = 0xD8C,
            AURA_TABLE_1 = 0xD88,
            AURA_TABLE_2 = 0xD90,
            AURA_SIZE = 0x30,
            AURA_SPELL_ID = 0x18,
            AURA_STACK = 0x1D, // ? // TODO Check for AURA_STACK offset
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xC7C91C,
            Multiplicator = 0x10,
        }
    }
}