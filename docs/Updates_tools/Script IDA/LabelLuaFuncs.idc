#include <idc.idc>

/************************************************************************
   Desc:		Label each lua function based on its appropriate name
   Author:  kynox
   Credit:	bobbysing for RenameFunc
   Website: http://www.gamedeception.net
*************************************************************************/

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

static main()
{
	auto registerFunc, xRef;
	registerFunc = FindBinary( 0, SEARCH_DOWN, "8B 45 08 50 56 E8 ? ? ? ? 6A ? 56 E8 ? ? ? ? 68 ? ? ? ? 56" );
	
	if ( registerFunc == BADADDR )
	{
		Message( "LabelLuaFuncs: Error, couldn't find registerFunc.\n" );
		return;
	}
	
	registerFunc = PrevFunction( registerFunc );
	
	for( xRef = RfirstB( registerFunc ); xRef != BADADDR; xRef = RnextB( registerFunc, xRef ) )
	{
		auto movAddr, cmpAddr, structBase;
		movAddr = PrevHead( PrevHead( xRef, 8 ), 8 );
		
		cmpAddr = NextHead( xRef, xRef+8 );
		cmpAddr = NextHead( cmpAddr, cmpAddr+8 );
		cmpAddr = NextHead( cmpAddr, cmpAddr+8 );
		
		structBase = GetOperandValue( movAddr, 1 );
		
		if ( strstr( GetDisasm( movAddr ), "[" ) > -1 )
		{
			auto numFuncs, i;
			numFuncs = GetOperandValue( cmpAddr, 1 ) / 8;
			
			for ( i = 0; i < numFuncs; i++ )
			{
				HandleLuaFunc( structBase );
				
				structBase = structBase + 8;
			}		
		} else
		{
			HandleLuaFunc( structBase );
		}
	}
}