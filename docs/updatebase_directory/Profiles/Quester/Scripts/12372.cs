/* Use Item With HotSpots
Check if there is HotSpots in the objective */

if (questObjective.IgnoreFight)
			nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
		
int idmob = 0;

while(!nManager.Wow.Helpers.Quest.IsObjectiveCompleted(12372, 2, 5) || !nManager.Wow.Helpers.Quest.IsObjectiveCompleted(12372, 1, 3))
{	
	idmob = !nManager.Wow.Helpers.Quest.IsObjectiveCompleted(12372, 1, 3) ? 27608 : 27682;
		
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(idmob, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
	Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
	/* If Entry found continue, otherwise continue checking around HotSpots */
	if (unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList ))
	{
		
		Logging.Write("Unit Found");
		/* Entry found, GoTo */
		
		while(unit.IsValid && unit.Position.DistanceTo(ObjectManager.Me.Position) > 20f)
		{
			ClickToMove.CGPlayer_C__ClickToMove(unit.Position.X, unit.Position.Y, unit.Position.Z, 0,
                                (int) ClickToMoveType.Move, 0.5f);
			Thread.Sleep(500);
		}
		
		Thread.Sleep(500);
		
		
			
		

		/* Target Reached */
		MovementManager.StopMove();
		MountTask.DismountMount();
		
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
		

		MovementManager.Face(unit);
		
		Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

		/* Target Reached */
		MovementManager.StopMove();
		MountTask.DismountMount();

	
		/* Wait for the Use Item cast to be finished, if any */
		while (unit.IsValid && !unit.IsDead)
		{
			Lua.RunMacroText("/click OverrideActionBarButton3");
			Thread.Sleep(500);
			Lua.RunMacroText("/click OverrideActionBarButton1");
			Thread.Sleep(Usefuls.Latency);
		}

		
		nManagerSetting.AddBlackList(unit.Guid, 30*1000);
		

		/* Wait if necessary */
		if (questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);

		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
		
	}
}
//Index 1 and 2 done going for the cratere

	Point arrivalB = new Point(3147.319f, 491.8316f, 43.53974f);
	Point arrivalA = new Point(3197.59f, 476.4211f, 65.86134f);
	List<Point> list = nManager.Wow.Helpers.PathFinder.FindPath(ObjectManager.Me.Position, arrivalA);
	var oWoot = ObjectManager.GetObjectByGuid(ObjectManager.Me.TransportGuid);
	WoWUnit woot = new WoWUnit(0);
	
	if(oWoot is WoWUnit)
	{
		woot = oWoot as WoWUnit;
	}
	
	foreach(Point p in list)
	{
		ClickToMove.CGPlayer_C__ClickToMove(p.X, p.Y, 110, 0,
                                (int) ClickToMoveType.Move, 0.5f);
		Thread.Sleep(500);
		while(woot.GetMove){Thread.Sleep(500);}
		Lua.RunMacroText("/click OverrideActionBarButton3");
		
	}
	Thread.Sleep(1500);
	
	list = nManager.Wow.Helpers.PathFinder.FindPath(ObjectManager.Me.Position, arrivalB);
	
	foreach(Point p in list)
	{
		ClickToMove.CGPlayer_C__ClickToMove(p.X, p.Y, p.Z, 0,
                                (int) ClickToMoveType.Move, 0.5f);
		Thread.Sleep(500);
		while(woot.GetMove){Thread.Sleep(500);}
		Lua.RunMacroText("/click OverrideActionBarButton3");
		
	}
	
	Lua.RunMacroText("/click OverrideActionBarButton6");
	
	while(woot.IsCast) {Thread.Sleep(500);}
	
	Thread.Sleep(5000);
	
	nManager.Wow.Helpers.Usefuls.EjectVehicle();
	

return true;