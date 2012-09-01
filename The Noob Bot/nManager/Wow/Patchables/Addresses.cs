namespace nManager.Wow.Patchables
{
    /// <summary>
    /// Offset and Pointer for Wow 16016
    /// </summary>
    public class Addresses
    {
        /// <summary>
        /// ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0x0; //  OK (Server side defined: 0xDC9298)
        }
        public enum ObjectManager
        {
            objectManager = 0x462C, // OK
            firstObject = 0xCC, // OK
            nextObject = 0x3C, // OK
            localGuid = 0xD0, // OK
        }

        /// <summary>
        /// DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xAD743C, // OK
            DX_DEVICE_IDX = 0x27F8, // OK
            ENDSCENE_IDX = 0xA8, // OK
        }

        /// <summary>
        /// Is Swimming (Script_IsSwimming) [[base+offset1]+offset2]
        /// </summary>
        public enum IsSwimming
        {
            flag = 0x100000, // OK
            offset1 = 0xE4, // OK
            offset2 = 0x38, // OK
        }

        /// <summary>
        /// Is Flying (Script_IsFlying) [[base+offset1]+offset2]
        /// </summary>
        public enum IsFlying
        {
            flag = 0x1000000, // OK
            offset1 = 0xE4, // OK
            offset2 = 0x38, // OK
        }

        /// <summary>
        /// DBC
        /// </summary>
        public enum DBC
        {
            spell = 0xBBC10C, // OK
            SpellCastTimes = 0xBBBB8C, // OK
            SpellRange = 0xBBC088, // OK
            SpellMisc = 0xBBBFAC, // OK
            FactionTemplate = 0xBBA010, // OK
        }

        /// <summary>
        /// GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            buildWowVersion = 0xAB9634, // OK
            gameState = 0xC6B8DE, // OK
            isLoadingOrConnecting = 0xBC4328, // OK
            continentId = 0xA15824, // OK
            AreaId = 0xC6B974, // OK
            lastWowErrorMessage = 0xC6ACE0, // OK
            zoneMap = 0xC6B8D4, // OK
            subZoneMap = 0xC6B8D0, // OK
        }

        /// <summary>
        /// Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xCDAAF8,
            playerName = 0xDC92D8, // OK
            PlayerComboPoint = 0xC6B9B9,
            RetrieveCorpseWindow = 0xC6B98C, // OK
        }

        /// <summary>
        /// Bar manager
        /// </summary>
        public enum BarManager
        {
            slotIsEnable = 0xCDBF70,// OK
            startBar = 0xCDC510,// OK
            nbBar = 0xCDBF70, // OK
            nextSlot = 0x4,// OK
        }

        /// <summary>
        /// Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x850, // OK
            UNIT_FIELD_X = 0x7E0, // OK
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // OK
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // OK
            UNIT_FIELD_R = 0x7F0, // OK
            UNIT_FIELD_H = 0x8AC, // ?
            unitName1 = 0x968, // CGUnit_C__GetUnitName // OK
            unitName2 = 0x64, // CGUnit_C__GetUnitName // OK
            CastingSpellID = 0xC08, // Script_UnitCastingInfo // OK
            ChannelSpellID = 0xC20, // Script_UnitChannelInfo // OK
            TransportGUID = 0x7D8, // CGUnit_C__GetTransportGUID // OK
        }

        /// <summary>
        /// Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0xF0, // OK
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4, // OK
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8, // OK
            objName1 = 0x1B8, // OK
            objName2 = 0xB4, // OK
        }

        /// <summary>
        /// Battleground
        /// </summary>
        public enum Battleground
        {
            statPvp = 0xA8BC80, // OK
            pvpExitWindow = 0xCC8930, // OK
            selectedBattleGroundID = 0xCC88D4,  // OK
        }

        /// <summary>
        /// Spell Book
        /// </summary>
        public enum SpellBook
        {
            knownSpell = 0xCC8080, // OK
            nbSpell = 0xCC807C, // OK
        }

        /// <summary>
        /// Chat
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xC6D740, // OK
            NextMessage = 0x17C0, // OK
            msgFormatedChat = 0x3C, // OK
            chatBufferPos = 0xCC6CD8, // OK
        }

        /// <summary>
        /// Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xC2BA04, // OK
            CTM_PUSH = CTM + 0x24, // OK
            CTM_X = CTM + 0x8C, // OK
            CTM_Y = CTM_X + 0x4, // OK
            CTM_Z = CTM_Y + 0x4, // OK
        }

        /// <summary>
        /// Virtual Function
        /// </summary>
        public enum VMT
        {
            CGUnit_C__GetFacing = 36, // OK
        }

        /// <summary>
        /// Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x3FCE40, // OK
            FrameScript_ExecuteBuffer = 0x755A0, // OK
            CGPlayer_C__ClickToMove = 0x494AF0, // OK
            ClntObjMgrGetActivePlayerObj = 0x3390, // OK
            FrameScript__GetLocalizedText = 0x48EBC0, // OK
            CGWorldFrame__Intersect = 0x6ED710, // OK
            Spell_C__HandleTerrainClick = 0x3EBE40, // OK
            Interact = 0x57AD10, // OK
        }

        /// <summary>
        /// Corpse Player
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xC6BC60, // OK
            Y = X + 0x4, // OK
            Z = Y + 0x4, // OK
        }

        /// <summary>
        /// Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            nameStorePtr = 0xBB6C50 + 0x8, // OK
            nameMaskOffset = 0x024, // OK
            nameBaseOffset = 0x01c, // OK
            nameStringOffset = 0x021, // OK
        }

        /// <summary>
        /// Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xDC9460 + 0x6, // OK
            battlerNetWindow = 0xBC4328, // OK
        }

        /// <summary>
        /// Active AutoLoot
        /// </summary>
        public enum AutoLoot
        {
            AutoLoot_Activate_Pointer = 0xC6BB10, // OK
            AutoLoot_Activate_Offset = 0x30, // OK
        }

        /// <summary>
        /// Active Auto Cast
        /// </summary>
        public enum AutoSelfCast
        {
            AutoSelfCast_Activate_Pointer = AutoLoot.AutoLoot_Activate_Pointer - 0x4, // OK
            AutoSelfCast_Activate_Offset = 0x30, // OK
        }

        /// <summary>
        /// Active Auto Interact CTM
        /// </summary>
        public enum AutoInteract
        {
            AutoInteract_Activate_Pointer = 0xC6BAF0, // OK
            AutoInteract_Activate_Offset = 0x30, // OK
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
            AURA_COUNT_1 = 0x1058, // OK
            AURA_COUNT_2 = 0xD5C, // OK
            AURA_TABLE_1 = 0xD58, // OK
            AURA_TABLE_2 = 0xD60, // OK
            AURA_SIZE = 0x30, // OK
            AURA_SPELL_ID = 0x18,   // OK
            AURA_STACK = 0xE, // ? // TODO Check for AURA_STACK offset
        }

    }
}
