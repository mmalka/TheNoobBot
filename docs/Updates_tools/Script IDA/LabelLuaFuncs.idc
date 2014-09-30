#include <idc.idc>

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

static Luafunc_GetName( structAddr )
{
	return GetString( Dword( structAddr ), -1, ASCSTR_C );
}

static Luafunc_GetFunc( structAddr )
{
	return Dword( structAddr + 4 );
}

static HandleLuaFunc( structBase )
{
 	auto funcName, funcAddr;
	funcName = Luafunc_GetName( structBase );
	funcAddr = Luafunc_GetFunc( structBase );	

	RenameFunc( funcAddr, form( "Script_%s", funcName ) );
}

static HandleLuaFuncWithClass( structBase, className )
{
 	auto funcName, funcAddr;
	funcName = Luafunc_GetName( structBase );
	funcAddr = Luafunc_GetFunc( structBase );	

	RenameFunc( funcAddr, form( "Script_%s_%s", className, funcName ) );
}

static main()
{
	// Normal func:
	auto registerFunc, xRef;
	registerFunc = FindBinary( 0, SEARCH_DOWN, "55 8B EC A1  ?  ?  ?  ? 56 6A  ? FF 75  ? 8B F0 50 E8  ?  ?  ?  ? FF 75  ? 56" );
	
	if ( registerFunc == BADADDR )
	{
		Message( "LabelLuaFuncs: Error, couldn't find FrameScript_RegisterFunction.\n" );
		return;
	}
	
	registerFunc = NextFunction(PrevFunction( registerFunc ));

	for( xRef = RfirstB( registerFunc ); xRef != BADADDR; xRef = RnextB( registerFunc, xRef ) )
	{
		auto structBase, numFuncs, i;

		structBase = Dword(xRef - 0x4);
		
		if (Byte( xRef + 0xA ) == 0x83)
		{
			numFuncs = Byte( xRef + 0xC );
		}
		else
		{
			numFuncs = Dword( xRef + 0xC );
		}
		
		if (numFuncs < 50000)
		{
			Message( "Found 0x%x, count: 0x%x, (xref: 0x%x)\n", structBase,  numFuncs, xRef);
			for ( i = 0; i < numFuncs; i = i + 0x8 )
			{
				HandleLuaFunc( structBase + i );
			}	
		}
	}
	
	// Special func:
	registerFunc = BADADDR;
	xRef = BADADDR;
	
	registerFunc = FindBinary( 0, SEARCH_DOWN, "83 C4 1C 43 3B 5D 0C" );
	
	if ( registerFunc == BADADDR )
	{
		Message( "LabelLuaFuncs: Error, couldn't find FrameScript_RegisterSpecialFunction.\n" );
		return;
	}
	
	registerFunc = NextFunction( PrevFunction( registerFunc ) );
 
	for( xRef = RfirstB( registerFunc ); xRef != BADADDR; xRef = RnextB( registerFunc, xRef ) )
	{
		auto offsetClassName,className;
		structBase = Dword( xRef - 0x5 + 0x1 );
		numFuncs = Byte( xRef - 0x7 + 0x1 );
		offsetClassName = Dword( xRef - 0xC + 0x1 );
		className = GetString( offsetClassName, -1, ASCSTR_C );
		if (numFuncs < 50000)
		{
			Message( "Found 0x%x, count: 0x%x\n", structBase,  numFuncs);
			for ( i = 0; i < numFuncs * 0x8; i = i + 0x8 )
			{
				HandleLuaFuncWithClass( structBase + i,  className);
			}	
		}
	}
}