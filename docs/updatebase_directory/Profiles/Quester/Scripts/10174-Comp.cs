WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(19644, questObjective.IsDead), questObjective.IgnoreNotSelectable, true, questObjective.AllowPlayerControlled);

if((unit.IsValid && unit.CanTurnIn) || nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(10174) || nManager.Wow.Helpers.Quest.GetQuestCompleted(10174))
	return true;
	
return false;
