namespace nManager.Wow.Patchables
{
    /// <summary>
    ///   Offset and Pointer for Wow 18414
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        ///   ObjectManager
        /// </summary>
        public class ObjectManagerClass
        {
            public static uint clientConnection = 0xECA700;
            public static uint sCurMgr = 0xCADAF8; // It's the direct pointer to objectManager clientConn+objectManager
        }

        public enum ObjectManager
        {
            objectManager = 0x62C, // toFind // to be used with clientConnection or bypassed if using sCurMgr.
            localGuid = 0xF8, // ClntObjMgrGetActivePlayer_0 // E7B3A0 = localGUID complete?
            objectGUID = 0x28,
            objectTYPE = 0xC, // ClntObjMgrGetActivePlayer
            firstObject = 0xD8, // ClntObjMgrEnumVisibleObjects (?)
            nextObject = 0x3C,
        }

        /// <summary>
        ///   DirectX9
        /// </summary>
        public enum Hooking
        {
            DX_DEVICE = 0xC277E8, // ClientInitializeGame, first offset
            DX_DEVICE_IDX = 0x2854, // DX9_DEVICE_IDX_FOUND (93EA2)
            ENDSCENE_IDX = 0xA8,
        }

        /// <summary> Movement Flags</summary>
        /// [[base+offset1]+offset2]
        public enum MovementFlagsOffsets
        {
            Offset1 = 0x124, // near any movement flag
            Offset2 = 0x40,
        }

        public enum Party
        {
            PartyOffset = 0xDE604C, // Script_SendChatMessage First offset/4th block
            NumOfPlayers = 0xCC, // Script_GetNumGroupMembers
            NumOfPlayers_SuBGroup = 0xD0, // NumOFPlayers+4
            PlayerGuid = 0x10, // toCheck
        }

        /// <summary>
        ///   PetBattle
        /// </summary>
        public enum PetBattle
        {
            IsInBattle = 0xBA8A10, // LUA: Script_C_PetBattles_IsInBattle|Script_GetAuraInfo
        }

        /// <summary>
        ///   DBC
        /// </summary>
        public enum DBC
        {
            ItemClass = 0xC7F8D8,
            ItemSubClass = 0xC7FDF0, 
            FactionTemplate = 0xC7EDDC, // toCheck rows
            Lock = 0xC80234,
            QuestPOIPoint = 0xC8091C,
            ResearchSite = 0xC80B58,
        }

        /// <summary>
        ///   GameInfo offset
        /// </summary>
        public enum GameInfo
        {
            continentId = 0xB4D46C,
            continentIdOffset = 0xF0, // toFind // ?? mia
            buildWowVersion = 0xB94E74, // toFind // toUpdate mia yet
            gameState = 0xD8816E,
            isLoadingOrConnecting = 0xC8D198,
            AreaId = 0xCB4368, // toFind // toChecklel
            MapTextureId = 0xBACE38,
            zoneMap = 0xEB41B4, // toFind // toChecklel
            subZoneMap = 0xEB41B0, // toFind // toChecklel
            // saving
            TextBoxActivated = 0xBBE9AC, // toFind // toUpdate
            LastHardwareAction = 0xC27474,
        }

        /// <summary>
        ///   Player Offset
        /// </summary>
        public enum Player
        {
            petGUID = 0xDD4A00, // toFind // toUpdate
            playerName = 0xECA740, // ida: GetPlayerName
            PlayerComboPoint = 0xD65BF9, // toFind // toUpdate
            RetrieveCorpseWindow = 0xD65BCC, // toFind // toUpdate
            // Some offsets to refine descriptor
            SkillValue = 0x200, // toFind // toUpdate
            SkillMaxValue = 0x400, // toFind // toUpdate
            // saving
            RuneStartCooldown = 0xDFED0C, // Script_GetRuneCount
        }

        /// <summary>
        ///   EventsListener
        /// </summary>
        public enum EventsListener
        {
            EventsCount = 0xD35048, // toFind
            BaseEvents = EventsCount + 0x4, // toFind // toUpdate
            EventOffsetName = 0x14, // toFind // toUpdate
            EventOffsetCount = 0x48, // toFind // toUpdate
        }


        /// <summary>
        ///   Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_FIELD_X = 0xA50, // toFind // found with a ugly while
            UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // toFind
            UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // toFind
            UNIT_FIELD_R = UNIT_FIELD_X + 0x10, // toFind
            CastingSpellID = 0xF38, // Script_UnitCastingInfo
            ChannelSpellID = 0xF58, // Script_UnitChannelInfo
            CanInterrupt = 0xC64, // toFind // toUpdate // check // Script_UnitCastingInfo/Script_UnitChannelInfo
            TransportGUID = 0xA40, // CGUnit_C__HasVehicleTransport
            TaxiStatus = 0xC0, // toFind // toUpdate
            DBCacheRow = 0xBC4, // CGUnit_C__GetUnitName
            CachedSubName = 0x0, // beginning of DBCacheRow pointer = CachedSubName
            CachedName = 0x7C,
            CachedTypeFlag = 0x4C, // toFind, I can check memory if I have a demo-value ?
            CachedQuestItem1 = 0x30, // toFind, I can check memory if I have a demo-value ?
            CachedModelId1 = 0x5C, // toFind, I can check memory if I have a demo-value ?
            // saving
            UnitClassificationOffset1 = DBCacheRow, // toFind // Script_UnitClassification + 0x39 (CGUnit_C::GetCreatureRank)
            UnitClassificationOffset2 = 0x20, // toFind // toUpdate // Script_UnitClassification + 0x39 (CGUnit_C::GetCreatureRank)
            IsBossOffset1 = DBCacheRow,
            IsBossOffset2 = 0x5C, // function wow IsBoss() (or function at Script_UnitLevel + 0xB7)
        }

        /// <summary>
        ///   Game Object Descriptor
        /// </summary>
        public enum GameObject
        {
            GAMEOBJECT_FIELD_X = 0x2A0,
            GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4,
            GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8,
            PackedRotationQuaternion = 0x108, // toFind // toUpdate // I have no idea about what fct is behind this
            TransformationMatrice = 0x1C4, // toFind // toUpdate // CGGameObject_C::GetMatrix (fct name to be confirmed)
            DBCacheRow = 0x26C, // CGGameObject_C::GetName
            CachedIconName = 0x08, // toFind // toUpdate
            CachedCastBarCaption = 0x0C, // toFind // toUpdate
            CachedName = 0xB4, // CGGameObject_C__GetName_2
            CachedData0 = 0x14, // toFind // toUpdate // CGGameObject_C::GetLockRecord
            CachedSize = CachedData0 + (0x04 * 32), // just after the 32 data uint32
            CachedQuestItem1 = CachedSize + 0x04, // just after the size float
        }

        /// <summary>
        ///   Battleground
        /// </summary>
        public enum Battleground
        {
            StatPvp = 0xBACAFC, // Script_InActiveBattlefield_Sub
            PvpExitWindow = 0xDE7058, // Script_GetBattlefieldWinner
            MaxBattlegroundId = 0xDE7024, // Script_GetMaxBattlefieldID
        }

        /// <summary>
        ///   Fishing
        /// </summary>
        public enum Fishing
        {
            BobberHasMoved = 0x104,
        }

        /// <summary>
        ///   Spell Book
        /// </summary>
        public enum SpellBook
        {
            SpellBookNumSpells = 0xDE5D3C, // Script_SetSpellBookItem
            SpellBookSpellsPtr = SpellBookNumSpells + 0x4, // CGSpellBook__MakeKnownSpellModelsLocal
            MountBookNumMounts = 0xDE5D88 + 0x4 * 0x4, // Script_GetNumCompanions
            MountBookMountsPtr = MountBookNumMounts + 0x4,
        }

        /// <summary>
        ///   Chat // ida: ChatSystem
        /// </summary>
        public enum Chat
        {
            chatBufferStart = 0xD8A130,
            NextMessage = 0x17E8,
            msgFormatedChat = 0x65,
            chatBufferPos = 0xDE44AC,
        }

        /// <summary>
        ///   Click To Move
        /// </summary>
        public enum ClickToMove
        {
            CTM = 0xD2AF98, // GetClickToMoveStruct
            CTM_PUSH = CTM + 0x1C, // toFind
            CTM_X = CTM + 0x8C, // toFind
            CTM_Y = CTM_X + 0x4, // toFind
            CTM_Z = CTM_Y + 0x4, // toFind
        }

        /// <summary>
        ///   Virtual Function
        /// </summary>
        public enum VMT
        {
            CGUnit_C__GetFacing = 0x2C, // *4 > MovementGetTransportFacing
        }

        /// <summary>
        ///   Wow function addresses
        /// </summary>
        public enum FunctionWow
        {
            ClntObjMgrGetActivePlayer = 0x004B79DE, // made useless by 128bits GUID or it will crash injects
            ClntObjMgrGetActivePlayerObj = 0x00003994,
            FrameScript_ExecuteBuffer = 0x00023C19, 
            CGUnit_C__InitializeTrackingState = 0x002E525C, // alias CGPlayer_C__ClickToMove
            FrameScript__GetLocalizedText = 0x002D6AE0, 
            CGWorldFrame__Intersect = 0x004F905E, 
            Spell_C_HandleTerrainClick = 0x00245896, 
            CGUnit_C__Interact = 0x008FFE14, 
            // saving
            IsOutdoors = 0x0, // ?
            UnitCanAttack = 0x0, // ? 
        }

        /// <summary>
        ///   Corpse Player // ida: CGWorldMap::SetMapToCurrentZone
        /// </summary>
        public enum CorpsePlayer
        {
            X = 0xD884C8,
            Y = X + 0x4,
            Z = X + 0x8,
        }

        /// <summary>
        ///   Get Players name
        /// </summary>
        public enum PlayerNameStore
        {
            PlayerNameStorePtr = 0xC82390,
            PlayerNameNextOffset = 0x14,
            PlayerNameStringOffset = 0x11,
        }

        /// <summary>
        ///   Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xECA8E8 + 0x6,
        }

        /// <summary>
        ///   Activate some settings
        /// </summary>
        public enum ActivateSettings
        {
            AutoInteract_Activate_Pointer = 0xD882F4, // CGUnit_C__CanAutoInteract
            AutoDismount_Activate_Pointer = 0xD882FC, // CGUnit_C__CanAutoDismount
            AutoLoot_Activate_Pointer = 0xD88314, // CGGameUI__IsAutoLooting
            AutoSelfCast_Activate_Pointer = 0xD88324, // CGGame_UI__IsAutoSelfCast
            Activate_Offset = 0x34,
        }

        /// <summary>
        ///   Quest related
        /// </summary>
        public enum Quests
        {
            QuestGiverStatus = 0xF4,
            // TODO Retrieve unknown Quests offsets
            /*ActiveQuests = 0x0,
            SelectedQuestId = 0x0,
            TitleText = 0x0,
            GossipQuests = 0x0,
            GossipQuestNext = 0x0,*/
        }

        /// <summary>
        ///   Get Buff CGUnit_C__HasAura2
        /// </summary>
        public enum UnitBaseGetUnitAura
        {
            AuraTable1 = 0x10A0,
            AuraTable2 = 0x480,
            AuraStack = 0x24, // 0x24 or 0xC9 or ???? ( supposed to give the number of stack of the buff, 1 or more (example Shaman Enhancement increase Maelstrom Weapon stacks)
            AuraSpellStart = 0xC9, // 0x24 or 0xC9 or ???? (supposed to give the time at which it started???)
            AuraSpellId = 0x30,
            AuraSize = 0x48,

        }

        /// <summary>
        ///   Get Power Index
        /// </summary>
        public enum PowerIndex
        {
            PowerIndexArrays = 0xD2ABF4, // PowerTypePointer
            Multiplicator = 0x10, // toCheck
        }
    }
}