
/*
int progresscount = nManager.Wow.Helpers.Quest.GetCurrentInternalIndexCount(26954,1);

if ( progresscount == 0)
  Logging.Write("26954 logging - started");
else
  Logging.Write("26954 logging - resumed in progress " + progresscount );
*/


if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(26954))
{
    //Logging.Write("26954 logging - resumed with quest marked complete");
    return true;
}

WoWUnit unit;
WoWUnit trollpetunit;

Npc questEventPos = new Npc();
questEventPos = new Npc
{
    Entry = 0,
    Position = questObjective.Position,
    Name = "Hilltop kill Diseased Hawks event",
    ContinentIdInt = Usefuls.ContinentId,
    Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
};

/*
if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 5)
    Logging.Write("26954 logging - moving to event location");
*/

// going to move to quest event position
while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 5)
{
    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
    {
        //Logging.Write("Eeek combat detected - returning control for now...");
        return false;
    }

    MovementManager.FindTarget(ref questEventPos, 5);
    Thread.Sleep(500);
/*
    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(questObjective.Position) <= 5)
    Logging.Write("26954 logging - arrived");
*/
}


trollpetunit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(44269), false, false, true);
while( trollpetunit.InCombat && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && trollpetunit.Target == 0 )
{
//        Logging.Write("26954 logging - troll thinks he's in a fight, let him get over it");
        Thread.Sleep(500 + Usefuls.Latency);
}

while( trollpetunit.Target == 0 && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !trollpetunit.InCombat )
{
    // are there hawks in range?
    unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(44481));		
    while ( nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) > 50 )    
    {
        //Logging.Write("26954 logging - waiting for in range on " + unit.Name + " @ " + nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) );
        Thread.Sleep(2000 + Usefuls.Latency);
        unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(44481));
    }
    unit = null;

    Logging.Write("26954 logging - poke the troll to cast..");
    Interact.InteractWith(trollpetunit.GetBaseAddress);
    Thread.Sleep(2000 + Usefuls.Latency );

/*
    Logging.Write("26954 logging - troll target " + trollpetunit.Target );
    Logging.Write("26954 logging - me combat? " + nManager.Wow.ObjectManager.ObjectManager.Me.InCombat );
    Logging.Write("26954 logging - troll combat? " + trollpetunit.InCombat );
*/

    if (questObjective.WaitMs > 0)
        Thread.Sleep(questObjective.WaitMs);
}



// troll has attacked a hawk, so assist the troll
WoWObject obj = ObjectManager.GetObjectByGuid(trollpetunit.Target);
unit = new WoWUnit(obj.GetBaseAddress);			

if (unit.IsValid)
{
    while(nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) > questObjective.Range )
    {
        Logging.Write("26954 logging - Holding for combat... "+ unit.Name + " @ range:" +
                      nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) );
        Thread.Sleep(500);
    }

    MovementManager.Face(unit);
    ObjectManager.Me.Target = unit.Guid;
    
    Logging.Write("26954 logging - returning control..");
    return true;
}



