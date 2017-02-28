WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, true, questObjective.AllowPlayerControlled);

if((unit.IsValid && unit.CanTurnIn) || nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(questObjective.InternalQuestId) || nManager.Wow.Helpers.Quest.GetQuestCompleted(questObjective.InternalQuestId))
	return true;
	
return false;
