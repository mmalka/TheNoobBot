WoWUnit wowUnit = new WoWUnit(0);
wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreBlackList);

if(wowUnit.IsValid)
{
	Logging.Write("Unit Found" + wowUnit.Name);
	if(wowUnit.Position.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
	{
		if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
		return false;

		Logging.Write("Use Bomb!");
		ItemsManager.UseItem(questObjective.UseItemId, wowUnit.Position);
		if(questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);
			
		questObjective.IsObjectiveCompleted = true;
		
	}     
}