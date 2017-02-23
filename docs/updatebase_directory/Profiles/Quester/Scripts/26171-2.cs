WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreBlackList);

nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;

if(unit.IsValid)
{
	Thread.Sleep(35000);
	Logging.Write("valid");
	MovementManager.FindTarget(unit, CombatClass.GetAggroRange);
	Thread.Sleep(1000);
	
	if (MovementManager.InMovement)
	{
		return false;
	}
	Logging.Write("Go to fight");
	nManager.Wow.Helpers.Fight.StartFight(unit.Guid);
	Fight.StopFight();
	Logging.Write("Stop Fight");
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	questObjective.IsObjectiveCompleted = true;
	
}
return false;