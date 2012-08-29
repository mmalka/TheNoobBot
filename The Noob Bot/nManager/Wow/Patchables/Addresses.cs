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
        public enum ObjectManager
        {
            clientConnection = 0xDC9298, //  OK
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
            slotIsEnable = 0xCDBCA0,// OK
            startBar = 0xCDC510,// OK
            nbBar = 0xCDBF70, // OK
            nextSlot = 0x4,// OK
        }

        /// <summary>
        /// Unit Field Descriptor
        /// </summary>
        public enum UnitField
        {
            UNIT_SPEED = 0x80C, // ?
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
            statPvp = 0x0, // Not Update
            pvpExitWindow = 0x0, // Not Update
            selectedBattleGroundID = 0x0, // Not Update
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
            CGUnit_C__GetFacing = 36, // ?
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
            nameStorePtr = 0xBB6C50 + 0x8, // ?
            nameMaskOffset = 0x024, // ?
            nameBaseOffset = 0x01c, // ?
            nameStringOffset = 0x020, // ?
        }

        /// <summary>
        /// Wow login addresses
        /// </summary>
        public enum Login
        {
            realmName = 0xDC9460 + 0x6, // OK
            battlerNetWindow = 0xBC4328, // ?
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
            AURA_STACK = 0xE, // ?
        }

    }



    /*
        /// <summary>
        /// Offset and Pointer for Wow 4.3.3 15354
        /// </summary>
        public class Addresses
        {
            /// <summary>
            /// ObjectManager
            /// </summary>
            public class ObjectManager
            {
                public static uint clientConnection = 0x0; //0x9BC9F8,
                public static uint objectManager = 0x463C;
                public static uint firstObject = 0xC0;
                public static uint nextObject = 0x3C;
                public static uint localGuid = 0xC8;
            }

            internal class Hooking
            {
                public static uint DX_DEVICE = 0xABD694;
                public static uint DX_DEVICE_IDX = 0x2800;
                public static uint ENDSCENE_IDX = 0xA8;
            }

            /// <summary>
            /// Is Swimming (Lua_IsSwimming) [[base+offset1]+offset2]
            /// </summary>
            public enum IsSwimming
            {
                flag = 0x100000,
                offset1 = 0x100,
                offset2 = 0x38,
            }

            /// <summary>
            /// Is Flying (Lua_IsFlying) [[base+offset1]+offset2]
            /// </summary>
            public enum IsFlying
            {
                flag = 0x1000000,
                offset1 = 0x100,
                offset2 = 0x38,
            }

            /// <summary>
            /// DBC
            /// </summary>
            public enum DBC
            {
                spell = 0x998600, // g_SpellDB
                SpellCastTimes = 0x9982D4, //  g_SpellCastTimesDB
                SpellRange = 0x9985AC, // g_SpellRangeDB
            }

            /// <summary>
            /// GameInfo offset
            /// </summary>
            public enum GameInfo
            {
                buildWowVersion = 0xAB242C,
                gameState = 0xAD5636,
                isLoadingOrConnecting = 0xAB9BC4,
                continentId = 0x8A1710,
                AreaId = 0xB30904,
                lastWowErrorMessage = 0xAD4A38,
                zoneMap = 0xAD562C,
                subZoneMap = 0xAD5628,
                consoleExecValue = 0xA4B578,
            }

            /// <summary>
            /// Player Offset
            /// </summary>
            public enum Player
            {
                LastTargetGUID = 0xAD5660,
                petGUID = 0xB41D68,
                playerName = 0x9BCA38,
                PlayerComboPoint = 0xAD5701,
                RetrieveCorpseWindow = 0xAD56D4,
            }

            /// <summary>
            /// Unit Relation
            /// </summary>
            public enum UnitRelation
            {
                FACTION_POINTER = 0x997328,
                FACTION_TOTAL = FACTION_POINTER + 0x4,
                FACTION_START_INDEX = FACTION_POINTER + 0xC,

                HOSTILE_OFFSET_1 = 0x14,
                HOSTILE_OFFSET_2 = 0x0C,
                FRIENDLY_OFFSET_1 = 0x10,
                FRIENDLY_OFFSET_2 = 0x0C,
            }

            /// <summary>
            /// Bar manager
            /// </summary>
            public enum BarManager
            {
                slotIsEnable = 0xB42010,
                startBar = 0xB42490,
                nbBar = 0xB426D0,
                nbBarOne = 0xB426D4,
                nextBar = 0x4,
            }

            /// <summary>
            /// Unit Field Descriptor
            /// </summary>
            public enum UnitField
            {
                UNIT_SPEED = 0x80C,
                UNIT_FIELD_X = 0x790,
                UNIT_FIELD_Y = UNIT_FIELD_X + 0x4,
                UNIT_FIELD_Z = UNIT_FIELD_X + 0x8,
                UNIT_FIELD_R = 0x7A0,
                UNIT_FIELD_H = 0x8AC,
                unitName1 = 0x91C, // CGUnit_C__GetUnitName + 0x142
                unitName2 = 0x64, // CGUnit_C__GetUnitName + 0x15D
                CastingSpellID = 0xA34, // Script_UnitCastingInfo
                ChannelSpellID = 0xA48, // Script_UnitChannelInfo
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
                statPvp = 0x910B38,
                pvpExitWindow = 0xB34968,
                selectedBattleGroundID = 0xB3490C,
            }

            /// <summary>
            /// Text box of the wow chat
            /// </summary>
            public enum TextBoxChat
            {
                baseBoxChat = 0x9D1C14,
                baseBoxChatPtr = 0x208, // ?
                statBoxChat = 0xAC4FE8,
            }

            /// <summary>
            /// Spell Book
            /// </summary>
            public enum SpellBook
            {
                knownSpell = 0xB31EA0,
                nbSpell = 0xB31E9C,
            }

            /// <summary>
            /// Chat
            /// </summary>
            public enum Chat
            {
                chatBufferStart = 0xAD7380,
                NextMessage = 0x17C0,
                msgFormatedChat = 0x3C,
                chatBufferPos = 0xB30918,
            }

            /// <summary>
            /// Click To Move
            /// </summary>
            public enum ClickToMove
            {
                CTM = 0x9D43D0,
                CTM_PUSH = CTM + 0x1C,
                CTM_X = CTM + 0x8C,
                CTM_Y = CTM_X + 0x4,
                CTM_Z = CTM_Y + 0x4,
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
                ClntObjMgrGetActivePlayer = 0x8D560,
                FrameScript_ExecuteBuffer = 0x43A810,
                CGPlayer_C__ClickToMove = 0x1C1A10,
                ClntObjMgrGetActivePlayerObj = 0x3200,
                FrameScript__GetLocalizedText = 0x1BB6E0,
                CGWorldFrame__Intersect = 0x321F60,
                Spell_C__HandleTerrainClick = 0x4B5570,
                Interact = 0x4D7480,
            }

            /// <summary>
            /// Corpse Player
            /// </summary>
            public enum CorpsePlayer
            {
                X = 0xAD599C,
                Y = X + 0x4,
                Z = Y + 0x4,
            }

            /// <summary>
            /// Get Players name
            /// </summary>
            public enum PlayerNameStore
            {
                nameStorePtr = 0x9962C0 + 0x8,
                nameMaskOffset = 0x024, // ?
                nameBaseOffset = 0x01c, // ?
                nameStringOffset = 0x020, // ?
            }

            /// <summary>
            /// Wow login addresses
            /// </summary>
            public enum Login
            {
                realmName = 0x9BCBC0 + 0x6,
                battlerNetWindow = 0xAB9BC4,
            }

            /// <summary>
            /// Active AutoLoot
            /// </summary>
            public enum AutoLoot
            {
                AutoLoot_Activate_Pointer = 0xAD5854,
                AutoLoot_Activate_Offset = 0x30,
            }

            /// <summary>
            /// Active Auto Cast
            /// </summary>
            public enum AutoSelfCast
            {
                AutoSelfCast_Activate_Pointer = 0xAD5860,
                AutoSelfCast_Activate_Offset = 0x30,
            }

            /// <summary>
            /// Active Auto Interact CTM
            /// </summary>
            public enum AutoInteract
            {
                AutoInteract_Activate_Pointer = 0xAD5834,
                AutoInteract_Activate_Offset = 0x30,
            }

            /// <summary>
            /// Get Buff CGUnit_C__GetAura
            /// </summary>
            public enum UnitBaseGetUnitAura
            {
                AURA_COUNT_1 = 0xE90,
                AURA_COUNT_2 = 0xC14,
                AURA_TABLE_1 = 0xC10,
                AURA_TABLE_2 = 0xC18,
                AURA_SIZE = 0x28, // ?
                AURA_SPELL_ID = 0x8,  // ?
                AURA_STACK = 0xF, // ?
            }
        }

        /// <summary>
        /// Offset and Pointer for Wow 4.3.2 15211 and 4.3.2 15211
        /// </summary>
        public class Addresses
        {
            /// <summary>
            /// ObjectManager
            /// </summary>
            public class ObjectManager
            {
                public static uint clientConnection = 0x0; //0x9BD030,
                public static uint objectManager = 0x463C;
                public static uint firstObject = 0xC0;
                public static uint nextObject = 0x3C;
                public static uint localGuid = 0xC8;
            }

            internal class Hooking
            {
                public static uint HookedFunction = 0x4A128A;
                public static uint startHook = 0x0; 
            }

            /// <summary>
            /// Is Swimming (Lua_IsSwimming) [[base+offset1]+offset2]
            /// </summary>
            public enum IsSwimming
            {
                flag = 0x100000,
                offset1 = 0x100,
                offset2 = 0x38,
            }

            /// <summary>
            /// Is Flying (Lua_IsFlying) [[base+offset1]+offset2]
            /// </summary>
            public enum IsFlying
            {
                flag = 0x1000000,
                offset1 = 0x100,
                offset2 = 0x38,
            }

            /// <summary>
            /// DBC
            /// </summary>
            public enum DBC
            {
                spell = 0x998C38, // g_SpellDB
                SpellCastTimes = 0x99890C, //  g_SpellCastTimesDB
                SpellRange = 0x998BE4, // g_SpellRangeDB
            }

            /// <summary>
            /// GameInfo offset
            /// </summary>
            public enum GameInfo
            {
                buildWowVersion = 0xAB2A64,
                gameState = 0xAD5C76,
                isLoadingOrConnecting = 0xABA1FC,
                continentId = 0x8A1710,
                AreaId = 0xB30F3C,
                lastWowErrorMessage = 0xAD5078,
                zoneMap = 0xAD5C6C,
                subZoneMap = 0xAD5C68,
                consoleExecValue = 0xA4BBB0,
            }

            /// <summary>
            /// Player Offset
            /// </summary>
            public enum Player
            {
                LastTargetGUID = 0xAD5CA0,
                petGUID = 0xB423A0,
                playerName = 0x9BD070,
                PlayerComboPoint = 0xAD5D41,
                RetrieveCorpseWindow = 0xAD5D14,
            }

            /// <summary>
            /// Unit Relation
            /// </summary>
            public enum UnitRelation
            {
                FACTION_START_INDEX = 0x99796C,
                FACTION_TOTAL = FACTION_START_INDEX - 0x4,
                FACTION_POINTER = FACTION_START_INDEX + 0xC,
                HOSTILE_OFFSET_1 = 0x14,
                HOSTILE_OFFSET_2 = 0x0C,
                FRIENDLY_OFFSET_1 = 0x10,
                FRIENDLY_OFFSET_2 = 0x0C,
            }

            /// <summary>
            /// Bar manager
            /// </summary>
            public enum BarManager
            {
                slotIsEnable = 0xB42648,
                startBar = 0xB42AC8,
                nbBar = 0xB42D08,
                nbBarOne = 0xB42D0C,
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
                unitName1 = 0x91C, // CGUnit_C__GetUnitName + 0x142
                unitName2 = 0x64, // CGUnit_C__GetUnitName + 0x15D
                CastingSpellID = 0xA34, // Script_UnitCastingInfo
                ChannelSpellID = 0xA48, // Script_UnitChannelInfo
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
                statPvp = 0x911140,
                pvpExitWindow = 0xB34FA0,
                selectedBattleGroundID = 0xB34F44, 
            }

            /// <summary>
            /// Text box of the wow chat
            /// </summary>
            public enum TextBoxChat
            {
                baseBoxChat = 0x9D224C,
                baseBoxChatPtr = 0x208, // ?
                statBoxChat = 0xAC5628, 
            }

            /// <summary>
            /// Spell Book
            /// </summary>
            public enum SpellBook
            {
                knownSpell = 0xB324D8,
                nbSpell = 0xB324D4, 
            }

            /// <summary>
            /// Chat
            /// </summary>
            public enum Chat
            {
                chatBufferStart = 0xAD79B8,
                NextMessage = 0x17C0,
                msgFormatedChat = 0x3C,
                chatBufferPos = 0xB30F50, 
            }

            /// <summary>
            /// Click To Move
            /// </summary>
            public enum ClickToMove
            {
                CTM = 0x9D4A08,
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
                ClntObjMgrGetActivePlayer = 0x8D3B0,
                FrameScript_ExecuteBuffer = 0x43B0A0,
                CGPlayer_C__ClickToMove = 0x1C1E00,
                ClntObjMgrGetActivePlayerObj = 0x3200,
                FrameScript__GetLocalizedText = 0x1BBAD0,
                CGWorldFrame__Intersect = 0x322850,
                Spell_C__HandleTerrainClick = 0x4B5DD0,
                Interact = 0x4D77B0,
            }

            /// <summary>
            /// Corpse Player
            /// </summary>
            public enum CorpsePlayer
            {
                X = 0xAD5FD4,
                Y = X + 0x4,
                Z = Y + 0x4,
            }

            /// <summary>
            /// Get Players name
            /// </summary>
            public enum PlayerNameStore
            {
                nameStorePtr = 0x9968F8 + 0x8, 
                nameMaskOffset = 0x024, // ?
                nameBaseOffset = 0x01c, // ?
                nameStringOffset = 0x020, // ?
            }

            /// <summary>
            /// Wow login addresses
            /// </summary>
            public enum Login
            {
                realmName = 0x9BD1F8 + 0x6,
                battlerNetWindow = 0xABA1FC, 
            }

            /// <summary>
            /// Active AutoLoot
            /// </summary>
            public enum AutoLoot
            {
                AutoLoot_Activate_Pointer = 0xAD5E8C,
                AutoLoot_Activate_Offset = 0x30, 
            }

            /// <summary>
            /// Active Auto Cast
            /// </summary>
            public enum AutoSelfCast
            {
                AutoSelfCast_Activate_Pointer = 0xAD5E98,
                AutoSelfCast_Activate_Offset = 0x30, 
            }

            /// <summary>
            /// Active Auto Interact CTM
            /// </summary>
            public enum AutoInteract
            {
                AutoInteract_Activate_Pointer = 0xAD5E6C,
                AutoInteract_Activate_Offset = 0x30, 
            }

            /// <summary>
            /// Get Buff CGUnit_C__GetAura
            /// </summary>
            public enum UnitBaseGetUnitAura
            {
                AURA_COUNT_1 = 0xE90,
                AURA_COUNT_2 = 0xC14,
                AURA_TABLE_1 = 0xC10,
                AURA_TABLE_2 = 0xC18, 
                AURA_SIZE = 0x28, // ?
                AURA_SPELL_ID = 0x8,  // ?
                AURA_STACK = 0xE, // ?
            }

            /// <summary>
            /// Anti AccountSecurity
            /// </summary>
            public enum AccountSecurity
            {
                AccountSecurityClassPtr = 0x9BF4C0, 
                injectionStart = 0xF,
            }

            /// <summary>
            /// Cheat addresse
            /// </summary>
            internal enum Cheat
            {
                NoSwim = 0x0,//0x1D9EEF,
                WaterWalk = 0x0,//0x31,
                fly1 = 0x0,//0x38,
                fly2 = 0x0,//0x40,
                fly3 = 0x0,//0x4D,
                NoClip = 0x0,//0x33194F,
                speed = 0x0,//0x17B232,
                OsGetAsyncTimeMs = 0x0,//0x478500,
                WallClimb1 = 0x0,//0x2018C6,
                afk = 0x4C449B,
            }
        }

        /*
        /// <summary>
        /// Offset and Pointer for Wow 4.3.0 15005 and 4.3.0a 15050
        /// </summary>
        public class Addresses
        {
            /// <summary>
            /// ObjectManager
            /// </summary>
            public class ObjectManager
            {
                public static uint clientConnection = 0x0; //0x9BE678,
                public static uint objectManager = 0x463C;
                public static uint firstObject = 0xC0;
                public static uint nextObject = 0x3C; // 3C ?
                public static uint localGuid = 0xC8;
            }

            internal class Hooking
            {
                public static uint HookedFunction = 0x4A298A; //
                public static uint startHook = 0x0; // ?
            }

            /// <summary>
            /// Is Swimming (Lua_IsSwimming) [[base+offset1]+offset2]
            /// </summary>
            public enum IsSwimming
            {
                flag = 0x100000,
                offset1 = 0x100,
                offset2 = 0x38,
            }

            /// <summary>
            /// Is Flying (Lua_IsFlying) [[base+offset1]+offset2]
            /// </summary>
            public enum IsFlying
            {
                flag = 0x1000000,
                offset1 = 0x100,
                offset2 = 0x38,
            }

            /// <summary>
            /// DBC
            /// </summary>
            public enum DBC
            {
                spell = 0x0099A280, // g_SpellDB
                SpellCastTimes = 0x999F54, //  g_SpellCastTimesDB
                SpellRange = 0x99A22C, // g_SpellRangeDB
            }

            /// <summary>
            /// GameInfo offset
            /// </summary>
            public enum GameInfo
            {
                buildWowVersion = 0xAB40A4,
                gameState = 0xAD7296,
                isLoadingOrConnecting = 0xABB83C,
                continentId = 0x8A26B0,
                AreaId = 0xB32554,
                lastWowErrorMessage = 0xAD6698,
                zoneMap = 0xAD728C,
                subZoneMap = 0xAD7288,
                consoleExecValue = 0xA4D1F8,
            }

            /// <summary>
            /// Player Offset
            /// </summary>
            public enum Player
            {
                LastTargetGUID = 0xAD72C0,
                petGUID = 0xB439B8,
                playerName = 0x9BE6B8,
                PlayerComboPoint = 0xAD7361,
                RetrieveCorpseWindow = 0xAD7334,
            }

            /// <summary>
            /// Unit Relation
            /// </summary>
            public enum UnitRelation
            {
                FACTION_START_INDEX = 0x998FA8,
                FACTION_TOTAL = FACTION_START_INDEX + 0x4,
                FACTION_POINTER = FACTION_START_INDEX + 0xC,
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
                slotIsEnable = 0xB43C60,
                startBar = 0xB440E0,
                nbBar = 0xB44320,
                nbBarOne = 0xB44324,
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
                unitName1 = 0x91C, // CGUnit_C__GetUnitName + 0x142
                unitName2 = 0x64, // CGUnit_C__GetUnitName + 0x15D
                CastingSpellID = 0xA34, // Script_UnitCastingInfo
                ChannelSpellID = 0xA48, // Script_UnitChannelInfo
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
                statPvp = 0x912770,
                pvpExitWindow = 0xB365B8,
                selectedBattleGroundID = 0xB3655C,
            }

            /// <summary>
            /// Text box of the wow chat
            /// </summary>
            public enum TextBoxChat
            {
                baseBoxChat = 0x9D3894,
                baseBoxChatPtr = 0x208, // ?
                statBoxChat = 0xAC6C58,
            }

            /// <summary>
            /// Spell Book
            /// </summary>
            public enum SpellBook
            {
                knownSpell = 0xB33AF0,
                nbSpell = 0xB33AEC,
            }

            /// <summary>
            /// Chat
            /// </summary>
            public enum Chat
            {
                chatBufferStart = 0xAD8FD0,
                NextMessage = 0x17C0,
                msgFormatedChat = 0x3C,
                chatBufferPos = 0xB32568,
            }

            /// <summary>
            /// Click To Move
            /// </summary>
            public enum ClickToMove
            {
                CTM = 0x9D6050,
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
                ClntObjMgrGetActivePlayer = 0x8D4E0,
                FrameScript_ExecuteBuffer = 0x43C010,
                CGPlayer_C__ClickToMove = 0x1C14E0,
                ClntObjMgrGetActivePlayerObj = 0x34B0,
                FrameScript__GetLocalizedText = 0x1BB0C0,
                CGWorldFrame__Intersect = 0x320D90,
                Spell_C__HandleTerrainClick = 0x4B7470,
                Interact = 0x4D8BA0,
            }

            /// <summary>
            /// Corpse Player
            /// </summary>
            public enum CorpsePlayer
            {
                X = 0xAD75E8,
                Y = X + 0x4,
                Z = Y + 0x4,
            }

            /// <summary>
            /// Get Players name
            /// </summary>
            public enum PlayerNameStore
            {
                nameStorePtr = 0x997F40 + 0x8,
                nameMaskOffset = 0x024, // ?
                nameBaseOffset = 0x01c, // ?
                nameStringOffset = 0x020, // ?
            }

            /// <summary>
            /// Wow login addresses
            /// </summary>
            public enum Login
            {
                realmName = 0x9BE840 + 0x6,
                battlerNetWindow = 0xABB83C,
            }

            /// <summary>
            /// Active AutoLoot
            /// </summary>
            public enum AutoLoot
            {
                AutoLoot_Activate_Pointer = 0xAD74A0,
                AutoLoot_Activate_Offset = 0x30,
            }

            /// <summary>
            /// Active Auto Cast
            /// </summary>
            public enum AutoSelfCast
            {
                AutoSelfCast_Activate_Pointer = 0xAD74AC,
                AutoSelfCast_Activate_Offset = 0x30,
            }

            /// <summary>
            /// Active Auto Interact CTM
            /// </summary>
            public enum AutoInteract
            {
                AutoInteract_Activate_Pointer = 0xAD7480,
                AutoInteract_Activate_Offset = 0x30,
            }

            /// <summary>
            /// Get Buff CGUnit_C__GetAura
            /// </summary>
            public enum UnitBaseGetUnitAura
            {
                AURA_COUNT_1 = 0xE90,
                AURA_COUNT_2 = 0xC14,
                AURA_TABLE_1 = 0xC10,
                AURA_TABLE_2 = 0xC18,
                AURA_SIZE = 0x28, // ?
                AURA_SPELL_ID = 0x8,  // ?
                AURA_STACK = 0xE, // ?
            }

            /// <summary>
            /// Anti AccountSecurity
            /// </summary>
            public enum AccountSecurity
            {
                AccountSecurityClassPtr = 0x9C0B08,
                injectionStart = 0xF,
            }

            /// <summary>
            /// Cheat addresse
            /// </summary>
            internal enum Cheat
            {
                NoSwim = 0x1D970F,
                WaterWalk = 0x204852,
                fly1 = 0x205519,
                fly2 = 0x620361,
                fly3 = 0x621E4E,
                NoClip = 0x32FF9F,
                speed = 0x17A8C2,
                OsGetAsyncTimeMs = 0x479C00,
                WallClimb1 = 0x200F16,
                afk = 0x4C5A7B,
            }
        }
        */
}
