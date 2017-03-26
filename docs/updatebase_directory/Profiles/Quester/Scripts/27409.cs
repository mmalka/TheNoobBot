/* Use Item With HotSpots on corpse after killing the mob
   Check if there is HotSpots in the objective */
if (questObjective.Hotspots.Count <= 0)
{
	/* Objective CSharpScript with script UseItemWithHotSpots requires valid "Hotspots" */
	Logging.Write("UseItemWithHotSpots requires valid 'HotSpots'.");
	questObjective.IsObjectiveCompleted = true;
	return false;
}

/* Move to Zone/Hotspot */
if (!MovementManager.InMovement)
{
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
  
WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (unit.IsValid || node.IsValid)
{
	/* Entry found, GoTo */
	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */
	
	while((node.IsValid && ObjectManager.Me.Position.DistanceTo(node.Position) >= questObjective.Range) || (unit.IsValid && ObjectManager.Me.Position.DistanceTo(unit.Position) >= questObjective.Range))
	{	
		var listEntry = new List<int>(); //Cant use questObjective.Entry in the predicate
		listEntry = questObjective.Entry;
		if((ObjectManager.Me.InCombat && !questObjective.IgnoreFight && nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer().FindAll(x=> !listEntry.Contains(x.Entry)).Count > 0) || ObjectManager.Me.IsDeadMe)
			return false;
		if(node.IsValid)
		{
			baseAddress = MovementManager.FindTarget(node, questObjective.Range);
		}
		if(unit.IsValid)
		{
			if(!unit.IsDead)
				nManager.Wow.Helpers.Fight.StartFight(unit.Guid);
			baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
		}
		Thread.Sleep(500);
	}
	
	/* Target Reached */
	
	MovementManager.StopMove();
	MountTask.DismountMount();
	
	if (questObjective.IgnoreFight)
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;

	//Ignore blacklist to True since we want to get the mob that was killed
	WoWUnit unitDead = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, true), questObjective.IgnoreNotSelectable, true, questObjective.AllowPlayerControlled);

	
	if (node.IsValid)
	{
		MovementManager.Face(node);
		Interact.InteractWith(node.GetBaseAddress);
	}
	else if (unitDead.IsValid)
	{
		MovementManager.Face(unitDead);
		Interact.InteractWith(unitDead.GetBaseAddress);
	}
	
	MovementManager.StopMove();
	MountTask.DismountMount();
	
	if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
		return false;

	ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
	
	Thread.Sleep(Usefuls.Latency +1500);
	
	/* Wait for the Use Item cast to be finished, if any */
	while (ObjectManager.Me.IsCast)
	{
		Thread.Sleep(Usefuls.Latency);
	}
	
	if (node.IsValid)
	{
		nManagerSetting.AddBlackList(node.Guid, 30*1000);
	}
	else if (unit.IsValid)
	{
		nManagerSetting.AddBlackList(unitDead.Guid, 30*1000);
	}
	
	Thread.Sleep(questObjective.WaitMs);
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	
}
