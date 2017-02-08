  // nManager.Wow.ObjectManager.WoWUnit Worgen = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(45270));
nManager.Wow.ObjectManager.WoWUnit Worgen = nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(45270).Find(x => x.Position.Y <= 875 && x.Position.DistanceTo(ObjectManager.Me.Position) <= 150 && !x.IsDead);

if (Worgen.IsValid && Worgen.Position.DistanceTo(ObjectManager.Me.Position) <= 150)
{
	var listP = new System.Collections.Generic.List<Point>();
	listP.Add(Worgen.Position);

   
	nManager.Wow.Helpers.MovementManager.Go(listP);

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
}