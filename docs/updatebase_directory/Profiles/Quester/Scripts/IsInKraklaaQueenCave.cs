
Point cavePosition = new Point(-976.3683f, 5625.232f, 4.900234f);

if (cavePosition.DistanceTo(ObjectManager.Me.Position) <= 5f)
    return true;

Point entrancePosition = new Point(-876.7358f, 5698.211f, -26.45047f);  

MovementManager.Go(PathFinder.FindPath(entrancePosition));
while (MovementManager.InMovement)
    Thread.Sleep(100);

if (entrancePosition.DistanceTo(ObjectManager.Me.Position) > 5f)
{
    MountTask.DismountMount(true);
    MovementManager.Go(new List<Point> { ObjectManager.Me.Position, entrancePosition });
    while (MovementManager.InMovement)
        Thread.Sleep(100);
}


MovementManager.Go(new List<Point> { ObjectManager.Me.Position, cavePosition });
while (MovementManager.InMovement)
    Thread.Sleep(100);

if (cavePosition.DistanceTo(ObjectManager.Me.Position) <= 5f)
    return true;

Logging.Write("Unable to auto move to Faronis temple, you have to do it.");
return false;