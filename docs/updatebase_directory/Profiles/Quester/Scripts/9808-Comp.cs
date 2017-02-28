Point p = new Point(189.7542f, 8485.297f, 26.95859f);

if(ObjectManager.Me.Position.DistanceTo(p) <= 5)
	return false;
	
return true;