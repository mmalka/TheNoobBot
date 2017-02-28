WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(16994, false), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);

if(unit.IsValid || nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(9370) || nManager.Wow.Helpers.Quest.GetQuestCompleted(9370))
	return true;
	
Thread.Sleep(2000);
return false;