nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;

while(!nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(14293))
{
	Lua.RunMacroText("/click OverrideActionBarButton1");
	Thread.Sleep(1000);
}

while(nManager.Wow.Helpers.Usefuls.PlayerUsingVehicle)
{
	Thread.Sleep(1500);
}

return true;