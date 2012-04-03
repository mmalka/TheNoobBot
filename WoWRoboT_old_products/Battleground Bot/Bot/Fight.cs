using WowManager;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.Useful;
using WowManager.WoW.WoWObject;

namespace Battleground_Bot.Bot
{
    internal static class Fight
    {
        private const float SearchDistanceN = 40;

        public static ulong GetFight()
        {
            try
            {
                WoWUnit wowUnit = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? new WoWUnit(ObjectManager.GetNearestWoWPlayer(ObjectManager.GetWoWUnitAlliance()).GetBaseAddress) : new WoWUnit(ObjectManager.GetNearestWoWPlayer(ObjectManager.GetWoWUnitHorde()).GetBaseAddress);

                if ((wowUnit.GetDistance > SearchDistanceN || wowUnit.GetBaseAddress <= 0) &&
                    Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].TargetFaction.Count > 0)
                    wowUnit =
                        new WoWUnit(
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByFaction(
                                    Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].TargetFaction, true)).
                                GetBaseAddress);

                if (ObjectManager.GetNumberAttackPlayer() > 0)
                    wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetUnitAttackPlayer());
                if (wowUnit != null)
                {
                    if (wowUnit.GetDistance < SearchDistanceN &&
                        wowUnit.Position.DistanceZ(ObjectManager.Me.Position) <= 15)
                        return wowUnit.Guid;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static void Attack(ulong guid)
        {
            WowManager.Navigation.MovementManager.StopMove();
            Mount.DismountMount();
            if (guid > 0 && Config.Bot.BotStarted)
            {
                var tUnit = new WoWUnit(ObjectManager.GetObjectByGuid(guid).GetBaseAddress);
                if (tUnit.SummonedBy > 0)
                    return;

                Log.AddLog(Translation.GetText(Translation.Text.Attack) + " " + tUnit.Name);
            }
            else if (Config.Bot.BotStarted)
            {
                return;
            }
            WowManager.Log.AddLog("Start combat guid=" + guid);
            ulong t = WowManager.WoW.PlayerManager.Fight.StartFight(guid, true);
            WowManager.Log.AddLog("End combat guid=" + guid);
            if (guid > 0 && t == 0 && ObjectManager.GetNumberAttackPlayer() == 0)
                Config.Bot.BlackList.Add(guid);
            if (t > 0)
            {
                try
                {
                    var targetNpc = new WoWUnit(ObjectManager.GetObjectByGuid(t).GetBaseAddress);

                    if (targetNpc.GetBaseAddress > 0)
                        if (TraceLine.TraceLineGo(targetNpc.Position) && !targetNpc.IsDead)
                        {
                            Log.AddLog(Translation.GetText(Translation.Text.Mob_stuck_adding_to_the_BlakcList));
                            Config.Bot.BlackList.Add(t);
                            ObjectManager.blackListMobAttack.Add(t);
                            return;
                        }
                }
                catch
                {
                }
            }
            if (t > 0)
            {
                Config.Bot.Kills++;
            }
        }
    }
}