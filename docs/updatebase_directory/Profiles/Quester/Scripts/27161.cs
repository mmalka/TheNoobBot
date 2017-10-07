// run the gauntlet!
// 
// outside the tower are a constant stream of skeleton.
// quest objective is to get inside the tower to bomb Scourge Bone Animus, 1 of.
// side objective is to also kill some skeleton, 5 of.

// Scourge Bone Animus Bunny is WoWUnit 44360
// the skele "Stickbone Berserker" are WoWUnit 44329

// character is armed with a bomb, Lang's Hand Grenades, item 60849.
// it has 1 seconds cooldowns.

// will start outside the tower at QO position.
// run inside tower, throwing bomb until get to the SBA, turn around and run back to position.

// this is a gauntlet, so there is no fighting of aggro mobs.
// get in, get out,  then fend off the consequences.
 

//Logging.Write("27161 logging - started");

if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(27161))
{
    //Logging.Write("resume of 27161 - quest is marked complete");
    return true;
}


Npc questEventPos = new Npc();
questEventPos = new Npc
{
    Entry = 0,
    Position = questObjective.Position,
    Name = "Tower Gauntlet start point",
    ContinentIdInt = Usefuls.ContinentId,
    Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
};


//Logging.Write("27161 logging - moving to event location");
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


//Logging.Write("27161 logging - arrived");


//not fighting..  running!
nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;


// get the SBA details
WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead),
                                               questObjective.IgnoreNotSelectable, questObjective.IgnoreBlackList, questObjective.AllowPlayerControlled);
		
	
//Logging.Write("27161 logging - going in");


// run into tower, towards the SBA, abort early if the quest completes.		
while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) >= 5  || 
        !nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(27161) )
{
    MovementManager.FindTarget(unit, 5);
    ItemsManager.UseItem(ItemsManager.GetItemNameById( questObjective.UseItemId ));
    ClickOnTerrain.ClickOnly(nManager.Wow.ObjectManager.ObjectManager.Me.Position);     
    Thread.Sleep(1000);  
}



// Ran in, now get out.

//Logging.Write("27161 logging - heading out");

Npc EscapeTowerPos = new Npc();
EscapeTowerPos = new Npc
{
    Entry = 0,
    Position = questObjective.Position,
    Name = "Escape the Tower",
    ContinentIdInt = Usefuls.ContinentId,
    Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
};


while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 5)
{
    MovementManager.FindTarget(ref EscapeTowerPos, 5);
    ItemsManager.UseItem(ItemsManager.GetItemNameById( questObjective.UseItemId ));
    ClickOnTerrain.ClickOnly(nManager.Wow.ObjectManager.ObjectManager.Me.Position);     
    Thread.Sleep(1000);
}

//Logging.Write("27161 logging - should be out");


// lock n load the weapons, combat authorised!
nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;

MovementManager.StopMove();
Thread.Sleep(Usefuls.Latency);

//Logging.Write("27161 logging - finished");


return true;

