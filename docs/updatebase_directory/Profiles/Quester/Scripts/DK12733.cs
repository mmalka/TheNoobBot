WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));
Point pos;
uint baseAddress;
if (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) && unit.IsValid)
{
	if (unit.IsValid)
	{
		pos = new Point(unit.Position);
		baseAddress = unit.GetBaseAddress;
	}
	else
	{
		if (questObjective.InternalQuestId > 0)
		{
			if (!nManager.Wow.Helpers.Quest.GetLogQuestId().Contains(questObjective.InternalQuestId))
				questObjective.IsObjectiveCompleted = true;
		}
		return true;
	}
	MovementManager.Go(PathFinder.FindPath(pos));
	Thread.Sleep(500 + Usefuls.Latency);
	while (MovementManager.InMovement && pos.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 3.9f)
	{
		if (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted))
			return false;
		Thread.Sleep(100);
	}

	MountTask.DismountMount();
	MovementManager.StopMove();
	Interact.InteractWith(baseAddress);
	Thread.Sleep(Usefuls.Latency);
	while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
	{
		Thread.Sleep(Usefuls.Latency);
	}

	if (questObjective.GossipOptionsInteractWith != 0)
	{
		Thread.Sleep(250 + Usefuls.Latency);
		nManager.Wow.Helpers.Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
	}
	
	Logging.Write("Waiting Duel");
	while(!nManager.Wow.ObjectManager.ObjectManager.Target.IsHostile)

	Thread.Sleep(500);
	Logging.Write("Start Fight");
	Fight.StartFight(unit.Guid);
	
	while(nManager.Wow.ObjectManager.ObjectManager.Target.IsHostile)
	{
		Fight.StartFight(unit.Guid);
		Thread.Sleep(500);
	}
		
	nManagerSetting.AddBlackList(unit.Guid, 5000);
}
else if (!MovementManager.InMovement)
{
	/* Move to Zone/Hotspot */
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