if (!MovementManager.InMovement)
{
    WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
    if (!nManagerSetting.IsBlackListedZone(node.Position) && !nManagerSetting.IsBlackListed(node.Guid) && node.IsValid)
    {
        WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(90487), node.Position, true, true, true);
        if (unit.IsValid && unit.Position.DistanceTo(node.Position) > 10f)
        {
            nManagerSetting.AddBlackList(node.Guid, 1000*60*3);
            return false;
        }
        MovementManager.Go(PathFinder.FindPath(node.Position));
        Thread.Sleep(500 + Usefuls.Latency);
        while (MovementManager.InMovement && node.Position.DistanceTo(ObjectManager.Me.Position) > 3.9f)
        {
            if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
                return true;
            Thread.Sleep(100);
        }
    
        MountTask.DismountMount();
        MovementManager.StopMove();
        Interact.InteractWith(node.GetBaseAddress);
        Thread.Sleep(1000);
    }
    else if (!MovementManager.InMovement)
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
}