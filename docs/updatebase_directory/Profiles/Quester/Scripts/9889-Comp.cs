WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(18262, questObjective.IsDead), questObjective.IgnoreNotSelectable, true, questObjective.AllowPlayerControlled);

if((unit.IsValid && unit.CanTurnIn) || nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(9889) || nManager.Wow.Helpers.Quest.GetQuestCompleted(9889))
	return true;
	
return false;
