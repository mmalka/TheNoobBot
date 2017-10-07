/* Defend a fix npc
check the flags of the mob that are attacking, you might need IgnoreBlackList */
if(!questObjective.Position.IsValid)
	questObjective.Position = ObjectManager.Me.Position;

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
	
	WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,
		questObjective.AllowPlayerControlled);
		
	WoWUnit hostile = new WoWUnit(0);
	
	hostile = nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitNearPlayer()
	.FindAll(x => x.GetDistance <= 20 && x.IsHostile)
	.Find(x => x.Target == ObjectManager.Me.Guid || x.Target == unit.Guid);

	if(hostile != null && hostile.IsValid)
	{
		nManager.Wow.Helpers.Fight.StartFight(hostile.Guid);
		
		nManagerSetting.AddBlackList(hostile.Guid, 30*1000);
	
		/* Wait if necessary */
		if (questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);
	
	}
	nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	return true;
	
}