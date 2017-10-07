bool reached = false;

Point waitFlagshipPoint = questObjective.Position; // new Point("1475.782 ; -40.53578 ; 476.9899 ; Flying");
Npc FlagShipWaiting = new Npc();

if(questObjective.ExtraObject1 is bool)
{
	reached = (bool)questObjective.ExtraObject1; // as bool;
}

if(!reached)
{
	FlagShipWaiting = new Npc
	{
		Entry = 0,
		Position = waitFlagshipPoint,
		Name = "FlagShip Waiting Point",
		ContinentIdInt = Usefuls.ContinentId,
		Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
	};


	while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(waitFlagshipPoint) >= 5)
	{

		MovementManager.FindTarget(ref FlagShipWaiting, 5);
		Thread.Sleep(500);
	}

	//Waiting point reached, waiting for unit
	Logging.Write("Waiting for the GunShip to arrive next to us.");
	questObjective.ExtraObject1 = true;
	MovementManager.StopMove();
	return false;
}

uint baseAddress = 0;
WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);

if(unit.IsValid)
{
	//baseAddress = MovementManager.FindTarget(unit, 5);
	//Spam CTM to reach the unit
	ClickToMove.CGPlayer_C__ClickToMove(unit.Position.X, unit.Position.Y, unit.Position.Z, 0,
                                (int) ClickToMoveType.Move, 0.5f);

	if (MovementManager.InMovement)
	{
		return false;
	}
	
	if(unit.IsValid && unit.GetDistance > questObjective.Range)
	{
		return false;
	}
	
	/* Target Reached */
	MovementManager.StopMove();
	nManager.Wow.Helpers.Usefuls.DisMount();
	questObjective.ExtraObject1 = null;
}
