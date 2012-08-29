#include <idc.idc>


static _Trim(str, chrs)
{
	auto from ;
	auto to ;
	auto chr ;

	from = 0 ;
	to = 0 ;

	while(1)
	{
		if ( from >= strlen(str) ) break ;
		chr = substr(str, from, from+1) ;
		if ( strstr(chrs, chr) == -1 )
			break ;
		from++ ;
	}

	to = strlen(str) - 1 ;
	while(1)
	{
		if ( to <= from ) break ;
		chr = substr(str, to, to+1) ;
		if ( strstr(chrs, chr) == -1 )
		{
			to++ ;
			break ;
		}
		to-- ;
		
	}
	return substr(str, from, to) ;
	
}

static main()
{

	auto fname ;
	auto str, fh ;
	auto left, right, idx ;
	auto addr ;

	auto _code_prefix ;
	auto _data_prefix ;


	_code_prefix = "";
	_data_prefix = "";

	fname = AskFile(0, "*.*", "Select 'symbols' file. TY Barthen!") ;

	if ( fname == "" ) { Message("\nwhatever..."); return ; }

	fh = fopen(fname, "rt") ;
	if ( !fh ) { Message("\nCan't open the file %s", fname); return ; }

	while ( (str=readstr(fh) ) != -1 )
	{
		str = _Trim(str, " \t\n\r") ;
		idx = strstr(str, " ") ;
		if ( idx == -1 ) idx = strstr(str, "\t") ;
		if ( idx == -1 ) continue ;

		left = _Trim(substr(str, 0, idx), " \t\n\r") ;				// addr
		right = substr(str, idx, -1) ;
		right = _Trim(right, " \t\n\r") ;								// name
		
		//right = _Trim(substr(str, 0, idx), " \t\n\r") ;				// addr
		//left = substr(str, idx, -1) ;
		//left = _Trim(left, " \t\n\r") ;								// name

		idx = strstr(right, "(") ;
		if ( idx != -1 ) right = substr(right, 0, idx) ;			// drop down from func(args....) args part, keep func

		addr = xtol(left) ;
		if ( addr == 0 || addr == BADADDR ) { Message("\nMisformatted addr ? '%s' - %x\n", str, addr); continue; }
		if ( hasUserName(GetFlags(addr)) ) 
		{
			Message("\nAddr: %a already has user name %s... skipping", addr, Name(addr) ) ;
			continue ;
		}
		if ( isCode(GetFlags(addr)) )
		{
			right = _code_prefix + right ;
		} else if( isData(GetFlags(addr)) )
		{
			right = _data_prefix + right ;
		}
		else
		{
			right = "__wth_is_this_one_" + right ;
		}
		Message("\n%a -> %s", addr, right) ;
		MakeName(addr, right) ;
//		Message("Addr: %a Name: '%s' : isData %d isCode %d isRef %d hasName %d\n", addr, right, isData(GetFlags(addr)), isCode(GetFlags(addr)), isRef(GetFlags(addr)), hasUserName(GetFlags(addr)) ) ;
		
	}


}