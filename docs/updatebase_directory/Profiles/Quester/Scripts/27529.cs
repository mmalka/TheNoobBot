/*Interact With HotSpots on ghost after killing the mob
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
		nManager.Wow.Helpers.Quest.TravelToQuestZone(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)], ref questObjective.TravelToQuestZone, questObjective.ContinentId,questObjective.ForceTravelToQuestZone);
		MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		MovementManager.GoLoop(questObjective.PathHotspots);
	}
}
  
WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
WoWUnit unitGhost = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(11064, false), false, false, false);

Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (unit.IsValid || node.IsValid || unitGhost.IsValid)
{
	/* Entry found, GoTo */
	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */
	
	while((node.IsValid && ObjectManager.Me.Position.DistanceTo(node.Position) >= questObjective.Range) || (unit.IsValid && ObjectManager.Me.Position.DistanceTo(unit.Position) >= questObjective.Range))
	{	
		if((ObjectManager.Me.InCombat && !questObjective.IgnoreFight) || ObjectManager.Me.IsDeadMe)
			return false;
		if(node.IsValid)
		{
			baseAddress = MovementManager.FindTarget(node, questObjective.Range);
		}
		if(unit.IsValid)
		{
			baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
		}
		Thread.Sleep(500);
	}
	
	/* Target Reached */
	
	MovementManager.StopMove();
	MountTask.DismountMount();
	
	if (questObjective.IgnoreFight)
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
	
	if (node.IsValid)
	{
		MovementManager.Face(node);
		Interact.InteractWith(node.GetBaseAddress);
		nManagerSetting.AddBlackList(node.Guid, 30*1000);
	}
	else if (unitGhost.IsValid)
	{
		MovementManager.Face(unitGhost);
		Interact.InteractWith(unitGhost.GetBaseAddress);
		nManagerSetting.AddBlackList(unitGhost.Guid, 30*1000);
	}
	
	Thread.Sleep(Usefuls.Latency); 

	/* Wait for the interact cast to be finished, if any */
	while (ObjectManager.Me.IsCast)
	{
		Thread.Sleep(Usefuls.Latency);
	}

	/* Do Gossip if necessary */
	if (questObjective.GossipOptionsInteractWith != 0)
	{
		Thread.Sleep(250 + Usefuls.Latency);
		nManager.Wow.Helpers.Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
	}

	if (Others.IsFrameVisible("StaticPopup1Button1"))
		Lua.RunMacroText("/click StaticPopup1Button1");
	if (ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
		return false;
	
	
	Thread.Sleep(questObjective.WaitMs);
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	
}
