/* Interact with object then use item on mob
   Entries hardcoded
   Check if there is HotSpots in the objective */
if (questObjective.Hotspots.Count <= 0)
{
	/* Objective CSharpScript with script InteratWithHotSpots requires valid "Hotspots" */
	Logging.Write("UseItemWithHotSpots requires valid 'HotSpots'.");
	questObjective.IsObjectiveCompleted = true;
	return false;
}
bool CurrentAllowMount = nManagerSetting.CurrentSetting.UseGroundMount;
nManagerSetting.CurrentSetting.UseGroundMount= true;
/* Move to Zone/Hotspot */
if (!MovementManager.InMovement)
{
  
	if (questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
	{
		MovementManager.Go(PathFinder.FindPath(questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		MovementManager.GoLoop(questObjective.Hotspots);
	}
}
   
WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(184867));
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress;

/* If Entry found continue, otherwise continue checking around HotSpots */
if (node.IsValid)
{
	/* Entry found, GoTo */
	/*Logging.Write("Node FOund");*/
	 
	baseAddress = MovementManager.FindTarget(node); /* Move toward node */
	pos = new Point(node.Position);
	
	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */
	
	Npc vNpc = new Npc();
	vNpc = new Npc
	{
		Entry = 1337,
		Position = pos,
		Name = "Power Converter",
		ContinentIdInt = Usefuls.ContinentId,
		Faction = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
	};
	
	while(ObjectManager.Me.Position.DistanceTo(pos) >= questObjective.Range)
	{	
		if(ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
			return false;
		MovementManager.FindTarget(ref vNpc, questObjective.Range);  
		Thread.Sleep(500);
	}
	/*Logging.Write("Node reached");*/
	/* Target Reached */
	MovementManager.StopMove();
	MountTask.DismountMount();
	
	
	if (questObjective.IgnoreFight)
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;

	MovementManager.Face(node);
	Interact.InteractWith(node.GetBaseAddress);
	/*Logging.Write("Interact");*/
	

	while (ObjectManager.Me.IsCast)
	{
		Thread.Sleep(Usefuls.Latency);
	}
	Thread.Sleep(2500);
	
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(20021, questObjective.IsDead));
	Interact.InteractWith(unit.GetBaseAddress);

	if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
		return false;
	
	ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
	Thread.Sleep(questObjective.WaitMs);
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	
}
nManagerSetting.CurrentSetting.UseGroundMount = CurrentAllowMount;