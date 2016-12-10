
if(questObjective.Entry.Count <= 0)
{
    Logging.Write("Objective CSharpScript with script FollowEntry require at least one entry");
    questObjective.IsObjectiveCompleted = true;
    return false;
}

WoWUnit wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, false), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
if (!wowUnit.IsValid)
{
    // Accept a little timeout and try again (sometime the NPC need ~1-2 seconds to change his ID
    Thread.Sleep(2000);
    wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, false), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
}

if (!wowUnit.IsValid)
{
    if (!questObjective.Position.IsValid || questObjective.Position.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
    {
        Logging.Write("Objective CSharpScript with script FollowEntry has found no entry to follow");
        questObjective.IsObjectiveCompleted = true;
        return false;
    }
	MovementManager.Go(PathFinder.FindPath(questObjective.Position));
    return true;
}

if (wowUnit.Position.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
{
    questObjective.IsObjectiveCompleted = true;
    return true;
}

if (wowUnit.IsMounted)
    MountTask.Mount();
else
    MountTask.DismountMount();
    
bool resultSuccess = false;
List<Point> listPoint = PathFinder.FindPath(wowUnit.Position, out resultSuccess);
if (resultSuccess)
    MovementManager.Go(listPoint);
else
    MovementManager.Go(new List<Point> { ObjectManager.Me.Position, wowUnit.Position }); // When bot is not able to find a path to target

while (MovementManager.InMovement)
    Thread.Sleep(100);
return true;