if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(25006) || nManager.Wow.Helpers.Quest.GetQuestCompleted(25006))
	return true;

WoWUnit wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(38981, questObjective.IsDead), questObjective.IgnoreBlackList);


if(!wowUnit.IsValid)
	return false;
	
return true;
