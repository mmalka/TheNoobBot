/* Kill mob first,  then Use Item  at dead mob
this is a quick reverse on the,  use item then kill mob.


Check if there is HotSpots in the objective */


try
{
	if (questObjective.Hotspots.Count <= 0)
	{
		/* Objective CSharpScript with script UseItemWithHotSpots requires valid "Hotspots" */
		Logging.Write("KillMobThenUseItem requires valid 'HotSpots'.");
		questObjective.IsObjectiveCompleted = true;
		return false;
	}

	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
	Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
	uint baseAddress = 0;

	/* If Entry found continue, otherwise continue checking around HotSpots */
	if (unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList ))
	{
		if(nManager.Wow.Helpers.PathFinder.FindPath(unit.Position).Count <= 0)
		{
			nManagerSetting.AddBlackList(unit.Guid, 30*1000);
			return false;
		}
		
		if (questObjective.IgnoreFight)
			nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
		/* Entry found, GoTo */
		
		baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
		
		Thread.Sleep(500);
		
		//Pre Select Target
		if (unit.IsValid && unit.Position.DistanceTo(ObjectManager.Me.Position) <= 60 && ObjectManager.Target.Guid != unit.Guid)
		{
			ObjectManager.Me.Target = unit.Guid;
			//Interact.InteractWith(unit.GetBaseAddress);
		}
			
		if(unit.IsValid && unit.GetDistance < questObjective.Range)
		{
			Logging.Write("TARGET REACHED");
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
				if (unit.IsValid && unit.GetDistance > questObjective.Range)
					return false;
			}
			else
			{
				if (baseAddress <= 0)
					return false;
				if (baseAddress > 0 && (unit.IsValid && unit.GetDistance > questObjective.Range))
					return false;
				
			}
		}
		
				
				
			
		MovementManager.Face(unit);
		
		Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

		Interact.InteractWith(unit.GetBaseAddress); //Interact With Unit to Attack it
		
		Fight.StartFight(unit.Guid);


// possibility of range difference?
// to review later.


		if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
			return false;

		ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));

		Thread.Sleep(Usefuls.Latency + 250);

		/* Wait for the Use Item cast to be finished, if any */
		while (ObjectManager.Me.IsCast)
		{
			Thread.Sleep(Usefuls.Latency);
		}


			
		nManagerSetting.AddBlackList(unit.Guid, 30*1000);


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
}
catch (Exception ex)
{
	Logging.Write(ex.Message);
}
finally
{
	/*nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;*/
}