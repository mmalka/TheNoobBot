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
************************************************************/
if (!MovementManager.InMovement)
{
    Point onPosition = new Point();
    WoWUnit wowUnit = new WoWUnit(0);
    if (string.IsNullOrEmpty(questObjective.LuaMacro))
    {
        Logging.Write("Objective CSharpScript with script UseLuaMacroOnPosition requires a valid \"LuaMacro\" for QuestId: " + questObjective.QuestId);
        questObjective.IsObjectiveCompleted = true;
        return false;
    }
    if (questObjective.Entry.Count <= 0)
    {
        if (questObjective.Position.IsValid)
        {
            var target = new Npc { Name = "Objective position for QuestId", Position = questObjective.Position, Entry = questObjective.QuestId };
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
            Logging.Write("Objective CSharpScript with script UseLuaMacroOnPosition requires a valid \"Hotspots\" with Entry for QuestId: " + questObjective.QuestId);
            questObjective.IsObjectiveCompleted = true;
            return false;
        }

        //if (ObjectManager.Me.Target > 0 && ObjectManager.Me.Target == ObjectManager.Target.Guid && ObjectManager.Target.IsValid && (questObjective.IsDead || ObjectManager.Target.IsAlive) && questObjective.Entry.Contains(ObjectManager.Target.Entry))
        //{
        //    wowUnit = ObjectManager.Target;
        //}
        //else 
        List<WoWUnit> availableEntries = new List<WoWUnit>();
        if (questObjective.IsDead)
        {
            ObjectManager.GetWoWUnitByEntry(questObjective.Entry, true).ForEach(delegate (WoWUnit unit)
            {
                if (unit.IsDead && !unit.IsAlive && !unit.GetDescriptor<UnitFlags>(nManager.Wow.Patchables.Descriptors.UnitFields.Flags).HasFlag(UnitFlags.PreventEmotes))
                    availableEntries.Add(unit);
            });

            if (availableEntries.Count == 0)
                availableEntries = ObjectManager.GetWoWUnitByEntry(questObjective.Entry, false);
        }
        else
            availableEntries = ObjectManager.GetWoWUnitByEntry(questObjective.Entry, false);

        wowUnit = ObjectManager.GetNearestWoWUnit(availableEntries, questObjective.IgnoreBlackList);

        if (/*!IsInAvoidMobsList(wowUnit) &&*/ !nManager.nManagerSetting.IsBlackListedZone(wowUnit.Position) && !nManager.nManagerSetting.IsBlackListed(wowUnit.Guid) && wowUnit.IsValid)
        {
    
            /*
             * Check if mob has a specific buff active if defined in objective
             */
            if (questObjective.BuffId > 0 && wowUnit.HaveBuff((uint)questObjective.BuffId))
            {
                // TODO: Should be great to have the option of creatorGUID
                Auras.UnitAura aura = wowUnit.UnitAura((uint)questObjective.BuffId);
                if (aura.IsValid)
                {
                    Logging.Write("DEBUG: Mob has the aura (" + wowUnit.Guid.ToString() + ")");
                    nManagerSetting.AddBlackList(wowUnit.Guid, aura.AuraDuration + 1000);
                    return false;
                }
            }


            /*
                Check if:
                - Objective don't want a dead mob
                - Mob is not attackable
                - Mob is dead and is taped by me
            */
            if (questObjective.IsDead && !wowUnit.IsDead && wowUnit.IsHostile)
            {
                if (wowUnit.IsDead || (wowUnit.InCombat && !questObjective.CanPullUnitsAlreadyInFight))
                {
                    Logging.Write("DEBUG: Blacklist mob because in combat or is dead");
                    nManagerSetting.AddBlackList(wowUnit.Guid, 3 * 60 * 1000);
                    return false;
                }
                else
                {
                    Logging.Write("DEBUG: Resolve combat");
    
                    Interact.InteractWith(wowUnit.GetBaseAddress);
                    MovementManager.FindTarget(wowUnit, CombatClass.GetAggroRange);
                    Thread.Sleep(50);
                    // TODO: When lockedTarget is available set the possibility to defend during the way
                    if (MovementManager.InMovement)
                        return false;
                    Logging.Write("Attacking Lvl " + wowUnit.Level + " " + wowUnit.Name);

                    System.Threading.Thread thread = new System.Threading.Thread((System.Threading.ParameterizedThreadStart)delegate (object guid) { Fight.StartFight((UInt128)guid); });
                    thread.IsBackground = true;
                    thread.Start(wowUnit.Guid);
                    Thread.Sleep(100);
                    if (!wowUnit.IsDead && wowUnit.HealthPercent >= 100.0f)
                    {
                        nManagerSetting.AddBlackList(wowUnit.Guid, 3 * 60 * 1000);
                        Logging.Write("Can't reach " + wowUnit.Name + ", blacklisting it.");
                        thread.Abort();
                        return false;
                    }
    
                    while (!wowUnit.IsDead && !ObjectManager.Me.IsDead)
                        Thread.Sleep(200);

                    if (wowUnit.IsDead)
                    {
                        Logging.Write("DEBUG: Dead: " + wowUnit.IsDead);
                        nManager.Statistics.Kills++;
                        Thread.Sleep(50 + Usefuls.Latency);
                        while (!ObjectManager.Me.IsMounted && ObjectManager.Me.InCombat && ObjectManager.GetNumberAttackPlayer() <= 0)
                            Thread.Sleep(50);
                        Fight.StopFight();
                    }
                    thread.Abort();
                }
            }

            if (!questObjective.IsDead || !wowUnit.IsHostile || (wowUnit.IsDead && (!wowUnit.IsTapped || wowUnit.IsTappedByMe || wowUnit.IsLootable)))
            {
                Logging.Write("DEBUG: Mob is targeted for the action ");
    
                ObjectManager.Me.Target = wowUnit.Guid;
                Interact.InteractWith(wowUnit.GetBaseAddress);
                MovementManager.StopMove();
                onPosition = wowUnit.Position;
                MovementManager.FindTarget(wowUnit, questObjective.Range);
                Thread.Sleep(50);
                // TODO: When lockedTarget is available set the possibility to defend during the way
                if (MovementManager.InMovement)
                    return false;
            }
            else
                return false;
            
        }
        else if (!MovementManager.InMovement)
        {
            Logging.Write("DEBUG: Resolve movement");
            if (questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
            {
                MovementManager.Go(PathFinder.FindPath(questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)]));
            }
            else
            {
                MovementManager.GoLoop(questObjective.Hotspots);
            }
            return false;
        }
    }

    Logging.Write("In evitable combat: " + ObjectManager.Me.InInevitableCombat);
    if (ObjectManager.Me.InInevitableCombat)
        return false;

    Logging.Write("Position: " + onPosition.IsValid);
    if (!onPosition.IsValid)
        return false;

    MovementManager.StopMove();
    Lua.RunMacroText(questObjective.LuaMacro);
    Thread.Sleep(200);
    ClickOnTerrain.ClickOnly(onPosition);
    if (questObjective.WaitMs > 0)
        Thread.Sleep(questObjective.WaitMs);
    questObjective.IsObjectiveCompleted = true;
    if (wowUnit.IsValid)
        nManagerSetting.AddBlackList(wowUnit.Guid, 3 * 60 * 1000);
    Thread.Sleep(50);
}