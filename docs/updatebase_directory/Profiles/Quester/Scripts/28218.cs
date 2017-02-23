WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);

if(!unit.InCombat)
{
	Thread.Sleep(questObjective.WaitMs);
}
else
{
	Fight.StartFight(unit.Guid);
	
	while(!nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(28218))
	{
		Thread.Sleep(5000);
	}
	questObjective.IsObjectiveCompleted = true;
	return true;
}
	
	