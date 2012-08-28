    public class Addresses
    {
        /// <summary>
        /// ObjectManager
        /// </summary>
        public enum ObjectManager
        {
            clientConnection = 0xFFFFFFFF,
            objectManager = 0xFFFFFFFF,
            firstObject = 0xC0, // ?
            nextObject = 0x3C, // ?
            localGuid = 0xC8, // ?
        }

        /// <summary>
        /// DirectX9
        /// </summary>
        public enum D3D9
        {
            DX_DEVICE = 0x1027904,
            DX_DEVICE_IDX = 0xFFFFFFFF,
            ENDSCENE_IDX = 0xA8, // ?
        }

        /// <summary>
        /// For directX 11 Wow
        /// </summary>
        public enum D3D11
        {
            DX_DEVICE = 0xFFFFFFFF,
            CreateDXGIFactory1 = 0x1027904,
            CreateDXGIFactory1Pt = 0x27F4, // ?
            CreateDXGIFactory1Vtable = 0x8, // ?
            startHook = 0x0, // ?
        }

        /// <summary>
        /// Is Swimming (Lua_IsSwimming) [[base+offset1]+offset2]
        /// </summary>
        public enum IsSwimming
        {
            flag = 0xFFFFFFFF,
            offset1 = 0xFFFFFFFF,
            offset2 = 0xFF,
        }

        /// <summary>
        /// Is Flying (Lua_IsFlying) [[base+offset1]+offset2]
        /// </summary>
        public enum IsFlying
        {
            flag = 0xFFFFFFFF,
            offset1 = 0xFFFFFFFF,
            offset2 = 0xFF,
        }

        /// <summary>
        /// GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            wowVersion = 0x1009B0C,
            gameState = 0xFFFFFFFF,
            isLoadingOrConnecting = 0xFFFFFFFF,
            continentId = 0xFFFFFFFF,
            AreaId = 0x12282F4,
            lastWowErrorMessage = 0xFFFFFFFF,
            zoneMap = 0x11CCF54,
            subZoneMap = 0xFFFFFFFF,
            consoleExecValue = 0xFFFFFFFF,
        }

        /// <summary>
        /// Player Offset
        /// </summary>
        public enum Player
        {
           LastTargetGUID = 0xFFFFFFFF,
           petGUID = 0xFFFFFFFF,
           playerName = 0xFFFFFFFF,
           PlayerComboPoint = 0x11CD031,
           RetrieveCorpseWindow = 0xFFFFFFFF,
        }

        /// <summary>
        /// Unit Relation
        /// </summary>
        public enum UnitRelation
        {
           FACTION_START_INDEX = 0xFFFFFFFF,
           FACTION_TOTAL =  FACTION_START_INDEX + 0x4,
           FACTION_POINTER =  FACTION_START_INDEX + 0xC,
           HOSTILE_OFFSET_1 = 0x14, // ?
           HOSTILE_OFFSET_2 = 0x0C, // ?
           FRIENDLY_OFFSET_1 = 0x10, // ?
           FRIENDLY_OFFSET_2 = 0x0C, // ?
        }

        /// <summary>
        /// Bar manager
        /// </summary>
        public enum BarManager
        {
           slotIsEnable = 0x123D750,
           startBar = 0x123DCF0,
           nbBar = 0xFFFFFFFF,
           nbBarOne = 0xFDC4A0,
           nextBar = 0x4,
        }

        /// <summary>
        /// Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
           UNIT_SPEED = 0x80C, // ?
           UNIT_FIELD_X = 0x790, // ?
           UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // ?
           UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // ?
           UNIT_FIELD_R = 0x7A0, // ?
           UNIT_FIELD_H = 0x8AC, // ?
           unitName1 = 0xFFFFFFFF, // CGUnit_C__GetUnitName + 0x142
           unitName2 = 0xFF, // CGUnit_C__GetUnitName + 0x15D
           CastingSpellID = 0xFFFFFFFF, // Script_UnitCastingInfo
           ChannelSpellID = 0xFFFFFFFF, // Script_UnitChannelInfo
           TransportGUID = 0x788, // CGUnit_C__GetTransportGUID+0
        }

        /// <summary>
        /// Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
           GAMEOBJECT_FIELD_X = 0x110, // ?
           GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4, // ?
           GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8, // ?
           GAMEOBJECT_CREATED_BY = 0x8 * 4, // ?
           objName1 = 0x1CC, // ?
           objName2 = 0xB4, // ?
        }

        /// <summary>
        /// Battleground
        /// </summary>
        public enum Battleground
        {
           statPvp = 0xFFFFFFFF, 
           pvpExitWindow = 0x1229F10, 
           selectedBattleGroundID = 0x1229EB4, 
        }

        /// <summary>
        /// Text box of the wow chat
        /// </summary>
        public enum TextBoxChat
        {
           baseBoxChat = 0x10272F8, 
           baseBoxChatPtr = 0x208, // ?
           statBoxChat = 0xFFFFFFFF, 
        }

        /// <summary>
        /// Spell Book
        /// </summary>
        public enum SpellBook
        {
           knownSpell = 0xFFFFFFFF, 
           nbSpell = 0x1229660, 
        }

        /// <summary>
        /// Chat
        /// </summary>
        public enum Chat
        {
           chatBufferStart = 0xFFFFFFFF, 
           NextMessage = 0xFFFFFFFF, 
           msgFormatedChat = 0xFFFFFFFF, 
           chatBufferPos = 0xFFFFFFFF, 
        }

        /// <summary>
        /// Click To Move
        /// </summary>
        public enum ClickToMove
        {
           CTM = 0xFFFFFFFF, 
           CTM_PUSH = CTM + 0x1C, // ?
           CTM_X = CTM + 0x8C, // ?
           CTM_Y = CTM_X + 0x4, // ?
           CTM_Z = CTM_Y + 0x4, // ?
        }

        /// <summary>
        /// Virtual Function
        /// </summary>
        public enum VMT
        {
           Interact = 45, // ?
           CGUnit_C__GetFacing = 13, // ?
        }

        /// <summary>
        /// Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
           ClntObjMgrGetActivePlayer = 0xFFFFFFFF, 
           FrameScript_ExecuteBuffer = 0xFFFFFFFF, 
           CGPlayer_C__ClickToMove = 0x940440, 
           ClntObjMgrGetActivePlayerObj = 0x404000, 
           FrameScript__GetLocalizedText = 0xFFFFFFFF, 
           CGWorldFrame__Intersect = 0xFFFFFFFF, 
           Spell_C__HandleTerrainClick = 0xFFFFFFFF, 
        }

        /// <summary>
        /// Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
           X = 0x11CD2D8, 
           Y = X + 0x4,
           Z = Y + 0x4,
        }

        /// <summary>
        /// Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
           nameStorePtr = 0xFFFFFFFF + 0x8, 
           nameMaskOffset = 0x024, // ?
           nameBaseOffset = 0x01c, // ?
           nameStringOffset = 0x020, // ?
        }

        /// <summary>
        /// Wow login addresses
        /// </summary>
        public enum Login
        {
           realmName = 0x132AC40 + 0x6, 
           battlerNetWindow = 0xFFFFFFFF, 
        }

        /// <summary>
        /// Active AutoLoot
        /// </summary>
        public enum AutoLoot
        {
           AutoLoot_Activate_Pointer = 0xFFFFFFFF, 
           AutoLoot_Activate_Offset = 0xFF, 
        }

        /// <summary>
        /// Active Auto Cast
        /// </summary>
        public enum AutoSelfCast
        {
           AutoSelfCast_Activate_Pointer = 0x11CD194, 
           AutoSelfCast_Activate_Offset = 0x30, 
        }

        /// <summary>
        /// Active Auto Interact CTM
        /// </summary>
        public enum AutoInteract
        {
           AutoInteract_Activate_Pointer = 0x11CD168, 
           AutoInteract_Activate_Offset = 0x30, 
        }

        /// <summary>
        /// Get Buff CGUnit_C__GetAura
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
           AURA_COUNT_1 = 0xFFFFFFFF, 
           AURA_COUNT_2 = 0xFFFFFFFF, 
           AURA_TABLE_1 = 0xFFFFFFFF, 
           AURA_TABLE_2 = 0xFFFFFFFF, 
           AURA_SIZE = 0x28, // ?
           AURA_SPELL_ID = 0x8,  // ?
           AURA_STACK = 0xE, // ?
        }

        /// <summary>
        /// Anti Warden
        /// </summary>
        public enum Warden
        {
           WardenClassPtr = 0x111E808, 
           injectionStart = 0xF,
        }

        /// <summary>
        /// Cheat addresse
        /// </summary>
        internal enum Cheat
        {
           NoSwim = 0x95B68B, 
           WaterWalk = 0x31, 
           fly1 = 0x38, 
           fly2 = 0x40, 
           fly3 = 0x4D, 
           NoClip = 0xFFFFFFFF, 
           speed = 0xFFFFFFFF, 
           OsGetAsyncTimeMs = 0x5907E0, 
           WallClimb1 = 0x15, 
           afk = 0xA2E56B, 
        }

        /// <summary>
        /// Anti Warden
        /// </summary>
        public class Warden2
        {
           public static uint WardenPtr2 = 0x8C14D0;
           public static uint WardenDec2 = 0xF;
        }

     }

