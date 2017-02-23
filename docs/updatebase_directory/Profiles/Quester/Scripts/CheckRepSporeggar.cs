//_, _, standingID, _, _, _, _, _, _, _, _, _, _, _, _, _= GetFactionInfoByID(970);
string standing;
string randomString = Others.GetRandomString(Others.Random(4, 10));
standing = Lua.LuaDoString("_, _," + randomString + ", _, _, _, _, _, _, _, _, _, _, _, _, _= GetFactionInfoByID(970)",randomString);

if(nManager.Helpful.Others.ToInt32(standing) >= 4)
	return true;
	
return false;