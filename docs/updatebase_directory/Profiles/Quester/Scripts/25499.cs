	if(!MovementManager.InMovement)
	{
		if (questObjective.IgnoreFight) //Dont ignore fight if we are too far from the mob... 
				nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
				
		Npc quester = nManager.Wow.Helpers.QuestersDB.GetNpcByEntry(39413);
		
		nManager.Wow.Helpers.Quest.QuestPickUp(ref quester, "Agility Training: Run Like Hell!", 25499);
		
		Thread.Sleep(1500);
		
		Logging.Write("PAS QUETE");
		if(!nManager.Wow.Helpers.Quest.GetLogQuestId().Contains(25499))
			return false;
		
		Logging.Write("GOOD JAI LA QUETE");
		/*if(unit.IsValid && unit.GetDistance <= questObjective.Range)
		{
			//Logging.Write("TARGET REACHED" + unit.GetDistance);
			/* Target Reached *//*
			MovementManager.StopMove();
			MountTask.DismountMount();
		}
		else if (MovementManager.InMovement)
			return false;
		*/
		
				
		//Interact.InteractWith(unit.GetBaseAddress); //Interact With Unit to Attack it
		
		Thread.Sleep(1000);
		
		

		questObjective.Position = ObjectManager.Me.Position;
		
		while(!(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(25499) || nManager.Wow.Helpers.Quest.GetQuestCompleted(25499)) && !ObjectManager.Me.IsDeadMe)
			LoopHotSpots(questObjective);
		
		return true;
	}
} //Close Try
	catch (Exception e)
	{
		Logging.WriteError("Script: " + e);
	}
	return false;
} //Close Function

public static void LoopHotSpots(Quester.Profile.QuestObjective questObjective)
{
			
	Logging.Write("Moving with ClickToMove");
	MountTask.Mount();
	System.Threading.Thread.Sleep(500);
	var listP = new List<Point>();
	listP.Add(ObjectManager.Me.Position);
	listP.AddRange(questObjective.Hotspots);
	MovementManager.Go(listP);
	while(MovementManager.InMovement)
	{	
		System.Threading.Thread.Sleep(100);
		if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(25499) || nManager.Wow.Helpers.Quest.GetQuestCompleted(25499))
			break;
	}
	MovementManager.StopMove();
	return;
}

public static bool random() //not used, just to close the script
{
	try //Bim Try open random !!!
	{