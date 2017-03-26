		if(ObjectManager.Me.InTransport)
		{
			
		
			//Get saved current lvl (0 if its run the first time)
			_lvlCurrent = questObjective.ExtraInt;
			
			if(_lvlCurrent == LEVEL_UNKNOWN && !IsClimingTree)
			{
				
				//We dont have the aura, we are at the top
				_lvlCurrent = LEVEL_TOP;
				Logging.Write("We dont know level, top of tree, current : " + _lvlCurrent.ToString());
			}

			if(_lvlCurrent == LEVEL_UNKNOWN)
			{
				Logging.Write("We dont know level, go up");
				//First Run? Dont know where we are, lets climb at the top
				ClimbUpTop();
			}
			
			if(HasBearItem())
			{
				Logging.Write("HasBear, go up if not");
				if(_lvlCurrent != LEVEL_TOP)
				{
					ClimbUpTop();
					return false;
				}
				
				AimAndLaunchBear();
				return false;
			}
			else
			{	//No Bears in inventory
				if(_lvlCurrent == LEVEL_TOP)
				{
					Logging.Write("No Bears, we are at top, going down");
					ClimbDown(); //Go down to pickup bears
					return false;
				}
				
				WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,questObjective.AllowPlayerControlled);
				
				if(unit.IsValid)
				{
					Interact.InteractWith(unit.GetBaseAddress);
					nManagerSetting.AddBlackList(unit.Guid, 30*1000);
					Thread.Sleep(500);
					return false;
				}
			}
		}		
	} //Close the try here
	catch(Exception e)
	{
		Logging.WriteError("Script: " + e);
	}
	
	finally
	{
		//Save current Level inside the ExtraInt field
		questObjective.ExtraInt = _lvlCurrent;
	}
	return false;
}

void ClimbUpTop()
{
	while(_lvlCurrent != LEVEL_TOP)
	{
		Point lastLocation = ObjectManager.Me.Position;
		
		Thread.Sleep(500);
		
		Lua.RunMacroText("/click OverrideActionBarButton1");

		Thread.Sleep(2000);
		
		if(!IsClimingTree)
			_lvlCurrent = LEVEL_TOP;
		else
			_lvlCurrent = _lvlCurrent + 1;
		
		Logging.Write("Current Level Up : " + _lvlCurrent.ToString());
	}
}

void ClimbDown()
{
	Point lastLocation = ObjectManager.Me.Position;
	
	Lua.RunMacroText("/click OverrideActionBarButton2");

	Thread.Sleep(2500);
	Logging.Write(ObjectManager.Me.Position.DistanceTo(lastLocation).ToString());
	
	Logging.Write("ClimbDown current lvl " + _lvlCurrent.ToString());
	
	_lvlCurrent = _lvlCurrent - 1;
}

void PickUpBear()
{

}

bool HasBearItem()
{
	return (nManager.Wow.Helpers.ItemsManager.GetItemCount(54439) >= 1);
}

private const float AIM_ANGLE = -0.97389394044876f;

void AimAndLaunchBear()
{
	Logging.Write("Launch");
	WoWUnit trampoline= ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(41200,false),true, false,false);

	MovementManager.FaceCTM(trampoline);
	
	Thread.Sleep(2000);
	
	float angle = trampoline.Position.Z - ObjectManager.Me.Position.Z;
	float currentAngle = 0f;
	float angleAdjust = 0f;
	string randomString = Others.GetRandomString(Others.Random(4, 10));
	
	currentAngle = Others.ToSingle(Lua.LuaDoString(randomString + "=VehicleAimGetAngle()", randomString));
	
	angleAdjust = currentAngle - AIM_ANGLE;
	
	Lua.LuaDoString("VehicleAimDecrement(" + angleAdjust + ")");
	
	Thread.Sleep(2000);
	
	Lua.RunMacroText("/click OverrideActionBarButton4");
}
 
bool IsClimingTree { get { return ObjectManager.Me.HaveBuff(74920); } }

private const int LEVEL_BOTTOM = 1;
private const int LEVEL_TOP = 5;
private const int LEVEL_UNKNOWN = 0;
private int _lvlCurrent = LEVEL_UNKNOWN;

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
