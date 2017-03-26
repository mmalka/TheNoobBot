WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);

if(unit.IsValid && !unit.IsDead)
{
	Point p = new Point(-9190.168f,-2777.448f,89.10762f);
	while(unit.Position.DistanceTo(p) >= 5f)
	{
		Thread.Sleep(500);
	}
	questObjective.IsObjectiveCompleted = true;
	return true;
}else
{
	questObjective.IsObjectiveCompleted = true;
	return true;
}

return false;