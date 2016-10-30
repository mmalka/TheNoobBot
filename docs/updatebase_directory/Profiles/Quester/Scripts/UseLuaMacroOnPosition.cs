/*******************************************************
    * USAGE: 
    * *****************************************************
    - OPTIONS:
    WaitMs: Will wait xxxx millisecond after having done the actions
    Range: Distance to be to perform actions

    - On player position:
    <Objective>CSharpScript</Objective>
    <Script>=UseLuaMacroOnPosition.cs</Script>
    <LuaMacro>/click TheButtonYouWantToClick</LuaMacro>

    - On given position
    <Objective>CSharpScript</Objective>
    <Script>=UseLuaMacroOnPosition.cs</Script>
    <LuaMacro>/click TheButtonYouWantToClick</LuaMacro>
    <Position>
        <X></X>
        <Y></Y>
        <Z></Z>
    </Position>

    - On previously killed mob or an NPC, will target the mob first if the action is on target
    <Objective>CSharpScript</Objective>
    <Script>=UseLuaMacroOnPosition.cs</Script>
    <LuaMacro>/click TheButtonYouWantToClick</LuaMacro>
    <Entry>
        <int>xxx</int>
    </Entry>
    <Hotspots>
        <Point>
            <X></X>
            <Y></Y>
            <Z></Z>
        </Point>
    </Hotspots>
    * 
    */
Point onPosition = new Point();
if (string.IsNullOrEmpty(questObjective.LuaMacro))
{
    /* Objective CSharpScript with script UseLuaMacroOnPosition requires a valid "LuaMacro" for QuestId: " + QuestId*/
    Logging.Write("UseLuaMacroOnPosition require 'LuaMacro'.");
    questObjective.IsObjectiveCompleted = true;
    return false;
}
if (questObjective.Entry.Count <= 0)
{
    if (questObjective.Position.IsValid)
    {
        var target = new Npc { Name = "Objective position for QuestId", Position = questObjective.Position, Entry = QuestID };
        MovementManager.FindTarget(ref target, questObjective.Range);
        Thread.Sleep(100);
        if (MovementManager.InMovement)
            return false;
        onPosition = questObjective.Position;
    }
    else
        onPosition = ObjectManager.Me.Position;
}
else if (questObjective.Entry.Count > 0)
{
    if (questObjective.Hotspots.Count <= 0)
    {
        /* Objective CSharpScript with script UseLuaMacroOnPosition requires a valid "Hotspots" with Entry for QuestId: " + QuestId*/
        Logging.Write("UseLuaMacroOnPosition require 'LuaMacro'.");
        questObjective.IsObjectiveCompleted = true;
        return false;
    }
    if (!MovementManager.InMovement)
    {
        if (questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
        {
            MovementManager.Go(PathFinder.FindPath(questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)]));
        }
        else
        {
            MovementManager.GoLoop(questObjective.Hotspots);
        }
    }
    WoWUnit wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreBlackList);
    if ( /*!IsInAvoidMobsList(wowUnit) && */!nManager.nManagerSetting.IsBlackListedZone(wowUnit.Position) && !nManager.nManagerSetting.IsBlackListed(wowUnit.Guid) && wowUnit.IsValid)
    {
        if (!wowUnit.IsHostile || (!wowUnit.IsAlive && (!wowUnit.IsTapped || (wowUnit.IsTapped && wowUnit.IsTappedByMe))))
        {
            uint baseAddress = MovementManager.FindTarget(wowUnit, questObjective.Range);
            Thread.Sleep(100);
            if (MovementManager.InMovement)
                return false;
            onPosition = wowUnit.Position;
            Interact.InteractWith(baseAddress);
            MovementManager.StopMove();
        }
        else if (questObjective.CanPullUnitsAlreadyInFight || !wowUnit.InCombat)
        {
            /*if (QuestingTask.lockedTarget == null)
        {
            QuestingTask.lockedTarget = wowUnit;
        }
        else
        {
            if (QuestingTask.lockedTarget.IsValid && QuestingTask.lockedTarget.IsAlive)
                wowUnit = QuestingTask.lockedTarget;
        }*/
            MovementManager.FindTarget(wowUnit, CombatClass.GetAggroRange);
            Thread.Sleep(100);
            if (MovementManager.InMovement)
                return false;
            Logging.Write("Attacking Lvl " + wowUnit.Level + " " + wowUnit.Name);
            UInt128 unkillable = Fight.StartFight(wowUnit.Guid);
            if (!wowUnit.IsDead && unkillable != 0 && wowUnit.HealthPercent >= 100.0f)
            {
                nManagerSetting.AddBlackList(unkillable, 3*60*1000);
                Logging.Write("Can't reach " + wowUnit.Name + ", blacklisting it.");
                return false;
            }
            if (wowUnit.IsDead)
            {
                nManager.Statistics.Kills++;
                if (!wowUnit.IsTapped || (wowUnit.IsTapped && wowUnit.IsTappedByMe))
                {
                    onPosition = wowUnit.Position;
                    Interact.InteractWith(wowUnit.GetBaseAddress);
                    MovementManager.StopMove();
                }
                Thread.Sleep(50 + Usefuls.Latency);
                while (!ObjectManager.Me.IsMounted && ObjectManager.Me.InCombat && ObjectManager.GetNumberAttackPlayer() <= 0)
                {
                    Thread.Sleep(50);
                }
                Fight.StopFight();
            }
            /*QuestingTask.lockedTarget = null;*/
        }
    }
}
if (ObjectManager.Me.InInevitableCombat)
    return false;

if (!onPosition.IsValid)
    return false;
MovementManager.StopMove();
Lua.RunMacroText(questObjective.LuaMacro);
Thread.Sleep(200);
ClickOnTerrain.ClickOnly(onPosition);
if (questObjective.WaitMs > 0)
    Thread.Sleep(questObjective.WaitMs);
questObjective.IsObjectiveCompleted = true;
Thread.Sleep(50);