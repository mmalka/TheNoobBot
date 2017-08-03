/* Kill Mobs around unit to make it out of combat, then use item on it
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
		
		if (node.IsValid)
		{
			unit = new WoWUnit(0);
			baseAddress = MovementManager.FindTarget(node, questObjective.Range);
		}
		if (unit.IsValid)
		{
			node = new WoWGameObject(0);
			baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
		}
		Thread.Sleep(500);
		
	
		if((node.IsValid && node.GetDistance < questObjective.Range) || (unit.IsValid && unit.GetDistance < questObjective.Range))
		{
			/* Target Reached */
			MovementManager.StopMove();
			MountTask.DismountMount();
		}
		else
		{
			if (MovementManager.InMovement)
				return false;
			if (questObjective.IgnoreNotSelectable)
			{
				if ((node.IsValid && node.GetDistance > questObjective.Range) || (unit.IsValid && unit.GetDistance > questObjective.Range))
					return false;
			}
			else
			{
				if (baseAddress <= 0)
					return false;
				if (baseAddress > 0 && ((node.IsValid && node.GetDistance > questObjective.Range) || (unit.IsValid && unit.GetDistance > questObjective.Range)))
					return false;
				
			}
		}
		
		
		//Pre Select Target
		if (node.IsValid && node.Position.DistanceTo(ObjectManager.Me.Position) <= 60 && ObjectManager.Target.Guid != node.Guid)
		{
			Interact.InteractWith(node.GetBaseAddress);
		}
		else if (unit.IsValid && unit.Position.DistanceTo(ObjectManager.Me.Position) <= 60 && ObjectManager.Target.Guid != unit.Guid)
		{	
			Lua.LuaDoString("ClearTarget()");
			
			if(questObjective.ExtraString == "InteractWith")
			{
				Interact.InteractWith(unit.GetBaseAddress);
			}
			else
			{
				ObjectManager.Me.Target = unit.Guid;
			}
		}
		
		if (node.IsValid)
		{
			MovementManager.Face(node);
		}
		else if (unit.IsValid)
		{
			MovementManager.Face(unit);
		}
		
		Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

		/* Target Reached */
		MovementManager.StopMove();
		MountTask.DismountMount();

		if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
			return false;
		
		
		ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));

		Thread.Sleep(Usefuls.Latency + 250);

		/* Wait for the Use Item cast to be finished, if any */
		while (ObjectManager.Me.IsCast)
		{
			Thread.Sleep(Usefuls.Latency);
		}

		if (node.IsValid)
		{
			nManagerSetting.AddBlackList(node.Guid, 60*1000);
		}
		else if (unit.IsValid)
		{
			
			Interact.InteractWith(unit.GetBaseAddress); //Interact With Unit to Attack it
			nManagerSetting.AddBlackList(unit.Guid, 60*1000);
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