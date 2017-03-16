/* Run Lua On Target*/

nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;

while(!nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(14212))
{
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
	Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
		
	/* If Entry found continue, otherwise continue checking around HotSpots */
	if (unit.IsValid && !nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid))
	{
		/* Entry found, GoTo */

		Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */
		
		MovementManager.Face(unit);
		Interact.InteractWith(unit.GetBaseAddress);

		Lua.RunMacroText(questObjective.LuaMacro);
		ClickOnTerrain.ClickOnly(unit.Position);
		
		Thread.Sleep(Usefuls.Latency + 150);

		if (unit.IsValid)
		{
			nManagerSetting.AddBlackList(unit.Guid, 30*1000);
		}

		/* Wait if necessary */
		if (questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);

		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	}
}
return true;
