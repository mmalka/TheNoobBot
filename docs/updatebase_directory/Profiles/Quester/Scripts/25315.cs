Lua.RunMacroText("/click OverrideActionBarButton1");
Thread.Sleep(9200);
Lua.RunMacroText("/click OverrideActionBarButton2");
Thread.Sleep(9200);
Lua.RunMacroText("/click OverrideActionBarButton3");
Thread.Sleep(9200);

if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(25315))
{
	Lua.RunMacroText("/click OverrideActionBarButton5");
}