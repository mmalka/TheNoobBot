// Script adapted from Kynox's solution
#include <ida.idc>
 
static main(){
    auto curAddr, xref, count, sPath, hFile;
 
	// WowClientDB_Common__Load - 4.2.2 build 14545
	curAddr = FindBinary( 0, SEARCH_DOWN, "55 8B EC 81 EC 08 01 00 00 53 56 57 8B 7D 08 8D" );
 
    if ( curAddr == BADADDR ){
        Message("Can't find WowClientDB_Common__Load, aborting...\n");
        return;
    }
 
	// store it where our database file is!
	sPath = ExtractPath( GetIdbPath() ) + "ClientDBCTables.h";
 
	// open our header file
	hFile = fopen( sPath, "w" );
	if ( hFile != -1 ){
		fprintf( hFile, "// %s\n", GetWoWVersionString() );
		fprintf( hFile, "/*----------------------------------\n" );
		fprintf( hFile, "Client DB Dumper 14545 - IDC Script\n" );
		fprintf( hFile, "by Tanaris4\n\n" );
		fprintf( hFile, "Credits:\n" );
		fprintf( hFile, "Kynox\n" );
		fprintf( hFile, "-----------------------------------*/\n\n" );
		fprintf( hFile, "typedef enum ClientDB{\n\n" );
	}
 
    // time to loop through and find all cross references to the wow DB_Common_Load function we found above!
    for ( xref = RfirstB(curAddr); xref != BADADDR; xref = RnextB(curAddr, xref) ) {
        auto prevFunc, disasm, disasmAddr, listStart;
 
        prevFunc = PrevFunction( xref );
        disasmAddr = xref;
 
        // search for the correct offset
        do{
       	 	disasm = GetDisasm( disasmAddr );
 
        	if ( disasm == BADADDR ){
        		break;
        	}
        	if ( disasmAddr < prevFunc ){
        		break;
        	}
 
        	// match yay!
			if ( strstr( disasm, "mov" ) > -1 && strstr( disasm, "off" ) > -1 && strstr( disasm, "dword" ) == -1 )
            	break;
 
        	disasmAddr = PrevHead( disasmAddr, prevFunc );
   		} while ( 1 );
 
   		listStart = GetOperandValue(disasmAddr, 1);
 
   		if ( listStart == BADADDR ){
   			continue;
   		}
 
   		// was this a pointer to the real list?
   		if ( strstr( disasm, "ds:" ) > -1 ){
   			listStart = Dword(listStart);
   		}
 
   		do{
			auto dbNameOffset, dbStruct, dbName;
 
			dbStruct = Dword(listStart);
	    	dbNameOffset = Dword(listStart + 0x4);
 
	    	// invalid :(  /tear
        	if ( dbStruct == 0 || dbNameOffset == 0 || dbStruct == 0xFFFFFFFF || dbNameOffset == 0xFFFFFFFF ){
            	break;
            }
 
			// grab the name of this table
        	dbName = WoWDb_GetName(dbNameOffset);
 
        	if ( strlen(dbName) == 0 ){
        		break;
        	}
 
			// save to file!
			if ( hFile != -1 ){
				fprintf( hFile, "\t%sDBTable = 0x%X,\n", dbName, dbStruct );
			}
 
			// IDA doesn't make these dwords dammit! Let's do it!
			MakeDword(xref);
			MakeDword(xref+0x4);
			MakeDword(dbStruct);
			MakeDword(dbNameOffset);
			MakeDword(dbNameOffset+0xC);
 
			listStart = listStart + 8;
        	count++;
 
   		} while( 1 );
    }
 
    Message("Saved %u tables to %s\n", count, sPath);
 
	if ( hFile != -1 ){
		fprintf( hFile, "} ClientDB;\n" );
	}
 
	fclose(hFile);
}
 
static ExtractPath( sPath ){
	auto dwIndex;
	for ( dwIndex = strlen( sPath ); strstr( substr( sPath, dwIndex, -1 ), "/" ) && dwIndex > 0; dwIndex-- );
	return substr( sPath, 0, dwIndex + 1 );
}
 
static WoWDb_GetName( dbBase ){
    auto dbName;
 
    // mov     eax, offset aDbfilesclientA ; "DBFilesClient\\Achievement.dbc"
    dbName = GetString( Dword(dbBase), -1, ASCSTR_C );
 
    // Return the the token after \ and before .
    return substr( dbName, strstr( dbName, "\\" ) + 1, -5 );
}
 
static GetWoWVersionString(){
	auto sVersion, sBuild, sDate;
 
	sVersion = FindBinary( INF_BASEADDR, SEARCH_DOWN, "\"=> WoW Version %s (%s) %s\"" );
 
	if( sVersion == BADADDR )
	{
		Message( "Version format string not found" );
		return 0;
	}
 
	sVersion = DfirstB( sVersion );
 
	if( sVersion == BADADDR )
	{
		Message( "Version string unreferences" );
		return 0;
	}
 
	sVersion = PrevHead( sVersion, 0 );
	sBuild = PrevHead( sVersion, 0 );
	sDate = PrevHead( sBuild, 0 );
 
	sVersion = GetOperandValue( sVersion, 1 );
	sBuild = GetOperandValue( sBuild, 1 );
	sDate = GetOperandValue( sDate, 1 );
 
	sVersion = GetString( sVersion, -1, ASCSTR_C );
	sBuild = GetString( sBuild, -1, ASCSTR_C );
	sDate = GetString( sDate, -1, ASCSTR_C );
 
	return form( "Version: %s  Build number: %s  Build date: %s\n", sVersion, sBuild, sDate );
}