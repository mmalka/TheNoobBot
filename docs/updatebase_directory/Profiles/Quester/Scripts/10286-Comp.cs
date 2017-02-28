WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(20159, questObjective.IsDead), questObjective.IgnoreNotSelectable, true, questObjective.AllowPlayerControlled);

if((unit.IsValid && unit.CanTurnIn) || nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(10286) || nManager.Wow.Helpers.Quest.GetQuestCompleted(10286))
	return true;
	
return false;

