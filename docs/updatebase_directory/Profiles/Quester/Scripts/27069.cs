/*
Interact With Hotspots
Modified version to check for a buff needed to complete the quest

*/

/* Check if there is HotSpots in the objective */
if (questObjective.Hotspots.Count <= 0)
{
	/* Objective CSharpScript with script InteractWithHotSpots requires valid "Hotspots" */
	Logging.Write("InteractWithHotSpots requires valid 'HotSpots'.");
	questObjective.IsObjectiveCompleted = true;
	return false;
}



if(!ObjectManager.Me.HaveBuff(83839))
{

		Logging.Write("Sea Pup Buff Lost, Lets go take it back!");
        uint baseAddress2 = 0;
        Point admiralHatchetpos = new Point(1068.75f,1601.3f,28.00147f);
        Npc AdmiralHatchet = new Npc();
        AdmiralHatchet = new Npc
        {
            Entry = 44916,
            Position = admiralHatchetpos,
            Name = "Admiral Hatchet",
            ContinentIdInt = Usefuls.ContinentId,
            Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
        };

        while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(admiralHatchetpos) >= 5)
        {
			if(ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
				return false;
			
            baseAddress2 = MovementManager.FindTarget(ref AdmiralHatchet, 5);
            Thread.Sleep(500);
        }
        MovementManager.StopMove();
        Interact.InteractWith(baseAddress2);
		Thread.Sleep(2000);
		nManager.Wow.Helpers.Quest.SelectGossipOption(1);

}

/* Move to Zone/Hotspot */
if (!MovementManager.InMovement)
{
  
	if (questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
	{
		nManager.Wow.Helpers.Quest.TravelToQuestZone(questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)], ref questObjective.TravelToQuestZone, questObjective.ContinentId,questObjective.ForceTravelToQuestZone);
		MovementManager.Go(PathFinder.FindPath(questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		MovementManager.GoLoop(questObjective.Hotspots);
	}
}

/* Search for Entry */
WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
WoWUnit unit =ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */;
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress = 0;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (node.IsValid || unit.IsValid)
{
	/* Entry found, GoTo */
	if(unit.IsValid)
	{
		baseAddress = MovementManager.FindTarget(unit); /* Move toward unit */
		pos = new Point(unit.Position);
	}
	else if (node.IsValid) 
	{
		baseAddress = MovementManager.FindTarget(node); /* Move toward node */
		pos = new Point(node.Position);
	}
  
	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

	Npc vNpc = new Npc();
	vNpc = new Npc
	{
		Entry = 1337,
		Position = pos,
		Name = "Target",
		ContinentIdInt = Usefuls.ContinentId,
		Faction = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
	};
	
	while(ObjectManager.Me.Position.DistanceTo(pos) >= questObjective.Range)
	{	
		if(ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
			return false;
		baseAddress = MovementManager.FindTarget(ref vNpc, questObjective.Range);
		Thread.Sleep(500);
	}
	
	/* Entry reached, dismount */
	MovementManager.StopMove();
	MountTask.DismountMount();
   
	if (questObjective.IgnoreFight)
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
	
	/* Interact With Entry */
	if (node.IsValid)
	{
		MovementManager.Face(node);
		Interact.InteractWith(node.GetBaseAddress);
		nManagerSetting.AddBlackList(node.Guid, 30*1000);
	}
	else if (unit.IsValid)
	{
		MovementManager.Face(unit);
		Interact.InteractWith(unit.GetBaseAddress);
		nManagerSetting.AddBlackList(unit.Guid, 30*1000);
	}
	
	Thread.Sleep(Usefuls.Latency); 

	/* Wait for the interact cast to be finished, if any */
	while (ObjectManager.Me.IsCast)
	{
		Thread.Sleep(Usefuls.Latency);
	}

	/* Do Gossip if necessary */
	if (questObjective.GossipOptionsInteractWith != 0)
	{
		Thread.Sleep(250 + Usefuls.Latency);
		nManager.Wow.Helpers.Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
	}

	if (Others.IsFrameVisible("StaticPopup1Button1"))
		Lua.RunMacroText("/click StaticPopup1Button1");
	if (ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
		return false;
	
	/* Wait if necessary */
	if(questObjective.WaitMs > 0)
	Thread.Sleep(questObjective.WaitMs);

	/* Interact Completed - InternalIndex and count is used to determine if obj is completed - questObjective.IsObjectiveCompleted = true; */
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
}