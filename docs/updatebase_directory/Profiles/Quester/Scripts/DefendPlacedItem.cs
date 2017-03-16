/* Defend an item that you use */
/* TODO ADD EXTRA FIELD FOR ITEM ENTRY ("NPC ID")!!! 
Dont forget PlayerControlled (usually) and check the flags of the mob that are attacking, you might need IgnoreBlackList */

if(ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 5f)
{
	MovementManager.Go(PathFinder.FindPath(ObjectManager.Me.Position,questObjective.Position));
	while(MovementManager.InMovement && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > 5f)
	{	
		if (ObjectManager.Me.IsDeadMe)
			return false;
		if(ObjectManager.Me.InCombat)
		{
			MountTask.DismountMount();
			return false; //Let the bot kill the hostiles!
		}	
		Thread.Sleep(500);
	}

}
else
{
	
	WoWUnit itemPlaced = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(26678, questObjective.IsDead),questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
		
	//Place item if its not there
	if(!itemPlaced.IsValid)
	{
		if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
		return false;

		ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
		Thread.Sleep(500);
		return true;
	}
	
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
	
	if (unit.IsValid && (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) || questObjective.IgnoreBlackList )) 
	{
		if(unit.Target == itemPlaced.Guid)
		{
			nManager.Wow.Helpers.Fight.StartFight(unit.Guid);
		}
		
		nManagerSetting.AddBlackList(unit.Guid, 30*1000);
	
		/* Wait if necessary */
		if (questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);

		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
		return true;
	}
}