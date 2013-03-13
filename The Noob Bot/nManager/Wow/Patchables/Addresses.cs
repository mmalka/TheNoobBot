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
            objectManager = 0x462C,
            firstObject = 0xCC,
            nextObject = 0x3C,
            localGuid = 0xD0,
            objectGUID = 0x30,
            objectTYPE = 0x10,
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xB9D4C4,
            DX_DEVICE_IDX = 0x2808,
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
            PartyOffset = 0xDADA74,
            NumOfPlayers = 0xC4,
            NumOfPlayers_SuBGroup = 0xC8,
            PlayerGuid = 0x10,
        }

        public enum PetBattle
        {
            IsInBattle = 0xB55080,
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            Spell = 0xC825D8,
            SpellCastTimes = 0xC82084,
            SpellRange = 0xC82580,
            SpellMisc = 0xC824A4,
            FactionTemplate = 0xC804B0,
            Lock = 0xC81500,
            LockType = 0xC8152C,
            Map = 0xC832D4,
            ResearchSite = 0xC81C0C,
            QuestPOIPoint = 0xC81A54,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xAD7814,
            buildWowVersion = 0xB7F6C4,
            gameState = 0xD50F26,
            isLoadingOrConnecting = 0xC8AA50,
            AreaId = 0xB56B60,
            lastWowErrorMessage = 0xD50328,
            zoneMap = 0xD50F1C,
            subZoneMap = 0xD50F18,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDBF660,
            playerName = 0xEAEAA8,
            PlayerComboPoint = 0xD51009,
            RetrieveCorpseWindow = 0xD50FDC,
            // Some offsets to refine descriptor
            SkillValue = 0x200,
            SkillMaxValue = 0x400,
        }

        /// <summary>
        ///   Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xDC0C18,
            startBar = 0xDC11BC,
            nbBar = 0xDC0C18,
            nextSlot = 0x4,
        }

        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x870,
            UNIT_FIELD_X = 0x800,
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10,
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
            statPvp = 0xB56908,
            pvpExitWindow = 0xDAE168,
            selectedBattlegroundId = 0xDAE10C,
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            knownSpell = 0xDAD8DC,
            nbSpell = 0xDAD8D8,
        }

        /// <summary>
        ///   Chat
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD52D98,
            NextMessage = 0x17C8,
            msgFormatedChat = 0x44,
            chatBufferPos = 0xDAC510,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xD02C00,
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
            CGUnit_C__GetFacing = 0x24,
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x44A5D0,
            FrameScript_ExecuteBuffer = 0x75910,
            CGPlayer_C__ClickToMove = 0x4EB200,
            ClntObjMgrGetActivePlayerObj = 0x32D0,
            FrameScript__GetLocalizedText = 0x4E3E00,
            CGWorldFrame__Intersect = 0x75D180,
            Spell_C__HandleTerrainClick = 0x438AD0,
            Interact = 0x5E23B0,
        }

        /// <summary>
        ///   Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD512B8,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xC7CEB0 + 0x8,
            nameMaskOffset = 0x024,
            nameBaseOffset = 0x18,
            nameStringOffset = 0x21,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xEAEC30 + 0x6,
            battlerNetWindow = 0xC8AA50,
        }

        /// <summary>
        ///   Active AutoLoot
        /// </summary>
        public enum AutoLoot
        {
            AutoLoot_Activate_Pointer = 0xD51160,
            AutoLoot_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Active Auto Cast
        /// </summary>
        public enum AutoSelfCast
        {
            AutoSelfCast_Activate_Pointer = 0xD5116C,
            AutoSelfCast_Activate_Offset = 0x30,
        }

        /// <summary>
        ///   Active Auto Interact CTM
        /// </summary>
        public enum AutoInteract
        {
            AutoInteract_Activate_Pointer = 0xD51140,
            AutoInteract_Activate_Offset = 0x30,
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
            AURA_COUNT_1 = 0x10C8,
            AURA_COUNT_2 = 0xDC8,
            AURA_TABLE_1 = 0xDC8,
            AURA_TABLE_2 = 0xDCC,
            AURA_SIZE = 0x30,
            AURA_SPELL_ID = 0x18,
            AURA_STACK = 0x1D, // Valid ??? // TODO Check for AURA_STACK offset
        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xD0288C,
            Multiplicator = 0x10,
        }
    }
}