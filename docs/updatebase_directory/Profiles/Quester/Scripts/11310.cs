WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	questObjective.AllowPlayerControlled);
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
uint baseAddress = 0;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList ))
{
	if(nManager.Wow.Helpers.PathFinder.FindPath(unit.Position).Count <= 0)
	{
		nManagerSetting.AddBlackList(unit.Guid, 30*1000);
		return false;
	}
	
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
	
	/* Entry found, GoTo */
	
	baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
	
	Thread.Sleep(500);
		
	if (MovementManager.InMovement)
		return false;
	
	if (baseAddress <= 0)
		return false;
	if (baseAddress > 0 && (unit.IsValid && unit.GetDistance > questObjective.Range))
		return false;
		
	/* Target Reached */
	MovementManager.StopMove();
	MountTask.DismountMount();
	
	
	nManager.Wow.Helpers.Keybindings.DownKeybindings(nManager.Wow.Enums.Keybindings.ACTIONBUTTON1);
	Thread.Sleep(1000);
	nManager.Wow.Helpers.Keybindings.UpKeybindings(nManager.Wow.Enums.Keybindings.ACTIONBUTTON1);


	nManagerSetting.AddBlackList(unit.Guid, 30*1000);
	

	/* Wait if necessary */
	if (questObjective.WaitMs > 0)
		Thread.Sleep(questObjective.WaitMs);

	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	return true;
}
	/* Move to Zone/Hotspot */
else if (!MovementManager.InMovement)
{
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	if (questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
	{
		if(nManager.Wow.Helpers.Quest.TravelToQuestZone(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)],
			ref questObjective.TravelToQuestZone, questObjective.ContinentId, questObjective.ForceTravelToQuestZone))
			return false;
		MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		MovementManager.GoLoop(questObjective.PathHotspots);
	}
}