if(!ObjectManager.Me.InTransport) //Go to bear and Gossip with him to get on him!!!
{
	WoWUnit bear = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.ExtraInt, questObjective.IsDead));
	
	if(ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 5 && !MovementManager.InMovement)
	{
		MovementManager.Go(PathFinder.FindPath(questObjective.Position));
		return false;
	}
	
	if(bear.IsValid)
	{
		
		MovementManager.Face(bear);
		Interact.InteractWith(bear.GetBaseAddress);
		Thread.Sleep(Usefuls.Latency + 250); 
		nManager.Wow.Helpers.Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
	}
	return false;
}
while(!(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(12821) || !nManager.Wow.Helpers.Quest.GetQuestCompleted(12821)) && ObjectManager.Me.InTransport && !ObjectManager.Me.IsDead)
{
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));

	if(unit.IsValid && unit.GetDistance <= 10)
	{
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
		
	nManager.Wow.Helpers.Keybindings.DownKeybindings(nManager.Wow.Enums.Keybindings.ACTIONBUTTON1);
	Thread.Sleep(questObjective.WaitMs);
	nManager.Wow.Helpers.Keybindings.UpKeybindings(nManager.Wow.Enums.Keybindings.ACTIONBUTTON1);

	ClickOnTerrain.ClickOnly(unit.Position);
	}
	Thread.Sleep(1000);
}

