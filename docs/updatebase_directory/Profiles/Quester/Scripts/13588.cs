
/* quest objective destroy twlight portal id 34316 x 1,  index 1
               and kill twilight rider id 34282 x12,  index 2
  
   this is a unit event, riding a dragon.
   Button 1 to fire    "emerald barrage"
   Button 6 to finish.   "land thessera"         
*/

nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;

if (questObjective.WaitMs > 0)
  Thread.Sleep(questObjective.WaitMs);


while(!nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(13588))
{
   WoWUnit unit;
  
  if ( nManager.Wow.Helpers.Quest.GetCurrentInternalIndexCount(13588, 1) == 1 )
  {
    Logging.Write("13588 logging - Portal done, looking for riders only");
    // portal done, target only riders
    unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(34282, questObjective.IsDead),
	                               questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);   
  }
  else if ( nManager.Wow.Helpers.Quest.GetCurrentInternalIndexCount(13588, 2) == 12 )
  {
    Logging.Write("13588 logging - Riders done, looking for portal only");
    // riders done, target only portal
    unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(34316, questObjective.IsDead),
	                               questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);    
  }
  else
  {
	  unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead),
	                               questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
    
  }

	Point pos = ObjectManager.Me.Position; /* Initialize or getting an error */
		
	/* If Entry found continue, otherwise continue checking around HotSpots */
	if (unit.IsValid && !nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid))
	{
		/* Entry found, GoTo */

		Thread.Sleep(100 + Usefuls.Latency); /* ZZZzzzZZZzz */
		
		MovementManager.Face(unit);
		Interact.InteractWith(unit.GetBaseAddress);

		Lua.RunMacroText(questObjective.LuaMacro);
		
		Thread.Sleep(Usefuls.Latency + 150);

/*
		if (unit.IsValid)
		{
			nManagerSetting.AddBlackList(unit.Guid, 30*1000);
		}
*/

		Thread.Sleep(2500);

		nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
	}
}

    Logging.Write("13588 logging - finished, head for home");

// to finish, need to run button 6.  land thessera

Lua.RunMacroText("/click OverrideActionBarButton6");

if (questObjective.WaitMs > 0)
  Thread.Sleep(questObjective.WaitMs);

return true;
