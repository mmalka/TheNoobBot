/*Custom script escort script to check if Lilian is still with us
 If she is not (dead or disapeared), remove quest and start again*/

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
		while(unit.Position.DistanceTo(ObjectManager.Me.Position) >=15 && unit.IsValid && !unit.IsDead && !unit.InCombat)
		{
			if(ObjectManager.Me.IsDeadMe)
				return false;
			if(unit.InCombat)
				break; //Exit loop to kill unit target
			Thread.Sleep(500);
			//Refresh unit
		//	unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	//questObjective.AllowPlayerControlled);
		}
	}
	
	MovementManager.Go(PathFinder.FindPath(ObjectManager.Me.Position,questObjective.Position));
	
	while(MovementManager.InMovement && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > 5f && unit.IsValid && !unit.IsDead && !unit.InCombat)
	{	
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
	
	WoWUnit hostile = new WoWUnit(0);
	
	if(unit.InCombat)
	{
		Logging.Write("Defend Unit");
		if(unit.Target > 0) //Vivian or Lilian ...sometimes pulls aggro without fighting back, so her Target is null. 
		{
			nManager.Wow.Helpers.Fight.StartFight(unit.Target);
		}
		else //Check for hostiles attacking her
		{	
			hostile = nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitNearPlayer()
			.FindAll(x => x.GetDistance <= 20 && x.IsHostile)
			.Find(x => x.Target == ObjectManager.Me.Guid || x.Target == unit.Guid);
			
			if(hostile != null && hostile.IsValid)
			{				
				nManager.Wow.Helpers.Fight.StartFight(hostile.Guid);
			}
		}
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
else //Lilian is dead, delete quest and restart 
{
	if(!nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(25046) && !nManager.Wow.Helpers.Quest.IsQuestFlaggedCompletedLUA(25046))
	{
	nManager.Wow.Helpers.Quest.AbandonQuest(25046); //TODO TRUE
	questObjective.IsObjectiveCompleted = true;
	}
}

return true;