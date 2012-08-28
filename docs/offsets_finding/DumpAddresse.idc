#include <ida.idc>

static main()
{
	auto sPath, hFile;
	sPath = "D:\\Addresses.cs";
	hFile = fopen( sPath, "w" );
	fprintf( hFile, "    public class Addresses\n    {\n");
	fprintf( hFile, "        /// <summary>\n        /// ObjectManager\n        /// </summary>\n        public enum ObjectManager\n        {\n" );
	
	
	fprintf( hFile, "            clientConnection = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 15 ? ? ? ? 89 82 ? ? 00 00 8B 15 ? ? ? ? 89 ? ? 00 00 00" ) + 0x2 ) );
	fprintf( hFile, "            objectManager = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 15 ? ? ? ? 89 82 ? ? 00 00 8B 15 ? ? ? ? 89 ? ? 00 00 00" ) + 0x8 ) );
	fprintf( hFile, "            firstObject = 0xC0, // ?\n" );
	fprintf( hFile, "            nextObject = 0x3C, // ?\n" );
	fprintf( hFile, "            localGuid = 0xC8, // ?\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// DirectX9\n        /// </summary>\n        public enum D3D9\n        {\n" );
	fprintf( hFile, "            DX_DEVICE = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 0D ? ? ? ? 68 ? ? ? ? 6A 00 E8 ? ? ? ? 8B 0D ? ? ? ? 68" ) + 0x2 ) );
	fprintf( hFile, "            DX_DEVICE_IDX = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 81 ? ? 00 00 8B 08 8B 51 ? 50 FF D2 85 C0 7D 2B 3D ? ? ? ? 75 24" ) + 0x2 ) );
	fprintf( hFile, "            ENDSCENE_IDX = 0xA8, // ?\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// For directX 11 Wow\n        /// </summary>\n        public enum D3D11\n        {\n" );
	 
	fprintf( hFile, "            DX_DEVICE = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "57 57 57 57 6A 01 57 89 ? ? FF 15" ) + 0xC ) );
	fprintf( hFile, "            CreateDXGIFactory1 = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC A1 ? ? ? ? 56 8B 75 08 57 8B F8 05" ) + 0x4 ) );
	fprintf( hFile, "            CreateDXGIFactory1Pt = 0x27F4, // ?\n" );
	fprintf( hFile, "            CreateDXGIFactory1Vtable = 0x8, // ?\n" );
	fprintf( hFile, "            startHook = 0x0, // ?\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Is Swimming (Lua_IsSwimming) [[base+offset1]+offset2]\n        /// </summary>\n        public enum IsSwimming\n        {\n" );
	
	fprintf( hFile, "            flag = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 80 ? ? ? ? F7 40 ? ? ? ? ? 74 19 D9 E8 83 EC 08 DD 1C 24 56" ) + 0x9 ) );
	fprintf( hFile, "            offset1 = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 80 ? ? ? ? F7 40 ? ? ? ? ? 74 19 D9 E8 83 EC 08 DD 1C 24 56" ) + 0x2 ) );
	fprintf( hFile, "            offset2 = 0x%X,\n", Byte( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 80 ? ? ? ? F7 40 ? ? ? ? ? 74 19 D9 E8 83 EC 08 DD 1C 24 56" ) + 0x8 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Is Flying (Lua_IsFlying) [[base+offset1]+offset2]\n        /// </summary>\n        public enum IsFlying\n        {\n" );
	
	fprintf( hFile, "            flag = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 41 D9 E8 83 EC 08 DD 1C 24 56" ) + 0x9 ) );
	fprintf( hFile, "            offset1 = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 41 D9 E8 83 EC 08 DD 1C 24 56" ) + 0x2 ) );
	fprintf( hFile, "            offset2 = 0x%X,\n", Byte( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 88 ? ? ? ? F7 41 ? ? ? ? ? 74 41 D9 E8 83 EC 08 DD 1C 24 56" ) + 0x8 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// GameInfo offset\n        /// </summary>\n        public enum GameInfo\n        {\n" );
	
	fprintf( hFile, "            wowVersion = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 45 18 68 00 08 00 00 50 68 ? ? ? ? 89 15 ? ? ? ? E8 ? ? ? ?" ) + 0x10 ) );
	fprintf( hFile, "            gameState = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "80 3D ? ? ? ? 00 75 0F 81 3D ? ? ? ? 00 02 00 00 74 03 33 C0 C3 B8 01" ) + 0x2 ) );
	fprintf( hFile, "            isLoadingOrConnecting = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "80 3D ? ? ? ? 00 75 0F 81 3D ? ? ? ? 00 02 00 00 74 03 33 C0 C3 B8 01" ) + 0xB ) );
	fprintf( hFile, "            continentId = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "F3 0F 10 45 10 8B 15 ? ? ? ? 56 57 8B 7D 08 8B C7 6B C0 58 89 88 ? ? ? ?" ) + 0x7 ) );
	fprintf( hFile, "            AreaId = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 83 EC 08 53 33 DB 39 1D ? ? ? ? 0F 84 ? ? ? ? A1 ? ? ? ?" ) + 0xB ) );
	fprintf( hFile, "            lastWowErrorMessage = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "68 ? ? ? 00 50 E8 ? ? ? ? B8 ? ? ? ? 8D 95 ? ? ? FF 83 C4 ?" ) + 0xC ) );
	fprintf( hFile, "            zoneMap = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC A1 ? ? ? ? 85 C0 75 05 B8 ? ? ? ? 50 8B 45 08 50 E8 ? ? ? ?" ) + 0x4 ) );
	fprintf( hFile, "            subZoneMap = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "A1 ? ? ? ? 85 C0 74 67 80 38 00 74 62 50 68 ? ? ? ? 8D 95 D4 FE FF FF" ) + 0x1 ) );
	fprintf( hFile, "            consoleExecValue = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 53 56 8B 75 08 57 BF ? ? ? ? BB ? 00 00 00 8D 45 08 50 56" ) + 0xA ) );

	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Player Offset\n        /// </summary>\n        public enum Player\n        {\n" );
	
	fprintf( hFile, "           LastTargetGUID = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 83 EC 08 A1 ? ? ? ? 8B 0D ? ? ? ? 89 45 F8 0B C1 89 4D FC" ) + 0x7 ) );
	fprintf( hFile, "           petGUID = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 0D ? ? ? ? 8B 45 08 83 EC ? 53 8B 5D 0C 56 3B C8 75 0A" ) + 0x5 ) );
	fprintf( hFile, "           playerName = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "3F 90 00 C3 CC CC CC CC CC CC CC CC CC CC 0F BE 05 ? ? ? ? F7 D8 1B C0 25 ? ? ? ? C3 CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC" ) + 0x11 ) );
	fprintf( hFile, "           PlayerComboPoint = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 08 89 0D ? ? ? ? 8B 50 04 8A 45 0C 89 15 ? ? ? ? A2 ? ? ? ? E8" ) + 0x15 ) );
	fprintf( hFile, "           RetrieveCorpseWindow = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 45 08 3B 05 ? ? ? ? 74 42 83 3D ? ? ? ? 04 A3 ? ? ? ?" ) + 0x8 ) );

	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Unit Relation\n        /// </summary>\n        public enum UnitRelation\n        {\n" );
	
	fprintf( hFile, "           FACTION_START_INDEX = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 55 08 8B 82 F8 00 00 00 8B 88 A4 00 00 00 51 B9 ? ? ? ? E8 ? ? ? ?" ) + 0x11 ) );
	fprintf( hFile, "           FACTION_TOTAL =  FACTION_START_INDEX + 0x4,\n" );
	fprintf( hFile, "           FACTION_POINTER =  FACTION_START_INDEX + 0xC,\n" );
	fprintf( hFile, "           HOSTILE_OFFSET_1 = 0x14, // ?\n" );
	fprintf( hFile, "           HOSTILE_OFFSET_2 = 0x0C, // ?\n" );
	fprintf( hFile, "           FRIENDLY_OFFSET_1 = 0x10, // ?\n" );
	fprintf( hFile, "           FRIENDLY_OFFSET_2 = 0x0C, // ?\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Bar manager\n        /// </summary>\n        public enum BarManager\n        {\n" );
	
	fprintf( hFile, "           slotIsEnable = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8D 4E 01 51 89 04 B5 ? ? ? ? 8B 45 F4 68 ? ? ? ? 68 ? 00 00 00" ) + 0x7 ) );
	fprintf( hFile, "           startBar = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 8B 45 08 8B 04 85 ? ? ? ? 85 C0 74 0E A9 00 00 00 F0 75 07" ) + 0x9 ) );
	fprintf( hFile, "           nbBar = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "A1 ? ? ? ? 40 53 89 45 FC 8B 45 0C 8B 58 04 56 57 85 DB 74 3A 8B 48 08" ) + 0x1 ) );
	fprintf( hFile, "           nbBarOne = 0x%X,\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B F0 B8 AB AA AA AA F7 E6 C1 EA 03 83 C4 08 85 D2 0F 94 C0 8D 7E 0C 85 C9" ) - 0x4 ) );
	fprintf( hFile, "           nextBar = 0x4,\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Unit Field Descriptor\n        /// </summary>\n        public enum UnitField\n        {\n" );
	
	fprintf( hFile, "           UNIT_SPEED = 0x80C, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_X = 0x790, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_Y = UNIT_FIELD_X + 0x4, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_Z = UNIT_FIELD_X + 0x8, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_R = 0x7A0, // ?\n" );
	fprintf( hFile, "           UNIT_FIELD_H = 0x8AC, // ?\n" );
	fprintf( hFile, "           unitName1 = 0x%X, // CGUnit_C__GetUnitName + 0x142\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B B6 ? ? 00 00 85 F6 74 17 0F B6 49 ? 8B" ) + 0x2 ) );
	fprintf( hFile, "           unitName2 = 0x%X, // CGUnit_C__GetUnitName + 0x15D\n", Byte( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B B6 ? ? 00 00 85 F6 74 17 0F B6 49 ? 8B" ) + 0x1C ) );
	fprintf( hFile, "           CastingSpellID = 0x%X, // Script_UnitCastingInfo\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "0F 84 54 02 00 00 E8 ? ? ? ? 8B 8B ? ? 00 00 8B 15 ? ? ? ? 3B CA 0F 8C ? ? ? ?" ) + 0xD ) );
	fprintf( hFile, "           ChannelSpellID = 0x%X, // Script_UnitChannelInfo\n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "0F 84 5F 01 00 00 E8 ? ? ? ? 8B 8B ? ? 00 00 8B 15 ? ? ? ? 3B CA 0F 8C ? ? ? ?" ) + 0xD ) );
	fprintf( hFile, "           TransportGUID = 0x788, // CGUnit_C__GetTransportGUID+0\n" );

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
	
	fprintf( hFile, "           statPvp = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "88 98 ? ? ? ? 83 C0 ? 3D ? 00 00 00 72 F0 89 1D ? ? ? ? 89 1D ? ? ? ?" ) + 0x12 ) );
	fprintf( hFile, "           pvpExitWindow = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 51 83 3D ? ? ? ? 00 75 15 8B 45 08 50 E8 ? ? ? ? 83 C4 04" ) + 0x6 ) );
	fprintf( hFile, "           selectedBattleGroundID = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "E8 ? ? ? ? 8B 16 8D 4D EC 89 15 ? ? ? ? E8 ? ? ? ? 5E 8B E5" ) + 0xC ) );

	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Text box of the wow chat\n        /// </summary>\n        public enum TextBoxChat\n        {\n" );
	
	fprintf( hFile, "           baseBoxChat = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "56 8B F1 E8 ? ? ? ? 83 3D ? ? ? ? 00 75 12 F6 86 ? ? 00 00 01" ) + 0xA ) );
	fprintf( hFile, "           baseBoxChatPtr = 0x208, // ?\n" );
	fprintf( hFile, "           statBoxChat = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 0D ? ? ? ? 41 89 0D ? ? ? ? 83 F9 01 75 29 8B 0D ? ? ? ?" ) + 0x2 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Spell Book\n        /// </summary>\n        public enum SpellBook\n        {\n" );

	fprintf( hFile, "           knownSpell = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "83 C0 FF 78 42 8B 35 ? ? ? ? 8B 0C 86 3B 51 04 75 05 83 39 01 74 32" ) + 0x7 ) );
	fprintf( hFile, "           nbSpell = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8D 45 F8 50 E8 ? ? ? ? 33 DB 33 FF 89 7D FC 39 1D ? ? ? ? 0F 86 ? ? ? ?" ) + 0x12 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Chat\n        /// </summary>\n        public enum Chat\n        {\n" );

	fprintf( hFile, "           chatBufferStart = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 45 08 03 C1 99 B9 ? 00 00 00 F7 F9 8B C2 69 C0 ? ? 00 00 05 ? ? ? ?" ) + 0x16 ) );
	fprintf( hFile, "           NextMessage = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 45 08 03 C1 99 B9 ? 00 00 00 F7 F9 8B C2 69 C0 ? ? 00 00 05 ? ? ? ?" ) + 0x11 ) );
	fprintf( hFile, "           msgFormatedChat = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 45 08 03 C1 99 B9 ? 00 00 00 F7 F9 8B C2 69 C0 ? ? 00 00 05 ? ? ? ?" ) + 0x7 ) );
	fprintf( hFile, "           chatBufferPos = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 45 08 03 C1 99 B9 ? 00 00 00 F7 F9 8B C2 69 C0 ? ? 00 00 05 ? ? ? ?" ) - 0x4 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Click To Move\n        /// </summary>\n        public enum ClickToMove\n        {\n" );

	fprintf( hFile, "           CTM = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "D9 1D ? ? ? ? 83 C4 08 53 56 6A 01 68 00 00 00 01 E8 ? ? ? ? 8B C8" ) + 0x2 ) );
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
    
    fprintf( hFile, "           ClntObjMgrGetActivePlayer = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "64 8B 0D ? 00 00 00 A1 ? ? ? ? 8B 14 81 8B 8A 08 00 00 00 85 C9 75 05" ) + 0x0 );  
	fprintf( hFile, "           FrameScript_ExecuteBuffer = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 51 83 05 ? ? ? ? 01 A1 ? ? ? ? 89 45 FC 74 12 83 3D ? ? ? ? 00" ) + 0x0 );  
	fprintf( hFile, "           CGPlayer_C__ClickToMove = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 83 EC 18 53 56 8B F1 8B 46 08 8B 08 57 3B 0D ? ? ? ? 75 5C" ) + 0x0 ); 
	fprintf( hFile, "           ClntObjMgrGetActivePlayerObj = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "E8 ? ? ? ? 68 ? 00 00 00 68 ? ? ? ? 6A ? 52 50 E8 ? ? ? ? 83 C4" ) + 0x0 ); 
	fprintf( hFile, "           FrameScript__GetLocalizedText = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 53 56 8B F1 8B 46 08 8B 58 04 57 8B 38 E8 ? ? ? ? 3B F8 75 10" ) + 0x0 ); 
	fprintf( hFile, "           CGWorldFrame__Intersect = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC A1 ? ? ? ? 8B 0C 85 ? ? ? ? 85 C9 75 04 32 C0 5D C3 8B 55 1C" ) + 0x0 ); 
	fprintf( hFile, "           Spell_C__HandleTerrainClick = 0x%X, \n", FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC A1 ? ? ? ? 85 C0 75 04 32 C0 5D C3 8B 15 ? ? ? ? 56 F6 C2 20" ) + 0x0 ); 
	
	fprintf( hFile, "        }\n\n" );

	fprintf( hFile, "        /// <summary>\n        /// Corpse Player\n        /// </summary>\n        public enum CorpsePlayer\n        {\n" );
    
	fprintf( hFile, "           X = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 15 ? ? ? ? 8B 45 08 89 10 8B 0D ? ? ? ? 89 48 04 8B 15 ? ? ? ?" ) + 0x2 ) );
	fprintf( hFile, "           Y = X + 0x4,\n" );
	fprintf( hFile, "           Z = Y + 0x4,\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Get Players name\n        /// </summary>\n        public enum PlayerNameStore\n        {\n" );
    
	fprintf( hFile, "           nameStorePtr = 0x%X + 0x8, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8D 55 EC 89 45 EC 8B 49 04 52 51 89 4D F0 50 B9 ? ? ? ? E8" ) + 0x10 ) );
	fprintf( hFile, "           nameMaskOffset = 0x024, // ?\n" );
	fprintf( hFile, "           nameBaseOffset = 0x01c, // ?\n" );
	fprintf( hFile, "           nameStringOffset = 0x020, // ?\n" );
	
	 
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Wow login addresses\n        /// </summary>\n        public enum Login\n        {\n" );
    
	fprintf( hFile, "           realmName = 0x%X + 0x6, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "50 81 E6 ? ? ? ? E8 ? ? ? ? 6A 58 6A 00 68 ? ? ? ? E8 ? ? ? ? 83" ) + 0x3 ) );
	fprintf( hFile, "           battlerNetWindow = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "80 3D ? ? ? ? 00 75 0F 81 3D ? ? ? ? 00 02 00 00 74 03 33 C0 C3 B8 01" ) + 0xB ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Active AutoLoot\n        /// </summary>\n        public enum AutoLoot\n        {\n" );
    
	fprintf( hFile, "           AutoLoot_Activate_Pointer = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "A1 ? ? ? ? 8B 0D ? ? ? ? 56 8B 70 ? 68 ? ? ? ? E8 ? ? ? ? 85 C0" ) + 0x1 ) );
	fprintf( hFile, "           AutoLoot_Activate_Offset = 0x%X, \n", Byte( FindBinary( INF_BASEADDR, SEARCH_DOWN, "A1 ? ? ? ? 8B 0D ? ? ? ? 56 8B 70 ? 68 ? ? ? ? E8 ? ? ? ? 85 C0" ) + 0xE ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Active Auto Cast\n        /// </summary>\n        public enum AutoSelfCast\n        {\n" );
    
	fprintf( hFile, "           AutoSelfCast_Activate_Pointer = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "68 ? ? 00 00 68 ? ? ? ? 6A 08 51 50 E8 ? ? ? ? 8B F0 A1 ? ? ? ? 83 C4" ) + 0x16 ) );
	fprintf( hFile, "           AutoSelfCast_Activate_Offset = 0x%X, \n", Byte( FindBinary( INF_BASEADDR, SEARCH_DOWN, "68 ? ? 00 00 68 ? ? ? ? 6A 08 51 50 E8 ? ? ? ? 8B F0 A1 ? ? ? ? 83 C4" ) + 0x1F ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Active Auto Interact CTM\n        /// </summary>\n        public enum AutoInteract\n        {\n" );
    
	fprintf( hFile, "           AutoInteract_Activate_Pointer = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 0D ? ? ? ? 83 79 ? 00 74 06 B8 01 00 00 00 C3 33 C0 C3 CC" ) + 0x2 ) );
	fprintf( hFile, "           AutoInteract_Activate_Offset = 0x%X, \n", Byte( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 0D ? ? ? ? 83 79 ? 00 74 06 B8 01 00 00 00 C3 33 C0 C3 CC" ) + 0x8 ) );

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
	fprintf( hFile, "        /// <summary>\n        /// Anti Warden\n        /// </summary>\n        public enum Warden\n        {\n" );
	
	fprintf( hFile, "           WardenClassPtr = 0x%X, \n", Dword( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 0D ? ? ? ? 8B 01 8B 50 08 56 FF D2 A1" ) + 0x2 ) );
    fprintf( hFile, "           injectionStart = 0xF,\n" );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Cheat addresse\n        /// </summary>\n        internal enum Cheat\n        {\n" );
	
	fprintf( hFile, "           NoSwim = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "83 C4 10 F3 0F 11 45 FC 85 DB 74" ) + 0x8 ) );
	fprintf( hFile, "           WaterWalk = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "56 8B F1 8B 8E 44 01 00 00 57 BF 11 01 10 00 E8 ? ? ? ? 84 C0 75 05" ) + 0x32 ) );
	fprintf( hFile, "           fly1 = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 81 BC 00 00 00 85 C0 74 1F 8B 50 20 84 D2 78 05 F6 C2 40 75 1C 85 C0" ) + 0x39 ) );
	fprintf( hFile, "           fly2 = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 83 EC 20 F3 0F 10 41 58 8D 45 F8 8D 55 FC 89 45 EC 89 55 F0 F3 0F 11 45 F4" ) + 0x41 ) );
	fprintf( hFile, "           fly3 = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "55 8B EC 83 7D 08 00 56 57 8B F1 74 11 8B 46 44 A9 00 00 00 10 74 07 A9 00 00 10 00" ) + 0x4E ) );
	fprintf( hFile, "           NoClip = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "39 7B 04 0F 85 ? ? ? ? 8D 55 DC 52 8D 45 CC 50 8D 4D BC 51 56 89 7D BC" ) + 0x0 ) );
	fprintf( hFile, "           speed = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "2B 87 28 01 00 00 85 C0 7E 62 50 56 E8 ? ? ? ? 8B 87 20 01 00 00 83 C4 08" ) + 0x0 ) );
	fprintf( hFile, "           OsGetAsyncTimeMs = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "B9 ? ? ? ? E9 ? ? ? ? CC CC CC CC CC CC 55 8B EC A1 ? ? ? ?" ) + 0x0 ) );
	fprintf( hFile, "           WallClimb1 = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "8B 0D ? ? ? ? 6B C0 34 F3 0F 10 05 ? ? ? ? 0F 2F 44 08 08 72 06" ) + 0x16 ) );
	fprintf( hFile, "           afk = 0x%X, \n", ( FindBinary( INF_BASEADDR, SEARCH_DOWN, "78 42 E8 ? ? ? ? 8B C8 E8 ? ? ? ? 85 C0 75 30 50 50 50 6A FF 68" ) + 0x0 ) );
	
	fprintf( hFile, "        }\n\n" );
	fprintf( hFile, "        /// <summary>\n        /// Anti Warden\n        /// </summary>\n        public class Warden2\n        {\n" );
	
	fprintf( hFile, "           public static uint WardenPtr2 = 0x8C14D0;\n" );
	fprintf( hFile, "           public static uint WardenDec2 = 0xF;\n" );

	fprintf( hFile, "        }\n\n" );
    fprintf( hFile, "     }\n\n" );
	
	
	fclose( hFile );

}