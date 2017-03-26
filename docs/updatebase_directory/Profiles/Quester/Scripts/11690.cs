/* Use Item With HotSpots
Check if there is HotSpots in the objective */

try
{
	if (questObjective.Hotspots.Count <= 0)
	{
		/* Objective CSharpScript with script UseItemWithHotSpots requires valid "Hotspots" */
		Logging.Write("UseItemWithHotSpots requires valid 'HotSpots'.");
		questObjective.IsObjectiveCompleted = true;
		return false;
	}

	WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
	Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
	int q = QuestID; /* not used but otherwise getting warning QuestID not used */
	uint baseAddress = 0;

	/* If Entry found continue, otherwise continue checking around HotSpots */
	if ((unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList )) ||
		(node.IsValid && (!nManagerSetting.IsBlackListedZone(node.Position) && !nManagerSetting.IsBlackListed(node.Guid) || questObjective.IgnoreBlackList)))
	{
		if (questObjective.IgnoreFight)
			nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
		/* Entry found, GoTo */
		if (node.IsValid)
		{
			baseAddress = MovementManager.FindTarget(node, questObjective.Range);
		}
		if (unit.IsValid)
		{
			baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
		}
		Thread.Sleep(500);
		
		//Pre Select Target
		if (node.IsValid && node.Position.DistanceTo(ObjectManager.Me.Position) <= 60 && ObjectManager.Target.Guid != node.Guid)
		{
			Interact.InteractWith(node.GetBaseAddress);
		}
		else if (unit.IsValid && unit.Position.DistanceTo(ObjectManager.Me.Position) <= 60 && ObjectManager.Target.Guid != unit.Guid)
		{
			ObjectManager.Me.Target = unit.Guid;
			//Interact.InteractWith(unit.GetBaseAddress);
		}
			
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
	
		//Fake NPC for Position
        Npc RescuePoint = new Npc();
        RescuePoint = new Npc
        {
            Entry = 0,
            Position = questObjective.Position,
            Name = "RescuePoint",
            ContinentIdInt = Usefuls.ContinentId,
            Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
        };


        while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 5)
        {

           MovementManager.FindTarget(ref RescuePoint, 5);
            Thread.Sleep(500);
        }
		
        MovementManager.StopMove();
			
		Thread.Sleep(Usefuls.Latency);
		
		Lua.RunMacroText("/click OverrideActionBarButton2");

		/* Wait if necessary */
		if (questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);

		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
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