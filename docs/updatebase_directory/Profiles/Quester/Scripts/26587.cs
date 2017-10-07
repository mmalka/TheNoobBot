WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
WoWPlayer woWPlayer = ObjectManager.GetNearestWoWPlayer(ObjectManager.GetObjectWoWPlayer());

while (woWPlayer.Position.DistanceTo(ObjectManager.Me.Position) < 75f)
{
    /* nManager.Helpful.Logging.Write("Player nearby, pausing"); */
    Thread.Sleep(1000);
}

if(unit.IsValid && !unit.IsDead)
{
	Point nearp = new Point(-9150.472f,-2722.793f,88.7262f);
	Point farp = new Point(-9184.017f,-2771.589f,88.6734f);
	if (unit.Position.DistanceTo(nearp) <= 25f)
	{
		while(unit.Position.DistanceTo(farp) <= 25f)
		{
			Thread.Sleep(500);
			/* nManager.Helpful.Logging.Write("R1: Waiting for Unit to Leave Far Point..."); */
		}
		while (unit.Position.DistanceTo(nearp) <= 25f)
		{
			Thread.Sleep(500);
			/* nManager.Helpful.Logging.Write("R1: Waiting for Unit to Leave Near Point..."); */
		}
	}
	else
	{
		while(unit.Position.DistanceTo(nearp) >= 10f)
		{
			Thread.Sleep(500);
			/* nManager.Helpful.Logging.Write("R2: Waiting for Unit to come back to Near Point...");*/
		}
		while (unit.Position.DistanceTo(nearp) <= 25f)
		{
			Thread.Sleep(500);
			/* nManager.Helpful.Logging.Write("R2: Waiting for Unit to Leave Near Point...");*/
		}
	}
	/* nManager.Helpful.Logging.Write("Run!"); */
	MountTask.DismountMount();
	questObjective.IsObjectiveCompleted = true;
	return true;
}else
{
	questObjective.IsObjectiveCompleted = true;
	return true;
}
return false;