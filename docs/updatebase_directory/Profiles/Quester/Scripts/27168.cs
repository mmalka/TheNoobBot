
/*
  Quest help script for "Those That Couldn't Let Go"
 
  Objective is to find 12 Withdrawn Spirits (45166),  and use the Holy Thurible (60861) item on them.
  The RNG will play, sometimes the spirit will fly away, quest count updates and can move onto next.
  Other time, spirit will aggro and need to fight it, before can move onto the next.
 
  Travel to Hotspot and navigate them has same codes from other Hotspot script.
*/

 

//looking for ghostie..
WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry));


// is there one near by?
if ((unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList )))
{
    // checking for good path to it?
    if(nManager.Wow.Helpers.PathFinder.FindPath(unit.Position).Count <= 0)
    {
        nManagerSetting.AddBlackList(unit.Guid, 30*1000);
        return false;
    }
    
    //Logging.Write("27168 logging - moving towards spirit");

    // there is, go towards it.
    while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) >= questObjective.Range)
    {
        if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
        {
            //Logging.Write("Eeek combat detected - returning control for now...");
            return false;
        }

        MovementManager.FindTarget(unit, questObjective.Range);
        Thread.Sleep(500);
    }


    MovementManager.StopMove();
    MountTask.DismountMount();
    if (unit.IsValid)
    {
        MovementManager.Face(unit);
    }
    if (unit.IsValid && unit.Position.DistanceTo(ObjectManager.Me.Position) <= 60 && ObjectManager.Target.Guid != unit.Guid)
    {	
        Lua.LuaDoString("ClearTarget()");
        ObjectManager.Me.Target = unit.Guid;
    }
    Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */


    // use the Holy Thurible on it...
    ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
    Thread.Sleep(Usefuls.Latency + 250);

    while (ObjectManager.Me.IsCast)
    {
        Thread.Sleep(Usefuls.Latency);
    }


    // hold for mob reaction to use item...
    //Logging.Write("27168 logging - holding for reaction ");
    Thread.Sleep(2000);


    if (ObjectManager.Me.InCombat)
    {
        //Logging.Write("Eeek combat detected");
        return false;
    }
    else
    {
        //Logging.Write("27168 logging - me not in combat.. move along..");
        nManagerSetting.AddBlackList(unit.Guid, 30*1000);
        Thread.Sleep(2000);
    }


    /* Wait if necessary */
    if (questObjective.WaitMs > 0)
        Thread.Sleep(questObjective.WaitMs);

    return true;
}
else if (!MovementManager.InMovement)
{
    //Logging.Write("27168 logging - no spirits near, hotspot searching...");

		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
		if (questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
		{
        if(nManager.Wow.Helpers.Quest.TravelToQuestZone(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)],
				ref questObjective.TravelToQuestZone, questObjective.ContinentId, questObjective.ForceTravelToQuestZone))
            return false;

      //Logging.Write("27168 logging - hotspot mm go");
			MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
		}
		else
		{
        //Logging.Write("27168 logging - hotspot mm looping");
        MovementManager.GoLoop(questObjective.PathHotspots);
    }
}

