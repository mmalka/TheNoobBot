WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(1908, questObjective.IsDead));

if(unit.IsValid)
{
nManager.Wow.Helpers.Keybindings.DownKeybindings(nManager.Wow.Enums.Keybindings.ACTIONBUTTON1);
Thread.Sleep(questObjective.WaitMs);
nManager.Wow.Helpers.Keybindings.UpKeybindings(nManager.Wow.Enums.Keybindings.ACTIONBUTTON1);

ClickOnTerrain.ClickOnly(unit.Position);

}
