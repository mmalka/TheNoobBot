WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	questObjective.AllowPlayerControlled);
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress = 0;

/* If Entry found continue*/
if (unit.IsValid)
{
	/* Entry found, GoTo */
	Npc EscortPositionNpc = new Npc();
	EscortPositionNpc = new Npc
	{
		Entry = 0,
		Position = questObjective.Position,
		Name = "Escort Position",
		ContinentIdInt = Usefuls.ContinentId,
		Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
	};

	
	baseAddress = MovementManager.FindTarget(ref EscortPositionNpc, questObjective.Range);
	
	Thread.Sleep(500);


		
	if(unit.InCombat)
	{
		nManager.Wow.Helpers.Fight.StartFight(unit.Target);
	}
	
	if (MovementManager.InMovement)
		return false;
		
	if (baseAddress <= 0)
		return false;
	if (baseAddress > 0 && (EscortPositionNpc.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range))
		return false;
	

	Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */

	/* Target Reached */
	MovementManager.StopMove();
	//MountTask.DismountMount();

	Thread.Sleep(Usefuls.Latency + 150);

	/* Wait if necessary */
	if (questObjective.WaitMs > 0)
		Thread.Sleep(questObjective.WaitMs);

}