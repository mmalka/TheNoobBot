#include <ida.idc>
 
static main(){
    auto curAddr, xref, count, sPath, hFile;
 
	// WowClientDB_Common__LoadInternal1
	curAddr = FindBinary( 0, SEARCH_DOWN, "55 8B EC 53 56 57 FF 75 0C 8B 7D 08 8B D9" );
	
    if ( curAddr == BADADDR ){
        Message("Can't find WowClientDB_Common__LoadInternal1, aborting...\n");
        return;
    }
	// WowClientDB_Common__Load
	curAddr = NextFunction( curAddr );
	
	// store it where our database file is!
	sPath = ExtractPath( GetIdbPath() ) + "ClientDBCTables.h";
 
	// open our header file
	hFile = fopen( sPath, "w" );
	if ( hFile != -1 ){
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
 
			// Rename ida:
			RenameFunc( dbStruct, form( "%sDBTable", dbName ) );
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
 
    Message("Saved and renamed %u tables to %s\n", count, sPath);
 
	if ( hFile != -1 ){
		fprintf( hFile, "} ClientDB;\n" );
	}
 
	fclose(hFile);
}

// 1 = Success, 0 = Failure
static RenameFunc( dwAddress, sFunction )
{
	auto dwRet;

	dwRet = MakeNameEx( dwAddress, sFunction, SN_NOWARN );

	if( dwRet == 0 )
	{
		auto sTemp, i;
		for( i = 0; i < 32; i++ )
		{
			sTemp = form( "%s_%i", sFunction, i );

			if( ( dwRet = MakeNameEx( dwAddress, sTemp, SN_NOWARN ) ) != 0 )
			{
				Message( "Info: Renamed to %s instead of %s\n", sTemp, sFunction );
				break;
			}
		}
	}
	return dwRet;	
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