WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(20159, questObjective.IsDead), questObjective.IgnoreNotSelectable, true, questObjective.AllowPlayerControlled);
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress = 0;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (unit.IsValid)
{
	Logging.Write("DEBUT");
	if (questObjective.IgnoreFight)
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
	/* Entry found, GoTo */

	if (unit.IsValid)
	{
		baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
	}
	Thread.Sleep(500);

	if (MovementManager.InMovement)
		return false;
		
	if (questObjective.IgnoreNotSelectable)
	{
		if (unit.IsValid && unit.GetDistance > questObjective.Range)
			return false;
	}
	else
	{
		if (baseAddress <= 0)
			return false;
		if (baseAddress > 0 && (unit.IsValid && unit.GetDistance > questObjective.Range))
			return false;
	}

	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

	/* Target Reached */
	MovementManager.StopMove();
	MountTask.DismountMount();

	
	MovementManager.Face(unit);
	Interact.InteractWith(unit.GetBaseAddress);
	
	/* Do Gossip if necessary */
	Logging.Write("Gossip");
	if (questObjective.GossipOptionsInteractWith != 0)
	{
		Thread.Sleep(250 + Usefuls.Latency);
		nManager.Wow.Helpers.Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
	}
	Thread.Sleep(250 + Usefuls.Latency);
	
	if (questObjective.GossipOptionsInteractWith != 0)
	{
		Thread.Sleep(250 + Usefuls.Latency);
		nManager.Wow.Helpers.Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
	}
	
	if(!unit.IsHostile && !unit.CanTurnIn)
	{	
		return false;
	}	
	else if(unit.IsHostile && !unit.CanTurnIn)
	{
		Logging.Write("Start Fight");
		nManager.Wow.Helpers.Fight.StartFight(unit.Guid);
		Thread.Sleep(750 + Usefuls.Latency);
	}
	
	Logging.Write("Fight End");
	/*if(unit.HealthPercent >=20) 
		return false;*/
		
	Logging.Write("HealthLow");
	
	while(!unit.CanTurnIn)
	{
		Thread.Sleep(2000);
		unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(20159, questObjective.IsDead), questObjective.IgnoreNotSelectable, true, questObjective.AllowPlayerControlled);
	}
	
	Logging.Write("Turn In");
	/*Npc _npc = nManager.Wow.Helpers.QuestersDB.GetNpcByEntry(20159);
	nManager.Wow.Helpers.Quest.QuestTurnIn(ref _npc, "Arelion's Secret", 10286);
	
	if(nManager.Wow.Helpers.Quest.GetLogQuestId().Contains(10286))
	{
		return false;
	}*/
	
	/* Wait if necessary */
	if (questObjective.WaitMs > 0)
		Thread.Sleep(questObjective.WaitMs);
	
	questObjective.IsObjectiveCompleted = true;
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	Logging.Write("FIN");
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
