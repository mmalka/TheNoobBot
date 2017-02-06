/* UseItem HotSpots (ClickToMove)! */
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
  
	if (questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
	{
		MovementManager.Go(PathFinder.FindPath(questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		Logging.Write("LOOP");
		MovementManager.GoLoop(questObjective.Hotspots);
	}
}

/* Search for Entry */
WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
Point pos;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (node.IsValid)
{
	/* Entry found, GoTo */
		
	var listP = new List<Point>();
	listP.Add(ObjectManager.Me.Position);
	listP.Add(node.Position);
	MovementManager.Go(listP);
	while(MovementManager.InMovement && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > 5f)
	{
		System.Threading.Thread.Sleep(100);
		if(ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
			return false;
	}
		

	/* Entry reached, stop and dismount */
	MovementManager.StopMove();
	MountTask.DismountMount();
   
	if (questObjective.IgnoreFight)
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
	
	/* Use item on Node  */
	ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
	Thread.Sleep(Usefuls.Latency); 
	/* Wait for the interact cast to be finished, if any */
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
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
}