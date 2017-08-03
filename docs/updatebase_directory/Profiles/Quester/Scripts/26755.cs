Logging.Write("Launch");
WoWUnit unit = ObjectManager.GetWoWUnitByEntry(questObjective.Entry).Find(x => x.GetDistance >= 20f);


MovementManager.FaceCTM(unit);

Thread.Sleep(500);

float angle = unit.Position.Z - ObjectManager.Me.Position.Z;
float currentAngle = 0f;
float angleAdjust = 0f;
string randomString = Others.GetRandomString(Others.Random(4, 10));

currentAngle = Others.ToSingle(Lua.LuaDoString(randomString + "=VehicleAimGetAngle()", randomString));

angleAdjust = currentAngle - angle;

Lua.LuaDoString("VehicleAimDecrement(" + angleAdjust + ")");

Thread.Sleep(500);

Lua.RunMacroText("/click OverrideActionBarButton1");