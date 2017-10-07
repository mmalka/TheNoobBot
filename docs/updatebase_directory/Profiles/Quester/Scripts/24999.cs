try
{
	if (questObjective.Hotspots.Count <= 0)
	{
		Logging.Write("UseItemWithHotSpots requires valid 'HotSpots'.");
		questObjective.IsObjectiveCompleted = true;
		return false;
	}

	if(questObjective.Range == 0)
		questObjective.Range = 5;
	
	WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
	Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
	//int q = QuestID; /* not used but otherwise getting warning QuestID not used */
	uint baseAddress = 0;

	/* If Entry found continue, otherwise continue checking around HotSpots */
	if ((unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList )) ||
		(node.IsValid && (!nManagerSetting.IsBlackListedZone(node.Position) && !nManagerSetting.IsBlackListed(node.Guid) || questObjective.IgnoreBlackList)))
	{
		if(nManager.Wow.Helpers.PathFinder.FindPath(node.IsValid ? node.Position : unit.Position).Count <= 0)
		{
			nManagerSetting.AddBlackList(node.IsValid ? node.Guid : unit.Guid, 30*1000);
			return false;
		}
		
		if (questObjective.IgnoreFight && unit.GetDistance <= 20) //Dont ignore fight if we are too far from the mob... 
			nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
		/* Entry found, GoTo */
	
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

		baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
		
		if((node.IsValid && node.GetDistance < questObjective.Range) || (unit.IsValid && unit.GetDistance <= questObjective.Range))
		{
			//Logging.Write("TARGET REACHED" + unit.GetDistance);
			/* Target Reached */
			MovementManager.StopMove();
			MountTask.DismountMount();
		}
		else
		{
			
			if (MovementManager.InMovement)
				return false;
			if(ObjectManager.Target.BuffStack(73133) < 3)
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
		
		/* Target Reached */
		MovementManager.StopMove();
		
		bool stunned = false;
		int currentCountStack = 0;
		bool loop = true;
		
		while(loop)
		{
			stunned = unit.GetDescriptor<UnitFlags>(nManager.Wow.Patchables.Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Stunned);
			
			if(unit.BuffStack(73133) == 2)
			{
				currentCountStack = 2;
			}
			
			if(currentCountStack == 2 && unit.BuffStack(73133) == 1) //Unit feared successfully
			{
				loop = false;
				break;
			}
			
			if(ObjectManager.Me.IsDeadMe || ObjectManager.Me.InCombat ) //In combat or dead, return
			{
				return false;
			}
			
			if(stunned) //murloc stunned, break
			{
				nManagerSetting.AddBlackList(unit.Guid, 60*1000);
				stunned = true;
				break;
			}
			
			ClickToMove.CGPlayer_C__ClickToMove(unit.Position.X, unit.Position.Y, unit.Position.Z, 0,
									(int) ClickToMoveType.Move, 0.5f);

			while(MovementManager.InMovement)
			{
				Thread.Sleep(50);
			}
		
			Thread.Sleep(50);
		}	

		nManagerSetting.AddBlackList(unit.Guid, 60*1000);
	}

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