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

	_worker2 = new System.Threading.Thread(() => nManager.Wow.Helpers.Fight.StartFight(unit.Guid));
	Thread.Sleep(500);

	_worker2.Start();

	Thread.Sleep(500);

	while (unit.HealthPercent >= 35)
	{
		if(!ObjectManager.Me.InCombat)
		{
			return false;
		}
		Thread.Sleep(50);
	}

	ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
	Thread.Sleep(500);
	ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	nManager.Wow.Helpers.Fight.InFight= false;
	nManager.Wow.Helpers.Fight.StopFight();

	_worker2 = null;
	Thread.Sleep(200);
}