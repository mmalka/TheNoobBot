Logging.Write("Starting cannon on worgen cs script quest 14218");
  
nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;

while(!nManager.Wow.Helpers.Quest.IsObjectiveCompleted(14218, 1, 80))
{

	WoWUnit Worgen = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(35229, false).FindAll( x => x.Position.Y <= 1670 && x.Position.X >= -1520 && x.Position.X <= -1440 && x.Position.DistanceTo(ObjectManager.Me.Position) <= 150 && !x.IsDead), false,false,false);

	if (Worgen.IsValid)
	{
		
//		Logging.Write("Found valid Worgen");
		
		MovementManager.FaceCTM(Worgen);
		Interact.InteractWith(Worgen.GetBaseAddress);

		float angle = Worgen.Position.Z - ObjectManager.Me.Position.Z;
		float currentAngle = 0f;
		string randomString = Others.GetRandomString(Others.Random(4, 10));

		currentAngle = Others.ToSingle(Lua.LuaDoString(randomString + "=VehicleAimGetAngle()", randomString));

		if (currentAngle < angle)
		{
			Lua.LuaDoString("VehicleAimIncrement(" + (angle - currentAngle) + ")");
		}
		else
		{
			Lua.LuaDoString("VehicleAimDecrement(" + (currentAngle - angle) + ");");
		}

		Lua.RunMacroText("/click OverrideActionBarButton1");
		
//		Logging.Write("fired cannon!");
		
		Thread.Sleep(1200);

	}
}

return true;
