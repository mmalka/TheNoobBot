WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(18261, questObjective.IsDead), questObjective.IgnoreNotSelectable, true, questObjective.AllowPlayerControlled);

if((unit.IsValid && unit.CanTurnIn) || nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(10107) || nManager.Wow.Helpers.Quest.GetQuestCompleted(10107))
	return true;
	
return false;

