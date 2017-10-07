
/*
  move to hotpsot for  Araj the Summoner, id 1852.
  kill Araj.   
  This spawns an object for looting, Araj's Phylactery ID 177241, the actual quest objective.
*/


//Logging.Write("27164 logging - started");

if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(27164))
{
    //Logging.Write("resume of 27164 - quest is marked complete");
    return true;
}

WoWGameObject node;
WoWUnit unit;


Npc questEventPos = new Npc();
questEventPos = new Npc
{
    Entry = 0,
    Position = questObjective.Position,
    Name = "Araj's Phylactery event",
    ContinentIdInt = Usefuls.ContinentId,
    Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
};

//Logging.Write("27164 logging - moving to event location");
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
}

//Logging.Write("27164 logging - arrived");



// will have to fight, to kill Araj the Summoner (1852) and get one to spawn.
// is there a chance, neither are up, so will wait.
Logging.Write("27164 logging - looking for Araj the Summoner");
unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry( 1852));
while( !unit.IsValid)
{
    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
    {
        //Logging.Write("Eeek combat detected - returning control for now...");
        return false;
    }
    Thread.Sleep(1000);
}

//Logging.Write("27164 logging - found Araj the Summoner");
if (unit.IsValid)
{
    MovementManager.Face(unit);
    Thread.Sleep(100 + Usefuls.Latency);
    ObjectManager.Me.Target = unit.Guid;
    
    //Logging.Write("27164 logging - starting fight with Araj the Summoner");
    nManager.Wow.Helpers.Fight.StartFight(unit.Guid);
    if(!unit.IsDead)
      return false;

    //Logging.Write("27164 logging - Araj the Summoner is dead");

    // there is chance them skele fighters are here.
    WoWUnit skele;
    skele = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry( 44329)); 
    if (skele.IsValid)
    {
        //Logging.Write("27164 logging - bombing skeles");
        ItemsManager.UseItem(ItemsManager.GetItemNameById( questObjective.UseItemId ));
        ClickOnTerrain.ClickOnly(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
    }
    Thread.Sleep(1000);
    skele = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry( 44329)); 
    if (skele.IsValid)
    {
        //Logging.Write("27164 logging - bombing skeles");
        ItemsManager.UseItem(ItemsManager.GetItemNameById( questObjective.UseItemId ));
        ClickOnTerrain.ClickOnly(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
    }


    //Logging.Write("27164 logging - is Araj lootable?");
    if ( unit.IsLootable )
    {
        //Logging.Write("27164 logging - yes - looting is true");
        Interact.InteractWith(unit.GetBaseAddress);
        Thread.Sleep(1000);
        //Logging.Write("27164 logging - done interact with dead for loot");
    }


    node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(177241));
    if (node.IsValid)
    {
        //Logging.Write("27164 logging - post fight interacting with Phylactery");
        Interact.InteractWith(node.GetBaseAddress);
        while (ObjectManager.Me.IsCast)
        {
            Thread.Sleep(Usefuls.Latency);
        }
    }
		if (questObjective.WaitMs > 0)
			Thread.Sleep(questObjective.WaitMs);
			
    //escape    
    //Logging.Write("27164 logging - moving away from spawn spot");
    while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 5)
    {
        if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
        {
            //Logging.Write("Eeek combat detected - returning control for now...");
            return false;
        }

        MovementManager.FindTarget(ref questEventPos, 5);
        Thread.Sleep(500);
    }
    
    return true;

}







