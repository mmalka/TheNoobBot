  //Go Get the mammoth!
if (!nManager.Wow.Helpers.Usefuls.PlayerUsingVehicle)
{
	uint baseAddressMams;
	WoWUnit mammoth = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(25743, false), false, false, false);

	if (!mammoth.IsValid)
	{
		//MovementManager.Go(PathFinder.FindPath(ObjectManager.Me.Position,new Point(3469.073f, 5294.998f, 39.33226f)));
		MovementManager.Go(PathFinder.FindPath(ObjectManager.Me.Position, questObjective.Position));

		while (MovementManager.InMovement && !mammoth.IsValid)
		{

			if (ObjectManager.Me.IsDeadMe || ObjectManager.Me.InCombat)
				return false;

			Thread.Sleep(500);
			//Find Mams
			mammoth = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(25743, false), false, false, false);

			if (mammoth.IsValid)
				break;

		}
	}
	else
	{
		//Mams found
		baseAddressMams = MovementManager.FindTarget(mammoth, questObjective.Range);

		if (MovementManager.InMovement)
			return false;

		if (baseAddressMams <= 0)
			return false;
		if (baseAddressMams > 0 && (mammoth.IsValid && mammoth.GetDistance > questObjective.Range))
			return false;

		MovementManager.Face(mammoth);
		Interact.InteractWith(baseAddressMams);
		return false;
	}
}


WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	questObjective.AllowPlayerControlled);
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress = 0;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList))
{
	if (questObjective.IgnoreFight)
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
	/* Entry found, GoTo */

	if (unit.IsValid)
	{
		baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
	}
	Thread.Sleep(500);

	//Pre Select Target
	if (unit.IsValid && unit.Position.DistanceTo(ObjectManager.Me.Position) <= 60 && ObjectManager.Target.Guid != unit.Guid)
	{
		ObjectManager.Me.Target = unit.Guid;
		//Interact.InteractWith(unit.GetBaseAddress);
	}

	if (MovementManager.InMovement)
		return false;

	if (baseAddress <= 0)
		return false;
	if (baseAddress > 0 && (unit.IsValid && unit.GetDistance > questObjective.Range))
		return false;

	Lua.RunMacroText("/click OverrideActionBarButton3");
	while (!unit.IsDead)
	{
		if (ObjectManager.Me.IsDeadMe)
			return false;
		if(unit.Position.DistanceTo(ObjectManager.Me.Position) >= 5)
			MovementManager.MoveTo(unit);
		MovementManager.Face(unit);
		Lua.RunMacroText("/click OverrideActionBarButton1");
		Thread.Sleep(500);
	}
	
	Thread.Sleep(2000 + Usefuls.Latency); /* ZZZzzzZZZzz */

	/* Target Reached */
	MovementManager.StopMove();
	
	nManager.Wow.Helpers.Usefuls.EjectVehicle();
	
	WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(188066));
	
	Thread.Sleep(500);
	
	while(node.GetDistance > questObjective.Range)
	{
		MovementManager.FindTarget(node, questObjective.Range);
		Thread.Sleep(500);
	}
	
	if (node.IsValid)
	{	
		MovementManager.Face(node);
		Interact.InteractWith(node.GetBaseAddress);
		Thread.Sleep(500);
		Interact.InteractWith(node.GetBaseAddress);
		Thread.Sleep(500);
		Interact.InteractWith(node.GetBaseAddress);
		Thread.Sleep(500);
	}
	
	Thread.Sleep(Usefuls.Latency + 250);

	/* Wait for the Use Item cast to be finished, if any */
	while (ObjectManager.Me.IsCast)
	{
		Thread.Sleep(Usefuls.Latency);
	}
	
	Thread.Sleep(Usefuls.Latency + 250);
	
	
	
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
		nManager.Wow.Helpers.Quest.TravelToQuestZone(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)],
			ref questObjective.TravelToQuestZone, questObjective.ContinentId, questObjective.ForceTravelToQuestZone);
		MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		MovementManager.GoLoop(questObjective.PathHotspots);
	}
}