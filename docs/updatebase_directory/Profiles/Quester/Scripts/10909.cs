/* Pull mob in range of item placed on the ground. Entries Hardcoded*/

/* Search for mob */
WoWUnit wowUnit1 = new WoWUnit(0);
wowUnit1 = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(16878, questObjective.IsDead), questObjective.IgnoreBlackList);
	
if (!nManagerSetting.IsBlackListedZone(wowUnit1.Position) &&
				!nManagerSetting.IsBlackListed(wowUnit1.Guid) && wowUnit1.IsAlive && wowUnit1.IsValid &&
				(questObjective.CanPullUnitsAlreadyInFight || !wowUnit1.InCombat))
{
	/*Logging.Write("Mob Found");*/
		
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
	
	MovementManager.FindTarget(wowUnit1, CombatClass.GetAggroRange);
	Thread.Sleep(500);
	/* Logging.Write("Going for the pull"); */
	while(ObjectManager.Me.Position.DistanceTo(wowUnit1.Position) > 5f)
	{
		MovementManager.FindTarget(wowUnit1, CombatClass.GetAggroRange);
		Thread.Sleep(500);
	}
	/* Target Reached */
	MovementManager.StopMove();

	if(Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) nManager.Wow.Patchables.Addresses.GameInfo.AreaId+4) != 3811)
	{
		/* Wrong SubZone to place Item, return (kill the mob) */
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
		return false;
	}
	
	if (ItemsManager.GetItemCount(questObjective.UseItemId) > 0 && !ItemsManager.IsItemOnCooldown(questObjective.UseItemId) && ItemsManager.IsItemUsable(questObjective.UseItemId))
	{
		Thread.Sleep(500); /* Wait For Stun (Interrupts Use Item) */
		if(ObjectManager.Me.IsStunned)
		{
			Logging.Write("STUNNED");
			nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
			return false;
		}
		Logging.Write("Place Relic");
		ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
		questObjective.Position = ObjectManager.Me.Position; /* Save Position  */
		Thread.Sleep(2000); /* Wait for the relic to be placed */
	}
	
	/* Go back to relic 185298 if Player too Far */
	
	Npc relic = new Npc();
	relic = new Npc
	{
		Entry = 185298,
		Position = questObjective.Position,
		Name = "Relic",
		ContinentIdInt = Usefuls.ContinentId,
		Faction = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
	};
	MovementManager.FindTarget(ref relic, 10f);
	
	while(ObjectManager.Me.Position.DistanceTo(questObjective.Position) > 10f)
	{
		MovementManager.FindTarget(ref relic, 10f);
		Thread.Sleep(500);
	}
	MovementManager.StopMove();
	
	/* Relic Reached, In range*/

	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	Thread.Sleep(500);
	
	/* Kill Mob */
	WoWUnit wowUnit2 = new WoWUnit(0);
	wowUnit2 = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(22454, questObjective.IsDead), questObjective.IgnoreBlackList);

	if(wowUnit2.IsValid)
	{
		Fight.StartFight(wowUnit2.Guid);
	}
}
else /*Move To Quest Spot*/
{
	Npc target = new Npc();
	target = new Npc
	{
		Entry = 1337,
		Position = questObjective.Position,
		Name = "Quest Spot",
		ContinentIdInt = Usefuls.ContinentId,
		Faction = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
	};
	MovementManager.FindTarget(ref target, questObjective.Range > 5f ? questObjective.Range : 0);
	if (MovementManager.InMovement)
		return false;

			
}