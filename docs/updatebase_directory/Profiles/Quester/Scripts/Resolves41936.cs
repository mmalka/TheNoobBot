Thread.Sleep(250 + Usefuls.Latency);

if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) < 5f)
{
  WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
  if (node.IsValid)
  {
    MovementManager.Go(PathFinder.FindPath(node.Position));
    while (MovementManager.InMovement && node.Position.DistanceTo(ObjectManager.Me.Position) > 3.9f)
    {
        if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
            return false;
        Thread.Sleep(100);
    }
    MountTask.DismountMount();
    MovementManager.StopMove();
    Interact.InteractWith(node.GetBaseAddress);
    return true;
  }

  WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, false));
  if (!unit.IsValid || unit.NotAttackable)
    return false;
  
  MovementManager.FindTarget(unit, CombatClass.GetAggroRange);
  Thread.Sleep(100);
  while(MovementManager.InMovement)
    Thread.Sleep(100 + Usefuls.Latency);
  
  MountTask.DismountMount();
  MovementManager.StopMove();
  Fight.StartFight(unit.Guid);
  while (ObjectManager.Me.InCombat && !unit.IsDead)
    Thread.Sleep(100 + Usefuls.Latency);

  return true;
}
else
  MovementManager.Go(PathFinder.FindPath(questObjective.Position));
