if(questObjective.Position.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
{
	if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
	return false;

	Logging.Write("Use Bomb!");
	ItemsManager.UseItem(questObjective.UseItemId, questObjective.Position);
	if(questObjective.WaitMs > 0)
		Thread.Sleep(questObjective.WaitMs);
		
	questObjective.IsObjectiveCompleted = true;
	
}     