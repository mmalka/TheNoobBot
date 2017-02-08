if (!MovementManager.InMovement)
{
	if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) < questObjective.Range)
	{
		WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
		Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
		int q = QuestID; /* not used but otherwise getting warning QuestID not used */
		
		uint baseAddress = 0;
		if (node.IsValid)
		{
			pos = new Point(node.Position);
			baseAddress = node.GetBaseAddress;
		
		}
	
		MovementManager.Go(PathFinder.FindPath(pos));
		Thread.Sleep(500 + Usefuls.Latency);
		while (MovementManager.InMovement && pos.DistanceTo(ObjectManager.Me.Position) > 3.9f)
		{
			if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
				return true;
			Thread.Sleep(100);
		}
	
		MountTask.DismountMount();
		MovementManager.StopMove();
		Interact.InteractWith(baseAddress);
		Thread.Sleep(Usefuls.Latency);
		while (ObjectManager.Me.IsCast)
		{
			Thread.Sleep(Usefuls.Latency);
		}
		
		if (Others.IsFrameVisible("QuestChoiceFrameOption1.OptionButton"))
			Logging.Write("Click");
			nManager.Wow.Helpers.Lua.LuaDoString("SendQuestChoiceResponse(546);");
			/* optionID, _, _, _= GetQuestChoiceOptionInfo(1);
			print(optionID);*/
		if (ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
			return true;
		Thread.Sleep(questObjective.WaitMs);
		questObjective.IsObjectiveCompleted = true;
	}
	else
	{
		MountTask.Mount();
		MovementManager.Go(PathFinder.FindPath(questObjective.Position));
	}
}