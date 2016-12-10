    
Point o = new Point(-650.7122f, 5496.997f, -7.587699f);
Point i = new Point(-686.2131f, 5517.639f, -10.01785f);

if (i.DistanceTo(ObjectManager.Me.Position) < 30f && i.DistanceZ(ObjectManager.Me.Position) < 3f)
{
    bool CurrentAllowMount = nManagerSetting.CurrentSetting.UseGroundMount;
    MountTask.DismountMount();
    nManagerSetting.CurrentSetting.UseGroundMount = false;

    MovementManager.Go(new List<Point> { ObjectManager.Me.Position, i, o });
    while (MovementManager.InMovement)
        Thread.Sleep(100);

    nManagerSetting.CurrentSetting.UseGroundMount = CurrentAllowMount;
    return true;
}
