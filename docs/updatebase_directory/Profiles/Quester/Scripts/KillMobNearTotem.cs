/*Kill mob near totem
Use ExtraInt for totem ID and AllowPlayerControlled
 */

WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	questObjective.AllowPlayerControlled);
Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
int q = QuestID; /* not used but otherwise getting warning QuestID not used */
uint baseAddress = 0;
uint baseAddressTotem = 0;


if (unit.IsValid && !nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid))
{
			
	if (unit.IsValid && !unit.Attackable)
	{	
		nManagerSetting.AddBlackList(unit.Guid, 30*10000); //Blacklist NotAttackable units
		return false;
	}
	
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true; 
		
	if(!nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer().Exists(x=> x.GetBaseAddress == unit.GetBaseAddress))
	{
		//Go Toward the unit until we aggro it
		baseAddress = MovementManager.FindTarget(unit, questObjective.Range);
	
		//Pre Select Target
		if (unit.IsValid && unit.Position.DistanceTo(ObjectManager.Me.Position) <= 80 && ObjectManager.Target.Guid != unit.Guid)
		{
			Interact.InteractWith(unit.GetBaseAddress);
		}
		
		
		Thread.Sleep(500);
		
		if (baseAddress <= 0)
			return false;
		if (baseAddress > 0 && (unit.IsValid && ((unit.GetDistance > questObjective.Range) && !ObjectManager.Me.InCombat)))
			return false;
			
		MountTask.DismountMount();
	}
	
	WoWUnit totem = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.ExtraInt, questObjective.IsDead),questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
	questObjective.AllowPlayerControlled);
	
	//Place totem if its not there OR replace it if the cooldown is up instead or running back to it
	if(!totem.IsValid || !ItemsManager.IsItemOnCooldown(questObjective.UseItemId) && ItemsManager.IsItemUsable(questObjective.UseItemId))
	{
		if ((ObjectManager.Me.Position.DistanceTo(unit.Position) >= 20) || ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
		return false;

		ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
		Thread.Sleep(500);
	}
	else
	{
		//The mob is aggro, go back to the totem to be in range of it		
		baseAddressTotem = MovementManager.FindTarget(totem, 5f);
		
		if (baseAddressTotem <= 0)
			return false;
		if (baseAddressTotem > 0 && (totem.IsValid && totem.GetDistance > questObjective.Range))
			return false;

		MovementManager.StopMove();
		MountTask.DismountMount();
		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	}
	
	Fight.StartFight(unit.GetBaseAddress);
	
	return false; //Let the bot kill the mob(s)
	
}
	/* Move to Zone/Hotspot */
else if (!MovementManager.InMovement)
{
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	if (questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
	{
		if(nManager.Wow.Helpers.Quest.TravelToQuestZone(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)],
			ref questObjective.TravelToQuestZone, questObjective.ContinentId, questObjective.ForceTravelToQuestZone))
			return false;
		MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
	}
	else
	{
		MovementManager.GoLoop(questObjective.PathHotspots);
	}
}
