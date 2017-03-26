
if(questObjective.Hotspots.Count > 0)
{	
	//nManagerSetting.CurrentSetting.ActivatePathFindingFeature = false;
	
	if(ObjectManager.Me.IsMounted)
		MountTask.DismountMount();
	
	foreach (Point hotspot in questObjective.Hotspots)
	{
		Logging.Write(hotspot.ToString());
		int iExtra = 0;
		int iHS = 0;
		if (questObjective.ExtraPoint.IsValid)
		{
			Point Extra = questObjective.ExtraPoint;
			iHS = questObjective.Hotspots.FindIndex(x => x == hotspot);
			iExtra = questObjective.Hotspots.FindIndex(x => x == Extra);
			if (iHS <= iExtra)
			{
				continue;
			}
		}
		
		ClickToMove.CGPlayer_C__ClickToMove(hotspot.X, hotspot.Y, hotspot.Z, 0,
							(int)ClickToMoveType.Move, 0.5f);

		Thread.Sleep(100);

		while (ObjectManager.Me.PositionAbsolute.DistanceTo2D(hotspot) >= 2)
		{
			Thread.Sleep(100);
		}
		
		if(ObjectManager.Me.InCombat)
		{
			Logging.Write("In Combat");
			questObjective.ExtraPoint = hotspot;
			nManager.Wow.Helpers.MovementManager.Face(ObjectManager.Target);
			return false;
		}
		
		if(questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);
		
		questObjective.ExtraPoint = new Point();
	}
	
}else if(questObjective.Position.IsValid)
{
	ClickToMove.CGPlayer_C__ClickToMove(questObjective.Position.X, questObjective.Position.Y, questObjective.Position.Z, 0,
								(int) ClickToMoveType.Move, 0.5f);
								
	while(MovementManager.InMovement)
	{
		Thread.Sleep(500);
	}
}

