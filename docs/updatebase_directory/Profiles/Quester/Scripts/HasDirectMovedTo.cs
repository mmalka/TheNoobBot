var onPosition = new Point();
if (questObjective.Position.IsValid)
{
  onPosition = questObjective.Position;
}
else if (questObjective.Hotspots.Count > 0)
{
  foreach (Point hotspot in questObjective.Hotspots)
  {
    if (hotspot.IsValid)
    {
      onPosition = hotspot;
      break;
    }
  }
}

if (!onPosition.IsValid)
{
  Logging.Write("HasDirectMoveTo script condition require a valid position or a valid hotspot");
  return false;
}

MovementManager.Go(new List<Point> { ObjectManager.Me.Position, onPosition });
Thread.Sleep(200);
while (MovementManager.InMovement) 
  Thread.Sleep(100);

return true;