namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 16650
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0xEADA68; // 0xEADA68
        }

        public enum ObjectManager
        {
            objectManager = 0x4830,
            firstObject = 0xCC, // Not found
            nextObject = 0x3C, // Not found
            localGuid = 0xD0, // Not found
            objectGUID = 0x30,
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
            PartyOffset = 0xDACA74,
            NumOfPlayers = 0xC4,
            NumOfPlayers_SuBGroup = 0xC8,
            PlayerGuid = 0x10,
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
            Spell = 0xC815D8,
            SpellCastTimes = 0xC81084,
            SpellRange = 0xC81580,
            SpellMisc = 0xC814A4,
            FactionTemplate = 0xC7F4B0,
            Lock = 0xC80500,
            LockType = 0xC8052C,
            Map = 0xC822D4,
            ResearchSite = 0xC80C0C,
            QuestPOIPoint = 0xC80A54,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xAD6814,
            buildWowVersion = 0xB7E6C4,
            gameState = 0xD4FF26,
            isLoadingOrConnecting = 0xC89A50,
            AreaId = 0xB4B94C,
            lastWowErrorMessage = 0xD4F328,
            zoneMap = 0xD4FF1C,
            subZoneMap = 0xD4FF18,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDBE660,
            playerName = 0xEADAA8,
            PlayerComboPoint = 0xD50009,
            RetrieveCorpseWindow = 0xD4FFDC,
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xDBFC18,
            startBar = 0xDC01BC, // STRUCTURE CHANGED ON 5.1.0.16309!
            nbBar = 0xDBFC18,
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
            CastingSpellID = 0xC60, // Script_UnitCastingInfo
            ChannelSpellID = 0xC78, // Script_UnitChannelInfo
            TransportGUID = 0x7F8, // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0x988, // CGUnit_C__GetUnitName
            CachedName = 0x78, // CGUnit_C__GetUnitName
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
            statPvp = 0xB55908,
            pvpExitWindow = 0xDAD168,
            selectedBattlegroundId = 0xDAD10C,
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            knownSpell = 0xDAC8DC,
            nbSpell = 0xDAC8D8,
        }

        /// <summary>
        ///   Chat
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD51D98,
            NextMessage = 0x17C8,
            msgFormatedChat = 0x3C,
            chatBufferPos = 0xDAB510,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xD01C00,
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
            CGUnit_C__GetFacing = 36, // Not found
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x44AF70,
            FrameScript_ExecuteBuffer = 0x75E10,
            CGPlayer_C__ClickToMove = 0x4EBF80,
            ClntObjMgrGetActivePlayerObj = 0x32B0,
            FrameScript__GetLocalizedText = 0x4E4B80,
            CGWorldFrame__Intersect = 0x75DC50,
            Spell_C__HandleTerrainClick = 0x4390D0,
            Interact = 0x5E2F20,
        }

        /// <summary>
        ///   Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD502B8,
            Y = X + 0x4,
            Z = Y + 0x4,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC7BEB0 + 0x8,
            nameMaskOffset = 0x024,
            nameBaseOffset = 0x01c,
            nameStringOffset = 0x021,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEADC30 + 0x6,
            battlerNetWindow = 0xC89A50,
        }

        /// <summary>
        ///   Active AutoLoot
        /// </summary>
        public enum AutoLoot
        {
            AutoLoot_Activate_Pointer = 0xD50160,
            AutoLoot_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Active Auto Cast
        /// </summary>
        public enum AutoSelfCast
        {
            AutoSelfCast_Activate_Pointer = 0xD5016C,
            AutoSelfCast_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Active Auto Interact CTM
        /// </summary>
        public enum AutoInteract
        {
            AutoInteract_Activate_Pointer = 0xD50140,
            AutoInteract_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            // TODO Retrieve unknown Quests offsets
            ActiveQuests = 0x0, // Not found
            SelectedQuestId = 0x0, // Not found
            TitleText = 0x0, // Not found
            GossipQuests = 0x0, // Not found
            GossipQuestNext = 0x0, // Not found
        }

        /// <summary>
        ///   Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AURA_COUNT_1 = 0x10C8,
            AURA_COUNT_2 = 0xDC8,
            AURA_TABLE_1 = 0xDC8,
            AURA_TABLE_2 = 0xDCC,
            AURA_SIZE = 0x30,
            AURA_SPELL_ID = 0x18,
            AURA_STACK = 0x1D, // Not found // ? // TODO Check for AURA_STACK offset
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xD0188C,
            Multiplicator = 0x10, // Not found
        }
    }
}