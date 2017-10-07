  // nManager.Wow.ObjectManager.WoWUnit Worgen = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(45270));
try //Strange problem here, the bot is causing an Error : System.NullReferenceException: Object reference not set to an instance of an object. at Main.Script(QuestObjective& questObjective)
{
              
	nManager.Wow.ObjectManager.WoWUnit Worgen = new WoWUnit(0);
					 
	Worgen = nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(45270).Find(x => x.Position.Y <= 875 && x.Position.DistanceTo(ObjectManager.Me.Position) <= 150 && !x.IsDead);

	if (Worgen.IsValid && Worgen.DistanceTo <= 150)
	{
			
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
	}  

  
}
catch (Exception)
{
	return false;
}
		