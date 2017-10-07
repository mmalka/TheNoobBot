
/*
  move to hotpsot for to kill mob.
  need to use item on mob ( a spear ) several times until mob comes out of hiding and able to fight.
  This appears to be health, more than the number of hits.
*/
   	
   	
//Logging.Write("13512 logging - started");

if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(13512) || nManager.Wow.Helpers.Quest.IsObjectiveCompleted(13512,questObjective.InternalIndex,1) )
{
    //Logging.Write("resume of 13512 - quest is marked complete");
    return true;
}

WoWUnit unit;


Npc questEventPos = new Npc();
questEventPos = new Npc
{
    Entry = 0,
    Position = questObjective.Position,
    Name = "Strategic Strikes event",
    ContinentIdInt = Usefuls.ContinentId,
    Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
};

//Logging.Write("13512 logging - moving to event location");
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

//Logging.Write("13512 logging - arrived");



// if mob not up yet, will fight while wait.
//Logging.Write("13512 logging - looking for quest mob");

unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry( questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,questObjective.AllowPlayerControlled);
while( !unit.IsValid)
{
    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
    {
        //Logging.Write("Eeek combat detected - returning control for now...");
        return false;
    }
    Thread.Sleep(1000);
    unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry( questObjective.Entry, questObjective.IsDead), questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList,questObjective.AllowPlayerControlled);
}

//Logging.Write("13512 logging - found " + unit.Name);
if (unit.IsValid)
{
    MovementManager.Face(unit);
    Thread.Sleep(100 + Usefuls.Latency);
    ObjectManager.Me.Target = unit.Guid;


    Logging.Write("13512 logging - starting 'use item' cycle until mob moves ");

    nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
    while( unit.IsValid && unit.HealthPercent >= 50.0f )
    {
        ItemsManager.UseItem(ItemsManager.GetItemNameById( questObjective.UseItemId ));
        Logging.Write("13512 logging - " + unit.Name + " health % is " + unit.HealthPercent );

        if (questObjective.WaitMs > 0)
            Thread.Sleep(questObjective.WaitMs);        
    }
    
    Thread.Sleep(1000);
     
    nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
    
    //Logging.Write("13512 logging - starting fight with " + unit.Name);

    nManager.Wow.Helpers.Fight.StartFight(unit.Guid);
    if(!unit.IsDead)
      return false;


    //Logging.Write("13512 logging - " +unit.Name + " is dead");

    
    return true;

}



