#include <ida.idc>

static main()
{
	auto sPath, hFile;
	
	sPath = "C:\\Users\\Remi\\Desktop\\Addresses.cs";
	hFile = fopen( sPath, "w" );
	fprintf( hFile, "    public class Addresses\n    {\n");
	fprintf( hFile, "        /// <summary>\n        /// ObjectManager\n        /// </summary>\n        public enum ObjectManager\n        {\n" );
	
	
	fprintf( hFile, "            clientConnection = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "33 C0 8B 0D ? ? ? ? D9 EE" ) + 0x4 ) );
	fprintf( hFile, "            objectManager = 0x462C, // ?\n" );
	fprintf( hFile, "            firstObject = 0xC0, // ?\n" );
	fprintf( hFile, "            nextObject = 0x3C, // ?\n" );
	fprintf( hFile, "            localGuid = 0xC8, // ?\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// DirectX9\n        /// </summary>\n        public enum Hooking\n        {\n" );
	fprintf( hFile, "            DX_DEVICE = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 0D ? ? ? ? 68 ? ? ? ? 6A 00 E8 ? ? ? ? 8B 0D ? ? ? ? 68" ) + 0x2 ) );
	fprintf( hFile, "            DX_DEVICE_IDX = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "83 B9 ? ? ? ? ? 74 3D 8B 81 ? ? ? ? 8B 08 8B 51 14 50 FF D2 85 C0 79 2B " ) + 0x2 ) );
	fprintf( hFile, "            ENDSCENE_IDX = 0xA8, // ?\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Is Swimming (Script_IsSwimming) [[base+offset1]+offset2]\n        /// </summary>\n        public enum IsSwimming\n        {\n" );
	
	fprintf( hFile, "            flag = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 1B" ) + 0x9 ) );
	fprintf( hFile, "            offset1 = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 1B" ) + 0x2 ) );
	fprintf( hFile, "            offset2 = 0x%X,\n", Byte( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 1B" ) + 0x8 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Is Flying (Script_IsFlying) [[base+offset1]+offset2]\n        /// </summary>\n        public enum IsFlying\n        {\n" );
	
	fprintf( hFile, "            flag = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 45" ) + 0x9 ) );
	fprintf( hFile, "            offset1 = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 45" ) + 0x2 ) );
	fprintf( hFile, "            offset2 = 0x%X,\n", Byte( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 45" ) + 0x8 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// DBC\n        /// </summary>\n        public enum DBC\n        {\n" );
	
	fprintf( hFile, "            spell = 0x%X, // g_SpellDB\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 41 D9 E8 83 EC 08 DD 1C 24 56" ) + 0x9 ) );
	fprintf( hFile, "            SpellCastTimes = 0x%X, //  g_SpellCastTimesDB\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 41 D9 E8 83 EC 08 DD 1C 24 56" ) + 0x2 ) );
	fprintf( hFile, "            SpellRange = 0x%X, // g_SpellRangeDB\n", Byte( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 41 D9 E8 83 EC 08 DD 1C 24 56" ) + 0x8 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// GameInfo offset\n        /// </summary>\n        public enum GameInfo\n        {\n" );
	
	fprintf( hFile, "            buildWowVersion = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 45 18 68 00 08 00 00 50 68 ? ? ? ? 89 15 ? ? ? ? E8 ? ? ? ?" ) + 0x10 ) );
	fprintf( hFile, "            gameState = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 83 EC 08 53 33 DB 38 1D ? ? ? ? 0F 84 ? ? ? ?" ) + 0xB ) );
	fprintf( hFile, "            isLoadingOrConnecting = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 81 EC ? ? ? ? 0F 57 C0 8B 0D ? ? ? ? 53 33 DB 89 1D ? ? ? ? 89 1D ? ? ? ? 89 1D ? ? ? ? 89 1D ? ? ? ? 89 1D ? ? ? ? 89 1D ? ? ? ? 89 1D ? ? ? ? 89 1D ? ? ? ? 89 1D ? ? ? ? F3 0F 11 45 ? F3 0F 11 45 ? F3 0F 11 45 ? F3 0F 11 45 ? 8B 01" ) + 0x23 ) );
	fprintf( hFile, "            continentId = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC A1 ? ? ? ? 81 EC ? ? ? ? 53 56 50 B9 ? ? ? ? E8 ? ? ? ? 33 DB 88 1D ? ? ? ?" ) + 0x4 ) );
	fprintf( hFile, "            AreaId = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "A1 ? ? ? ? 8B 0D ? ? ? ? 50 51 E8 ? ? ? ? 83 C4 08 84 C0 74 19" ) + 0x1 ) );
	fprintf( hFile, "            lastWowErrorMessage = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "6A 00 6A FF 56 E8 ? ? ? ? 83 C4 0C 8D" ) + 0x24 ) );
	fprintf( hFile, "            zoneMap = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC A1 ? ? ? ? 85 C0 75 05 B8 ? ? ? ? 50 8B 45 08 50 E8 ? ? ? ?" ) + 0x4 ) );
	fprintf( hFile, "            subZoneMap = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "68 ? ? ? ? 68 ? ? ? ? 56 E8 ? ? ? ? 83 C4 0C A3 ? ? ? ? EB 11" ) + 0x14 ) );
	fprintf( hFile, "            consoleExecValue = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 53 56 8B 75 08 57 BF ? ? ? ? BB ? 00 00 00 8D 45 08 50 56" ) + 0xA ) );

	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Player Offset\n        /// </summary>\n        public enum Player\n        {\n" );
	
	fprintf( hFile, "           LastTargetGUID = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 83 EC 08 A1 ?? ?? ?? ?? 8B 0D ?? ?? ?? ?? 8B D0 0B D1 89 45 F8 89 4D FC 74 4E 56" ) + 0x7 ) );
	fprintf( hFile, "           petGUID = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 0D ? ? ? ? 8B 15 ? ? ? ? 33 C0 A3 ? ? ? ? A3 ? ? ? ? A3 ? ? ? ? A3 ? ? ? ? A3 ? ? ? ? A3 ? ? ? ? A2 ? ? ? ? A2 ? ? ? ?" ) + 0xF ) );
	fprintf( hFile, "           playerName = 0x%X,\n", Dword( FindBinary( MAXADDR, SEARCH_UP, "0F BE 05 ? ? ? ? F7 D8 1B C0 25 ? ? ? ? C3" ) + 0x3 ) );
	fprintf( hFile, "           PlayerComboPoint = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 08 89 0D ? ? ? ? 8B 50 04 8A 45 0C 89 15 ? ? ? ? A2 ? ? ? ? E8" ) + 0x15 ) );
	fprintf( hFile, "           RetrieveCorpseWindow = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC E8 ? ? ? ? 03 45 08 A3 ? ? ? ? 75 0A" ) + 0x1E ) );

	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Unit Relation\n        /// </summary>\n        public enum UnitRelation\n        {\n" );
	
	fprintf( hFile, "           FACTION_POINTER = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 81 ? ? ? ? 8B 80 ? ? ? ? 50 B9 ? ? ? ? E8 ? ? ? ? 8B 40 0C C3" ) + 0xE ) );
	fprintf( hFile, "           FACTION_TOTAL =  FACTION_POINTER + 0x4,\n" );
	fprintf( hFile, "           FACTION_START_INDEX =  FACTION_POINTER + 0xC,\n" );
	fprintf( hFile, "           HOSTILE_OFFSET_1 = 0x14, // ?\n" );
	fprintf( hFile, "           HOSTILE_OFFSET_2 = 0x0C, // ?\n" );
	fprintf( hFile, "           FRIENDLY_OFFSET_1 = 0x10, // ?\n" );
	fprintf( hFile, "           FRIENDLY_OFFSET_2 = 0x0C, // ?\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Bar manager\n        /// </summary>\n        public enum BarManager\n        {\n" );
	
	fprintf( hFile, "           slotIsEnable = 0x%X,\n", Dword( FindBinary( MAXADDR, SEARCH_UP, "55 8B EC 51 56 57 33 FF 33 F6 8D 9B ? ? ? ?" ) + 0x23 ) );
	fprintf( hFile, "           startBar = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 45 08 8B 04 85 ? ? ? ? 85 C0 74 0E A9 00 00 00 F0 75 07" ) + 0x9 ) );
	fprintf( hFile, "           nbBar = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 56 57 8B 7D 08 6A 01 57 E8 ? ? ? ? 83 C4 08 E8 ? ? ? ? 8D 70 FF 83 FE 05" ) + 0x30 ) );
	fprintf( hFile, "           nbBarOne = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 45 08 85 C0 78 29 3D ? ? ? ? 73 22" ) + 0x14 ) );
	fprintf( hFile, "           nextBar = 0x4,\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Unit Field Descriptor\n        /// </summary>\n        public enum UnitField\n        {\n" );
	
	fprintf( hFile, "           UNIT_SPEED = 0x80C, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_X = 0x790, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_R = 0x7A0, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_H = 0x8AC, // ?\n" );
	fprintf( hFile, "           unitName1 = 0x91C, // CGUnit_C__GetUnitName + 0x142 ?\n" );
	fprintf( hFile, "           unitName2 = 0x64, // CGUnit_C__GetUnitName + 0x15D ?\n" );
	fprintf( hFile, "           CastingSpellID = 0xC08, // Script_UnitCastingInfo ?\n" );
	fprintf( hFile, "           ChannelSpellID = 0xC20, // Script_UnitChannelInfo ?\n" );
	fprintf( hFile, "           TransportGUID = 0x788, // CGUnit_C__GetTransportGUID+0 ?\n" );

	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Game Object Descriptor\n        /// </summary>\n        public enum GameObject\n        {\n" );
	
	fprintf( hFile, "           GAMEOBJECT_FIELD_X = 0x110, // ?\n" );
	fprintf( hFile, "           GAMEOBJECT_FIELD_Y = GAMEOBJECT_FIELD_X + 0x4, // ?\n" );
	fprintf( hFile, "           GAMEOBJECT_FIELD_Z = GAMEOBJECT_FIELD_X + 0x8, // ?\n" );
	fprintf( hFile, "           GAMEOBJECT_CREATED_BY = 0x8 * 4, // ?\n" );
	fprintf( hFile, "           objName1 = 0x1CC, // ?\n" );
	fprintf( hFile, "           objName2 = 0xB4, // ?\n" );

	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Battleground\n        /// </summary>\n        public enum Battleground\n        {\n" );
	
	fprintf( hFile, "           statPvp = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC A1 ? ? ? ? 83 EC 14 A8 01 75 04" ) + 0x4 ) );
	fprintf( hFile, "           pvpExitWindow = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 51 83 3D ? ? ? ? 00 75 15 8B 45 08 50 E8 ? ? ? ? 83 C4 04" ) + 0x6 ) );
	fprintf( hFile, "           selectedBattleGroundID = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "E8 ? ? ? ? 8B 16 8D 4D EC 89 15 ? ? ? ? E8 ? ? ? ? 5E 8B E5" ) + 0xC ) );

	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Text box of the wow chat\n        /// </summary>\n        public enum TextBoxChat\n        {\n" );
	
	fprintf( hFile, "           baseBoxChat = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "56 8B F1 E8 ? ? ? ? 83 3D ? ? ? ? 00 75 12 F6 86 ? ? 00 00 01" ) + 0xA ) );
	fprintf( hFile, "           baseBoxChatPtr = 0x208, // ?\n" );
	fprintf( hFile, "           statBoxChat = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 6A 00 E8 ? ? ? ? 83 C4 04 83 7D 08 00 74 21" ) + 0x15 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Spell Book\n        /// </summary>\n        public enum SpellBook\n        {\n" );

	fprintf( hFile, "           knownSpell = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 51 53 56 33 DB 53 68 ? ? ? ? 68 ? ? ? ? 6A 14 E8 ? ? ? ? 3B C3" ) + 0x3A ) );
	fprintf( hFile, "           nbSpell = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8D 45 F8 50 E8 ? ? ? ? 33 DB 33 FF 89 7D FC 39 1D ? ? ? ? 0F 86 ? ? ? ?" ) + 0x12 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Chat\n        /// </summary>\n        public enum Chat\n        {\n" );

	fprintf( hFile, "           chatBufferStart = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 45 08 03 05 ? ? ? ? B9 ? ? ? ? 99 F7 F9 8B C2 69 C0 ? ? ? ? 05 ? ? ? ? 5D C3" ) + 0x1D ) );
	fprintf( hFile, "           NextMessage = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 45 08 03 05 ? ? ? ? B9 ? ? ? ? 99 F7 F9 8B C2 69 C0 ? ? ? ? 05 ? ? ? ? 5D C3" ) + 0x18 ) );
	fprintf( hFile, "           msgFormatedChat = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 45 08 03 05 ? ? ? ? B9 ? ? ? ? 99 F7 F9 8B C2 69 C0 ? ? ? ? 05 ? ? ? ? 5D C3" ) + 0xD ) );
	fprintf( hFile, "           chatBufferPos = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 45 08 03 05 ? ? ? ? B9 ? ? ? ? 99 F7 F9 8B C2 69 C0 ? ? ? ? 05 ? ? ? ? 5D C3" ) + 0x8 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Click To Move\n        /// </summary>\n        public enum ClickToMove\n        {\n" );

	fprintf( hFile, "           CTM = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "D9 1D ? ? ? ? 83 C4 08 6A 01 53 56 6A 01 68 ? ? ? ? E8 ? ? ? ? 8B C8 E8 ? ? ? ?" ) + 0x2 ) );
	fprintf( hFile, "           CTM_PUSH = CTM + 0x1C, // ?\n" );
	fprintf( hFile, "           CTM_X = CTM + 0x8C, // ?\n" );
	fprintf( hFile, "           CTM_Y = CTM_X + 0x4, // ?\n" );
	fprintf( hFile, "           CTM_Z = CTM_Y + 0x4, // ?\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Virtual Function\n        /// </summary>\n        public enum VMT\n        {\n" );

	fprintf( hFile, "           Interact = 45, // ?\n" );
	fprintf( hFile, "           CGUnit_C__GetFacing = 13, // ?\n" );		  
        
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Wow function addresses\n        /// </summary>\n        public enum FunctionWow\n        {\n" );
    
    fprintf( hFile, "           ClntObjMgrGetActivePlayer = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 0D ? ? ? ? 85 C9 75 05 33 C0 33 D2 C3 8B 81 ? ? ? ?" ) + 0x0 );  
	fprintf( hFile, "           FrameScript_ExecuteBuffer = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 51 FF 05 ? ? ? ? A1 ? ? ? ? 89 45 FC 74 12" ) + 0x0 );  
	fprintf( hFile, "           CGPlayer_C__ClickToMove = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 83 EC 18 53 56 8B F1 8B 46 08 8B 08 57 3B 0D ? ? ? ?" ) + 0x0 ); 
	fprintf( hFile, "           ClntObjMgrGetActivePlayerObj = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "E8 ? ? ? ? 68 ? ? ? ? 68 ? ? ? ? 6A 10 52 50 E8 ? ? ? ? 83 C4 14 C3" ) + 0x0 ); 
	fprintf( hFile, "           FrameScript__GetLocalizedText = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 86 ? ? ? ? 0F B6 40 20 EB 0A" ) - 0x1D ); 
	fprintf( hFile, "           CGWorldFrame__Intersect = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 55 0C D9 02 56 8B 75 08 D8 26 D9 42 04 D8 66 04" ) + 0x0 ); 
	fprintf( hFile, "           Spell_C__HandleTerrainClick = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 45 08 8B 50 04 8D 48 08 8B 00 51 52 50 E8 ? ? ? ? 83 C4 0C 5D" ) + 0x0 ); 
	fprintf( hFile, "           Interact = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC E8 ? ? ? ? 68 ? ? ? ? 68 ? ? ? ? 6A 10 52 50 E8 ? ? ? ? 83 C4 14 85 C0 75 07 B8 01" ) + 0x0 );
	
	fprintf( hFile, "        }\n\n" );

	fprintf( hFile, "        /// <summary>\n        /// Corpse Player\n        /// </summary>\n        public enum CorpsePlayer\n        {\n" );
    
	fprintf( hFile, "           X = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "89 0D ? ? ? ? 8B 50 04 8B 4D 10" ) + 0x2 ) );
	fprintf( hFile, "           Y = X + 0x4,\n" );
	fprintf( hFile, "           Z = Y + 0x4,\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Get Players name\n        /// </summary>\n        public enum PlayerNameStore\n        {\n" );
    
	fprintf( hFile, "           nameStorePtr = 0x%X + 0x8, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "56 B9 ? ? ? ? E8 ? ? ? ? 8B 4E 44" ) + 0x2 ) );
	fprintf( hFile, "           nameMaskOffset = 0x024, // ?\n" );
	fprintf( hFile, "           nameBaseOffset = 0x01c, // ?\n" );
	fprintf( hFile, "           nameStringOffset = 0x020, // ?\n" );
	
	 
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Wow login addresses\n        /// </summary>\n        public enum Login\n        {\n" );
    
	fprintf( hFile, "           realmName = 0x%X + 0x6, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "50 81 E6 ? ? ? ?" ) + 0x3 ) );
	fprintf( hFile, "           battlerNetWindow = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "83 3D ? ? ? ? ? 75 1A 6A 02" ) + 0x2 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Active AutoLoot\n        /// </summary>\n        public enum AutoLoot\n        {\n" );
    
	fprintf( hFile, "           AutoLoot_Activate_Pointer = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "75 0F 8B 15 ? ? ? ? 8B 42 30" ) + 0x4 ) );
	fprintf( hFile, "           AutoLoot_Activate_Offset = 0x30, \n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Active Auto Cast\n        /// </summary>\n        public enum AutoSelfCast\n        {\n" );
    
	fprintf( hFile, "           AutoSelfCast_Activate_Pointer = AutoLoot.AutoLoot_Activate_Pointer - 0x4, \n" );
	fprintf( hFile, "           AutoSelfCast_Activate_Offset = 0x30, \n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Active Auto Interact CTM\n        /// </summary>\n        public enum AutoInteract\n        {\n" );
    
	fprintf( hFile, "           AutoInteract_Activate_Pointer = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "F3 0F 11 86 ? ? ? ? A1 ? ? ? ?" ) + 0x9 ) );
	fprintf( hFile, "           AutoInteract_Activate_Offset = 0x30, \n" );

	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Quest related\n        /// </summary>\n        public enum Quests\n        {\n" );
    
	fprintf( hFile, "           ActiveQuests = 0x274, \n" );
	fprintf( hFile, "           SelectedQuestId = 0x%X + 0x78, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "B8 ? ? ? ? BE ? ? ? ? 74 11" ) + 0x1 ) );
	fprintf( hFile, "           TitleText = 0x%X + 0x658, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "B8 ? ? ? ? BE ? ? ? ? 74 11" ) + 0x1 ) );
	fprintf( hFile, "           GossipQuests = 0x%X + 0x1E58, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "83 C4 20 88 1D ? ? ? ?" ) + 0x5 ) );
	fprintf( hFile, "           GossipQuestNext = 0x214, // ?\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Get Buff CGUnit_C__GetAura\n        /// </summary>\n        public enum UnitBaseGetUnitAura\n        {\n" );
    
	fprintf( hFile, "           AURA_COUNT_1 = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 81 ? ? 00 00 83 F8 FF 75 08 8B 91 ? ? 00 00 EB 02 8B D0" ) + 0x5 ) );
	fprintf( hFile, "           AURA_COUNT_2 = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 81 ? ? 00 00 83 F8 FF 75 08 8B 91 ? ? 00 00 EB 02 8B D0" ) + 0x10 ) );
	fprintf( hFile, "           AURA_TABLE_1 = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 81 ? ? 00 00 83 F8 FF 75 08 8B 91 ? ? 00 00 EB 02 8B D0" ) + 0x3E ) );
	fprintf( hFile, "           AURA_TABLE_2 = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 81 ? ? 00 00 83 F8 FF 75 08 8B 91 ? ? 00 00 EB 02 8B D0" ) + 0x2A ) );
	fprintf( hFile, "           AURA_SIZE = 0x28, // ?\n" );
	fprintf( hFile, "           AURA_SPELL_ID = 0x8,  // ?\n" );
	fprintf( hFile, "           AURA_STACK = 0xE, // ?\n" );

	fprintf( hFile, "        }\n\n" );
	
    fprintf( hFile, "     }\n\n" );
	
	
	fclose( hFile );

}