if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(9868) || nManager.Wow.Helpers.Quest.GetQuestCompleted(9868))
	return true;

if(nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 10)
	return false;
	
return true;
