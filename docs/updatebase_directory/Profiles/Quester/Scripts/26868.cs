

// if quest not complete, work on completion...
if(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(26868))
{
  Logging.Write("resume of 26868 - quest is marked complete");
}
else
{
    //Dummy NPC made for quest event position
    nManager.Wow.Class.Npc questEventPos = new nManager.Wow.Class.Npc();
    questEventPos = new nManager.Wow.Class.Npc
    {
        Entry = 0,
        Position = questObjective.Position,
        Name = "Quest Event Position",
        ContinentIdInt = Usefuls.ContinentId,
        Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ?
                  nManager.Wow.Class.Npc.FactionType.Horde : nManager.Wow.Class.Npc.FactionType.Alliance,
    };


    // going to move to quest event position
    while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(questObjective.Position) >= 5)
    {
        if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
        {
            Logging.Write("Eeek combat detected - returning control for now...");
            return false;
        }
        // need to ensure remain with the "clever plant disguise" buff
        if (!ObjectManager.Me.UnitAura(82788).IsValid)
        {
            Logging.Write("Missing 'clever plant disguise', redoing.. ");
            ItemsManager.UseItem(ItemsManager.GetItemNameById(60502));
        }
        MovementManager.FindTarget(ref questEventPos, 5);
        Thread.Sleep(500);
    }

    // Should now have arrived at quest event position

    Logging.Write("arrived at quest event position ");

    WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(44262, questObjective.IsDead));
    if ( unit.IsValid )
    {
        ObjectManager.Me.Target = unit.Guid;
        MovementManager.Face(unit);
        ItemsManager.UseItem(ItemsManager.GetItemNameById(60503));
        Thread.Sleep(Usefuls.Latency + 250);
    }

    // Quest should now be completed ok.
    while (!nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(26868))
    {
//        Logging.Write("Quest 26868 status = " + nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(26868).ToString());
        Thread.Sleep(500);
    }
}




//Dummy NPC made for escape
nManager.Wow.Class.Npc escapeEventPos = new nManager.Wow.Class.Npc();
escapeEventPos = new nManager.Wow.Class.Npc
{
    Entry = 0,
    Position = questObjective.ExtraPoint,
    Name = "Quest Escape Position",
    ContinentIdInt = Usefuls.ContinentId,
    Faction = nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde" ?
              nManager.Wow.Class.Npc.FactionType.Horde : nManager.Wow.Class.Npc.FactionType.Alliance,
};


while (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(questObjective.ExtraPoint) >= 5)
{
    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
    {
        Logging.Write("Eeek combat detected - returning control for now...");
        return false;
    }

    // need to ensure remain with the "clever plant disguise" buff
    if (!ObjectManager.Me.UnitAura(82788).IsValid)
    {
        Logging.Write("Missing 'clever plant disguise', redoing.. ");
        ItemsManager.UseItem(ItemsManager.GetItemNameById(60502));
    }
    MovementManager.FindTarget(ref escapeEventPos, 5);
    Thread.Sleep(500);
}

return true;
