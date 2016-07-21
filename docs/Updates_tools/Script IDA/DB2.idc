#include <ida.idc>
 
static main(){
    auto curAddr, xref;
 
	// DB2Load
	curAddr = FindBinary( 0, SEARCH_DOWN, "53 56 8B F1 33 DB C7 06 ? ? ? ? 8D 46 04 89 40 04" );
	
    if ( curAddr == BADADDR ){
        Message("Can't find DB2Load, aborting...\n");
        return;
    }
 
    // time to loop through and find all cross references to the wow DB_Common_Load function we found above!
    for ( xref = RfirstB(curAddr); xref != BADADDR; xref = RnextB(curAddr, xref) ) {
        auto prevFunc, nextFunc, disasm, disasmAddr, dbAddress, dbNameAddress;
 
        prevFunc = PrevFunction( xref );
		nextFunc = NextFunction( xref );
        disasmAddr = xref;
		
		disasmAddr = PrevHead( disasmAddr, prevFunc );
		disasm = GetDisasm( disasmAddr );
		if ( strstr( disasm, "mov" ) > -1 && strstr( disasm, "off" ) > -1 && strstr( disasm, "dword" ) > -1 && strstr( disasm, "ecx" ) > -1 )
        {
			dbAddress = GetOperandValue(disasmAddr, 1);
			if ( dbAddress == BADADDR ){
				continue;
			}
		}
		else
		{
			continue;
		}
		
		disasmAddr = NextHead( disasmAddr, nextFunc);
		disasmAddr = NextHead( disasmAddr, nextFunc);
		disasmAddr = NextHead( disasmAddr, nextFunc);
		disasmAddr = NextHead( disasmAddr, nextFunc);
		disasm = GetDisasm( disasmAddr );
		if ( strstr( disasm, "mov" ) > -1 && strstr( disasm, "off" ) > -1 && strstr( disasm, "dword" ) > -1 )
        {
			dbNameAddress = GetOperandValue(disasmAddr, 1);
			if ( dbNameAddress == BADADDR ){
				continue;
			}
			if (GetOperandValue(disasmAddr, 0) == dbAddress){
				continue;
			}
		}
		else
		{
			continue;
		}
		
		auto dbName;
		dbName = WoWDb_GetName(dbNameAddress);
		
		if ( strlen(dbName) == 0 ){
        		break;
        	}
		
		RenameFunc( dbAddress, form( "%sDB", dbName ) );
		Message( "%s = 0x%x\n", dbName, dbAddress );
    }
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
 
static WoWDb_GetName( dbBase ){
    auto dbName;
 
    dbName = GetString( Dword(dbBase), -1, ASCSTR_C );
 
    return substr( dbName, strstr( dbName, "\\" ) + 1, -5 );
}