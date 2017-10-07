

WoWUnit queen = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(89287, false));
WoWUnit oublion = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(89350, false));

if (!queen.IsValid || !oublion.IsValid)
{
    Thread.Sleep(1000);
    return true;
}

WoWUnit target = queen;
if (!target.IsValid || target.NotAttackable || target.IsDead)
    target = oublion;

    
if (!target.IsAlive)
{
    Thread.Sleep(1000);
    return true;
}

nManager.Wow.Helpers.Quest.GetSetIgnoreFight = true;
    
System.Threading.Thread thread = new System.Threading.Thread((System.Threading.ParameterizedThreadStart)delegate(object guid) { Fight.StartFight((UInt128)guid); });
thread.IsBackground = true;
thread.Start(target.Guid);
while (!target.IsDead && !ObjectManager.Me.IsDead)
{
    if (target.HaveBuff(178902))
    {
        nManagerSetting.AddBlackList(target.Guid, 10000);
        Fight.StopFight();
        Thread.Sleep(2000);
        ObjectManager.Me.Target = queen.Guid;
        if (queen.IsValid && queen.IsAlive)
            Fight.StartFight(queen.Guid);
        thread.Abort();
        nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
        return true;
    }
    Thread.Sleep(200);
}

thread.Abort();
    
nManager.Wow.Helpers.Quest.GetSetIgnoreFight = false;
return true;