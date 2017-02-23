/* Check if there is HotSpots in the objective */
if (questObjective.Hotspots.Count <= 0)
{
	/* Objective CSharpScript with script InteractWithHotSpots requires valid "Hotspots" */
	Logging.Write("InteractWithHotSpots requires valid 'HotSpots'.");
	questObjective.IsObjectiveCompleted = true;
	return false;
}


/* Search for Entry */
WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */;
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress = 0;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (node.IsValid && !nManagerSetting.IsBlackListedZone(node.Position) && !nManagerSetting.IsBlackListed(node.Guid))
{

	/* Entry found, GoTo */
	if(node.IsValid)
		baseAddress = MovementManager.FindTarget(node, questObjective.Range);
	
	Thread.Sleep(500);
	
	if (MovementManager.InMovement)
        return false;
	if (questObjective.IgnoreNotSelectable)
	{
	    if (node.IsValid && node.GetDistance > questObjective.Range)
	        return false;
	}
	else
	{
        if(baseAddress <= 0)
	        return false;
        if (baseAddress > 0 && (node.IsValid && node.GetDistance > questObjective.Range))
	    return false;
	}
  
	Thread.Sleep(500 + Usefuls.Latency); /* ZZZzzzZZZzz */

	/* Entry reached, dismount */
	MovementManager.StopMove();
	MountTask.DismountMount();
	Thread.Sleep(500);
	
	/* Interact With Entry */
	if (node.IsValid)
	{
		MovementManager.Face(node);
		Interact.InteractWith(node.GetBaseAddress);
	}
	
	Thread.Sleep(Usefuls.Latency); 

	if (questObjective.IgnoreFight)
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
		
	/* Wait for the interact cast to be finished, if any */
	while (ObjectManager.Me.IsCast)
	{
		Thread.Sleep(Usefuls.Latency);
	}
	
	

	Thread.Sleep(1000);
	
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(17034, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
	
	if(unit.IsValid)
	{
		MovementManager.Face(unit);
		Interact.InteractWith(unit.GetBaseAddress);
		
		if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
			return false;
		
		ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));

		while (ObjectManager.Me.IsCast)
		{
		Thread.Sleep(Usefuls.Latency);
		}
		
		if (ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
			return false;
		
		/* Wait if necessary */
		if(questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);

		/* Interact Completed - InternalIndex and count is used to determine if obj is completed - questObjective.IsObjectiveCompleted = true; */
	}
	
}else if (!MovementManager.InMovement)
{
	/* Move to Zone/Hotspot */
	if (questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
	{
		nManager.Wow.Helpers.Quest.TravelToQuestZone(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)], ref questObjective.TravelToQuestZone, questObjective.ContinentId,questObjective.ForceTravelToQuestZone);
		MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		MovementManager.GoLoop(questObjective.PathHotspots);
	}
}

nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;