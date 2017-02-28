Point p = new Point(3068.243f, 3597.192f, 144.0675f);

if(ObjectManager.Me.Position.DistanceTo(p) <= 10)
	return true;
	
if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(10652) || nManager.Wow.Helpers.Quest.GetQuestCompleted(10652))
	return true;
	
return false;