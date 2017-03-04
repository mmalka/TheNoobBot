if(!MovementManager.InMovement)
{
	if (questObjective.Position.IsValid && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > 5f)
	{ 
		Logging.Write("Moving with ClickToMove");
		MountTask.Mount();
		System.Threading.Thread.Sleep(500);
		var listP = new List<Point>();
		listP.Add(ObjectManager.Me.Position);
		listP.Add(questObjective.Position);
		MovementManager.Go(listP);
		while(MovementManager.InMovement && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > 5f)
		{
		    System.Threading.Thread.Sleep(100);
		}
		MovementManager.StopMove();
	}
	if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) <= 5f)
	{
		Logging.Write("Position Reached");
        questObjective.IsObjectiveCompleted = true; 
		return true;
    }
}