/*
This script is using a thread to make the method StartFight Async since we want to be
able to check the health of the mob being attacked (to do an action)
*/

List<int> entries = new List<int>();
entries.Add(2270);
entries.Add(2503);
entries.Add(2269);
System.Threading.Thread _worker2;

/* Move to Zone/Hotspot */
if (!MovementManager.InMovement)
{
	/* Move to Zone/Hotspot */
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

nManager.Wow.ObjectManager.WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(entries));

if(unit.IsValid)
{
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;

	while(ObjectManager.Me.Position.DistanceTo(unit.Position) >= 5)
	{
		if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
		{
			return false;
		}
		MovementManager.FindTarget(unit, 5);

	}

	if(ObjectManager.Me.WowClass == WoWClass.Mage)
	{
		_worker2 = new System.Threading.Thread(() => nManager.Wow.Helpers.Fight.StartFight(unit.Guid));
		_worker2.Start();
	}
	else
		Interact.InteractWith(unit.GetBaseAddress);
	
	Thread.Sleep(500);
	
	while (ObjectManager.Target.HealthPercent >= 35)
	{
		if(!ObjectManager.Me.InCombat)
		{
			return false;
		}
		Thread.Sleep(50);
	}
	
	ObjectManager.Me.StopCast();
	_worker2 = null;
	Fight.StopFight();
	Thread.Sleep(500);
	ObjectManager.Me.StopCast();
	
	Thread.Sleep(1000);
	ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId)); //UseItem 2 times because sometimes wow lags and thinks that the mob still has more than 35% hp
	Thread.Sleep(2000);
	ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	
	Thread.Sleep(200);
}