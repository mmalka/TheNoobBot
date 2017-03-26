WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
uint baseAddress = 0;
Logging.Write("ICI");
/* If Entry found continue, otherwise continue checking around HotSpots */
if (unit.IsValid)
{

	/* Entry found, GoTo */
	Logging.Write("ICI2");
	baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
	
	if(baseAddress == 0)
		baseAddress = unit.GetBaseAddress;
	
	Thread.Sleep(500);
	
	if (MovementManager.InMovement)
        return false;

	if (unit.IsValid && unit.GetDistance > questObjective.Range)
	{
		return false;
	}
	
  
	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

	/* Entry reached, dismount */
	MovementManager.StopMove();
	MountTask.DismountMount();
	
	/* Activate Beacon */
	MovementManager.Face(unit);
	Interact.InteractWith(unit.GetBaseAddress);	
	Logging.Write("ICI3");
	/* Do Gossip if necessary */
		nManager.Wow.Helpers.Quest.SelectGossipOption(2);
		Thread.Sleep(750 + Usefuls.Latency);
		nManager.Wow.Helpers.Quest.SelectGossipOption(1);
		Thread.Sleep(750 + Usefuls.Latency);
		nManager.Wow.Helpers.Quest.SelectGossipOption(1);
		Thread.Sleep(750 + Usefuls.Latency);
		nManager.Wow.Helpers.Quest.SelectGossipOption(1);
		Thread.Sleep(750 + Usefuls.Latency);
		nManager.Wow.Helpers.Quest.SelectGossipOption(2);
		Thread.Sleep(750 + Usefuls.Latency);

	if (Others.IsFrameVisible("StaticPopup1Button1"))
		Lua.RunMacroText("/click StaticPopup1Button1");
	if (ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
		return false;
	
	/* Wait if necessary */
	if(questObjective.WaitMs > 0)
		Thread.Sleep(questObjective.WaitMs);

	/* Interact Completed - InternalIndex and count is used to determine if obj is completed - questObjective.IsObjectiveCompleted = true; */

	
}

nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;