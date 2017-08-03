/* Kill Mobs around unit
Check if there is HotSpots in the objective */

try
{
	WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
	Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
	//int q = QuestID; /* not used but otherwise getting warning QuestID not used */
	uint baseAddress = 0;
	
	
	if(ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 20)
	{
		//Go near the mob
		Logging.Write("Going near Target");
		System.Collections.Generic.List<Point> Path = PathFinder.FindPath(ObjectManager.Me.Position, questObjective.Position);

        MovementManager.Go(Path);
		
		while(MovementManager.InMovement)
		{
			if(ObjectManager.Me.IsDeadMe || ObjectManager.Me.InCombat)
				return false;
		}
	}
	/* If Entry found continue, otherwise continue checking around HotSpots */
	else if ((unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList )) ||
		(node.IsValid && (!nManagerSetting.IsBlackListedZone(node.Position) && !nManagerSetting.IsBlackListed(node.Guid) || questObjective.IgnoreBlackList)))
	{
		if(nManager.Wow.Helpers.PathFinder.FindPath(node.IsValid ? node.Position : unit.Position).Count <= 0)
		{
			nManagerSetting.AddBlackList(node.IsValid ? node.Guid : unit.Guid, 30*1000);
			return false;
		}
		if (questObjective.IgnoreFight)
			nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
		/* Entry found, GoTo */
	
		while(unit.InCombat)
		{
			nManager.Wow.Helpers.Fight.StartFight(unit.Target);
			Thread.Sleep(1000);
		}

		/* Wait if necessary */
		if (questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);

		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
		return true;
	}
}
catch (Exception ex)
{
	Logging.Write(ex.Message);
}
finally
{
	/*nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;*/
}