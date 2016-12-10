
Point templePosition = new Point(-8.230906f, 6734.484f, 55.58817f);

if (templePosition.DistanceTo(ObjectManager.Me.Position) > 15f)
    return true;

Point entrancePosition = new Point(35.79251f, 6737.417f, 50.56242f);

MovementManager.Go(new List<Point> { ObjectManager.Me.Position, entrancePosition });
while (MovementManager.InMovement)
    Thread.Sleep(100);

if (templePosition.DistanceTo(ObjectManager.Me.Position) > 15f)
    return true;

Logging.Write("Unable to auto move out of Faronis temple, you have to do it.");
return false;