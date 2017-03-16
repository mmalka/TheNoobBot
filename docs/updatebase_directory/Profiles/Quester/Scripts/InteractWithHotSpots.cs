/* Check if there is HotSpots in the objective */
if (questObjective.Hotspots.Count <= 0)
{
	/* Objective CSharpScript with script InteractWithHotSpots requires valid "Hotspots" */
	Logging.Write("InteractWithHotSpots requires valid 'HotSpots'.");
	questObjective.IsObjectiveCompleted = true;
	return false;
}

if (questObjective.IgnoreFight)
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;

/* Search for Entry */
WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */;
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress = 0;

/* If Entry found continue, otherwise continue checking around HotSpots */
if ((unit.IsValid && !nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid)) || (node.IsValid && !nManagerSetting.IsBlackListedZone(node.Position) && !nManagerSetting.IsBlackListed(node.Guid)))
{

	/* Entry found, GoTo */
	if(node.IsValid)
	{
		unit = new WoWUnit(0);
		baseAddress = MovementManager.FindTarget(node, questObjective.Range);
	}
	else if(unit.IsValid)
	{
		node = new WoWGameObject(0);
		baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
	}
	Thread.Sleep(500);
	
	if (MovementManager.InMovement)
        return false;
	if (questObjective.IgnoreNotSelectable)
	{
	    if ((node.IsValid && node.GetDistance > questObjective.Range) || (unit.IsValid && unit.GetDistance > questObjective.Range))
	        return false;
	}
	else
	{
        if(baseAddress <= 0)
	        return false;
        if (baseAddress > 0 && ((node.IsValid && node.GetDistance > questObjective.Range) || (unit.IsValid && unit.GetDistance > questObjective.Range)))
	    return false;
	}
  
	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

	/* Entry reached, dismount */
	MovementManager.StopMove();
	MountTask.DismountMount();
	
	/* Interact With Entry */
	if (node.IsValid)
	{
		MovementManager.Face(node);
		Interact.InteractWith(node.GetBaseAddress);
		nManagerSetting.AddBlackList(node.Guid, 30*1000);
	}
	else if (unit.IsValid)
	{
		MovementManager.Face(unit);
		Interact.InteractWith(unit.GetBaseAddress);
		nManagerSetting.AddBlackList(unit.Guid, 30*1000);
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
	
	/* Wait if necessary */
	if(questObjective.WaitMs > 0)
		Thread.Sleep(questObjective.WaitMs);

	/* Interact Completed - InternalIndex and count is used to determine if obj is completed - questObjective.IsObjectiveCompleted = true; */

	
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