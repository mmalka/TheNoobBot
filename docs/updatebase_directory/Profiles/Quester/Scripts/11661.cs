WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	questObjective.AllowPlayerControlled);
	
if (unit.IsValid && unit.Position.DistanceTo(questObjective.Position) <=8)
{
	MovementManager.Face(unit);
	Interact.InteractWith(unit.GetBaseAddress);
	nManager.Wow.Helpers.Fight.StartFight(unit.Guid);
	
}

else
{
	List<Point> liste = new List<Point>();
	liste.Add(ObjectManager.Me.Position);
	liste.Add(questObjective.Position);
	
	MovementManager.Go(liste);

}

