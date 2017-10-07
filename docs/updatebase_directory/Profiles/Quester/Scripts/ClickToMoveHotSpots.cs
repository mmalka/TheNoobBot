if(!MovementManager.InMovement)
{
	
	questObjective.Position = ObjectManager.Me.Position;
	//Logging.Write(questObjective.Hotspots[listP.Count-1].DistanceTo(ObjectManager.Me.Position).ToString());
	if (questObjective.Position.IsValid && questObjective.Hotspots[questObjective.Hotspots.Count-1].DistanceTo(ObjectManager.Me.Position) > 5f)
	{ 
		if (questObjective.IgnoreFight) //Dont ignore fight if we are too far from the mob... 
			nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
			
		Logging.Write("Moving with ClickToMove");
		MountTask.Mount();
		System.Threading.Thread.Sleep(500);
		var listP = new List<Point>();
		listP.Add(ObjectManager.Me.Position);
		listP.AddRange(questObjective.Hotspots);
		MovementManager.Go(listP);
		while(MovementManager.InMovement && questObjective.Hotspots[questObjective.Hotspots.Count-1].DistanceTo(ObjectManager.Me.Position) > 5f)
		{
		    System.Threading.Thread.Sleep(100);
		}
		MovementManager.StopMove();
	}
	if (questObjective.Hotspots[questObjective.Hotspots.Count-1].DistanceTo(ObjectManager.Me.Position) <= 5f)
	{
		Logging.Write("Position Reached");
        questObjective.IsObjectiveCompleted = true; 
		return true;
    }
}