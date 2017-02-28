WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(21767, false), false, true,false);
	
return unit.IsValid || nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(10567) || nManager.Wow.Helpers.Quest.GetQuestCompleted(10567);
