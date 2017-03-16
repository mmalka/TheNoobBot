WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	questObjective.AllowPlayerControlled);
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress = 0;

/* If Entry found continue*/
if (unit.IsValid)
{
	/* Entry found, GoTo */
	if(unit.InCombat)
	{
		
		ObjectManager.Me.Target = unit.Target;
		MountTask.DismountMount();
		nManager.Wow.Helpers.Fight.StartFight(unit.Target);
		return false;
	}
	
	baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
	
	Thread.Sleep(500);	
	
	if (MovementManager.InMovement)
		return false;
		
	if (baseAddress <= 0)
		return false;
	if (baseAddress > 0 && (unit.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range))
		return false;

	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

	/* Target Reached */
	//MovementManager.StopMove();
	//MountTask.DismountMount();

	//Thread.Sleep(Usefuls.Latency + 150);

	/* Wait if necessary */
	if (questObjective.WaitMs > 0)
		Thread.Sleep(questObjective.WaitMs);

}