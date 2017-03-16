Point p = new Point(520.4606f, 3885.672f, 190.2297f);

/*if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(9410) || nManager.Wow.Helpers.Quest.GetQuestCompleted(9410))
	return true;*/

if(ObjectManager.Me.Position.DistanceTo(p) >= 10)
	return false;
	
return true;