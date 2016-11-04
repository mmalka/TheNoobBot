/* Check if there is HotSpots in the objective */
if (questObjective.Hotspots.Count <= 0)
{
	/* Objective CSharpScript with script InteractWithHotSpots requires valid "Hotspots" */
	Logging.Write("InteractWithHotSpots requires valid 'HotSpots'.");
	questObjective.IsObjectiveCompleted = true;
	return false;
}

/* Move to Zone/Hotspot */
if (!MovementManager.InMovement)
{
  
	if (questObjective.Hotspots[Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
	{
		MovementManager.Go(PathFinder.FindPath(questObjective.Hotspots[Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		MovementManager.GoLoop(questObjective.Hotspots);
	}
}

/* Search for Entry */
WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
WoWUnit unit =ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));
Point pos;
uint baseAddress;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (node.IsValid || unit.IsValid)
{
	/* Entry found, GoTo */
	if(unit.IsValid)
	{
		baseAddress = MovementManager.FindTarget(unit); /* Move toward unit */
	}
	else if (node.IsValid) 
	{
		baseAddress = MovementManager.FindTarget(node); /* Move toward node */
	}
  
	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

	if (MovementManager.InMovement)
		return false; /* Waiting to reach Entry */
		
	/* Entry reached, dismount */
	MovementManager.StopMove();
	MountTask.DismountMount();
   
	if (questObjective.IgnoreFight)
		Quest.GetSetIgnoreFight = true;
	
	/* Interact With Entry */
	Interact.InteractWith(baseAddress);
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
		Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
	}

	if (Others.IsFrameVisible("StaticPopup1Button1"))
		Lua.RunMacroText("/click StaticPopup1Button1");
	if (ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
		return false;
	
	/* Wait if necessary */
	if(questObjective.WaitMs > 0)
	Thread.Sleep(questObjective.WaitMs);

	/* Interact Completed - InternalIndex and count is used to determine if obj is completed - questObjective.IsObjectiveCompleted = true; */
	Quest.GetSetIgnoreFight = false;
}