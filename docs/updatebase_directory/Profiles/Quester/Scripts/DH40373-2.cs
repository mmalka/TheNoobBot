WoWUnit unit =ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry));
uint baseAddress = 0;
baseAddress = MovementManager.FindTarget(unit);
Thread.Sleep(2000); 
Interact.InteractWith(baseAddress);
Thread.Sleep(2000);
Lua.RunMacroText("/click QuestFrameCompleteQuestButton");