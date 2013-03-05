namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 16357
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xEADA68
        }

        public enum ObjectManager
        {
            objectManager = 0x462C, // Not found
            firstObject = 0xCC, // Not found
            nextObject = 0x3C, // Not found
            localGuid = 0xD0, // Not found
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xB9C4C,
            DX_DEVICE_IDX = 0x2808, // Not found
            ENDSCENE_IDX = 0xA8, // Not found
        }

        /// <summary>
        ///   Is Swimming (Script_IsSwimming) [[base+offset1]+offset2]
        /// </summary>
        public enum IsSwimming
        {
            flag = 0x100000, // Not found
            offset1 = 0xE4, // Not found
            offset2 = 0x38, // Not found
        }

        /// <summary>
        ///   Is Flying (Script_IsFlying) [[base+offset1]+offset2]
        /// </summary>
        public enum IsFlying
        {
            flag = 0x1000000, // Not found
            offset1 = 0xE4, // Not found
            offset2 = 0x38, // Not found
        }

        public enum Party
        {
            PartyOffset = 0xDACA74,
            NumOfPlayers = 0xC4, // Not found
            PlayerGuid = 0x10, // Not found
        }

        public enum PetBattle
        {
            IsInBattle = 0xDAC204, // LUA: C_PetBattles.IsInBattle
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            Spell = 0xBFDBC8, // Not found
            SpellCastTimes = 0xC81084,
            SpellRange = 0xBFDB44, // Not found
            SpellMisc = 0xBFDA68, // Not found
            FactionTemplate = 0xBFBACC, // Not found
            Lock = 0xBFCAF0, // Not found
            Map = 0xBFE8C4, // Not found
            ResearchSite = 0xBFD1D0, // Not found
            QuestPOIPoint = 0xBFD018, // Not found
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xA55824, // Not found
            buildWowVersion = 0xAFACDC, // Not found
            gameState = 0xCC9EFA, // Not found
            isLoadingOrConnecting = 0xC05F80, // Not found
            AreaId = 0xAC2B9C, // Not found
            lastWowErrorMessage = 0xCC92F8, // Not found
            zoneMap = 0xCC9EF0, // Not found
            subZoneMap = 0xCC9EEC, // Not found
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xD39520, // Not found
            playerName = 0xE28468, // Not found
            PlayerComboPoint = 0xCC9FDD, // Not found
            RetrieveCorpseWindow = 0xCC9FB0, // Not found
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xD3AAC0, // Not found
            startBar = 0xD3B064, // Not found // STRUCTURE CHANGED ON 5.1.0.16309!
            nbBar = 0xD3AAC0, // Not found
            nextSlot = 0x4, // Not found
        }

        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x858, // Not found
            UNIT_FIELD_X = 0x7E8, // Not found
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // Not found
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // Not found
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10, // Not found
            CastingSpellID = 0xC38, // Not found // Script_UnitCastingInfo
            ChannelSpellID = 0xC50, // Not found // Script_UnitChannelInfo
            TransportGUID = 0x7E0, // Not found // CGUnit_C__GetTransportGUID
            DBCacheRow = 0x970, // Not found // CGUnit_C__GetUnitName
            CachedName = 0x64, // Not found // CGUnit_C__GetUnitName
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0xF0, // Not found
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4, // Not found
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8, // Not found
            DBCacheRow = 0x1B8, // Not found
            CachedName = 0xB4, // Not found
            CachedData0 = 0x14, // Not found
            CachedData1 = 0x18, // Not found
            CachedData8 = 0x34, // Not found // (Data0 + 8 * 0x04)
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xACCAE8, // Not found
            pvpExitWindow = 0xD26F38, // Not found
            selectedBattlegroundId = 0xD26EDC, // Not found
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            knownSpell = 0xD266E0, // Not found
            nbSpell = 0xD266DC, // Not found
        }

        /// <summary>
        ///   Chat
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xCCBD70, // Not found
            NextMessage = 0x17C0, // Not found
            msgFormatedChat = 0x3C, // Not found
            chatBufferPos = 0xD25308, // Not found
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xC7CC90, // Not found
            CTM_PUSH = CTM + 0x1C, // Not found
            CTM_X = CTM + 0x8C, // Not found
            CTM_Y = CTM_X + 0x4, // Not found
            CTM_Z = CTM_Y + 0x4, // Not found
        }

        /// <summary>
        ///   Virtual Function
        /// </summary>
        public enum VMT
        {
            CGUnit_C__GetFacing = 36, // Not found
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x40E570, // Not found
            FrameScript_ExecuteBuffer = 0x75AC0, // Not found
            CGPlayer_C__ClickToMove = 0x4B26E0, // Not found
            ClntObjMgrGetActivePlayerObj = 0x33E0, // Not found
            FrameScript__GetLocalizedText = 0x4AB6A0, // Not found
            CGWorldFrame__Intersect = 0x721980, // Not found
            Spell_C__HandleTerrainClick = 0x3FCD50, // Not found
            Interact = 0x5A5DA0, // Not found
        }

        /// <summary>
        ///   Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xCCA28C, // Not found
            Y = X + 0x4, // Not found
            Z = Y + 0x4, // Not found
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xBF8508 + 0x8, // Not found
            nameMaskOffset = 0x024, // Not found
            nameBaseOffset = 0x01c, // Not found
            nameStringOffset = 0x021, // Not found
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xE285F0 + 0x6, // Not found
            battlerNetWindow = 0xC05F80, // Not found
        }

        /// <summary>
        ///   Active AutoLoot
        /// </summary>
        public enum AutoLoot
        {
            AutoLoot_Activate_Pointer = 0xCCA134, // Not found
            AutoLoot_Activate_Offset = 0x30, // Not found
        }

        /// <summary>
        ///   Active Auto Cast
        /// </summary>
        public enum AutoSelfCast
        {
            AutoSelfCast_Activate_Pointer = 0xCCA140, // Not found
            AutoSelfCast_Activate_Offset = 0x30, // Not found
        }

        /// <summary>
        ///   Active Auto Interact CTM
        /// </summary>
        public enum AutoInteract
        {
            AutoInteract_Activate_Pointer = 0xCCA114, // Not found
            AutoInteract_Activate_Offset = 0x30, // Not found
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            // TODO Retrieve unknown Quests offsets
            ActiveQuests = 0x0, // Not found // Not Update
            SelectedQuestId = 0x0, // Not found // Not Update
            TitleText = 0x0, // Not found // Not Update
            GossipQuests = 0x0, // Not found // Not Update
            GossipQuestNext = 0x0, // Not found // Not Update
        }

        /// <summary>
        ///   Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AURA_COUNT_1 = 0x1088, // Not found
            AURA_COUNT_2 = 0xD8C, // Not found
            AURA_TABLE_1 = 0xD88, // Not found
            AURA_TABLE_2 = 0xD90, // Not found
            AURA_SIZE = 0x30, // Not found
            AURA_SPELL_ID = 0x18, // Not found
            AURA_STACK = 0x1D, // Not found // ? // TODO Check for AURA_STACK offset
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xC7C91C, // Not found
            Multiplicator = 0x10, // Not found
        }
    }
}