// This script is required because an other player can start the fight before or after the bot and make it bug.
Point sternfathomPosition = new Point(-333.6571f, 6672.701f, 3.538227f);
Point entrancePosition = new Point(-347.0213f, 6656.839f, 1.14944f);
int WaitMsForCombat = 60 * 1000;

if (questObjective.WaitMs > 0)
{
    int timeToWait = questObjective.WaitMs;
    Point waitingSpot = new Point(-341.38f, 6645.994f, 1.200562f);

    MovementManager.Go(new List<Point> { ObjectManager.Me.Position, waitingSpot});
    while (MovementManager.InMovement)
        Thread.Sleep(100);

    MountTask.DismountMount(true); // Force dismount to use defensive behaviour

    while (timeToWait > 0)
    {
        if (ObjectManager.Me.IsDeadMe)
        {
            questObjective.WaitMs = 0;
            return false;
        }  
        if (ObjectManager.Me.InCombat)
        {
            questObjective.WaitMs = WaitMsForCombat;
            return false;
        }
        Thread.Sleep(100);
        timeToWait -= 100;
    }
    questObjective.WaitMs = 0;
}

if (sternfathomPosition.DistanceTo(ObjectManager.Me.Position) > 5f)
{
  MovementManager.Go(PathFinder.FindPath(entrancePosition));
  while (MovementManager.InMovement)
      Thread.Sleep(100);

  MovementManager.Go(new List<Point> { ObjectManager.Me.Position, sternfathomPosition});
  while (MovementManager.InMovement)
      Thread.Sleep(100);
}

WoWUnit sternfathom = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(89048));
Interact.InteractWith(sternfathom.GetBaseAddress);
Thread.Sleep(1000);
nManager.Wow.Helpers.Quest.SelectGossipOption(1);
Thread.Sleep(7000);
    

questObjective.WaitMs = WaitMsForCombat;
return false;