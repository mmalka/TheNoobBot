/* Use item to kill Garnoth
   Ignore Fight otherwise the CC will make the script bug
*/

try
{
	if (questObjective.Hotspots.Count <= 0)
	{
		/* Objective CSharpScript with script UseItemWithHotSpots requires valid "Hotspots" */
		Logging.Write("UseItemWithHotSpots requires valid 'HotSpots'.");
		questObjective.IsObjectiveCompleted = true;
		return false;
	}

	if(questObjective.Range == 0)
		questObjective.Range = 5;
	
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
	Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
	//int q = QuestID; /* not used but otherwise getting warning QuestID not used */
	uint baseAddress = 0;

	/* If Entry found continue, otherwise continue checking around HotSpots */
	if (unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList ))
	{
		if(nManager.Wow.Helpers.PathFinder.FindPath(unit.Position).Count <= 0)
		{
			nManagerSetting.AddBlackList(unit.Guid, 30*1000);
			return false;
		}
		
		if (questObjective.IgnoreFight && unit.GetDistance <= 35) //Dont ignore fight if we are too far from the mob... 
			nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
		/* Entry found, GoTo */
	
		//Pre Select Target
		if (unit.Position.DistanceTo(ObjectManager.Me.Position) <= 60 && ObjectManager.Target.Guid != unit.Guid)
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
	
		if(unit.IsValid && unit.GetDistance <= questObjective.Range)
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
		
		/* Target Reached */
		MovementManager.StopMove();
		MountTask.DismountMount();
				
		MovementManager.Face(unit);
	
		Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

		

		if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
			return false;
		
		
		ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));

		Thread.Sleep(Usefuls.Latency + 250);
		
		Interact.InteractWith(unit.GetBaseAddress); //Interact With Unit to Attack it
		
		int i = 4;
		
		while(!unit.IsDead && !ObjectManager.Me.IsDeadMe)
		{
			Lua.RunMacroText("/click OverrideActionBarButton1");
			Thread.Sleep(1500);
			
			
			if(i >= 4) //Face the mob, in case we are back to it
			{
				Lua.RunMacroText("/click OverrideActionBarButton2"); //Cast shield from time to time
				MovementManager.Face(unit);
				i = 0;
			}
			i = i + 1;
		}

		
		nManagerSetting.AddBlackList(unit.Guid, 60*1000);

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