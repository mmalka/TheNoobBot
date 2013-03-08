namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 16669
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; // 0xEAEA68
        }

        public enum ObjectManager
        {
            objectManager = 0x462C, // Not found
            firstObject = 0xCC, // Not found
            nextObject = 0x3C, // Not found
            localGuid = 0xD0, // Not found
            objectGUID = 0x30, // Not found
            objectTYPE = 0x10, // Not found
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xB9C4C4, // Not found
            DX_DEVICE_IDX = 0x2808, // Not found
            ENDSCENE_IDX = 0xA8, // Not found
        }

        /// <summary>
        ///   Is Falling (Script_IsFalling)
        /// </summary>
        public enum IsFalling
        {
            flag = 0x800, // Not found
            offset1 = 0xE4, // Not found
            offset2 = 0x38, // Not found
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
            PartyOffset = 0xDACA74, // Not found
            NumOfPlayers = 0xC4, // Not found
            NumOfPlayers_SuBGroup = 0xC8, // Not found
            PlayerGuid = 0x10, // Not found
        }

        public enum PetBattle
        {
            IsInBattle = 0xDAC204, // Not found
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            Spell = 0xC815D8, // Not found
            SpellCastTimes = 0xC81084, // Not found
            SpellRange = 0xC81580, // Not found
            SpellMisc = 0xC814A4, // Not found
            FactionTemplate = 0xC7F4B0, // Not found
            Lock = 0xC80500, // Not found
            LockType = 0xC8052C, // Not found
            Map = 0xC822D4, // Not found
            ResearchSite = 0xC80C0C, // Not found
            QuestPOIPoint = 0xC80A54, // Not found
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xAD6814, // Not found
            buildWowVersion = 0xB7E6C4, // Not found
            gameState = 0xD4FF26, // Not found
            isLoadingOrConnecting = 0xC89A50, // Not found
            AreaId = 0xB4B94C, // Not found
            lastWowErrorMessage = 0xD4F328, // Not found
            zoneMap = 0xD4FF1C, // Not found
            subZoneMap = 0xD4FF18, // Not found
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDBE660, // Not found
            playerName = 0xEADAA8, // Not found
            PlayerComboPoint = 0xD50009, // Not found
            RetrieveCorpseWindow = 0xD4FFDC, // Not found
            // Some offsets to refine descriptor
            SkillValue = 0x200, // Not found
            SkillMaxValue = 0x400, // Not found
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xDBFC18, // Not found
            startBar = 0xDC01BC, // Not found
            nbBar = 0xDBFC18, // Not found
            nextSlot = 0x4, // Not found
        }

        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x870, // Not found
            UNIT_FIELD_X = 0x800, // Not found
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // Not found
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // Not found
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10, // Not found
            CastingSpellID = 0xC60, // Not found // Script_UnitCastingInfo
            ChannelSpellID = 0xC78, // Not found // Script_UnitChannelInfo
            TransportGUID = 0x7F8, // Not found // CGUnit_C__HasVehicleTransport
            DBCacheRow = 0x988, // Not found // CGUnit_C__GetUnitName
            CachedName = 0x78, // Not found // CGUnit_C__GetUnitName
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0x1EC, // Not found
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4, // Not found
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8, // Not found
            DBCacheRow = 0x1B8, // Not found
            CachedName = 0xB0, // Not found
            CachedData0 = 0x14, // Not found
            CachedData1 = 0x18, // Not found
            CachedData8 = 0x34, // Not found // (Data0 + 8 * 0x04)
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xB55908, // Not found
            pvpExitWindow = 0xDAD168, // Not found
            selectedBattlegroundId = 0xDAD10C, // Not found
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            knownSpell = 0xDAC8DC, // Not found
            nbSpell = 0xDAC8D8, // Not found
        }

        /// <summary>
        ///   Chat
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD51D98, // Not found
            NextMessage = 0x17C8, // Not found
            msgFormatedChat = 0x3C, // Not found
            chatBufferPos = 0xDAB510, // Not found
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xD01C00, // Not found
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
            CGUnit_C__GetFacing = 0x24, // Not found
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x44AF70, // Not found
            FrameScript_ExecuteBuffer = 0x75E10, // Not found
            CGPlayer_C__ClickToMove = 0x4EBF80, // Not found
            ClntObjMgrGetActivePlayerObj = 0x32B0, // Not found
            FrameScript__GetLocalizedText = 0x4E4B80, // Not found
            CGWorldFrame__Intersect = 0x75DC50, // Not found
            Spell_C__HandleTerrainClick = 0x4390D0, // Not found
            Interact = 0x5E2F20, // Not found
        }

        /// <summary>
        ///   Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD502B8, // Not found
            Y = X + 0x4, // Not found
            Z = X + 0x8, // Not found
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC7BEB0 + 0x8, // Not found
            nameMaskOffset = 0x024, // Not found
            nameBaseOffset = 0x18, // Not found
            nameStringOffset = 0x21, // Not found
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEADC30 + 0x6, // Not found
            battlerNetWindow = 0xC89A50, // Not found
        }

        /// <summary>
        ///   Active AutoLoot
        /// </summary>
        public enum AutoLoot
        {
            AutoLoot_Activate_Pointer = 0xD50160, // Not found
            AutoLoot_Activate_Offset = 0x30, // Not found
        }

        /// <summary>
        ///   Active Auto Cast
        /// </summary>
        public enum AutoSelfCast
        {
            AutoSelfCast_Activate_Pointer = 0xD5016C, // Not found
            AutoSelfCast_Activate_Offset = 0x30, // Not found
        }

        /// <summary>
        ///   Active Auto Interact CTM
        /// </summary>
        public enum AutoInteract
        {
            AutoInteract_Activate_Pointer = 0xD50140, // Not found
            AutoInteract_Activate_Offset = 0x30, // Not found
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            // TODO Retrieve unknown Quests offsets
            ActiveQuests = 0x0, // Valid ???
            SelectedQuestId = 0x0, // Valid ???
            TitleText = 0x0, // Valid ???
            GossipQuests = 0x0, // Valid ???
            GossipQuestNext = 0x0, // Valid ???
        }

        /// <summary>
        ///   Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AURA_COUNT_1 = 0x10C8, // Not found
            AURA_COUNT_2 = 0xDC8, // Not found
            AURA_TABLE_1 = 0xDC8, // Not found
            AURA_TABLE_2 = 0xDCC, // Not found
            AURA_SIZE = 0x30, // Not found
            AURA_SPELL_ID = 0x18, // Not found
            AURA_STACK = 0x1D, // Not found // Valid ??? // TODO Check for AURA_STACK offset
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xD0188C, // Not found
            Multiplicator = 0x10, // Not found
        }
    }
}