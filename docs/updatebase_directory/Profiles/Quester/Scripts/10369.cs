WoWUnit wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreBlackList);

if(wowUnit.IsValid)
{	
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
	MovementManager.FindTarget(wowUnit, CombatClass.GetAggroRange);
	Thread.Sleep(100);
	
	if(MovementManager.InMovement && wowUnit.Position.DistanceTo(ObjectManager.Me.Position) >= 10f)
	{
		return false;
	}
	MountTask.DismountMount();
	Thread.Sleep(300);
		
	while(!wowUnit.IsElite)
	{
	    if (ObjectManager.Me.IsDead)
		   break;
		MovementManager.Face(wowUnit);
		Thread.Sleep(500);
		Interact.InteractWith(wowUnit.GetBaseAddress);
		Thread.Sleep(500);
		Logging.Write("Use Item");
		ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
		questObjective.IsObjectiveCompleted = true;
	}
	
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
}
else if (!MovementManager.InMovement && questObjective.Hotspots.Count > 0)
{
	// Need GoTo Zone:
	if (questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
	{
		nManager.Wow.Helpers.Quest.TravelToQuestZone(questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)], ref questObjective.TravelToQuestZone, questObjective.ContinentId,questObjective.ForceTravelToQuestZone);
		MovementManager.Go(PathFinder.FindPath(questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		// Start Move
		MovementManager.GoLoop(questObjective.Hotspots);
	}
}