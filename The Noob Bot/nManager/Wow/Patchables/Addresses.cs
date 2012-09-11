namespace nManager.Wow.Patchables
{
    /// <summary>
    /// Offset and Pointer for Wow 16048
    /// </summary>
    public class Addresses
    {
        /// <summary>
        /// ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; //  0xDC9598 OK
        }
        public enum ObjectManager
        {
            objectManager = 0x462C,
            firstObject = 0xCC,
            nextObject = 0x3C,
            localGuid = 0xD0,
        }

        /// <summary>
        /// DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xAD773C, // OK 
            DX_DEVICE_IDX = 0x27F8,
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary>
        /// Is Swimming (Script_IsSwimming) [[base+offset1]+offset2]
        /// </summary>
        public enum IsSwimming
        {
            flag = 0x100000,
            offset1 = 0xE4,
            offset2 = 0x38,
        }

        /// <summary>
        /// Is Flying (Script_IsFlying) [[base+offset1]+offset2]
        /// </summary>
        public enum IsFlying
        {
            flag = 0x1000000,
            offset1 = 0xE4,
            offset2 = 0x38,
        }

        /// <summary>
        /// DBC
        /// </summary>
        public enum DBC
        {
            Spell = 0xBBC40C, // OK
            SpellCastTimes = 0xBBBE8C, // OK
            SpellRange = 0xBBC388, // OK
            SpellMisc = 0xBBC2AC, // OK
            FactionTemplate = 0xBBA310, // OK
            Lock = 0xBBB360, // OK
            Map = 0xBBD104, // Futur usage // OK
            ResearchSite = 0xBBBA40, // Futur usage // OK
            QuestPOIPoint = 0xBBB888, // Futur usage // OK
        }

        /// <summary>
        /// GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            buildWowVersion = 0xAB9934, // OK
            gameState = 0xC6BBDE, // OK
            isLoadingOrConnecting = 0xBC4628, // OK
            continentId = 0xA15824, // OK
            AreaId = 0xC6BC74, // OK
            lastWowErrorMessage = 0xC6AFE0, // OK
            zoneMap = 0xC6BBD4, // OK
            subZoneMap = 0xC6BBD0, // OK
        }

        /// <summary>
        /// Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xCDADF8, // OK
            playerName = 0xDC95D8, // OK
            PlayerComboPoint = 0xC6BCB9, // OK
            RetrieveCorpseWindow = 0xC6BC8C, // OK
        }

        /// <summary>
        /// Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xCDC270,// OK
            startBar = 0xCDC810,// OK
            nbBar = 0xCDC270, // OK
            nextSlot = 0x4,
        }

        /// <summary>
        /// Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x850,
            UNIT_FIELD_X = 0x7E0,
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = 0x7F0,
            CastingSpellID = 0xC08, // Script_UnitCastingInfo
            ChannelSpellID = 0xC20, // Script_UnitChannelInfo
            TransportGUID = 0x7D8, // CGUnit_C__GetTransportGUID
            DBCacheRow = 0x968, // CGUnit_C__GetUnitName
            CachedName = 0x64, // CGUnit_C__GetUnitName
        }

        /// <summary>
        /// Game Object Descriptor
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
        /// Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xA8BE98, // OK
            pvpExitWindow = 0xCC8C30, // OK
            selectedBattleGroundID = 0xCC8BD4,  // OK
        }

        /// <summary>
        /// Spell Book
        /// </summary>
        public enum SpellBook
        {
            knownSpell = 0xCC8380, // OK
            nbSpell = 0xCC837C, // OK
        }

        /// <summary>
        /// Chat
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xC6DA40, // OK
            NextMessage = 0x17C0,
            msgFormatedChat = 0x3C,
            chatBufferPos = 0xCC6FD8, // OK
        }

        /// <summary>
        /// Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xC2BD04, // OK
            CTM_PUSH = CTM + 0x24,
            CTM_X = CTM + 0x8C,
            CTM_Y = CTM_X + 0x4,
            CTM_Z = CTM_Y + 0x4,
        }

        /// <summary>
        /// Virtual Function
        /// </summary>
        public enum VMT
        {
            CGUnit_C__GetFacing = 36,
        }

        /// <summary>
        /// Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x003FBB20, // OK
            FrameScript_ExecuteBuffer = 0x00075350, // OK
            CGPlayer_C__ClickToMove = 0x00493760, // OK
            ClntObjMgrGetActivePlayerObj = 0x000034D0, // OK
            FrameScript__GetLocalizedText = 0x0048D7F0, // OK
            CGWorldFrame__Intersect = 0x006ED070, // OK
            Spell_C__HandleTerrainClick = 0x3EA890, // OK
            Interact = 0x579B20, // OK
        }

        /// <summary>
        /// Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xC6BF60, // OK
            Y = X + 0x4,
            Z = Y + 0x4,
        }

        /// <summary>
        /// Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xBB6F50 + 0x8, // OK
            nameMaskOffset = 0x024,
            nameBaseOffset = 0x01c,
            nameStringOffset = 0x021,
        }

        /// <summary>
        /// Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xDC9760 + 0x6, // OK
            battlerNetWindow = 0xBC4628, // OK
        }

        /// <summary>
        /// Active AutoLoot
        /// </summary>
        public enum AutoLoot
        {
            AutoLoot_Activate_Pointer = 0xC6BE10, // OK
            AutoLoot_Activate_Offset = 0x30,
        }

        /// <summary>
        /// Active Auto Cast
        /// </summary>
        public enum AutoSelfCast
        {
            AutoSelfCast_Activate_Pointer = 0xC6BB1C, // OK
            AutoSelfCast_Activate_Offset = 0x30,
        }

        /// <summary>
        /// Active Auto Interact CTM
        /// </summary>
        public enum AutoInteract
        {
            AutoInteract_Activate_Pointer = 0xC6BDF0, // OK
            AutoInteract_Activate_Offset = 0x30,
        }

        /// <summary>
        /// Quest related
        /// </summary>
        public enum Quests
        {
			// TODO Retrieve unknown Quests offsets
            ActiveQuests = 0x0,  // Not Update
            SelectedQuestId = 0x0,  // Not Update
            TitleText = 0x0,  // Not Update
            GossipQuests = 0x0,  // Not Update
            GossipQuestNext = 0x0,  // Not Update
        }

        /// <summary>
        /// Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AURA_COUNT_1 = 0x1058,
            AURA_COUNT_2 = 0xD5C,
            AURA_TABLE_1 = 0xD58,
            AURA_TABLE_2 = 0xD60,
            AURA_SIZE = 0x30,
            AURA_SPELL_ID = 0x18,
            AURA_STACK = 0xE, // ? // TODO Check for AURA_STACK offset
        }

        /// <summary>
        /// Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xC2B98C, // OK
            Multiplicator = 0x10,
        }

    }
}
