WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	questObjective.AllowPlayerControlled);
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress = 0;

/* If Entry found continue*/
if (unit.IsValid)
{

	if(questObjective.DeactivateMount)
		MountTask.DismountMount();
	
	//If Unit too far from us, wait for it
	if(unit.Position.DistanceTo(ObjectManager.Me.Position) >=30) 
	{
		MovementManager.StopMove();
		while(unit.Position.DistanceTo(ObjectManager.Me.Position) >=15 && unit.IsValid && !unit.IsDead)
		{
			if(ObjectManager.Me.IsDeadMe)
				return false;
			if(unit.InCombat)
				break; //Exit loop to kill unit target
			Logging.Write("TROP LOIN");
			Thread.Sleep(500);
			//Refresh unit
		//	unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	//questObjective.AllowPlayerControlled);
		}
	}
	
	MovementManager.Go(PathFinder.FindPath(ObjectManager.Me.Position,questObjective.Position));
	Logging.Write("GO");
	while(MovementManager.InMovement && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > 5f && unit.IsValid && !unit.IsDead && !unit.InCombat)
	{	
		Logging.Write("IN WHILE");
		if(unit.Position.DistanceTo(ObjectManager.Me.Position) >=30)
			return false;
		if (ObjectManager.Me.IsDeadMe)
			return false;
		if(unit.InCombat)
			break; //Exit loop to kill unit target
		if(ObjectManager.Me.InCombat)
		{
			MountTask.DismountMount();
			return false; //Let the bot kill the hostiles!
		}	
		Thread.Sleep(500);
		//Refresh unit
		//unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	//questObjective.AllowPlayerControlled);
	}
	
	if(unit.InCombat)
	{
		nManager.Wow.Helpers.Fight.StartFight(unit.Target);
	}

	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

	/* Position Reached */
	MovementManager.StopMove();
	//MountTask.DismountMount();

	Thread.Sleep(Usefuls.Latency + 150);

	/* Wait if necessary */
	if (questObjective.WaitMs > 0)
		Thread.Sleep(questObjective.WaitMs);
	
	return true;
}