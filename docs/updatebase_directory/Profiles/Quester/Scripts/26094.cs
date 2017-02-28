if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
    return false;

WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));

if(unit.IsValid)
{
    Logging.Write("Use Bomb!");
    ItemsManager.UseItem(questObjective.UseItemId, unit.Position);
    if(questObjective.WaitMs > 0)
        Thread.Sleep(questObjective.WaitMs);
}
