Point stellagosa = new Point(-736.8931f, 7334.946f, 15.12196f); 
Point entrance = new Point(-655.7592f, 7278.829f, 24.12959f);

if (ObjectManager.Me.Position.Z < entrance.Z && ObjectManager.Me.Position.DistanceTo(stellagosa) <= 300f)
{
  MovementManager.Go(PathFinder.FindPath(entrance));
  while(MovementManager.InMovement)
  {
    Thread.Sleep(100);
  }
}

return true;