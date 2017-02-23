		List<WoWUnit> unitList = ObjectManager.GetWoWUnitByEntry(questObjective.Entry);
		float deltax;

		if (unitList.Count <= 0)
			return false;
		string randomString = Others.GetRandomString(Others.Random(4, 10));
		
		foreach (WoWUnit wUnit in unitList)
		{
			if (!wUnit.IsValid || wUnit.IsDead || !IsObjectValidTarget(wUnit, out deltax))
			{
				nManagerSetting.AddBlackList(wUnit.Guid, 10 * 1000);
				continue;
			}

			MovementManager.FaceCTM(wUnit);
			Thread.Sleep(100);
			Lua.LuaDoString("VehicleAimIncrement(((" + deltax + ") - VehicleAimGetAngle()));");
			
			Lua.RunMacroText(questObjective.LuaMacro);
			
			return false;
		}
	} //Close Try
	catch (Exception e)
	{
		Logging.WriteError("Script: " + e);
	}
	return false;
} //Close Function

public static bool IsObjectValidTarget(WoWObject obj, out float delta)
{
	delta = 0;
	if (!obj.IsValid)
		return false;
	if (!(obj is WoWUnit || obj is WoWGameObject))
		return false;
	float targetZ = obj is WoWUnit ? (obj as WoWUnit).Position.Z : (obj as WoWGameObject).Position.Z;
	float targetDistance = obj is WoWUnit ? (obj as WoWUnit).GetDistance2D : (obj as WoWGameObject).GetDistance2D;
	float v0Sqr = 80f*80f;
	float horizontalDistance = targetDistance;
	float heightDiff = targetZ - ObjectManager.Me.Position.Z;

	float tmp1 = 32.174f*(horizontalDistance*horizontalDistance);
	float tmp2 = 2f*heightDiff*v0Sqr;
	float radicalTerm = (v0Sqr*v0Sqr) - (32.174f*(tmp1 + tmp2));

	if (radicalTerm < 0f)
	{
		return false;
	}

	radicalTerm = (float) System.Math.Sqrt(radicalTerm);

	// Prefer the 'lower' angle, if its within the articulation range...
	float root = (float) System.Math.Atan((v0Sqr - radicalTerm)/(32.174f*horizontalDistance));
	if (-1.18f > root && root > 0.0f)
		root = (float) System.Math.Atan((v0Sqr + radicalTerm)/(32.174f*horizontalDistance));
	Thread.Sleep(50);

	if (-1.18f > root && root > 0.0f)
		return false;
	delta = root;
	return true;
}

public static bool random() //not used, just to close the script
{
	try //Bim Try open random !!!
	{