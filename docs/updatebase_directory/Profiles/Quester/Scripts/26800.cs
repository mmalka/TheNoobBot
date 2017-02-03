                WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));
                Point pos;
                uint baseAddress;
                if (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) && unit.IsValid)
                {
                    if (unit.IsValid)
                    {
                        pos = new Point(unit.Position);
                        baseAddress = unit.GetBaseAddress;
                    }
                    else
                    {
                        if (questObjective.InternalQuestId > 0)
                        {
                            if (!nManager.Wow.Helpers.Quest.GetLogQuestId().Contains(questObjective.InternalQuestId))
                                questObjective.IsObjectiveCompleted = true;
                        }
                        return true;
						
                    }
                    MovementManager.Go(PathFinder.FindPath(pos));
                    Thread.Sleep(500 + Usefuls.Latency);
                    while (MovementManager.InMovement && pos.DistanceTo(ObjectManager.Me.Position) > 3.9f)
                    {
                        if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
                            return false;
                        Thread.Sleep(100);
                    }
                    if (questObjective.IgnoreFight)
                    nManager.Wow.Helpers.Quest.GetSetIgnoreFight  = true;
                    MountTask.DismountMount();
                    MovementManager.StopMove();
                    Interact.InteractWith(baseAddress);
                    Thread.Sleep(Usefuls.Latency);
                    while (ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(Usefuls.Latency);
                    }

                    if (questObjective.GossipOptionsInteractWith != 0)
                    {
                        Thread.Sleep(250 + Usefuls.Latency);
                        	nManager.Wow.Helpers.Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
                    }
                    if (ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
                        return false;
                    Thread.Sleep(questObjective.WaitMs);
                    nManagerSetting.AddBlackList(unit.Guid, 600000);
					nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
                }
                else if (!MovementManager.InMovement && questObjective.Hotspots.Count > 0)
                {
                    // Mounting Mount
                    MountTask.Mount();
                    // Need GoTo Zone:
                    if (
                        questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5f)
                    {
                        MovementManager.Go(PathFinder.FindPath(questObjective.Hotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.Hotspots, ObjectManager.Me.Position)]));
                    }
                    else
                    {
                        // Start Move
                        MovementManager.GoLoop(questObjective.Hotspots);
                    }
                }