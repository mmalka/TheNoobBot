WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);

if(unit.IsValid)
{
	Point p = new Point(-9190.168f,-2777.448f,89.10762f);
	if(unit.Position.DistanceTo(p) <= 5f)
	{
		questObjective.IsObjectiveCompleted = true;
		return true;
	}
}

return false;