if(ObjectManager.Me.UnitAura(74008).IsValid) //YES
{
	Lua.RunMacroText("/click OverrideActionBarButton1");
}

if(ObjectManager.Me.UnitAura(74009).IsValid) //No
{
	Lua.RunMacroText("/click OverrideActionBarButton2");
}

if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(25496))
{
	Lua.RunMacroText("/click OverrideActionBarButton4");
}

Thread.Sleep(6000);