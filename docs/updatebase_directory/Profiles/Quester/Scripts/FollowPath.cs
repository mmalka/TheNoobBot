if (questObjective.Hotspots.Count <= 0)
{
    Logging.Write("Objective CSharpScript with script FollowPath require at least one hotspots");
    questObjective.CollectCount = 0;
    questObjective.IsObjectiveCompleted = true;
    return false;
}

Point reference = ObjectManager.Me.Position;
WoWUnit unit = new WoWUnit(0);
if (questObjective.Entry.Count > 0)
{
    unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
    if (!unit.IsValid)
    {
        Logging.Write("Objective CSharpScript with script FollowPath has not found NPC (" + questObjective.Entry + ")"); 
        questObjective.CollectCount = 0;
        questObjective.IsObjectiveCompleted = true;
        return false;
    }
    if (!unit.Position.IsValid)
    {
        Logging.Write("NPC (" + unit.Entry + ") Objective CSharpScript with script FollowPath has no valid position"); 
        questObjective.CollectCount = 0;
        questObjective.IsObjectiveCompleted = true;
        return false;
    }
    reference = unit.Position;
}
    
// If set the position will be used to set the objective as done
if (questObjective.CollectCount >= questObjective.Hotspots.Count || 
    (questObjective.Position.IsValid && questObjective.Position.DistanceTo(reference) <= questObjective.Range))
{
    questObjective.CollectCount = 0;
    questObjective.IsObjectiveCompleted = true;
    return true;
}
    
bool CurrentAllowMount = nManagerSetting.CurrentSetting.UseGroundMount;
// TODO: Change this to IgnoreMount when available
if(questObjective.IgnoreItemNotUsable)
{
    MountTask.DismountMount();
    nManagerSetting.CurrentSetting.UseGroundMount = false;
}

if(questObjective.IgnoreFight)
    nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;    

if(ObjectManager.Pet.IsValid && ObjectManager.Pet.IsAlive && questObjective.DismissPet)
{
	Spell Dismiss = new Spell("Dismiss Pet");
	Dismiss.Cast();
}
Thread.Sleep(200);
if (unit.IsValid)
{
    Point p = questObjective.Hotspots[questObjective.CollectCount];
    Point n = unit.Position;
    int limit = 100;
    while (limit > 0 && p.DistanceTo(unit.Position) > questObjective.Range)
    {
        if (!unit.IsValid)
        {
            MovementManager.StopMove();
            Logging.Write("NPC (" + unit.Entry + ") Objective CSharpScript with script FollowPath has disapear"); 
            questObjective.CollectCount = 0;
            questObjective.IsObjectiveCompleted = true;
            return false;
        }
        ClickToMove.CGPlayer_C__ClickToMove(p.X, p.Y, p.Z, unit.Guid, 4, 0.5f);
        Thread.Sleep(500);
        if (n.DistanceTo(unit.Position) < questObjective.Range)
            limit--;
        n = unit.Position;
    }
}
else
{

    MovementManager.Go(new List<Point> { reference, questObjective.Hotspots[questObjective.CollectCount] });

    while (MovementManager.InMovement)
    {
        if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted && !questObjective.IgnoreFight))
        {
            nManagerSetting.CurrentSetting.UseGroundMount = CurrentAllowMount;
            nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
            return true;
        }
        Thread.Sleep(100);
    }
}
questObjective.CollectCount++;

if (questObjective.WaitMs > 0)
  Thread.Sleep(questObjective.WaitMs);

nManagerSetting.CurrentSetting.UseGroundMount = CurrentAllowMount;
nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
return true;